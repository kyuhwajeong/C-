using SampleEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace _13장_수준높은쿼리
{
    class Program
    {
        static void Main(string[] args)
        {
            AddBooks();
            DisplayAllBooks();
            Console.Write("테스트용 데이터를 추가했습니다. 코드13-11을 실행합니다. 엔터키를 누르세요.");
            Console.ReadLine();
            UpdateBook();
            DisplayAllBooks();
            Console.WriteLine();

            Console.Write("코드13-12를 실행합니다. 엔터키를 누르세요.");
            Console.ReadLine();
            DeleteBook();
            DisplayAllBooks();
            Console.WriteLine();

            Console.Write("샘플 쿼리를 실행합니다. 엔터키를 누르세요.");
            Console.ReadLine();

            QuerySample01();
            Console.WriteLine("----서적을 출판년도 저자 이름 순서(각각 오름차순)로 정렬");
            QuerySample02();
            Console.WriteLine("----각 발행년도에 해당하는 서적 수를 구한다");
            QuerySample03();
            Console.WriteLine("----집필한 서적이 가장 많은 저자 한 명을 구한다");
            QuerySample04();
            Console.WriteLine("----");


            Console.Write("코드13-14를 실행합니다. 엔터키를 누르세요.");
            Console.ReadLine();
            foreach (var book in GetBooks()) {
                Console.WriteLine("{0} {1}", book.Title, book.Author.Name);
                //Console.WriteLine($"{book.Title} {book.Author.Name}");
            }

            Console.ReadLine();
        }

        // id=10 인 서적을 만들기 위해 서적을 추가한다
        // 테스트용 코드
        private static void AddBooks() {
            using (var db = new BooksDbContext()) {
                var author = db.Authors.Single(a => a.Name == "애거사 크리스티");
                var books = new Book[] {
                    new Book { Author = author, PublishedYear=1924, Title = "갈색 양복의 사나이" },
                    new Book { Author = author, PublishedYear=1925, Title = "침니스의 비밀" },
                    new Book { Author = author, PublishedYear=1926, Title = "애크로이드 살인 사건" },
                    new Book { Author = author, PublishedYear=1927, Title = "빅 포" },
                    new Book { Author = author, PublishedYear=1928, Title = "블루 트레인의 수수께끼" },
                    new Book { Author = author, PublishedYear=1929, Title = "세븐 다이얼스 미스터리" },
                    new Book { Author = author, PublishedYear=1930, Title = "목사관의 살인" },
                    new Book { Author = author, PublishedYear=1930, Title = "신비의 사나이 할리 퀸" },
                };
                foreach (var book in books) {
                    db.Books.Add(book);
                }
                db.SaveChanges();
            }
        }


        // List 13-11
        private static void UpdateBook() {
            using (var db = new BooksDbContext()) {
                var book = db.Books.Single(x => x.Title == "별의 계승자");
                book.PublishedYear = 2016;
                db.SaveChanges();
            }
        }

        // List 13-12
        private static void DeleteBook() {
            using (var db = new BooksDbContext()) {
                var book = db.Books.SingleOrDefault(x => x.Id == 10);
                if (book != null) {
                    db.Books.Remove(book);
                    db.SaveChanges();
                }
            }
        }

        // '13.10 수준높은 쿼리'에 나온 예제
        private static void QuerySample01() {
            using (var db = new BooksDbContext()) {
                var authors = db.Authors
                                .Where(a => a.Books.Count() > 2);//집필한 서적이 두 권 이상인 저자를 구한다
                foreach (var author in authors) {
                    Console.WriteLine("{0} {1} {2}",author.Name, author.Gender, author.Birthday);
                    //Console.WriteLine($"{author.Name} {author.Gender} {author.Birthday}");
                }
            }

            Console.ReadLine();
        }

        private static void QuerySample02() {
            using (var db = new BooksDbContext()) {
                var books = db.Books
                              .OrderBy(b => b.PublishedYear)    // 서적을 출판년도
                              .ThenBy(b => b.Author.Name);      // 저자 이름 순서(각각 오름차순)로 정렬
                foreach (var book in books) {
                    Console.WriteLine("{0} {1} {2}",book.Title, book.PublishedYear, book.Author.Name);
                    //Console.WriteLine($"{book.Title} {book.PublishedYear} {book.Author.Name}");
                }
            }
        }


        private static void QuerySample03() {
            using (var db = new BooksDbContext()) {
                var groups = db.Books
                               .GroupBy(b => b.PublishedYear)// 각 발행년도에 해당하는 서적 수를 구한다
                               .Select(g => new {
                                   Year = g.Key,
                                   Count = g.Count()
                               });
                foreach (var g in groups) {
                    Console.WriteLine("{0} {1}",g.Year, g.Count);
                    //Console.WriteLine($"{g.Year} {g.Count}");
                }
            }
        }

        private static void QuerySample04() {
            using (var db = new BooksDbContext()) {
                var author = db.Authors
                               .Where(a => a.Books.Count() ==
                                      db.Authors.Max(x => x.Books.Count()))
                               .First(); // 집필한 서적이 가장 많은 저자 한 명을 구한다
                Console.WriteLine("{0} {1} {2}",author.Name, author.Gender, author.Birthday);
            }
        }

        // List 13-14  // 관련된 엔터티도 한꺼번에 읽어 들이는 코드
        static IEnumerable<Book> GetBooks() {
            using (var db = new BooksDbContext()) {
                return db.Books
                         .Where(b => b.PublishedYear > 1900)
                         .Include("Author")  //.Include(nameof(Author)) //<- Include메서드의 인수에는 함께 읽어 들일 엔터티 이름도 지정 
                                             //C# 6.0 버전용 컴파일될 때 식별자의 이름을 문자열로 만듦
                         .ToList();
            }
        }

        static IEnumerable<Book> GetAllBooks() {
            using (var db = new BooksDbContext()) {
                return db.Books.ToList();
            }
        }

        // List 13-8
        static void DisplayAllBooks() {
            var books = GetAllBooks();
            foreach (var book in books) {
                Console.WriteLine("{0} {1} {2}",book.Id, book.Title, book.PublishedYear);
                //Console.WriteLine($"{book.Id} {book.Title} {book.PublishedYear}");
            }
        }
    }
}
