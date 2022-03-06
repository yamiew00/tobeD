using System;
using System.Collections.Generic;
using System.Text.Json;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {

    public class SendRequest {
        protected IMongoLogger logger { get; set; }
        protected WebRequestHelper webHelper { get; set; }

        protected MessageModel message { get; set; }
        public string ErrorMessage { get { return message.TakeMessage(); } }

        public SendRequest(IMongoLogger logger) {
            this.logger = logger;
            this.webHelper = new WebRequestHelper(logger);
            message = new MessageModel();
        }

        protected Response<T> send<T>(string url, Dictionary<string, string> optionHeader, bool record = false) {
            try {
                string response = webHelper.sendAPI(HttpMethod.GET, url, optionHeader);
                if (record) {
                    logger.logPayload(url, optionHeader, response);
                }

                return response<T>(response);
            } catch (Exception ex) {
                logger.logException("SendRequest", ex);
                return default(Response<T>);
            }
        }

        protected Response<T> send<T>(string url, Dictionary<string, string> optionHeader, object data, bool record = false) {
            try {
                string response = webHelper.sendAPI(HttpMethod.POST, url, optionHeader, data);
                if (record) {
                    logger.logPayload(url, optionHeader, response);
                }
                return response<T>(response);
            } catch (Exception ex) {
                logger.logException("SendRequest", ex);
                return default(Response<T>);
            }
        }

        protected OneExamResponse<T> sendOneExam<T>(string url, Dictionary<string, string> optionHeader, object data, bool record = false) {
            try {
                string response = webHelper.sendAPI(HttpMethod.POST, url, optionHeader, data);
                if (record) {
                    logger.logPayload(url, optionHeader, response);
                }
                if (string.IsNullOrWhiteSpace(response)) {
                    return default(OneExamResponse<T>);
                }
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                };
                OneExamResponse<object> responseData = JsonSerializer.Deserialize<OneExamResponse<object>>(response, options);
                if (responseData.isSuccess()) {
                    return JsonSerializer.Deserialize<OneExamResponse<T>>(response, options);
                }
                OneExamResponse<string> responseMessage = JsonSerializer.Deserialize<OneExamResponse<string>>(response, options);
                message.addMessage(responseMessage.content);
                return null;
            } catch (Exception ex) {
                logger.logException("sendOneExam", ex);
                return default(OneExamResponse<T>);
            }
        }

        private Response<T> response<T>(string apiResponse) {
            if (string.IsNullOrWhiteSpace(apiResponse)) {
                return default(Response<T>);
            }
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Response<T>>(apiResponse, options);
        }
    }
}