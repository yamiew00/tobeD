using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ExamKeeper.Views;
using Microsoft.Extensions.Hosting;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {
    [ExcludeFromCodeCoverage]
    public class ResourceCache : IHostedService, IDisposable {
        static Timer _timer;
        private MongoLogger logger = new InitMongoLog("ResourceCache");
        private ResourceLibrary resourceAPI { get; set; }
        private BookSelectionModel model { get; set; }
        public ResourceCache() {
            resourceAPI = ResourceLibrary.Instance(logger);
            model = new BookSelectionModel(resourceAPI, new MongoSetting(), new InitMongoCache(MongoCollection.Selections));
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            _timer = new Timer(RefreshCache, null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(487)); // 8hours & 7minutes
            return Task.CompletedTask;
        }

        private int execCount = 0;

        public void RefreshCache(object state) {
            //利用 Interlocked 計數防止重複執行
            Interlocked.Increment(ref execCount);
            if (execCount == 1) {
                try {
                    logger.simpleLog("RefreshCache", "Start");
                    setGeneralSelection();
                } catch (Exception ex) {
                    logger.logException("RefreshCache", ex);
                }
            }
            Interlocked.Decrement(ref execCount);
        }

        private void setGeneralSelection() {
            string year = DateTimes.currentSchoolYear();
            EduSubject eduSubject = resourceAPI.getEduSubject(string.Empty);
            foreach (KeyValuePair<string, List<CodeMap>> edu in eduSubject.eduSubject) {
                foreach (CodeMap subject in edu.Value) {
                    GeneralBookSelection res = model.getGeneralSelection(edu.Key, subject.code, year);
                    if (res == null) {
                        logger.simpleLog("RefreshCache", $"Error:{model.ErrorMessage}");
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            //調整Timer為永不觸發，停用定期排程
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() {
            _timer?.Dispose();
        }
    }
}