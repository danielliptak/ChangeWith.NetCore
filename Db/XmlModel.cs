using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Change.Db
{
    [XmlRoot("Envelope", Namespace = GesmesNameSpace)]
    public class EcbEnvelope
    {
        public string Id { get; set; }
        public const string GesmesNameSpace = "http://www.gesmes.org/xml/2002-08-01";
        public const string EcbNameSpace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

        [XmlElement("Sender", Namespace = GesmesNameSpace)]
        public EcbSender Sender { get; set; }

        [XmlElement("subject", Namespace = GesmesNameSpace)]
        public string EcbSubject { get; set; }

        [XmlArray("Cube", Namespace = EcbNameSpace)]
        [XmlArrayItem("Cube")]
        public List<CubeRoot> CubeRootEl { get; set; }

        public class EcbSender
        {
            public string Id { get; set; }
            [XmlElement("name")]
            public string Name { get; set; }
        }

        public class CubeRoot
        {
            public string Id { get; set; }
            [XmlAttribute("time")]
            public string Time { get; set; }

            [XmlElement("Cube")]
            public List<CubeItem> CubeItems { get; set; }

            public class CubeItem
            {
                public string Id { get; set; }
                [XmlAttribute("rate")]
                public string RateStr { get; set; }

                //[XmlIgnore]
                //public decimal Rate => decimal.Parse(RateStr.ToString());

                [XmlAttribute("currency")]
                public string Currency { get; set; }
            }
        }
    }
}
