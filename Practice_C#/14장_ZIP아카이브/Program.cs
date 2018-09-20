using Gushwell.CsBook;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14장_ZIP아카이브
{
    class Program
    {
        // 이번 예제는 코드에서 지정한 파일과 폴더가 없으면 실행할 수 없습니다.
        // 자신이 직접 환경을 만들기 바랍니다.
        static void Main(string[] args)
        {
            // 연속 실행시킬 경우를 대비해서 불필요한 파일을 삭제한다
            if (Directory.Exists(@"C:\Temp\zip"))
                Directory.Delete(@"C:\Temp\zip", recursive: true);
            if (File.Exists(@"c:\Archives\newArchive.zip"))
                File.Delete(@"c:\Archives\newArchive.zip");
            SampleCodeRunner.Run();
        }

        [SampleCode("Chapter 14")]
        class SampleCode
        {

            [ListNo("List 14-21 아카이브에 있는 모든 파일을 추출한다")]
            public void ExtractToDirectory()
            {
                var archiveFile = @"c:\Archives\example.zip";
                var destinationFolder = @"c:\Temp\zip";
                if (!Directory.Exists(destinationFolder))
                {
                    ZipFile.ExtractToDirectory(archiveFile, destinationFolder);
                }
            }

            [ListNo("List 14-22 아카이브에서 파일의 목록을 구한다")]
            public void GetEntries()
            {
                var archiveFile = @"c:\Archives\example.zip";
                using (ZipArchive zip = ZipFile.OpenRead(archiveFile))
                {
                    var entries = zip.Entries;
                    foreach (var entry in entries)
                    {
                        Console.WriteLine(entry.FullName);
                    }
                }
            }

            [ListNo("List 14-23 아카이브에서 임의의 파일을 추출한다")]
            public void ExtractToFile()
            {
                var name = "sample.txt";
                var archiveFile = @"c:\Archives\example.zip";
                using (var zip = ZipFile.OpenRead(archiveFile))
                {
                    // 처음에 발견된 파일을 추출한다
                    var entry = zip.Entries.FirstOrDefault(x => x.Name == name);
                    if (entry != null)
                    {
                        var destPath = Path.Combine(@"c:\Temp\", entry.FullName);
                        Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                        entry.ExtractToFile(destPath, overwrite: true);
                    }
                }
            }

            // 메서드를 연속으로 실행할 수 있게 하기 위해 폴더 이름은 책에 나온 코드와 다릅니다
            [ListNo("List 14-24 디렉토리 안에 있는 파일을 아카이브로 만든다")]
            public void CreateFromDirectory()
            {
                var sourceFolder = @"c:\Temp\zip";
                var archiveFile = @"c:\Archives\newArchive.zip";
                ZipFile.CreateFromDirectory(sourceFolder, archiveFile, CompressionLevel.Fastest, includeBaseDirectory: false);
            }
        }
    }
}
