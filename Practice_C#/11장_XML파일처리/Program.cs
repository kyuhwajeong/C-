using Gushwell.CsBook;
using Section02;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _11장_XML파일처리
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleCodeRunner.Run();
        }
    }
    [SampleCode("Chapter 11")]
    class SampleCode
    {
        [ListNo("List 11-2 특정요소를 구한다")]
        public void GetAllElements1()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var xelements = xdoc.Root.Elements();
            foreach (var xnovelist in xelements)
            {
                XElement xname = xnovelist.Element("name");
                Console.WriteLine(xname.Value);
            }
        }


        [ListNo("List 11-3 특정 요소를 형변환해서 구한다")]
        public void GetAllElements2()
        {
            var xdoc = XDocument.Load("novelists.xml");
            foreach (var xnovelist in xdoc.Root.Elements())
            {
                var xname = xnovelist.Element("name");
                var birth = (DateTime)xnovelist.Element("birth");
                Console.WriteLine("{0} {1}", xname.Value, birth.ToShortDateString());
            }
        }


        [ListNo("List 11-4 속성을 구한다")]
        public void GetAttribute() {
            var xdoc = XDocument.Load("novelists.xml");
            foreach (var xnovelist in xdoc.Root.Elements()) {
                var xname = xnovelist.Element("name");
                XAttribute xeng = xname.Attribute("eng");
                Console.WriteLine("{0} {1}", xname.Value, xeng.Value);
                //Console.WriteLine("{0} {1}", xname.Value, xeng?.Value);
            }
        }


        [ListNo("List 11-5 조건을 지정해 XML요소를 구한다")]
        public void ExtractElements()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var xnovelists = xdoc.Root.Elements()
                                 .Where(x => ((DateTime)x.Element("birth")).Year >= 1900);
            foreach (var xnovelist in xnovelists)
            {
                var xname = xnovelist.Element("name");
                var birth = (DateTime)xnovelist.Element("birth");
                Console.WriteLine("{0} {1}", xname.Value, birth.ToShortDateString());
            }
        }

        [ListNo("List 11-6 XML 요소를 정렬한다")]
        public void SortElements()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var xnovelists = xdoc.Root.Elements()
                                 .OrderBy(x => (string)(x.Element("name").Attribute("eng")));
            foreach (var xnovelist in xnovelists)
            {
                var xname = xnovelist.Element("name");
                var birth = (DateTime)xnovelist.Element("birth");
                Console.WriteLine("{0} {1}", xname.Value, birth.ToShortDateString());
            }
        }

        [ListNo("List 11-7 중첩된 자식 요소를 구한다")]
        public void GetNestingElements()
        {
            var xdoc = XDocument.Load("novelists.xml");
            foreach (var xnovelist in xdoc.Root.Elements())
            {
                var xname = xnovelist.Element("name");
                var works = xnovelist.Element("masterpieces")
                                     .Elements("title")
                                     .Select(x => x.Value);
                Console.WriteLine("{0} - {1}", xname.Value, string.Join(", ", works));
            }
        }

        [ListNo("List 11-8 자손 요소를 구한다")]
        public void GetDescendants()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var xtitles = xdoc.Root.Descendants("title");
            foreach (var xtitle in xtitles)
            {
                Console.WriteLine(xtitle.Value);
            }
        }

        [ListNo("List 11-9 익명 클래스의 객체 형태로 요소를 구한다")]
        public void Projection()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var novelists = xdoc.Root.Elements()
                                .Select(x => new
                                {
                                    Name = (string)x.Element("name"),
                                    Birth = (DateTime)x.Element("birth"),
                                    Death = (DateTime)x.Element("death")
                                });
            foreach (var novelist in novelists)
            {
                Console.WriteLine("{0} ({1}-{2})",
                                   novelist.Name, novelist.Birth.Year, novelist.Death.Year);
            }
        }

        [ListNo("List 11-10 사용자 지정 클래스의 객체 형태로 요소를 구한다")]
        public void Projection3()
        {
            var novelists = ReadNovelists();
            foreach (var novelist in novelists)
            {
                Console.WriteLine("{0} ({1}-{2}) - {3}",
                    novelist.Name, novelist.Birth.Year, novelist.Death.Year,
                    string.Join(", ", novelist.Masterpieces));
            }
        }

        // List 11-10
        public IEnumerable<Novelist> ReadNovelists()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var novelists = xdoc.Root.Elements()
                                .Select(x => new Novelist
                                {
                                    Name = (string)x.Element("name"),
                                    EngName = (string)(x.Element("name").Attribute("eng")),
                                    Birth = (DateTime)x.Element("birth"),
                                    Death = (DateTime)x.Element("death"),
                                    Masterpieces = x.Element("masterpieces")
                                                     .Elements("title")
                                                     .Select(title => title.Value)
                                                     .ToArray()
                                });
            return novelists.ToArray();
        }

        [ListNo("List 11-11 문자열로부터 XDocment를 생성")]
        public void CreateXDocumentFromString()
        {
            string xmlstring =
                  @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                    <novelists>
                      <novelist>
                        <name eng=""Agatha Christie"">아가사 크리스티</name>
                        <birth>1890-09-15</birth>
                        <death>1976-01-12</death>
                        <masterpieces>
                          <title>그리고 아무도 없었다</title>
                          <title>오리엔트 특급 살인</title>
                        </masterpieces>
                      </novelist>
                    </novelists>";
            var xdoc = XDocument.Parse(xmlstring);

            // 내용을 확인한다
            Display(xdoc);

        }

        [ListNo("List 11-12 문자열로부터 XElement를 생성")]
        public void CreateXElementFromString()
        {
            string elmstring =
              @"<novelist>
                  <name kana=""O. Henry"">오 헨리</name>
                  <birth>1862-10-11</birth>
                  <death>1910-06-05</death>
                  <masterpieces>
                    <title>현자의 선물</title>
                    <title>마지막 잎새</title>
                  </masterpieces>
                </novelist>";
            XElement element = XElement.Parse(elmstring);  // 생성해서

            var xdoc = XDocument.Load("novelists.xml");
            xdoc.Root.Add(element);

            // 내용을 확인한다
            Display(xdoc);
        }


        [ListNo("List 11-13 함수 생성으로 XDocument 객체를 조합한다")]
        public void CreateXDocumentManually()
        {
            var novelists = new XElement("novelists",
              new XElement("novelist",
                new XElement("name", ">마크 트웨인", new XAttribute("eng", "Mark Twain")),
                new XElement("birth", "1835-11-30"),
                new XElement("death", "1910-03-21"),
                new XElement("masterpieces",
                    new XElement("title", "톰 소여의 모험"),
                    new XElement("title", "허클베리 핀의 모험"),
                    new XElement("title", "왕자와 거지")
                )
              ),
              new XElement("novelist",
                  new XElement("name", "어니스트 헤밍웨이", new XAttribute("eng", "Ernest Hemingway")),
                  new XElement("birth", "1899-07-21"),
                  new XElement("death", "1961-07-02"),
                  new XElement("masterpieces",
                     new XElement("title", "무기여 잘 있거라"),
                     new XElement("title", "노인과 바다")
                  )
              )
            );
            var xdoc = new XDocument(novelists);

            // 내용을 확인한다  ToString()로 XML형식 문자열을 구할 수 있다
            Console.WriteLine(xdoc.ToString());
        }

        [ListNo("List 11-14 컬렉션으로부터 XDocument를 생성한다")]
        public void CreateXDocumentFromCollection()
        {
            // Novelist의 목록을 준비한다
            var novelists = new List<Novelist> {
              new Novelist {
                Name = "마크 트웨인",
                EngName = "Mark Twain",
                Birth = DateTime.Parse("1835-11-30"),
                Death = DateTime.Parse("1910-03-21"),
                Masterpieces = new string[] { "톰 소여의 모험", "허클베리 핀의 모험", },
              },
              new Novelist {
                  Name = "어니스트 헤밍웨이",
                  EngName = "Ernest Hemingway",
                  Birth = DateTime.Parse("1899-07-21"),
                  Death = DateTime.Parse("1961-07-02"),
                  Masterpieces = new string[] { "무기여 잘 있거라", "노인과 바다", },
              },

            };

            // Linq to Objects를 사용해서 목록의 내용을 XElement 시퀀스로 변환한다
            var elements = novelists.Select(x =>
              new XElement("novelist",
                new XElement("name", x.Name, new XAttribute("eng", x.EngName)),
                new XElement("birth", x.Birth),
                new XElement("death", x.Death),
                new XElement("masterpieces", x.Masterpieces.Select(t => new XElement("title", t)))
              )
            );

            // 가장 위에 있는 novelists 요소를 생성한다
            var root = new XElement("novelists", elements);

            // root 요소를 지정해서 XDocument 오브젝트를 생성한다
            var xdoc = new XDocument(root);

            // 내용을 확인한다
            Display(xdoc);

        }

        [ListNo("List 11-15 요소를 추가한다")]
        public void AddElement()
        {
            var element = new XElement("novelist",
                new XElement("name", "찰스 디킨스", new XAttribute("eng", "Charles Dickens")),
                new XElement("birth", "1812-02-07"),
                new XElement("death", "1870-06-09"),
                new XElement("masterpieces",
                  new XElement("title", "올리버 트위스트"),
                  new XElement("title", "크리스마스 캐럴")
                )
              );
            var xdoc = XDocument.Load("novelists.xml");
            xdoc.Root.Add(element); // 끝지점에 추가
            //xdoc.Root.AddFirst(element); // 시작지점에 추가하고 싶다면 AddFirst 사용
            // 이후는 확인용 코드이다
            foreach (var xnovelist in xdoc.Root.Elements())
            {
                var xname = xnovelist.Element("name");
                var birth = (DateTime)xnovelist.Element("birth");
                Console.WriteLine("{0} {1}", xname.Value, birth.ToShortDateString());
            }
            //Display(xdoc);
        }

        [ListNo("List 11-16 요소를 삭제한다")]
        public void RemoveElement()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var elements = xdoc.Root.Elements()
                               .Where(x => x.Element("name").Value == "마크 트웨인");
            elements.Remove();
            Display(xdoc);
        }

        [ListNo("List 11-17 요소를 치환한다(1)")]
        public void ReplaceElement1()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var element = xdoc.Root.Elements()
                                   .Single(x => x.Element("name").Value == "마크 트웨인");
            string elmstring =
              @"<novelist>
                <name eng=""Mark Twain"">마크 트웨인</name>
                <birth>1835-11-30</birth>
                <death>1910-03-21</death>
                <masterpieces>
                  <title>도금시대</title>
                  <title>아서 왕 궁정의 코네티컷 양키</title>
                </masterpieces>
              </novelist>";
            var newElement = XElement.Parse(elmstring);
            element.ReplaceWith(newElement);
            Display(xdoc);
        }

        [ListNo("List 11-18 요소를 치환한다(2)")]
        public void ReplaceElement2()
        {
            var xdoc = XDocument.Load("novelists.xml");
            Display(xdoc);
            var element = xdoc.Root.Elements()
                              .Single(x => x.Element("name").Value == "마크 트웨인")
                              .Element("masterpieces");
            var newElement = new XElement("masterpieces",
                new XElement("title", "도금시대"),
                new XElement("title", "아서 왕 궁정의 코네티컷 양키")
            );
            element.ReplaceWith(newElement);
            Display(xdoc);
        }

        [ListNo("List 11-19 요소를 치환한다(3)")]
        public void ReplaceElement3()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var element = xdoc.Root.Elements()
                              .Select(x => x.Element("name"))
                              .Single(x => x.Value == "마크 트웨인");
            element.Value = "마크 트웨인";
            Display(xdoc);
        }

        // 책에서는 로드한 XML을 그대로 저장하는 코드를 게재했지만
        // 이 코드는 요소를 수정하고나서 저정한다
        [ListNo("List 11-20 XDocument 객체를 저장한다")]
        public void SaveXMLDocument()
        {
            var xdoc = XDocument.Load("novelists.xml");
            var element = xdoc.Root.Elements()
                              .Select(x => x.Element("name"))
                              .Single(x => x.Value == "마크 트웨인");
            element.Value = "마크 트웨인";
            xdoc.Save("newNovelists.xml");
            //xdoc.Save("newNovelists.xml", SaveOptions.DisableFormatting); // 공백 및 줄바꿈을 제외한 XML 파일을 작성

            var xnewdoc = XDocument.Load("newNovelists.xml");
            Display(xnewdoc);
        }

        [ListNo("List 11-22 쌍 정보를 XML에 저장한다")]
        public void CrearePairData()
        {
            var option = new XElement("option");
            option.SetElementValue("enabled", true);
            option.SetElementValue("min", 0);
            option.SetElementValue("max", 100);
            option.SetElementValue("step", 10);
            var root = new XElement("settings", option);
            root.Save("sample.xml");

            Display("sample.xml");
        }

        // 실행되는 순서는 List 11-22, List 11-24
        [ListNo("List 11-24 XML 파일에서 쌍 정보를 읽어 들인다")]
        public void ReadPairData()
        {
            var xdoc = XDocument.Load("sample.xml");
            var option = xdoc.Root.Element("option");
            Console.WriteLine((bool)option.Element("enabled"));
            Console.WriteLine((int)option.Element("min"));
            Console.WriteLine((int)option.Element("max"));
            Console.WriteLine((int)option.Element("step"));
        }

        // 실행되는 순서는 List 11-23, List 11-25
        [ListNo("List 11-23 쌍 정보를 속성의 형태를 저장한다")]
        public void CreatePairData2()
        {
            var option = new XElement("option");
            option.SetAttributeValue("enabled", true);
            option.SetAttributeValue("min", 0);
            option.SetAttributeValue("max", 100);
            option.SetAttributeValue("step", 10);
            var root = new XElement("settings", option);
            root.Save("sample.xml");
            Display("sample.xml");
        }

        [ListNo("List 11-25 XML 파일에서 쌍 정보를 읽어 들인다(속성의 형태로)")]
        public void ReadPairDat2()
        {
            var xdoc = XDocument.Load("sample.xml");
            var option = xdoc.Root.Element("option");
            Console.WriteLine((bool)option.Attribute("enabled"));
            Console.WriteLine((int)option.Attribute("min"));
            Console.WriteLine((int)option.Attribute("max"));
            Console.WriteLine((int)option.Attribute("step"));
        }

        [ListNo("List 11-26 Dictionary객체를 XML 파일에 저장한다")]
        public void DictionaryToXml() {
            var dict = new Dictionary<string, string> {
                {"IAEA","국제 원자력 기구"},
                {"IMF" , "국제통화기금"},
                {"ISO" , "국제 표준화 기구"},
            };
            //var dict = new Dictionary<string, string> {
            //    ["IAEA"] = "국제 원자력 기구",
            //    ["IMF"] = "국제통화기금",
            //    ["ISO"] = "국제 표준화 기구",
            //};
            var query = dict.Select(x => new XElement("word",
                                           new XAttribute("abbr", x.Key),
                                           new XAttribute("korean", x.Value)));
            var root = new XElement("abbreviations", query);
            root.Save("abbreviations.xml");

            Display("abbreviations.xml");
        }

        [ListNo("List 11-27 XML 파일로부터 Dictionary 객체를 생성한다(1)")]
        public void DictionaryFromXml()
        {
            var xdoc = XDocument.Load("abbreviations.xml");
            var pairs = xdoc.Root.Elements()
                            .Select(x => new
                            {
                                Key = x.Attribute("abbr").Value,
                                Value = x.Attribute("korean").Value
                            });
            var dict = pairs.ToDictionary(x => x.Key, x => x.Value);
            foreach (var d in dict)
            {
                Console.WriteLine(d.Key + "=" + d.Value);
            }
        }

        [ListNo("List 11-28 XML 파일로부터 Dictionary 객체를 생성한다(2)")]
        public void DictionaryFromXml2()
        {
            var xmlstring = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <abbreviations>
                  <IAEA>국제 원자력 기구</IAEA>
                  <IMF>국제통화기금</IMF>
                  <ISO>국제 표준화 기구</ISO>
                </abbreviations>";
            var xwork = XDocument.Parse(xmlstring);
            xwork.Save("abbreviations2.xml");

            // List 11-28
            var xdoc = XDocument.Load("abbreviations2.xml");
            var pairs = xdoc.Root.Elements()
                            .Select(x => new
                            {
                                Key = x.Name.LocalName,
                                Value = x.Value
                            });
            var dict = pairs.ToDictionary(x => x.Key, x => x.Value);
            foreach (var d in dict)
            {
                Console.WriteLine(d.Key + "=" + d.Value);
            }
        }

        private void Display(XDocument xdoc)
        {
            // 이 아래에 있는 코드는 확인용이다
            foreach (var xnovelist in xdoc.Root.Elements())
            {
                var xname = xnovelist.Element("name");
                var birth = (DateTime)xnovelist.Element("birth");
                var death = (DateTime)xnovelist.Element("death");
                var masterpieces = xnovelist.Element("masterpieces").Elements().Select(x => x.Value);

                Console.WriteLine("{0}({1}-{2}) - {3}", xname.Value, birth.ToShortDateString(), death.ToShortDateString(),
                    string.Join(", ", masterpieces));
            }
            //Console.WriteLine();
        }

        public void Display(string filename)
        {
            var lines = File.ReadLines(filename);

            // 이 아래에 있는 코드는 확인용이다
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }
}
