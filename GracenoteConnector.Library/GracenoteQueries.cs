using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GracenoteConnector.Library
{
    /// <remarks/>
    [XmlRootAttribute(ElementName = "QUERIES")]
    public class Queries<T> where T : IQuery
    {
        /// <remarks/>
        public Auth AUTH { get; set; }

        /// <remarks/>
        public T QUERY { get; set; }

        public string ToXmlString()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            XmlSerializer querySerializer = new XmlSerializer(typeof(Queries<T>));
            StringBuilder xml = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = true,
            };

            using (XmlWriter writer = XmlWriter.Create(xml, settings))
            {
                querySerializer.Serialize(writer, this, ns);
            }

            return xml.ToString();
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class Auth
    {
        /// <remarks/>
        public string CLIENT { get; set; }

        /// <remarks/>
        public string USER { get; set; }
    }

    public interface IQuery
    {

    }

    [XmlTypeAttribute(AnonymousType = true)]
    public class RegisterQuery : IQuery
    {
        /// <remarks/>
        [XmlAttribute]
        public string CMD
        {
            get
            {
                return "REGISTER";
            }
            set
            {
                // TODO: setがないとXmlSerializerに無視されるため。どうにかしたい。
                throw new NotSupportedException();
            }
        }

        public string CLIENT { get; set; }
    }


    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class AlbumTocQuery : IQuery
    {
        public AlbumTocQuery()
        {
        }

        public AlbumTocQuery(int[] toc)
        {
            this.TOC = new Toc()
            {
                OFFSETS = string.Join(" ", toc),
            };
        }

        /// <remarks/>
        [XmlAttribute]
        public string CMD
        {
            get
            {
                return "ALBUM_TOC";
            }
            set
            {
                // TODO: setがないとXmlSerializerに無視されるため。どうにかしたい。
                throw new NotSupportedException();
            }
        }

        /// <remarks/>
        public string MODE
        {
            get
            {
                return "SINGLE_BEST";
            }
            set
            {
                // TODO: setがないとXmlSerializerに無視されるため。どうにかしたい。
                throw new NotSupportedException();
            }
        }

        /// <remarks/>
        public Toc TOC { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class Toc
    {
        /// <remarks/>
        public string OFFSETS { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class AlbumFetchQuery : IQuery
    {
        public string GN_ID { get; set; }

        public AlbumFetchQuery()
        {
        }

        public AlbumFetchQuery(string gracenoteId)
        {
            this.GN_ID = gracenoteId;
        }

        /// <remarks/>
        [XmlAttribute]
        public string CMD
        {
            get
            {
                return "ALBUM_FETCH";
            }
            set
            {
                // TODO: setがないとXmlSerializerに無視されるため。どうにかしたい。
                throw new NotSupportedException();
            }
        }
    }
}
