using System.Xml.Serialization;
using System.Collections.Generic;

namespace GracenoteConnector.Library
{
    /// <remarks/>
    [XmlRootAttribute(ElementName = "RESPONSES")]
    public class Responses<T>
    {
        /// <remarks/>
        public T RESPONSE { get; set; }

        public string MESSAGE { get; set; }
    }

    public class Responce
    {
        public enum ResponseStatus
        {
            OK,
            ERROR,
            NO_MATCH,
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public ResponseStatus STATUS { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class RegisterResponse : Responce
    {
        public string USER { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class AlbumTocResponse : Responce
    {
        /// <remarks/>
        [XmlElement]
        public Album[] ALBUM { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class Album
    {
        /// <remarks/>
        public string GN_ID { get; set; }

        /// <remarks/>
        public string ARTIST { get; set; }

        /// <remarks/>
        public string TITLE { get; set; }

        /// <remarks/>
        public string PKG_LANG { get; set; }

        /// <remarks/>
        public int DATE { get; set; }

        /// <remarks/>
        public Genre GENRE { get; set; }

        /// <remarks/>
        public int TRACK_COUNT { get; set; }

        /// <remarks/>
        [XmlElementAttribute("TRACK")]
        public Track[] TRACK { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class Genre
    {
        /// <remarks/>
        [XmlAttributeAttribute()]
        public int NUM { get; set; }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int ID { get; set; }

        /// <remarks/>
        [XmlTextAttribute()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class Track
    {
        /// <remarks/>
        public int TRACK_NUM { get; set; }

        /// <remarks/>
        public string GN_ID { get; set; }

        /// <remarks/>
        public string ARTIST { get; set; }
        
        /// <remarks/>
        public string TITLE { get; set; }

        /// <remarks/>
        public Genre GENRE { get; set; }
    }
}
