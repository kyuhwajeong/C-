using Gushwell.CsBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section06 {

    [SampleCode("Chapter 16")]
    class Section0601  {
        private  List<Book> books;

        public Section0601() {
            books = new List<Book> {
               new Book { Title = "こころ", Price = 541, Pages = 543 },
               new Book { Title = "二都物語", Price = 842, Pages = 1232 },
               new Book { Title = "人間失格", Price = 643, Pages = 334 },
               new Book { Title = "伊豆の踊子", Price = 432, Pages = 286 },
               new Book { Title = "銀河鉄道の夜", Price = 543, Pages = 385 },
               new Book { Title = "遠野物語", Price = 666, Pages = 381 },
            };
        }

        [ListNo("List 16-16 PLINQ로 병렬 처리")]
        public void AsParallel() {
            var selected = books.AsParallel()
                                .Where(b => b.Price > 500 && b.Pages > 400)
                                .Select(b => new { b.Title });
            foreach (var book in selected) {
                Console.WriteLine(book.Title);
            }
        }

        [ListNo("List 16-17 PLINQ로 병렬 처리한다(순서를 보장)")]
        public void AsParallelAsOrdered() {
            var selected = books.AsParallel()
                                .AsOrdered()
                                .Where(b => b.Price > 500 && b.Pages > 400)
                                .Select(b => new { b.Title });
            foreach (var book in selected) {
                Console.WriteLine(book.Title);
            }
        }

        [ListNo("List 16-18 PLINQ로 동시 실행되는 태스크의 최대 수를 제한한다")]
        public void WithDegreeOfParallelism() {
            var selected = books.AsParallel()
                                .WithDegreeOfParallelism(8)
                                .Where(b => b.Price > 500 && b.Pages > 400)
                                .Select(b => new { b.Title });
            foreach (var book in selected) {
                Console.WriteLine(book.Title);
            }
        }

        // ForAll안에 있는 람다식이 병렬로 실행. 속도 향상을 기대할 수 있는 것은 'ForAll 안에서 처리할 하나하나의 처리 비용이 클 때'또는
        // '처리할 데이터가 대량으로 있을 때' 그리고 병렬로 동작하기 때문에 실행 순서도 보장되지 않으므로 이 점에도 주의해야 한다.
        [ListNo("List 16-19 ForAll 메서드 병렬")]
        public void ForAll() {
            var selected = books.AsParallel()
                                .Where(b => b.Price > 500);
            selected.AsParallel().ForAll(book => {
                Console.WriteLine(book.Title);
            });
        }

    }
}
