﻿using CSharpPhrase.CustomSection;
using Gushwell.CsBook;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14장_응용프로그램_구성파일
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleCodeRunner.Run();
        }

        [SampleCode("Chapter 14")]
        class SampleCode
        {

            [ListNo("List 14-9 appSettings정보를 구한다")]
            public void GetAppSettingsData()
            {
                var enableTraceStr = ConfigurationManager.AppSettings["EnableTrace"];
                var enableTrace = bool.Parse(enableTraceStr);
                var timeoutStr = ConfigurationManager.AppSettings["Timeout"];
                int timeout = int.Parse(timeoutStr);
            }

            [ListNo("List 14-10 appSettings정보를 모두 구한다")]
            public void GetAllAppSettingsData()
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                foreach (var key in appSettings.AllKeys)
                {
                    string value = appSettings[key];
                    Console.WriteLine(value);
                }
            }

            [ListNo("List 14-14 구성 파일을 읽어 들이는 예")]
            public void GetCustomSection()
            {
                var cs = ConfigurationManager.GetSection("myAppSettings") as MyAppSettings;
                var option = cs.TraceOption;
                Console.WriteLine(option.BufferSize);
                Console.WriteLine(option.Enabled);
                Console.WriteLine(option.FilePath);
            }
        }
    }
}
