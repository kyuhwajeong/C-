using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section01 {
    class Program {
        // List 17-3
        static void Main(string[] args) {
            var greetings = new List<GreetingBase>() {
               new GreetingMorning(),
               new GreetingAfternoon(),
               new GreetingEvening(),
            };
            Console.WriteLine("상속을 사용해 다형성을 구현(추상클래스)");
            foreach (var obj in greetings) {
                string msg = obj.GetMessage();
                Console.WriteLine(msg);
            }
            Console.ReadLine();
        }
    }
}
