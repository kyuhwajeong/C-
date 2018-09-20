using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5_18
{
    class Program
    {
        static void Main(string[] args)
        {
            aa();
        }


        void any()
        {
            var target = "C# Programming";
            var isExists = target.Any(c => Char.IsUpper(c));
        }

        public static void split()
        {
            var text = "The quick brown fox jumps over the lazy dog.";
            var words = text.Split(new[] { ' ', '.' });
        }

        public static void createArray()
        {
            var target = "Novelist\t=\t김민중";
            var chars = target.SkipWhile(c => c != '=')
                              .Skip(1)
                              .Where(c => !char.IsWhiteSpace(c))
                              .ToArray();
            var str = new string(chars);
        }

        public static void toDigit()
        {
            int number = 12345;
            var s1 = number.ToString();
            var s2 = number.ToString("#");
            var s3 = number.ToString("0000000");
            var s4 = number.ToString("#,0");
        }

        public static void toDigit2()
        {
            decimal distance = 9876.123m;
            var s1 = distance.ToString();
            var s2 = distance.ToString("#");
            var s3 = distance.ToString("#,0.0");
            var s4 = distance.ToString("#,0.000");
        }

        public static void toDigit3()
        {
            int number = 0;
            var s1 = number.ToString();
            var s2 = number.ToString("#");
            var s3 = number.ToString("0000000");
            var s4 = number.ToString("#,0");

            decimal distance = 0.0m;
            var s5 = distance.ToString();
            var s6 = distance.ToString("#");
            var s7 = distance.ToString("#,0.0");
            var s8 = distance.ToString("#,0.000");
        }

        public static void toDigit4()
        {
            int number = 12345;
            var s1 = string.Format("{0,10}", number);
            var s2 = string.Format("{0,10:#,0}", number);
            var s3 = string.Format("{0,10}", number);
            var s4 = string.Format("{0,10:0.0}", number);
        }

        public static void aa()
        {
            //var article = 12;
            //var clause = 5;
            //var header = String.Format("제{0,2}조{1,2}항}", article, clause);

		var numbers = new List<int> {9,7,5,4,2,0,4,1,0,4};
		bool isAllPositive = numbers.All(n => n>0);

        }
    }
}
