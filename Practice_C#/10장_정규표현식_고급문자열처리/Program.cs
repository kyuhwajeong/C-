using Gushwell.CsBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _10장_정규표현식_고급문자열처리
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleCodeRunner.Run();
        }

        [SampleCode("Chapter 10")]
        class SampleCode
        {
            [ListNo("List 10-1 지정된 패턴에 일치하는 문자열이 있는지 여부 판정")]
            public void IsMatch01()
            {
                var text = "private List<string> results = new List<string>();";
                bool isMatch = Regex.IsMatch(text, @"List<\w+>");//\w 단어*에 사용된 임의의 문자 하나와 일치 +직전의 요소가 1번 이상 반복되는 것과 일치
                if (isMatch)
                    Console.WriteLine("찾았습니다.");
                else
                    Console.WriteLine("찾지 못했습니다.");
            }

            [ListNo("List 10-2 IsMatch 인스턴스 메서드를 사용한 예")]
            public void IsMatch02()
            {
                var text = "private List<string> results = new List<string>();";
                var regex = new Regex(@"List<\w+>");
                bool isMatch = regex.IsMatch(text);
                if (isMatch)
                    Console.WriteLine("찾았습니다.");
                else
                    Console.WriteLine("찾지 못했습니다.");
            }

            [ListNo("List 10-3 지정한 패턴으로 문자열이 시작되는지 여부를 판단")]
            public void StartWith()
            {
                var text = "using System.Text.RegularExpressions;";
                bool isMatch = Regex.IsMatch(text, @"^using");  // ^은 행의 시작 지점을 나타낸다.
                if (isMatch)
                    Console.WriteLine("'using'으로 시작됩니다.");
                else
                    Console.WriteLine("'using'으로 시작되지 않습니다.");
            }

            [ListNo("List 10-4 지정한 패턴으로 문자열이 끝나는지 여부를 판정")]
            public void EndWith()
            {
                var text = "Regex 클래스를 사용해서 문자열을 처리하는 방법을 설명합니다.";
                bool isMatch = Regex.IsMatch(text, @"합니다.$"); // $은 행의 끝지점을 나타낸다. 
                if (isMatch)
                    Console.WriteLine("'합니다.'로 끝납니다.");
                else
                    Console.WriteLine("'합니다.'로 끝나지 않습니다.");
            }


            [ListNo("List 10-5 지정한 패턴에 완전히 일치하는지 여부를 판단")]
            public void PerfectMatch()
            {
                var strings = new[] { "Microsoft Windows", "Windows Server", "Windows", };
                var regex = new Regex(@"^(W|w)indows$");
                var count = strings.Count(s => regex.IsMatch(s));
                Console.WriteLine("{0}행과 일치", count);
            }

            [ListNo("List 10-6 지정한 패턴에 완전히 일치하는지 여부를 판단(나쁜예)")]
            public void BadPerfectMatch()
            {
                var strings = new[] { "Microsoft Windows", "Windows Server", "Windows", };
                var regex = new Regex(@"(W|w)indows");
                var count = strings.Count(s => regex.IsMatch(s));
                Console.WriteLine("{0}행과 일치", count);
            }

            [ListNo("List 10-7 지정한 패턴에 완전히 일치하는지 여부를 판정")]
            public void PerfectMatch02()
            {
                var strings = new[] { "13000", "-50.6", "0.123",  "+180.00",
                "10.2.5", "320-0851", " 123", "$1200", "500원", };
                var regex = new Regex(@"^[-+]?(\d+)(\.\d+)?$");
                foreach (var s in strings)
                {
                    var isMatch = regex.IsMatch(s);
                    if (isMatch)
                        Console.WriteLine(s);
                }
            }

            [ListNo("List 10-8 처음 나오는 문자열을 찾는다")]
            public void NormalSearch()
            {
                var text = "Regex 클래스에 있는 Match 메서드를 사용합니다.";
                Match match = Regex.Match(text, @"\p{IsHangulSyllables}+");//@"\p{IsHangulSyllables} 은 한글을 나타내는 정규 표현식, +를 붙였으므로 한문자 이상의 한글과 일치
                if (match.Success)
                    Console.WriteLine("{0} {1}", match.Index, match.Value);

                //Console.ReadLine();
            }

            [ListNo("List 10-9 일치하는 모든 문자열을 Matches메서드로 찾는다")]
            public void SearchAll()
            {
                var text = "private List<string> results = new List<string>();";
                var matches = Regex.Matches(text, @"List<\w+>"); // Matches 반환값은 MatchCollection형이다.
                foreach (Match match in matches)
                {
                    Console.WriteLine("Index={0}, Length={1}, Value={2}",
                            match.Index, match.Length, match.Value);
                }
            }

            [ListNo("List 10-10 일치하는 모든 문자열을 NextMatch 메서드를 찾는다")]
            public void SearchAllWithNextMatch()
            {
                var text = "private List<string> results = new List<string>();";
                Match match = Regex.Match(text, @"List<\w+>");
                while (match.Success)
                {
                    Console.WriteLine("Index={0}, Length={1}, Value={2}",
                                      match.Index, match.Length, match.Value);
                    match = match.NextMatch();
                }
            }

            [ListNo("List 10-11 Matches메서드의 결과에 LINQ를 적용한다.")]
            public void SearchAndLinq()
            {
                var text = "private List<string> results = new List<string>();";
                var matches = Regex.Matches(text, @"\b[a-z]+\b")
                                   .Cast<Match>()
                                   .OrderBy(x => x.Length);
                foreach (Match match in matches)
                {
                    Console.WriteLine("Index={0}, Length={1}, Value={2}",
                                      match.Index, match.Length, match.Value);
                }
            }

            [ListNo("List 10-12 일치한 부분 문자열의 일부만을 꺼낸다.")]
            public void CapturingGroup()
            {
                var text = "C#에는 《값형》과 《참조형》이라는 두 가지의 형이 존재합니다.";
                var matches = Regex.Matches(text, @"《([^《》]+)》");
                foreach (Match match in matches)
                {
                    Console.WriteLine("<{0}>", match.Groups[1]);
                }
                //Console.ReadLine();
            }

            [ListNo("List 10-13 대/소문자를 구분하지 않고 매칭.")]
            public void IgnoreMatch()
            {
                var text = "kor, KOR, Kor";
                var mc = Regex.Matches(text, @"\bkor\b",RegexOptions.IgnoreCase);
                foreach (Match m in mc)
                {
                    Console.WriteLine(m.Value);
                }
                //Console.ReadLine();
            }

            [ListNo("List 10-14 줄바꿈 코드를 포함한 문자열을 대상으로 한다")]
            public void Linefeed()
            {
                var text = "Word\nExcel\nPowerPoint\nOutlook\nOneNote\n";
                var pattern = @"^[a-zA-Z]{5,7}$";
                var mc = Regex.Matches(text, pattern, RegexOptions.Multiline);
                foreach (Match m in mc)
                {
                    Console.WriteLine("{0} {1}",m.Index, m.Value);
                }
                //Console.ReadLine();
            }

            [ListNo("List 10-15 Regex.Replace를 사용한 치환 처리의 예(1)")]
            public void Replace01()
            {
                var text = "C# 공부를 쪼끔씩 진행해보자.";
                var pattern = @"쪼금씩|쪼끔씩|쬐끔씩";
                var replaced = Regex.Replace(text, pattern, "조금씩");
                Console.WriteLine(replaced);
            }


            [ListNo("List 10-16 Regex.Replace를 사용한 치환 처리의 예(2)")]
            public void Replace02()
            {
                var text = "Word, Excel ,PowerPoint , Outlook,OneNote";
                var pattern = @"\s*,\s*";
                var replaced = Regex.Replace(text, pattern, ", ");
                Console.WriteLine(replaced);
            }

            [ListNo("List 10-17 Regex.Replace를 사용한 치환 처리의 예(3)")]
            public void Replace03()
            {
                var text = "foo.htm bar.html baz.htm";
                var pattern = @"\.(htm)\b";
                var replaced = Regex.Replace(text, pattern, ".html");
                Console.WriteLine(replaced);
            }

            [ListNo("List 10-18 그룹화 기능을 이용한 치환(1)")]
            public void ReplaceWithGroup01()
            {
                var text = "1024바이트,8바이트 문자,바이트,킬로바이트";
                var pattern = @"(\d+)바이트";
                var replaced = Regex.Replace(text, pattern, "$1byte");
                Console.WriteLine(replaced);
            }


            [ListNo("List 10-19  그룹화 기능을 이용한 치환(2)")]
            public void ReplaceWithGroup02()
            {
                var text = "1234567890123456";
                var pattern = @"(\d{4})(\d{4})(\d{4})(\d{4})";
                var replaced = Regex.Replace(text, pattern, "$1-$2-$3-$4");
                Console.WriteLine(replaced);
            }

            [ListNo("List 10-20  그룹화 기능을 이용한 치환(3)")]
            public void Split()
            {
                var text = "Word, Excel ,PowerPoint , Outlook,OneNote";
                var pattern = @"\s*,\s*";

                string[] substrings = Regex.Split(text, pattern);
                foreach (var match in substrings)
                {
                    Console.WriteLine("'{0}'", match);
                }
            }

            [ListNo("List 10-21 수량자 {n,}을 사용한 예")]
            public void Quantifier01()  // 영문자로 시작하고 그 뒤에 나오는 숫자가 다섯 문자 이상 연속하는 부분 문자열과 일치시키는 예
            {
                var text = "a123456 b123 Z12345 AX98765";
                var pattern = @"\b[a-zA-Z][0-9]{5,}\b";
                var matches = Regex.Matches(text, pattern);
                foreach (Match m in matches)
                    Console.WriteLine("'{0}'", m.Value);
            }

            [ListNo("List 10-22 수량자 {n,m}을 사용한 예")]
            public void Quantifier02()
            {
                var text = "シーズン、ゴールド、シーソー、ゴールデンなどと一致します。スウェーデンやノートなどとは一致しません。";
                var pattern = @"(\b|[^\p{IsKatakana}])(\p{IsKatakana}ー\p{IsKatakana}{2,3})(\b|[^\p{IsKatakana}])";
                var matches = Regex.Matches(text, pattern);
                foreach (Match m in matches)
                    Console.WriteLine("'{0}'", m.Groups[2]);
            }

            [ListNo("List 10-23 최장 일치 원칙의 예")]
            public void GreedyMatching()
            {
                var text = "<person><name>김삿갓</name><age>22</age></person>";
                var pattern = @"<.+>";
                var matches = Regex.Matches(text, pattern);
                foreach (Match m in matches)
                    Console.WriteLine("'{0}'", m.Value);
            }

            [ListNo("List 10-24 정규 표현식을 조작해서 최단으로 일치시킨다")]
            public void LazyMatching()
            {
                var text = "<person><name>김삿갓</name><age>22</age></person>";
                var pattern = @"<(\w[^>]+)>";
                var matches = Regex.Matches(text, pattern);
                foreach (Match m in matches)
                    Console.WriteLine("'{0}'", m.Groups[1].Value);
            }


            [ListNo("List 10-25 최단 일치의 수량자를 사용해 최단으로 일치시키는 예(1)")]
            public void LazyMatchingWithQuantifier01()
            {
                var text = "<person><name>김삿갓</name><age>22</age></person>";
                var pattern = @"<(\w+?)>";
                var matches = Regex.Matches(text, pattern);
                foreach (Match m in matches)
                    Console.WriteLine("'{0}'", m.Groups[1].Value);
            }

            [ListNo("List 10-26 최단 일치의 수량자를 사용해 최단으로 일치시키는 예(2)")]
            public void LazyMatchingWithQuantifier02()
            {
                var text = "<p>가나다라마</p><p>바사아자차</p>";
                var pattern = @"<p>(.*?)</p>";
                var matches = Regex.Matches(text, pattern);
                foreach (Match m in matches)
                    Console.WriteLine("'{0}'", m.Groups[1].Value);
            }

            [ListNo("List 10-27 역참조 구문을 사용한 예(1)")]
            public void Backreference01() // 문자가 두번 연속으로 나오는 문자열을 구하는 코드
            {
                var text = "도로를 지나가는 차들이 뛰뛰하고 경적을 울리면 반대쪽 차들이 빵빵하고 울렸다.";
                var pattern = @"(\w)\1";
                var matches = Regex.Matches(text, pattern);
                foreach (Match m in matches)
                    Console.WriteLine("'{0}'", m.Value);
            }
            [ListNo("List 10-28 역참조 구문을 사용한 예(2)")]
            public void Backreference02()
            {
                var text = "기러기 펠리컨 청둥오리 오리너구리 토마토 pops push pop";
                var pattern = @"\b(\w)\w\1\b";
                var matches = Regex.Matches(text, pattern);
                foreach (Match m in matches)
                    Console.WriteLine("'{0}'", m.Value);
            }
        }
    }
}
