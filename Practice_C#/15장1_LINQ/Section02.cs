using Gushwell.CsBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter15 {
    [SampleCode("Chapter 15")]
    class SampleCodeSection02 {
        [ListNo("List 15-2 어떤 조건 안에서 최댓값을 구한다")]
        public void MaximumPrice() {
            var price = Library.Books
                               .Where(b => b.CategoryId == 1)
                               .Max(b => b.Price);
            Console.WriteLine(price);

        }

        [ListNo("List 15-3 최솟값인 요소를 하나만 구한다")]
        public void MostShortTitleBook() {
            var min = Library.Books
                             .Min(x => x.Title.Length);
            var book = Library.Books
                              .First(b => b.Title.Length == min);
            Console.WriteLine(book);
        }

        // Min메서드가 여러 번 호출되므로 프로그램의 성능에 좋지 않은 영향을 준다.
        [ListNo("List 15-3 성능에 좋지 않음(비추천)")]
        public void MostShortTitleBook2()
        {
            var book = Library.Books
                              .First(b => b.Title.Length == 
                                          Library.Books.Min(x => x.Title.Length));
            Console.WriteLine(book);
        }

        [ListNo("List 15-4  평가값 이상인 요소를 모두 구한다")]
        public void AboveAverage() {
            var average = Library.Books
                                 .Average(x => x.Price);
            var aboves = Library.Books
                                .Where(b => b.Price > average);
            foreach (var book in aboves) {
                Console.WriteLine(book);
            }
        }

        [ListNo("List 15-5 중복을 제거한다")]
        public void Distinct() {
            var query = Library.Books
                               .Select(b => b.PublishedYear)
                               .Distinct()
                               .OrderBy(y => y);
            foreach (var n in query)
                Console.WriteLine(n);
        }

        [ListNo("List 15-6 여러 개의 키로 나열한다")]
        public void SortByMultipleKeys() {
            var books = Library.Books
                               .OrderBy(b => b.CategoryId)
                               .ThenByDescending(b => b.PublishedYear);
            foreach (var book in books) {
                Console.WriteLine(book);
            }
        }

        [ListNo("List 15-7 Where메서드에서 Contains를 이용한다")]
        public void ContainsSample() {
            var years = new int[] { 2013, 2016 };
            var books = Library.Books
                               .Where(b => years.Contains(b.PublishedYear));
            foreach (var book in books) {
                Console.WriteLine(book);
            }
        }


        [ListNo("List 15-8 발행연도를 기준으로 그룹화한다")]
        public void GroupBySample() {
            var groups = Library.Books
                                .GroupBy(b => b.PublishedYear)
                                .OrderBy(g => g.Key);
            foreach (var g in groups) {
                Console.WriteLine(g.Key+"년");
                //Console.WriteLine($"{g.Key}년");
                foreach (var book in g) {
                    Console.WriteLine("  "+book);
                    //Console.WriteLine($"  {book}");
                }
            }
        }

        [ListNo("List 15-9 각 발행연도 그룹에서 가장 가격이 비싼 서적을 구한다")]
        public void GroupBySample2() {
            var selected = Library.Books
                                  .GroupBy(b => b.PublishedYear)
                                  .Select(group => group.OrderByDescending(b => b.Price)
                                                        .First())
                                  .OrderBy(o => o.PublishedYear);
            foreach (var book in selected) {
                Console.WriteLine(book.PublishedYear +"년 "+book.Title+" ("+book.Price+")");
                //Console.WriteLine($"{book.PublishedYear}년 {book.Title} ({book.Price})");
            }
        }


        [ListNo("List 15-10 ToLookup으로 발행연도별로 그룹화한다")]
        public void ToLookupSample() {
            var lookup = Library.Books
                                .ToLookup(b => b.PublishedYear);
            var books = lookup[2014];
            foreach (var book in books) {
                Console.WriteLine(book);
            }
        }
    }
}
