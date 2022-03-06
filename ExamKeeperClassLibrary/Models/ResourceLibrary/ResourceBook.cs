using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamKeeperClassLibrary.Models.ResourceLibrary
{

    public class ResourceBook
    {
        /// <summary>
        /// 學年度
        /// </summary>
        [JsonIgnore]
        public int Year;

        /// <summary>
        /// 課綱
        /// </summary>
        [JsonIgnore]
        public string Curriculum;

        /// <summary>
        /// 學制科目
        /// </summary>
        [JsonIgnore]
        public string EduSubject;

        /// <summary>
        /// 書本id
        /// </summary>
        public string BookId;

        /// <summary>
        /// 冊次名稱
        /// </summary>
        public string VolumeName;

        /// <summary>
        /// 出版社。
        /// </summary>
        [JsonIgnore]
        public string Version;

        /// <summary>
        /// 樹狀章節
        /// </summary>
        [JsonProperty("data")]
        public List<ResourceChapter> _ChapterTree = new List<ResourceChapter>();

        /// <summary>
        /// 章節(攤平資料)
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, ResourceChapter> Chapters = new Dictionary<string, ResourceChapter>();

        /// <summary>
        /// 新增知識點
        /// </summary>
        /// <param name="chapterUID"></param>
        /// <param name="knowledge"></param>
        public void AddKnowledge(string chapterUID, ResourceKnowledge knowledge)
        {
            if (Chapters.TryGetValue(chapterUID, out var chapter))
            {
                chapter.AddKnowledge(knowledge);
            }
            else
            {
                Chapters[chapterUID] = new ResourceChapter()
                {
                    UID = chapterUID,
                    Knowledges = new List<ResourceKnowledge>()
                    {
                        knowledge
                    }
                };
            }
        }

        /// <summary>
        /// 新增除了知識點以外的所有相關訊息
        /// </summary>
        /// <param name="chapter"></param>
        public void AddNode(ResourceChapter chapter)
        {
            if (Chapters.TryGetValue(chapter.UID, out var result))
            {
                result.Name = chapter.Name;
                result.UID = chapter.UID;
                result.ParentUID = chapter.ParentUID;
                result.Code = chapter.Code;
                return;
            }

            Chapters[chapter.UID] = new ResourceChapter()
            {
                Name = chapter.Name,
                UID = chapter.UID,
                ParentUID = chapter.ParentUID,
                Code = chapter.Code
            };
        }
    }
}
