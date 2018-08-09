using SampleEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            //InsertBooks();
            //AddAuthors();
            //AddBooks();
            UpdateBook();
            //DisplayAllBooks();
        }

        static void InsertBooks()
        {
            using (var db = new BooksDbContext())
            {
                var book1 = new Book
                {
                    Title = "별의 계승자",
                    PublishedYear = 1977,
                    Author = new Author
                    {
                        Birthday = new DateTime(1941, 6, 27),
                        Gender = "M",
                        Name = "제임스 P. 호건",
                    }
                };

                db.Books.Add(book1);
                var book2 = new Book
                {
                    Title = "타임머신",
                    PublishedYear = 1895,
                    Author = new Author
                    {
                        Birthday = new DateTime(1866, 9, 21),
                        Gender = "M",
                        Name = "허버트 조지 웰즈",
                    }
                };
                db.Books.Add(book2);
                db.SaveChanges(); // 데이터베이스를 업데이트한다.
                Console.WriteLine("{0} {1}",book1.Id, book2.Id);
            }
        }

        // Authors만 추가한다.
        private static void AddAuthors()
        {
            using(var db =  new BooksDbContext())
            {
                var author1 = new Author
                {
                    Birthday = new DateTime(1890, 09, 15),
                    Gender = "F",
                    Name = "애거사 크리스티",
                };
                db.Authors.Add(author1);
                var author2 = new Author
                {
                    Birthday = new DateTime(1812, 02, 07),
                    Gender = "M",
                    Name = "찰스 디킨스",
                };
                db.Authors.Add(author2);
                db.SaveChanges();
            }
        }

        // 이미 등록된 Author를 사용해 서적을 추가한다.
        private static void AddBooks()
        {
            using (var db = new BooksDbContext())
            {
                var author1 = db.Authors.Single(a => a.Name == "애거사 크리스티");
                var book1 = new Book
                {
                    Title = "그리고 아무도 없었다",
                    PublishedYear = 1939,
                    Author = author1,
                };
                db.Books.Add(book1);
                var author2 = db.Authors.Single(a => a.Name == "찰스 디킨스");
                var book2 = new Book
                {
                    Title = "두 도시 이야기",
                    PublishedYear = 1859,
                    Author = author2,
                };
                db.Books.Add(book2);
                db.SaveChanges();
            }
        }

        // 데이터를 수정한다.
        private static void UpdateBook()
        {
            using (var db = new BooksDbContext())
            {
                var book = db.Books.Single(x => x.Title == "별의 계승자");
                book.PublishedYear = 2016;
                db.SaveChanges();
            }
        }

        private static void DeleteBook()
        {
            using (var db = new BooksDbContext())
            {
                var book = db.Books.SingleOrDefault(x => x.Id == 10);
                if(book != null)
                {
                    db.Books.Remove(book);
                    db.SaveChanges();
                }
            }
        }

        static IEnumerable<Book> GetBooks()
        {
            using(var db = new BooksDbContext())
            {
                return db.Books
                    .Where(book => book.Author.Name.StartsWith("제임스"))
                    .ToList();
            }
        }

        static void DisplayAllBooks()
        {
            var books = GetBooks();
            foreach(var book in books)
            {
                Console.WriteLine("{0} {1}", book.Title, book.PublishedYear);
            }
            Console.ReadLine();
        }
    }
}
