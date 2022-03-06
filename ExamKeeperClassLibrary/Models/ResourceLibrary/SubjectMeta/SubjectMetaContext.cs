using MySqlConnector;
using System;
using System.Collections.Generic;

namespace ExamKeeperClassLibrary.Models.ResourceLibrary.SubjectMeta
{
    /// <summary>
    /// subjectMeta比較特別。「全部」的資料都載進快取。
    /// todo: 這裡的快取有兩份!
    /// </summary>
    public class SubjectMetaContext
    {
        private readonly string ConnectionString;

        /// <summary>
        /// <subject, UID, Subjectmeta>
        /// </summary>
        private Dictionary<string, Dictionary<string, SubjectMeta>> MetaUIDDictionary = new Dictionary<string, Dictionary<string, SubjectMeta>>();

        private Dictionary<string, List<SubjectMeta>> MetaDictionary = new Dictionary<string, List<SubjectMeta>>();

        //"server=35.236.136.171;port=3306;user id=nani-back-end;password=exbnXQy?D#h5DzHK;database=subjectMeta";
        public SubjectMetaContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private T TryGetDBValue<T>(object obj)
        {
            if (obj.GetType() == typeof(DBNull))
            {
                return default;
            }
            return (T)obj;
        }

        public Dictionary<string, SubjectMeta> GetDictionarySubject(string subject)
        {
            using var mySql = new MySqlConnection(ConnectionString);

            mySql.Open();
            var statement = "SELECT * FROM Meta" + subject;
            var reader = new MySqlCommand(statement, mySql).ExecuteReader();

            var result = new Dictionary<string, SubjectMeta>();

            while (reader.Read())
            {
                var uid = (string)reader["UID"];
                var newMeta = new SubjectMeta()
                {
                    UID = uid,
                    MetaType = (string)reader["metaType"],
                    Code = (string)reader["code"],
                    Name = (string)reader["name"],
                    ParentUID = TryGetDBValue<string>(reader["parentUID"]),
                    Curriculum = TryGetDBValue<string>(reader["curriculum"]),
                    Subject = subject,
                };
                result[uid] = newMeta;

                //第二份快取
                if(MetaDictionary.TryGetValue(subject, out var list))
                {
                    list.Add(newMeta);
                }
                else
                {
                    MetaDictionary[subject] = new List<SubjectMeta>() { newMeta };
                }
            }
            mySql.Close();

            //空值處理
            if(!MetaDictionary.TryGetValue(subject, out var _))
            {
                MetaDictionary[subject] = new List<SubjectMeta>();
            }

            return result;
        }

        /// <summary>
        /// 填充所有subjectMEta資料。約2.5秒
        /// </summary>
        /// <param name="subjects"></param>
        public void Populate(IEnumerable<string> subjects)
        {
            foreach (var subject in subjects)
            {
                MetaUIDDictionary[subject] = GetDictionarySubject(subject);
            }
        }

        public SubjectMeta GetByUID(string subject, string uid)
        {
            if (MetaUIDDictionary[subject].TryGetValue(uid, out var result))
            {
                return result;
            }
            return new SubjectMeta()
            {
                Code = default,
                Name = default
            };
        }

        public IEnumerable<SubjectMeta> GetMeta(string eduSubject)
        {
            return MetaDictionary[eduSubject];
        }
    }
}
