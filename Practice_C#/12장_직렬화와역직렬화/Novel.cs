using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Section01 {
    // List 12-1
    public class Novel {
        public string Title { get; set; }
        public string Author { get; set; }
        //XmlIgnore속성으로 직렬화의 대상에서 제외한다.
        [XmlIgnore]
        public int Published { get; set; }
        public override string ToString() {
            return string.Format("[Title={0}, Author={1}, Published={2}]",
                                  Title, Author, Published);
        }
    }

}
