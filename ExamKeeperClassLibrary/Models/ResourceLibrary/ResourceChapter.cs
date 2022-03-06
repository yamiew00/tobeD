using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExamKeeperClassLibrary.Models.ResourceLibrary
{
    public class ResourceChapter
    {
        [JsonIgnore]
        public string UID;

        public string Name;

        public string Code;

        [JsonIgnore]
        public string ParentUID;

        /// <summary>
        /// 子節點
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ResourceChapter> Data;

        /// <summary>
        /// 只有末節點才會存知識點
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ResourceKnowledge> Knowledges;

        public void AddKnowledge(ResourceKnowledge knowledge)
        {
            Knowledges.Add(knowledge);
            Knowledges = Knowledges.OrderBy(knowledge => knowledge.Code).ToList();
        }

        public void AddChild(ResourceChapter resourceChapter)
        {
            if(Data == null)
            {
                Data = new List<ResourceChapter>() { resourceChapter };
            }
            else
            {
                Data.Add(resourceChapter);
                Data = Data.OrderBy(chapter => chapter.Code).ToList();
            }
        }
    }
}
