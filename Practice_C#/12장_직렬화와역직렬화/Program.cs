using Gushwell.CsBook;
using Section01;
using Section02;
using Section03;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace _12장_직렬화와역직렬화
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleCodeRunner.Run();
        }

        [SampleCode("Chapter 12")]
        class SampleCode
        {
            [ListNo("List 12-2")]
            public void Serialize()
            {
                var novel = new Novel
                {
                    Author = "제임스 P. 호건",
                    Title = "별의 계승자",
                    Published = 1977,
                };
                var settings = new XmlWriterSettings
                {
                    Encoding = new System.Text.UTF8Encoding(false),
                    Indent = true,
                    IndentChars = "  ",
                };

                // 동일한 응용 프로그램 안에서 객체의 내용을 XML형식으로 저장하고 나중에 다시 복원해서 이용하려면 DataContractSerializer 클래스를 사용
                using (var writer = XmlWriter.Create("novel.xml", settings))
                {
                    var serializer = new DataContractSerializer(novel.GetType());  // System.Runtime.Serialization 어셈블리 프로젝트의 참조에 추가
                    serializer.WriteObject(writer, novel);
                }

                Display("novel.xml");
            }


            [ListNo("List 12-3 DataContractSerializer를 사용해 역직렬화한다")]
            public void Deserialize()
            {
                using (var reader = XmlReader.Create("novel.xml"))
                {
                    var serializer = new DataContractSerializer(typeof(Novel));
                    var novel = serializer.ReadObject(reader) as Novel;
                    Console.WriteLine(novel);
                }
            }

            [ListNo("List 12-4 컬렉션ㅇ르 직렬화한다")]
            public void SerializeCollection()
            {
                var novels = new Novel[] {
               new Novel {
                  Author = "제임스 P. 호건",
                  Title = "별의 계승자",
                  Published = 1977,
               },
               new Novel {
                  Author = "허버트 조지 웰즈",
                  Title = "타임머신",
                  Published = 1895,
               },
            };
                using (var writer = XmlWriter.Create("novels.xml"))
                {
                    var serializer = new DataContractSerializer(novels.GetType());
                    serializer.WriteObject(writer, novels);
                }

                Display("novels.xml");
            }

            [ListNo("List 12-5 컬렉션 객체로 역직렬화한다")]
            public void DeserializeCollection()
            {
                using (XmlReader reader = XmlReader.Create("novels.xml"))
                {
                    var serializer = new DataContractSerializer(typeof(Novel[]));
                    var novels = serializer.ReadObject(reader) as Novel[];
                    foreach (var novel in novels)
                    {
                        Console.WriteLine(novel);
                    }
                }
            }

            // 응용 프로그램 간에 XML 형식의 데이터를 주고받으려면 XmlSerializer 클래스를 이용하는 것이 편리
            [ListNo("List 12-6 XmlSerializer를 사용해 직렬화한다")]
            public void SerializeToFile()
            {
                var novel = new Novel2
                {
                    Author = "제임스 P. 호건",
                    Title = "별의 계승자",
                    Published = 1977,
                };
                using (var writer = XmlWriter.Create("novel2.xml"))
                {
                    var serializer = new XmlSerializer(novel.GetType());
                    serializer.Serialize(writer, novel);
                }

                Display("novel2.xml");

            }

            [ListNo("List 12-6 SerializeToString")]
            public void SerializeToString()
            {
                var novel = new Novel2
                {
                    Author = "제임스 P. 호건",
                    Title = "별의 계승자",
                    Published = 1977,
                };
                var sb = new StringBuilder();
                using (var writer = XmlWriter.Create(sb))
                {
                    var serializer = new XmlSerializer(novel.GetType());
                    serializer.Serialize(writer, novel);
                }
                var xmlText = sb.ToString();
                Console.WriteLine(xmlText);
            }

            [ListNo("List 12-6 SerializeToStream")]
            public void SerializeToStream()
            {
                var novel = new Novel2
                {
                    Author = "제임스 P. 호건",
                    Title = "별의 계승자",
                    Published = 1977,
                };
                var stream = new MemoryStream();
                using (var writer = XmlWriter.Create(stream))
                {
                    var serializer = new XmlSerializer(novel.GetType());
                    serializer.Serialize(writer, novel);
                }
                // 버퍼에 있는 데이터를 모두 스트림에 라이트한다
                stream.Flush();

                // Position을 0으로 지정해서 리와인드한다
                stream.Position = 0;
                // StreamReader를 사용해서 MemoryStream의 내용을 읽어들인다
                var reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    Console.WriteLine(line);
                }
            }


            [ListNo("List 12-7 XmlSerializer를 사용해 역직렬화한다")]
            public void Deserialize2()
            {
                using (var reader = XmlReader.Create("novel2.xml"))
                {
                    var serializer = new XmlSerializer(typeof(Novel2));
                    var novel = serializer.Deserialize(reader) as Novel2;
                    // 아래는 내용을 확인하기 위한 코드이다
                    Console.WriteLine(novel);
                }
            }

            //XmlIgnore속성으로 직렬화의 대상에서 제외한다.
            [ListNo("List 12-8 XmlIgnore속성을 추가한 Novel클래스")]
            public void DeserializeFromString()
            {
                string xmlText = GetXmlString();

                using (var reader = XmlReader.Create(new StringReader(xmlText)))
                {
                    var serializer = new XmlSerializer(typeof(Novel2));
                    var novel = serializer.Deserialize(reader) as Novel2;
                    Console.WriteLine(novel);
                }
            }

            private static string GetXmlString()
            {
                var novel = new Novel2
                {
                    Author = "제임스 P. 호건",
                    Title = "별의 계승자",
                    Published = 1977,
                };
                var sb = new StringBuilder();
                using (var writer = XmlWriter.Create(sb))
                {
                    var serializer = new XmlSerializer(typeof(Novel2));
                    serializer.Serialize(writer, novel);
                }
                var xmlText = sb.ToString();
                return xmlText;
            }

            // 코드12,12에 있는 클래스 정의를 시리얼화한다
            [ListNo("List 12-11 컬렉션을 직력화한다")]
            public void SerializeCollection2()
            {
                var novels = new Novel2[] {
               new Novel2 {
                  Author = "제임스 P. 호건",
                  Title = "별의 계승자",
                  Published = 1977,
               },
               new Novel2 {
                  Author = "허버트 조지 웰즈",
                  Title = "타임머신",
                  Published = 1895,
               },
            };
                var novelCollection = new NovelCollection2
                {
                    Novels = novels
                };

                using (var writer = XmlWriter.Create("novels2.xml"))
                {
                    var serializer = new XmlSerializer(novelCollection.GetType());
                    serializer.Serialize(writer, novelCollection);
                }

                Display("novels2.xml");
            }

            [ListNo("List 12-12 XmlArray속성과 XmlArrayItem속성을 이용")]
            public void SerializeXmlArray()
            {
                // 다음은 p.310 아래쪽에 나온 코드입니다.
                // 이 코드를 실행했을 때 책에 나온 설명과 일치되도록 하려면 Novelist 클래스(Novelist.cs)에 추가한
                // 속성 네 개를 모두 주석 처리하고 단독으로 실행하기 바랍니다.

                var novelist = new Novelist
                {
                    Name = "아서 C. 클라크",
                    Masterpieces = new string[] {
                    " 2001 스페이스 오디세이",
                    " 유년기의 끝",
                }
                };
                using (var writer = XmlWriter.Create("novelist.xml"))
                {
                    var serializer = new XmlSerializer(novelist.GetType());
                    serializer.Serialize(writer, novelist);
                }

                Display("novelist.xml");
            }


            [ListNo("List 12-13 컬렉션을 역직렬화한다")]
            public void SerializeArrayMember()
            {
                using (var reader = XmlReader.Create("novels2.xml"))
                {
                    var serializer = new XmlSerializer(typeof(NovelCollection2));
                    var novels = serializer.Deserialize(reader) as NovelCollection2;
                    foreach (var novel in novels.Novels)
                    {
                        Console.WriteLine(novel);
                    }
                }


                Display("novels2.xml");

            }

            //----
            [ListNo("List 12-15")]
            public void SerializeJson()
            {
                var novels = new Novel3[] {
                  new Novel3 {
                    Author = "아이작 아시모프",
                    Title = "나는 로봇이야",
                    Published = 1950,
                  },
                  new Novel3 {
                    Author = "조지 오웰",
                    Title = "1984",
                    Published = 1949,
                  },
                };
                using (var stream = new FileStream("novels.json", FileMode.Create,
                                                    FileAccess.Write))
                {
                    var serializer = new DataContractJsonSerializer(novels.GetType());
                    serializer.WriteObject(stream, novels);
                }

                Display("novels.json");
            }
            private static string SerializeToStringJson()
            {
                var novels = new Novel3[] {
                  new Novel3 {
                    Author = "아이작 아시모프",
                    Title = "나는 로봇이야",
                    Published = 1950,
                  },
                  new Novel3 {
                    Author = "조지 오웰",
                    Title = "1984",
                    Published = 1949,
                  },
                };
                using (var stream = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(novels.GetType());
                    serializer.WriteObject(stream, novels);
                    stream.Close();
                    var jsonText = Encoding.UTF8.GetString(stream.ToArray());
                    return jsonText;
                }
            }

            [ListNo("List 12-16 JSON 데이터를 역직렬화한다(Json파일로부터)")]
            public void DeserializeJson()
            {
                using (var stream = new FileStream("novels.json", FileMode.Open, FileAccess.Read))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Novel3[]));
                    var novels = serializer.ReadObject(stream) as Novel3[];
                    foreach (var novel in novels)
                        Console.WriteLine(novel);
                }
            }

            [ListNo("List 12-16 JSON 데이터를 역직렬화한다")]
            public void DeserializeFromStringJson()
            {
                var jsonText = SerializeToStringJson();
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonText);
                using (var stream = new MemoryStream(byteArray))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Novel3[]));
                    var novels = serializer.ReadObject(stream) as Novel3[];
                    foreach (var novel in novels)
                        Console.WriteLine(novel);
                }
            }


            [ListNo("List 12-17 Dictionary를 JSON데이터로 직렬화한다")]
            public void SerializeDict() {
            var abbreviationDict = new AbbreviationDict {
                Abbreviations = new Dictionary<string, string> {
                    {"ODA","정부개발원조"},
                    {"OECD","경제 협력 개발 기구"},
                    {"OPEC","석유 수출국 기구"},
                }
                //Abbreviations = new Dictionary<string, string> {
                //    ["ODA"] = "정부개발원조",
                //    ["OECD"] = "경제 협력 개발 기구",
                //    ["OPEC"] = "석유 수출국 기구",
                //}
            };
            var settings = new DataContractJsonSerializerSettings {
                UseSimpleDictionaryFormat = true, // true이면 {"ODA","정부개발원조" false이면 [{"Key":"ODA",Value":"정부개발원조"
            };
            using (var stream = new FileStream("abbreviations.json", FileMode.Create, FileAccess.Write)) {
                var serializer = new DataContractJsonSerializer(abbreviationDict.GetType(), settings);
                serializer.WriteObject(stream, abbreviationDict);
            }

            Display("abbreviations.json");
        }


            [ListNo("List 12-18 JSON 데이터를 Dictionary로 역직렬화한다")]
            public void DeserializeDict()
            {
                var settings = new DataContractJsonSerializerSettings
                {
                    UseSimpleDictionaryFormat = true,
                };
                using (var stream = new FileStream("abbreviations.json", FileMode.Open, FileAccess.Read))
                {
                    var serializer = new DataContractJsonSerializer(typeof(AbbreviationDict), settings);
                    var dict = serializer.ReadObject(stream) as AbbreviationDict;
                    foreach (var item in dict.Abbreviations)
                    {
                        Console.WriteLine("{0} {1}", item.Key, item.Value);
                    }
                }
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
}
