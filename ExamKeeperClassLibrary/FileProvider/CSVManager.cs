using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ExamKeeperClassLibrary.FileProvider
{
    /// <summary>
    /// csv處理器
    /// </summary>
    public class CSVManager
    {
        /// <summary>
        /// 製造csv檔。若指定路徑上已有檔案會「覆寫」。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static async Task WriteCSV<T>(string filePath, IEnumerable<T> list)
        {
            var writeConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            //先寫死為中文編碼
            using var writer = new StreamWriter(filePath, false, Encoding.GetEncoding("big5"));
            using var csv = new CsvWriter(writer, writeConfiguration);

            //新增欄位名稱
            csv.WriteHeader(typeof(T));
            await csv.NextRecordAsync();

            //新增值
            await csv.WriteRecordsAsync(list);
        }

        private static readonly Dictionary<string, bool> DirectoryExistence = new Dictionary<string, bool>();

        /// <summary>
        /// 新增資料夾。若指定路徑沒有資料夾才會新增。
        /// 需要填purpose是為了改善效能，讓系統不要一直做Directory.Exists
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="rootPath"></param>
        public static void AddDirectory(string purpose, string rootPath)
        {
            if(DirectoryExistence.TryGetValue(purpose, out var isExists))
            {
                if (isExists || Directory.Exists(rootPath))
                {
                    DirectoryExistence[purpose] = true;
                    return;
                }

                Directory.CreateDirectory(rootPath);
                DirectoryExistence[purpose] = true;
            }

            if (Directory.Exists(rootPath))
            {
                DirectoryExistence[purpose] = true;
                return;
            }
            Directory.CreateDirectory(rootPath);
        }
    }
}
