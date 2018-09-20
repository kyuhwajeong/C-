using Gushwell.CsBook;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8장_날짜와시간처리
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleCodeRunner.Run();

            //var dateTime = DateTime.Parse("2018/8/19");
            //DayOfWeek dayOfWeek = dateTime.DayOfWeek;  // enum value is 1(Sunday)


            //// 윤년을 판정
            //var isLeapYear = DateTime.IsLeapYear(2016);
            //if (isLeapYear)
            //    Console.WriteLine("윤년입니다.");
            //else
            //    Console.WriteLine("윤년이 아닙니다");

            //// 문자열을 DateTime 객체로 변환
            //DateTime dt1, dt2;
            //if(DateTime.TryParse("2017/6/21", out dt1))
            //    Console.WriteLine(dt1);
            //if (DateTime.TryParse("2017/6/21 10:41:38", out dt2))
            //    Console.WriteLine(dt2);

            ////DateTime dt3 = DateTime.Parse("20170621");  // FormatException 발생
            ////Console.WriteLine(dt3);

            //Console.WriteLine("\n\n날짜의 포맷");

            //Console.ReadKey();
        }


        [SampleCode("Chapter 8")]
        class SampleCode
        {
            [ListNo("List 8-1 DateTime 객체를 생성")]
            public void CreateDateTime()
            {
                var dt1 = new DateTime(2017, 10, 22);
                var dt2 = new DateTime(2016, 10, 22, 8, 45, 20);
                Console.WriteLine(dt1);
                Console.WriteLine(dt2);
            }

            [ListNo("List 8-2 Today속성과 Now속성")]
            public void TodayAndNow()
            {
                var today = DateTime.Today;
                var now = DateTime.Now;
                Console.WriteLine("Today : {0}", today);
                Console.WriteLine("Now : {0}", now);
            }

            [ListNo("List 8-3 DateTime에 포함된 속성")]
            public void GetPropertyValue()
            {
                var now = DateTime.Now;
                int year = now.Year;                 // 연도: Year
                int month = now.Month;               // 월: Month
                int day = now.Day;                   // 일: Day
                int hour = now.Hour;                 // 시: Hour
                int minute = now.Minute;             // 분: Minute
                int second = now.Second;             // 초: Second
                int millisecond = now.Millisecond;   // 1/100초: Millisecond

                //Console.WriteLine($"{now}");
                //Console.WriteLine($"{year}/{month}/{day} {hour}:{minute}:{second} {millisecond}");
            }

            [ListNo("List 8-4 지정한 날짜의 요일을 구함")]
            public void TestDayOfWeek()
            {
                var today = DateTime.Today;
                DayOfWeek dayOfWeek = today.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday)
                    Console.WriteLine("오늘은 일요일입니다.");
                else
                    Console.WriteLine("오늘은 일요일이 아닙니다.");
            }

            [ListNo("List 8-6 윤년을 판정")]
            public void IsLeapYear()
            {
                var isLeapYear = DateTime.IsLeapYear(2016);
                if (isLeapYear)
                    Console.WriteLine("윤년입니다.");
                else
                    Console.WriteLine("윤년이 아닙니다.");
            }


            [ListNo("List 8-7 문자열을 DateTime객체로 변환")]
            public void StringToDateTime()
            {
                DateTime dt1;
                if (DateTime.TryParse("2017/6/21", out dt1))
                    Console.WriteLine(dt1);
                DateTime dt2;
                if (DateTime.TryParse("2017/6/21 10:41:38", out dt2))
                    Console.WriteLine(dt2);
            }

            [ListNo("List 8-8 문자열을 DateTime객체로 변환2")]
            public void StringToDateTime3()
            {
                DateTime dt;
                if (DateTime.TryParse("平成28年3月15日", out dt))
                    Console.WriteLine(dt);
            }

            [ListNo("List 8-9 날짜를 문자열로 변환")]
            public void VariousToString()// 날짜를 문자열로 변환한다.
            {
                var date = new DateTime(2017, 10, 22, 21, 6, 47);
                var s1 = date.ToString("d");                           // 2017-10-22
                var s2 = date.ToString("D");                           // 2017년 10월 22일 일요일
                var s3 = date.ToString("yyyy-MM-dd");                  // 2017-10-22
                var s4 = date.ToString("yyyy년M월d일(ddd)");           // 2017년10월22일(일)
                var s5 = date.ToString("yyyy년MM월dd일 HH시mm분ss초"); // 2017년10월22일 21시06분47초
                var s6 = date.ToString("f");                           // 2017년 10월 22일 일요일 오후 9:06
                var s7 = date.ToString("F");                           // 2017년 10월 22일 일요일 오후 9:06:47
                var s8 = date.ToString("t");                           // 오후 9:06
                var s9 = date.ToString("T");                           // 오후 9:06:47
                var s10 = date.ToString("tt hh:mm");                   // 오후 09:06
                var s11 = date.ToString("HH시mm분ss초");               // 21시06분47초


                Console.WriteLine(s1);
                Console.WriteLine(s2);
                Console.WriteLine(s3);
                Console.WriteLine(s4);
                Console.WriteLine(s5);
                Console.WriteLine(s6);
                Console.WriteLine(s7);
                Console.WriteLine(s8);
                Console.WriteLine(s9);
                Console.WriteLine(s10);
                Console.WriteLine(s11);
            }

            [ListNo("List 8-10 날짜를 특정형식의 문자열로 표현")]
            public void Format() //날짜를 특정형식의 문자열로 표현한다.
            {
                var today = DateTime.Today;
                var str = string.Format("{0}년{1,2}월{2,2}일",
                                        today.Year, today.Month, today.Day);
                Console.WriteLine(str);
            }

            [ListNo("List 8-11 날짜를 일본식으로 표시")]
            public void JapaneseCalendar() //날짜를 일본식으로 표시
            {
                var date = new DateTime(2016, 8, 15);
                var culture = new CultureInfo("ja-JP");
                culture.DateTimeFormat.Calendar = new JapaneseCalendar();
                var str = date.ToString("ggyy年M月d日", culture);
                Console.WriteLine(str);
                var str2 = date.ToString("gg", culture);
                Console.WriteLine(str2);
                var str3 = date.ToString("ddd", culture);
                Console.WriteLine(str3);
            }

            [ListNo("List 8-12 지정한 날짜의 연호를 구한다")]
            public void GetEraName()  //지정한 날짜의 연호를 구한다.
            {
                var date = new DateTime(1995, 8, 24);
                var culture = new CultureInfo("ja-JP");
                culture.DateTimeFormat.Calendar = new JapaneseCalendar();
                var era = culture.DateTimeFormat.Calendar.GetEra(date);
                var eraName = culture.DateTimeFormat.GetEraName(era);
                Console.WriteLine(eraName);
            }

            [ListNo("List 8-13 지정한 날짜의 요일 문자열을 구한다.")]
            public void KoreanDayOfWeek()
            {
                var date = new DateTime(1998, 6, 25);
                var culture = new CultureInfo("ko-KR");
                culture.DateTimeFormat.Calendar = new KoreanCalendar();
                var dayOfWeek = culture.DateTimeFormat.GetDayName(date.DayOfWeek);
                Console.WriteLine(dayOfWeek);

                var shortDayOfWeek = culture.DateTimeFormat.GetShortestDayName(date.DayOfWeek);
                Console.WriteLine(shortDayOfWeek);
            }

            [ListNo("List 8-14 날짜와 시간을 비교한다.")]
            public void CompareDatetime()
            {
                var dt1 = new DateTime(2006, 10, 18, 1, 30, 21);
                var dt2 = new DateTime(2006, 11, 2, 18, 5, 28);
                if (dt1 < dt2)
                    Console.WriteLine("dt2 쪽이 미래입니다.");
                else if (dt1 == dt2)
                    Console.WriteLine("dt1와 dt2는 같은 시각입니다.");
            }

            [ListNo("List 8-15 날짜만 비교")]
            public void CompareDate()
            {
                var dt1 = new DateTime(2001, 10, 25, 1, 30, 21);
                var dt2 = new DateTime(2001, 10, 25, 18, 5, 28);
                if (dt1.Date < dt2.Date)
                    Console.WriteLine("dt2 쪽이 미래입니다.");
                else if (dt1.Date == dt2.Date)
                    Console.WriteLine("dt1와 dt2는 같은 날짜입니다.");
            }

            [ListNo("List 8-16 지정한 시분초 이후의 시각을 구한다.")]
            public void AddTimeSpan()
            {
                var now = DateTime.Now;
                var future = now + new TimeSpan(1, 30, 0);
                Console.WriteLine(future);
            }

            [ListNo("List 8-17 지정한 시분초 이전의 시각을 구한다")]
            public void SubtractTimeSpan()
            {
                var now = DateTime.Now;
                var past = now - new TimeSpan(1, 30, 0);
                Console.WriteLine(past);
            }

            [ListNo("List 8-18 n일 후와 n일 전의 날짜를 구한다.")]
            public void AddDays()
            {
                var today = DateTime.Today;
                var future = today.AddDays(20);
                var past = today.AddDays(-20);
                Console.WriteLine(future);
                Console.WriteLine(past);
            }

            [ListNo("List 8-19 n년 후와 n개월 후를 구한다.")]
            public void AddYearsMonths()
            {
                var date = new DateTime(2009, 10, 22);
                var future = date.AddYears(2).AddMonths(5);
                Console.WriteLine(future);
            }

            [ListNo("List 8-20 두 시각의 차를 구한다.")]
            public void DiffDatetime()
            {
                var date1 = new DateTime(2009, 10, 22, 1, 30, 20);
                var date2 = new DateTime(2009, 10, 22, 2, 40, 56);
                TimeSpan diff = date2 - date1;
                Console.WriteLine("두 시각의 차는 {0}일 {1}시간 {2}분 {3}초입니다.",
                                  diff.Days, diff.Hours, diff.Minutes, diff.Seconds);
                Console.WriteLine("총 {0}초입니다.", diff.TotalSeconds);
            }

            [ListNo("List 8-21 두 개의 날짜의 차를 구한다.")]
            public void DiffDays()
            {
                var dt1 = new DateTime(2016, 1, 20, 23, 0, 0);
                var dt2 = new DateTime(2016, 1, 21, 1, 0, 0);
                TimeSpan diff = dt2.Date - dt1.Date;
                Console.WriteLine("{0}일간", diff.Days);
            }

            [ListNo("List 8-22 두 개의 날짜의 차를 구하는 나쁜 코드 예")]
            public void BadDiffDays()
            {
                var dt1 = new DateTime(2016, 1, 20, 23, 0, 0);
                var dt2 = new DateTime(2016, 1, 21, 1, 30, 0);
                TimeSpan diff = dt2 - dt1;
                Console.WriteLine("{0}일간", diff.Days);
            }

            [ListNo("List 8-23 월의 말일을 구한다.")]
            public void GetEndOfMonth()
            {
                var today = DateTime.Today;
                // 해당 월에 몇일이 있는지를 구한다
                int day = DateTime.DaysInMonth(today.Year, today.Month);
                // 이 day를 사용해서 DateTime 오브젝트를 생성한다. endOfMonth가 해당 월의 말일이다
                var endOfMonth = new DateTime(today.Year, today.Month, day);
                Console.WriteLine(endOfMonth);
            }

            [ListNo("List 8-24 1월1일부터의 날짜 수를 구한다.")]
            // 
            public void GetTotalDays()
            {
                var today = DateTime.Today;
                int dayOfYear = today.DayOfYear;
                Console.WriteLine(dayOfYear);
            }

            [ListNo("List 8-25 1월1일부터의 날짜 수 구하는 나쁜 코드 예")]
            public void BadTotalDays()
            {
                var today = DateTime.Today;
                var baseDate = new DateTime(today.Year, 1, 1).AddDays(-1);
                TimeSpan ts = today - baseDate;
                Console.WriteLine(ts.Days);
            }

            [ListNo("List 8-26 다음 특정 요일을 구한다.")]
            public void SampleNextDay()
            {
                Console.WriteLine(NextDay(DateTime.Now, DayOfWeek.Friday));
                Console.WriteLine(NextDay(DateTime.Now, DayOfWeek.Monday));
            }

            // List 8-26
            public static DateTime NextDay(DateTime date, DayOfWeek dayOfWeek)
            {
                var days = (int)dayOfWeek - (int)(date.DayOfWeek);
                if (days <= 0)
                    days += 7;
                return date.AddDays(days);
            }

            [ListNo("List 8-27 나이를 구한다.")]
            public void SampleGetAge()
            {
                var birthday = new DateTime(1992, 4, 5);
                var today = DateTime.Today;
                int age = GetAge(birthday, today);
                Console.WriteLine(age);
            }

            // List 8-27
            public static int GetAge(DateTime birthday, DateTime targetDay)
            {
                var age = targetDay.Year - birthday.Year;
                if (targetDay < birthday.AddYears(age))
                {
                    age--;
                }
                return age;
            }

            [ListNo("List 8-28 지정한 날이 몇 주째에 있는지 계산")]
            public void SampleNthWeek()
            {
                var date = DateTime.Today;
                var nth = NthWeek(date);
                Console.WriteLine("{0}주째", nth);
            }

            // List 8-28
            public static int NthWeek(DateTime date)
            {
                var firstDay = new DateTime(date.Year, date.Month, 1);
                var firstDayOfWeek = (int)(firstDay.DayOfWeek);
                return (date.Day + firstDayOfWeek - 1) / 7 + 1;
            }

            [ListNo("List 8-29 지정한 달의 n번째 X요일을 구한다.")]
            public void SampleDayOfNthWeek()
            {
                DateTime day = DayOfNthWeek(2016, 9, DayOfWeek.Sunday, 3);
                Console.WriteLine(day);
                DateTime day1 = DayOfNthWeek(2017, 1, DayOfWeek.Sunday, 5);
                Console.WriteLine(day1);
                // 해당 날이 존재하지 않으면 예외가 발생한다
                // DateTime day2 = DayOfNthWeek(2017, 1, DayOfWeek.Wednesday, 5);
                // Console.WriteLine(day2);
            }

            // List 8-29
            public static DateTime DayOfNthWeek(int year, int month, DayOfWeek dayOfWeek, int nth)
            {
                // LINQ를 사용해서 첫 번째 X요일이 몇일인지를 구한다
                var firstDay = Enumerable.Range(1, 7)
                                         .Select(d => new DateTime(year, month, d))
                                         .First(d => d.DayOfWeek == dayOfWeek)
                                         .Day;
                // 첫 번째 X요일의 날짜에 7의 배수를 더하면 n번째 X요일을 구할 수 있다
                var day = firstDay + (nth - 1) * 7;
                return new DateTime(year, month, day);
            }


            [ListNo("List 8-30 지정한 달의 n번째 X요일을 구한다.2")]
            public void SampleDayOfNthWeek2()
            {
                DateTime day = DayOfNthWeek2(2016, 9, DayOfWeek.Sunday, 3);
                Console.WriteLine(day);
                DateTime day1 = DayOfNthWeek(2017, 1, DayOfWeek.Sunday, 5);
                Console.WriteLine(day1);
            }

            // List 8-30
            public static DateTime DayOfNthWeek2(int year, int month, DayOfWeek dayOfWeek, int nth)
            {
                // 해당 월의 1일이 무슨 요일인지를 구한다
                var firstDayOfWeek = (int)(new DateTime(year, month, 1)).DayOfWeek;
                // 이렇게 구한 firstDayOfWeek를 사용해서 첫 번째 X요일의 날짜를 구한다
                var firstDay = 1 + ((int)dayOfWeek - firstDayOfWeek);
                // 0보다 작으면 7을 더해서 첫 번째 X요일의 날짜를 알 수 있다
                if (firstDay <= 0)
                    firstDay += 7;
                // 첫 번째 X요일의 날짜에 7의 배수를 더하면 n번째 X요일을 구할 수 있다
                var day = firstDay + (nth - 1) * 7;
                return new DateTime(year, month, day);
            }
        }

    }
}
