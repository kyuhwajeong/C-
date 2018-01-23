using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPAddress01
{
    class Program
    {
        static void Main(string[] args)
        {
            string Address = Console.ReadLine();
            IPAddress IP = IPAddress.Parse(Address);
            Console.WriteLine("ip : {0}", IP.ToString());
        }
    }
}
