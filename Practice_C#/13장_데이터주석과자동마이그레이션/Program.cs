using SampleEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13장_데이터주석과자동마이그레이션
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BooksDbContext())
            {
                // [Column] Entity Framework에서 로그를 출력한다
                db.Database.Log = sql => { Debug.Write(sql); }; // Debug는 비주얼 스튜디오에 있는 [출력] 창에 표시

                var count = db.Books.Count();
                Console.WriteLine(count);              
            }
        }
    }
}
