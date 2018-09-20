using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section0102 {
    class Program {

        // List 17-17
        static void Main(string[] args) {
            var greetings = new List<IGreeting>() {
               new GreetingMorning(),
               new GreetingAfternoon(),
               new GreetingEvening(),
            };
            Console.WriteLine("상속을 사용해 다형성을 구현(인터페이스)");
            foreach (var obj in greetings) {
                string msg = obj.GetMessage();
                Console.WriteLine(msg);
            }
            Console.ReadLine();
        }
    }
}
