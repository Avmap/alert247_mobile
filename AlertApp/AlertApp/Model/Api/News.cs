using AlertApp.Resx;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class NewsEntryResponse
    {
        [DataMember]
        public List<NewsEntry> News;

        public NewsEntryResponse()
        {
            News = new List<NewsEntry>();
        }
    }

    [DataContract]
    public class GetNewsPostBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "lang")]
        public string Lang { get; set; }
    }

    [DataContract]
    public class NewsEntry
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string PublishDate { get; set; }

        #region Computed properties
        [DataMember]
        public bool IsAir{get{ return Category == NewsEntryCategory.AIR;}}
        public bool IsSea { get { return Category == NewsEntryCategory.SEA; } }
        public bool IsTransport { get { return Category == NewsEntryCategory.TRANSPORT; } }
        public bool IsGeneric { get { return Category == NewsEntryCategory.GENERIC; } }

        public bool IsSuccess { get { return Category == NewsEntryCategory.SUCCESS; } }

        public bool IsWarning { get { return Category == NewsEntryCategory.WARNING; } }

        public bool IsDanger { get { return Category == NewsEntryCategory.DANGER; } }

        public bool HasImage { get { return !string.IsNullOrEmpty(Image); } }

        public bool HasLink { get { return !string.IsNullOrEmpty(Link); } }

        public bool HasDescription { get { return !string.IsNullOrEmpty(Description); } }

        public bool HasDate { get { return !string.IsNullOrEmpty(PublishDate); } }
        #endregion

    }
    public class NewsEntryCategory
    {
        public const string AIR = "AIR";
        public const string SEA = "SEA";
        public const string TRANSPORT = "TRANSPORT";
        public const string GENERIC = "GENERIC";
        public const string SUCCESS = "SUCCESS";
        public const string WARNING = "WARNING";
        public const string DANGER = "DANGER";
    }

    

}
