using Gushwell.CsBook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Section02 {
    class Program {
        static void Main(string[] args) {
            SampleCodeRunner.Run();
        }
    }

    [SampleCode("Chapter 14")]
    class SampleCode  {

        [ListNo("List 14-6 어셈블리 버전을 구한다")]  
        public void AssemblyVersion() {   
            var asm = Assembly.GetExecutingAssembly();  // <메이저 버전>.<마이너 버전>.<빌드 번호>.<리비전 번호>
            var ver = asm.GetName().Version;
            Console.WriteLine("{0}.{1}.{2}.{3}",
               ver.Major, ver.Minor, ver.Build, ver.Revision);
        }

        [ListNo("List 14-7 파일 버전을 구한다")]
        public void FileVersion() {
            var location = Assembly.GetExecutingAssembly().Location;
            var ver = FileVersionInfo.GetVersionInfo(location);
            Console.WriteLine("{0} {1} {2} {3}",
                              ver.FileMajorPart, ver.FileMinorPart, ver.FileBuildPart, ver.FilePrivatePart);
        }


    }
}
