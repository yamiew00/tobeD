using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {
    public class ExportModel {
        private IMongoExport exportDB { get; set; }
        public ExportModel(IMongoExport db) {
            this.exportDB = db;
        }

        /// <summary> 寫入匯出紀錄 </summary>
        /// <param name="exportPaper">匯出資訊</param>
        public void exportRecord(ExportPaper exportPaper, Guid userUID) {
            string year = DateTimes.getTaiwanYear();
            ExportRecord record = new ExportRecord() {
                UID = exportPaper.UID,
                examUID = exportPaper.examUID,
                year = year,
                status = ExportStatus.Waiting.ToString(),
                user = userUID,
                systemTime = DateTime.Now
            };
            insertExportRecord(exportPaper, record, year);
        }

        /// <summary> 寫入匯出紀錄 </summary>
        /// <param name="exportPaper">匯出資訊</param>
        /// <param name="record">匯出紀錄</param>
        /// <param name="year">民國年</param>
        /// <returns></returns>
        private bool insertExportRecord(ExportPaper exportPaper, ExportRecord record, string year) {
            try {
                exportDB.addExportRecord(record);
                exportDB.addExportInfo(exportPaper, year);
            } catch {
                return false;
            }
            return true;
        }

        #region -讀取個人匯出紀錄-
        public List<ExportPaper> getExportPaper(Guid userUID) {
            List<ExportPaper> result = new List<ExportPaper>();
            string year = DateTimes.getTaiwanYear();
            string lastYear = DateTimes.getTaiwanYear(-1);
            result.AddRange(exportDB.getExportPapers(userUID, year));
            result.AddRange(exportDB.getExportPapers(userUID, lastYear));
            return result;
        }
        public ExportPaper getExportPaper(ExportRecord record) {
            return exportDB.getExportPaper(record.UID, record.year);
        }
        public ExportRecord getExportRecord(string UID) {
            return exportDB.getExportRecord(UID);;
        }
        public List<ExportRecord> getExportRecords(ExportStatus status) {
            return exportDB.getExportRecords(status.ToString());
        }
        public List<ExportRecord> getExportRecords(Guid userUID) {
            return exportDB.getExportRecords(userUID);
        }

        public Dictionary<string, ExportRecord> getExportRecords(List<string> UIDs) {
            List<ExportRecord> records = exportDB.getExportRecords(UIDs);
            if (Compare.EmptyCollection(records)) {
                return null;
            }
            return records.ToDictionary(o => o.UID, o => o);
        }
        #endregion

        #region -Export Setting-
        /// <summary> 取得使用者最後一次匯出設定 </summary>
        /// <param name="userUID">使用者UID</param>
        /// <param name="examUID">試卷UID</param>
        /// <returns></returns>
        public UserExamTypesetting getLastSetting(Guid userUID, string examUID) {
            return exportDB.getLastSetting(userUID, examUID);
        }
        /// <summary> 紀錄紙本卷設定 </summary>
        public void fileSetting(Guid userUID, string examUID, Typesetting setting) {
            UserExamTypesetting lastSetting = exportDB.getLastSetting(userUID, examUID);
            if (lastSetting == null) {
                lastSetting = new UserExamTypesetting(userUID, examUID);
            }
            lastSetting.typesetting = setting;
            exportDB.setLastSetting(lastSetting);
        }
        /// <summary> 紀錄線上卷設定 </summary>
        public void onlineSetting(Guid userUID, string examUID, OnlineSetting setting) {
            UserExamTypesetting lastSetting = exportDB.getLastSetting(userUID, examUID);
            if (lastSetting == null) {
                lastSetting = new UserExamTypesetting(userUID, examUID);
            }
            lastSetting.onlinesetting = setting;
            exportDB.setLastSetting(lastSetting);
        }
        #endregion
    }
}