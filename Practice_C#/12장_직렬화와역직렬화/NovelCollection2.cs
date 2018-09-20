using Section01;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Section02 {
    // List 12-10
    [XmlRoot("novels")]
    public class NovelCollection2 {
        [XmlElement(Type = typeof(Novel2), ElementName = "novel")]
        public Novel2[] Novels { get; set; }
    }


}
