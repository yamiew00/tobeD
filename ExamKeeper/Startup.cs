using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using ExamKeeperClassLibrary.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Newtonsoft;

namespace ExamKeeper
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowOrigins";
        public static IConfiguration Configuration { get; private set; }
        private bool isProduction { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //編碼
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //初始化
            services.InjectLibrary(Configuration);

            services.AddMvc()
                    .AddNewtonsoftJson();

            //使enum能夠正常顯示
            services.AddControllers()
                    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // 加入認證
            services
                .AddAuthentication("MemberProfile")
                .AddScheme<MemberAuthentication, TokenHandler>("MemberProfile", null);

            //加入授權
            services.AddAuthorization();

            if (!isProduction)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ExamKeeper API", Version = "v1" });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header. \r\n\r\n Enter your token in the text input below.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                        Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
                });
            }

            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        /* 確定後可設定內網
                        builder.WithOrigins("", ""); */
                        builder
                            .SetIsOriginAllowed(origin => true) // allow any origin
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string environmentName = (string)Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                env.EnvironmentName = environmentName;
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            isProduction = "Production".Equals(environmentName);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (!isProduction)
            {
                // Swagger 
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocumentTitle = "swagger - ExamKeeper";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExamKeeper API V1");
                });
                app.Run(ctx =>
                {
                    ctx.Response.Redirect("/swagger/");
                    return System.Threading.Tasks.Task.FromResult(0);
                });
            }
            else
            {
                app.UseDefaultFiles();
                app.UseStaticFiles();
            }
        }
    }
}