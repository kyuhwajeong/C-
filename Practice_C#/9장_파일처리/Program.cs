using Gushwell.CsBook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _9장_파일처리
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleCodeRunner.Run();
        }
    }

    [SampleCode("Chapter 9")]
    class SampleCode
    {
        // 서식 전체를 쓰면 @"C:\Example\Greeting.txt"; 이지만
        // 간단히 실행할 수 있도록 @"Greeting.txt" 로 썼습니다.

        [ListNo("List 9-1 텍스트 파일을 한 행씩 읽어 들인다.")]
        public void ReadLineSample()
        {
            var filePath = @"Greeting.txt";
            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        Console.WriteLine(line);
                    }
                }
            }
        }

        [ListNo("List 9-2 ReadLine메서드의 반환값으로 반복할지 여부를 판단하는 예(비추천)")]
        public void ReadLineSample2()
        {
            var filePath = @"Greeting.txt";
            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
        }

        [ListNo("List 9-3 try-finally를 이용한 파일 후처리(비추천)")]
        public void ReadLineSample3()
        {
            string filePath = @"Greeting.txt";
            StreamReader reader = new StreamReader(filePath, Encoding.UTF8);
            try
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    Console.WriteLine(line);
                }
            }
            finally
            {
                reader.Dispose();
            }
        }


        [ListNo("List 9-4 텍스트 파일을 한꺼번에 읽어 들인다.")]
        public void ReadAllLinesSample()  // 거대한 텍스트 파일이라면 끝까지 읽는 데 처리 지연이 발생하고 메모리를 압박함으로 주의
        {
            var filePath = @"Greeting.txt";
            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        [ListNo("List 9-5 텍스트 파일을 IEnumerable<string>으로 취급")]
        public void ReadLinesSample()
        {
            var filePath = @"sample.txt";
            var lines = File.ReadLines(filePath, Encoding.UTF8);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        [ListNo("List 9-6 첫 n행을 읽는다.")]
        public void ReadLinesTakeSample()
        {
            var filePath = @"sample.txt";
            var lines = File.ReadLines(filePath, Encoding.UTF8)
                            .Take(10)
                            .ToArray();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        [ListNo("List 9-7 조건에 일치하는 행의 개수를 센다.")]
        public void ReadLinesCountSample()
        {
            var filePath = @"sample.txt";
            var count = File.ReadLines(filePath, Encoding.UTF8)
                            .Count(s => s.Contains("C#"));
            Console.WriteLine(count);
        }


        [ListNo("List 9-8 조건에 일치한 행만 읽어 들인다.")]
        public void ReadLinesWhereSample()  //빈 문자열이나 공백인 행 이외의 행을 읽음
        {
            var filePath = @"sample.txt";
            var lines = File.ReadLines(filePath, Encoding.UTF8)
                            .Where(s => !String.IsNullOrWhiteSpace(s))
                            .ToArray();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        [ListNo("List 9-9 조건에 일치하는 행이 존재하는지 여부를 조사")]
        public void ReadLinesAnySample() // 숫자로만 구성된 행이 존재하는지 여부 조사
        {
            var filePath = @"sample.txt";
            var exists = File.ReadLines(filePath, Encoding.UTF8)
                             .Where(s => !String.IsNullOrEmpty(s))
                             .Any(s => s.All(c => Char.IsDigit(c)));

            Console.WriteLine(exists);
        }

        [ListNo("List 9-10 중복된 행을 제외하고 나열한다")]
        public void ReadLinesDistinctSample()
        {
            var filePath = @"sample2.txt";
            var lines = File.ReadLines(filePath, Encoding.UTF8)
                            .Distinct()
                            .OrderBy(s => s.Length)
                            .ToArray();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        [ListNo("List 9-11 행마다 어떤 변환 처리를 수행")]
        public void ReadLinesSelectSample()
        {
            var filePath = @"sample.txt";
            var lines = File.ReadLines(filePath)
                            .Select((s, ix) => String.Format("{0,4}: {1}", ix + 1, s))
                            .ToArray();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        [ListNo("List 9-12 텍스트 파일에 한 행씩 문자열을 출력")]
        public void WriteTextFile()
        {
            if (!Directory.Exists(@"C:\Example"))
            {
                Console.WriteLine("실행하려면 C:\\Example 폴더가 존재해야 합니다.");
                Directory.CreateDirectory(@"C:\Example\");
                return;
            }

            var filePath = @"C:\Example\고향의봄.txt";
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("나의 살던 고향은");
                writer.WriteLine("꽃피는 산골");
                writer.WriteLine("복숭아꽃 살구꽃");
                writer.WriteLine("아기 진달래");
            }
            DisplayLines(@"C:\Example\고향의봄.txt");
        }

        [ListNo("List 9-13 기존 텍스트 파일 끝에 행을 추가")]
        public void AppendTextFile()
        {
            var lines = new[] { "====", "울긋불긋 꽃대궐", "차리인 동네", };
            var filePath = @"C:\Example\고향의봄.txt";
            using (var writer = new StreamWriter(filePath, append: true))
            {
                foreach (var line in lines)
                    writer.WriteLine(line);
            }
            DisplayLines(@"C:\Example\고향의봄.txt");
        }

        [ListNo("List 9-14 문자열 배열을 한번에 파일에 출력")]
        public void WriteAllLinesSample()
        {
            var lines = new[] { "Seoul", "New Delhi", "Bangkok", "London", "Paris", };
            var filePath = @"C:\Example\Cities.txt";
            File.WriteAllLines(filePath, lines);

            DisplayLines(filePath);
        }

        [ListNo("List 9-15 LINQ 쿼리의 결과를 파일에 출력")]
        public void WriteResultOFQuerySample()
        {
            var names = new List<string> {
                "Seoul", "New Delhi", "Bangkok", "London", "Paris", "Berlin", "Canberra", "Hong Kong",
            };
            var filePath = @"C:\Example\Cities.txt";
            File.WriteAllLines(filePath, names.Where(s => s.Length > 5));

            DisplayLines(filePath);
        }


        [ListNo("List 9-16 파일의 첫머리에 행을 삽입")]
        public void InsertLines()
        {
            var originalFilePath = @"C:\Example\고향의봄.txt";
            var filePath = @"C:\Example\고향의봄2.txt";
            File.Copy(originalFilePath, filePath, overwrite: true);

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                using (var reader = new StreamReader(stream))
                using (var writer = new StreamWriter(stream))
                {
                    string texts = reader.ReadToEnd();
                    stream.Position = 0;
                    writer.WriteLine("삽입할 새 행1");
                    writer.WriteLine("삽입할 새 행2");
                    writer.Write(texts);
                }
            }
            DisplayLines(filePath);
        }

        [ListNo("List 9-17 파일의 첫머리에 행을 삽입(비추천)")]
        public void InsertLines2()
        {
            var originalFilePath = @"C:\Example\고향의봄.txt";
            var filePath = @"C:\Example\고향의봄2.txt";
            File.Copy(originalFilePath, filePath, overwrite: true);

            string texts = "";
            // 파일을 모두 읽어들인다
            using (var reader = new StreamReader(filePath))
            {
                texts = reader.ReadToEnd();
            }
            // 일단 닫는다

            // 파일을 다시 열어서 출력 처리를 실행한다
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("삽입할 새 행1");
                writer.WriteLine("삽입할 새 행2");
                writer.Write(texts);
            }
            DisplayLines(filePath);
        }

        private static void DisplayLines(string filePath)
        {
            var xlines = File.ReadAllLines(filePath, Encoding.UTF8);
            foreach (var line in xlines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        //------
        //public void Start()
        //{
        //    Directory.CreateDirectory(@"C:\Example\src");
        //    Directory.CreateDirectory(@"C:\Example\dest");

        //    if (!File.Exists(@"C:\Example\Greeting.txt"))
        //    {
        //        File.WriteAllText(@"C:\Example\Greeting.txt", "Sample Sample Sample");
        //    }
        //    if (!File.Exists(@"C:\Example\src\Greeting.txt"))
        //    {
        //        File.WriteAllText(@"C:\Example\src\Greeting.txt", "Sample Sample Sample");
        //    }
        //    if (!File.Exists(@"C:\Example\src\Greeting2.txt"))
        //    {
        //        var fi = File.Create(@"C:\Example\src\Greeting2.txt");
        //        fi.Close();
        //    }
        //    if (!File.Exists(@"C:\Example\src\oldfile.txt"))
        //    {
        //        var fi = File.Create(@"C:\Example\src\oldfile.txt");
        //        fi.Close();
        //    }
        //    if (!File.Exists(@"C:\Example\src\oldfile2.txt"))
        //    {
        //        var fi = File.Create(@"C:\Example\src\oldfile2.txt");
        //        fi.Close();
        //    }
        //    if (!File.Exists(@"C:\Example\source.txt"))
        //    {
        //        var fi = File.Create(@"C:\Example\source.txt");
        //        fi.Close();
        //    }

        //    File.Delete(@"C:\Example\src\newfile.txt");
        //    File.Delete(@"C:\Example\src\newfile2.txt");
        //    File.Delete(@"C:\Example\dest\Greeting.txt");
        //    File.Delete(@"C:\Example\dest\Greeting2.txt");
        //}

        [ListNo("List 9-18 File클래스를 사용해 파일의 존재를 확인")]
        public void ExistsFile()
        {
            if (!File.Exists(@"C:\Example\Greeting.txt"))
            {
                File.WriteAllText(@"C:\Example\Greeting.txt", "Sample Sample Sample");
            }

            if (File.Exists(@"C:\Example\Greeting.txt"))
            {
                Console.WriteLine("이미 존재합니다.");
            }
        }

        [ListNo("List 9-19 FileInfo클래스를 사용해 파일이 존재하는지 확인")]
        public void ExistsFile2()
        {
            var fi = new FileInfo(@"C:\Example\Greeting.txt");
            if (fi.Exists)
                Console.WriteLine("이미 존재합니다.");
        }


        [ListNo("List 9-20 File클래스를 사용해 파일을 삭제")]
        public void DeleteFile()
        {
            File.Delete(@"C:\Example\Greeting.txt");
        }

        [ListNo("List 9-21 FileInfo클래스를 사용해 파일을 삭제")]
        public void DeleteFile2()
        {
            var fi = new FileInfo(@"C:\Example\Greeting.txt");
            fi.Delete();
        }


        [ListNo("List 9-22 File클래스를 사용해 파일을 복사")]
        public void CopyFile() //복사할 곳에 이미 파일이 존재한다면 IOException 예외가 발생
        {
            if (!File.Exists(@"C:\Example\source.txt"))
            {
                var fi = File.Create(@"C:\Example\source.txt");
                fi.Close();
            }

            // 예제의 메서드를 연속으로 실행할 수 있게 하기 위함
            File.Delete(@"C:\Example\target.txt");

            // 여기서부터 실제 코드이다
            File.Copy(@"C:\Example\source.txt", @"C:\Example\target.txt");
        }


        [ListNo("List 9-23 FileInfo클래스를 사용해 파일을 복사")]
        public void CopyFile2() // 두번째 인수가 true이고 복사할 곳에 파일이 존재한다면 파일을 덮어씀. 
        //반환값은 복사되는 쪽 파일의FileInfo객체가 되므로 반환된 객체를 사용해 복사되는 쪽 파일을 계속해서 처리 가능
        {
            var fi = new FileInfo(@"C:\Example\source.txt");
            FileInfo dup = fi.CopyTo(@"C:\Example\target.txt", overwrite: true);
        }


        [ListNo("List 9-24 File클래스를 사용해 파일을 이동")]
        public void MoveFile()
        {
            Directory.CreateDirectory(@"C:\Example\src");
            Directory.CreateDirectory(@"C:\Example\dest");

            if (!File.Exists(@"C:\Example\src\Greeting.txt"))
            {
                File.WriteAllText(@"C:\Example\src\Greeting.txt", "Sample Sample Sample");
            }
            //if (!File.Exists(@"C:\Example\src\Greeting2.txt"))
            //{
            //    var fi = File.Create(@"C:\Example\src\Greeting2.txt");
            //    fi.Close();
            //}
            File.Delete(@"C:\Example\dest\Greeting.txt");
            File.Move(@"C:\Example\src\Greeting.txt", @"C:\Example\dest\Greeting.txt");
        }

        [ListNo("List 9-25 FileInfo클래스를 사용해 파일을 이동")]
        public void MoveFile2()
        {
            if (!File.Exists(@"C:\Example\src\Greeting2.txt"))
            {
                var fix = File.Create(@"C:\Example\src\Greeting2.txt");
                fix.Close();
            }
            File.Delete(@"C:\Example\dest\Greeting2.txt");

            var fi = new FileInfo(@"C:\Example\src\Greeting2.txt");
            fi.MoveTo(@"C:\Example\dest\Greeting2.txt");

        }

        [ListNo("List 9-26 File클래스를 사용해 파일 이름을 수정")]
        public void RenameFile()
        {
            if (!File.Exists(@"C:\Example\src\oldfile.txt"))
            {
                var fi = File.Create(@"C:\Example\src\oldfile.txt");
                fi.Close();
            }
            File.Delete(@"C:\Example\src\newfile.txt");

            File.Move(@"C:\Example\src\oldfile.txt", @"C:\Example\src\newfile.txt");
        }

        [ListNo("List 9-27 FileInfo클래스를 사용해 파일 이름을 수정")]
        public void RenameFile2()
        {
            if (!File.Exists(@"C:\Example\src\oldfile2.txt"))
            {
                var fix = File.Create(@"C:\Example\src\oldfile2.txt");
                fix.Close();
            }

            File.Delete(@"C:\Example\src\newfile2.txt");

            var fi = new FileInfo(@"C:\Example\src\oldfile2.txt");
            fi.MoveTo(@"C:\Example\src\newfile2.txt");
        }

        [ListNo("List 9-28 File클래스를 사용해 파일을 수정한 시간을 구한다")]
        public void GetLastWriteTime()
        {
            // 예제의 메서드를 연속으로 실행할 수 있게 하기 위함
            File.Move(@"C:\Example\dest\Greeting.txt", @"C:\Example\Greeting.txt");

            // 여기서부터 실제 코드이다
            var lastWriteTime = File.GetLastWriteTime(@"C:\Example\Greeting.txt");
            Console.WriteLine(lastWriteTime);
        }

        [ListNo("List 9-29 File클래스를 사용해 파일을 수정한 시간을 설정한다")]
        public void SetLastWriteTime()
        {
            File.SetLastWriteTime(@"C:\Example\Greeting.txt", DateTime.Now);

            var lastWriteTime = File.GetLastWriteTime(@"C:\Example\Greeting.txt");
            Console.WriteLine(lastWriteTime);
        }


        [ListNo("List 9-30 FileInfo클래스를 사용해 파일을 수정한 시간을 구한다")]
        public void GetLastWriteTime2()
        {
            var fi = new FileInfo(@"C:\Example\Greeting.txt");
            DateTime lastWriteTime = fi.LastWriteTime;
            Console.WriteLine(lastWriteTime);
        }

        [ListNo("List 9-31 FileInfo클래스를 사용해 파일을 수정된 시간을 설정한다")]
        public void SetLastWriteTime2()
        {
            var fi = new FileInfo(@"C:\Example\Greeting.txt");
            fi.LastWriteTime = DateTime.Now;

            var lastWriteTime = File.GetLastWriteTime(@"C:\Example\Greeting.txt");
            Console.WriteLine(lastWriteTime);
        }

        [ListNo("List 9-32 FileInfo클래스를 사용해 파일을 생성된 시간을 구한다")]
        public void GetCreationTime()
        {
            var finfo = new FileInfo(@"C:\Example\Greeting.txt");
            DateTime lastCreationTime = finfo.CreationTime;
            Console.WriteLine(lastCreationTime);
        }

        [ListNo("List 9-33 FileInfo클래스를 사용해 파일의 크기를 구한다")]
        public void GetFileSize()
        {
            var fi = new FileInfo(@"C:\Example\Greeting.txt");
            long size = fi.Length;
            Console.WriteLine(size);
        }

        //------

        [ListNo("List 9-34 디렉터리가 존재하는지 여부를 조사")]
        public void ExistsDirectory()
        {
            if (Directory.Exists(@"C:\Example"))
            {
                Console.WriteLine("존재합니다.");
            }
            else
            {
                Console.WriteLine("존재하지 않습니다.");
            }
        }

        [ListNo("List 9-35 디렉터리를 생성")]
        public void CreateDirectory()
        {
            DirectoryInfo di = Directory.CreateDirectory(@"C:\Example");
        }

        [ListNo("List 9-36 하위 디렉토리까지 작성")]
        public void CreateDirectory2()
        {
            DirectoryInfo di = Directory.CreateDirectory(@"C:\Example\temp");
        }

        [ListNo("List 9-37 디렉토리를 생성")]
        public void CreateDirectory3()
        {
            var di = new DirectoryInfo(@"C:\Example");
            di.Create();
        }

        //public void CreateDirectory4()
        //{
        //    DirectoryInfo di = Directory.CreateDirectory(@"C:\Example");
        //    // DirectoryInfo 오브젝트인 di는 이미 생성했다
        //    DirectoryInfo sdi = di.CreateSubdirectory("temp");
        //}

        [ListNo("List 9-38 디렉터리를 삭제")]
        public void DeleteDirectory()
        {
            Directory.Delete(@"C:\Example\temp");
        }



        [ListNo("List 9-39 하위 디렉토리까지 삭제")]
        public void DeleteDirectoryRecursive()
        {
            Directory.CreateDirectory(@"C:\Example\temp");// add

            Directory.Delete(@"C:\Example\temp", recursive: true);
        }


        [ListNo("List 9-40 디렉토리를 삭제(서브디렉터리까지 DirectoryInfo  사용")]
        public void DeleteDirectoryRecursive2()
        {
            Directory.CreateDirectory(@"C:\Example\temp");// add

            var di = new DirectoryInfo(@"C:\Example\temp");
            // DirectoryInfo 오브젝트인 di는 이미 생성했다
            di.Delete(recursive: true);
        }

        [ListNo("List 9-41 디렉토리를 이동시킨다.")]
        public void MoveDirectory()
        {
            Directory.CreateDirectory(@"C:\Example\temp");// add
            Directory.Delete(@"C:\MyWork", recursive: true);//add

            Directory.Move(@"C:\Example\temp", @"C:\MyWork");
        }

        [ListNo("List 9-42 디렉토리를 이동시킨다(DirectoryInfo사용)")]
        public void MoveDirectory2() // 디렉토리 이름이 변경되는 것으로 이해
        {
            Directory.CreateDirectory(@"C:\Example\temp");// add
            Directory.Delete(@"C:\MyWork", recursive: true);//add

            var di = new DirectoryInfo(@"C:\Example\temp");
            // DirectoryInfo 오브젝트인 di는 이미 생성했다
            di.MoveTo(@"C:\MyWork");
        }

        [ListNo("List 9-43 디렉터리 이름을 수정")]
        public void RenameDirectory()
        {
            Directory.CreateDirectory(@"C:\Example\temp");// add
            Directory.Delete(@"C:\Example\save", recursive: true);//add

            Directory.Move(@"C:\Example\temp", @"C:\Example\save");
        }

        [ListNo("List 9-44 디렉터리 이름 수정(DirectoryInfo)")]
        public void RenameDirectory2()
        {
            Directory.CreateDirectory(@"C:\Example\temp");// add
            Directory.Delete(@"C:\Example\save", recursive: true);//add

            var di = new DirectoryInfo(@"C:\Example\temp");
            // DirectoryInfo 오브젝트인 di는 이미 생성했다
            di.MoveTo(@"C:\Example\save");
        }


        [ListNo("List 9-45 디렉토리 목록을 한번에 구한다.")]
        public void GetDirectories()
        {
            var di = new DirectoryInfo(@"C:\Example");
            DirectoryInfo[] directories = di.GetDirectories();
            foreach (var dinfo in directories)
            {
                Console.WriteLine(dinfo.FullName);
            }
        }

        [ListNo("List 9-46 디렉터리 목록을 한번에 구한다(와일드카드를 지정)")]
        public void GetDirectoriesWithWildCard()
        {
            var di = new DirectoryInfo(@"C:\");
            DirectoryInfo[] directories = di.GetDirectories("P*");
            foreach (var dinfo in directories)
            {
                Console.WriteLine(dinfo.FullName);
            }
        }

        [ListNo("List 9-47 하위 디렉터리도 대상으로 해서 디렉터리 목록을 한번에 구한다")]
        public void GetAllDirectories()
        {
            var di = new DirectoryInfo(@"C:\Example");
            DirectoryInfo[] directories = di.GetDirectories("*", SearchOption.AllDirectories);
            foreach (var item in directories)
            {
                Console.WriteLine(item.FullName);
            }
        }



        [ListNo("List 9-48 디렉터리 목록을 열거한다")]
        public void EnumDirectories()
        {
            var di = new DirectoryInfo(@"C:\Example");
            var directories = di.EnumerateDirectories()
                                .Where(d => d.Name.Length >= 10);
            foreach (var item in directories)
            {
                Console.WriteLine("{0} {1}", item.FullName, item.CreationTime);
            }
        }

        [ListNo("List 9-49 파일 목록을 한번에 구한다")]
        public void GetFiles()
        {
            var di = new DirectoryInfo(@"C:\Windows");
            FileInfo[] files = di.GetFiles();
            foreach (var item in files)
            {
                Console.WriteLine("{0} {1}", item.Name, item.CreationTime);
            }
        }

        [ListNo("List 9-50 파일 목록을 열거한다")]
        public void EnumFiles()
        {
            var di = new DirectoryInfo(@"C:\Example");
            var files = di.EnumerateFiles("*.txt", SearchOption.AllDirectories)
                          .Take(20);
            foreach (var item in files)
            {
                Console.WriteLine("{0} {1}", item.Name, item.CreationTime);
            }
        }


        [ListNo("List 9-51 디렉터리와 파일 목록을 함께 구한다")]
        public void GetFilesAndDirectories()
        {
            var di = new DirectoryInfo(@"C:\Example");
            FileSystemInfo[] fileSystems = di.GetFileSystemInfos();
            foreach (var item in fileSystems)
            {
                if ((item.Attributes & FileAttributes.Directory) == FileAttributes.Directory)

                    Console.WriteLine("디렉터리:{0} {1}", item.Name, item.CreationTime);
                else
                    Console.WriteLine("파일:{0} {1}", item.Name, item.CreationTime);
            }
        }


        [ListNo("List 9-52 디렉터리와 파일 목록을 열거한다")]
        public void EnumFilesAndDirectories()
        {
            var di = new DirectoryInfo(@"C:\Example");
            var fileSystems = di.EnumerateFileSystemInfos();
            foreach (var item in fileSystems)
            {
                if ((item.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    Console.WriteLine("디렉터리:{0} {1}", item.Name, item.CreationTime);
                else
                    Console.WriteLine("파일:{0} {1}", item.Name, item.CreationTime);
            }
        }

        [ListNo("List 9-53 디렉터리와 파일이 변경된 시각을 설정한다")]
        public void ChangeLastWriteTime()
        {
            var di = new DirectoryInfo(@"C:\Example");
            FileSystemInfo[] fileSystems = di.GetFileSystemInfos();
            foreach (var item in fileSystems)
            {
                item.LastWriteTime = new DateTime(2016, 6, 4, 10, 10, 10);
            }
        }

        [ListNo("List 9-memo")]

        public void CurrentDirectorySample()
        {
            // 현재 디렉터리의 경로를 구한다
            var workdir = Directory.GetCurrentDirectory();
            Console.WriteLine(workdir);

            // 현재 디렉터리를 수정한다
            Directory.SetCurrentDirectory(@"C:\\Example");

            // 현재 디렉터리의 경로를 다시 구해서 콘솔에 출력해 확인한다
            var newWorkdir = Directory.GetCurrentDirectory();
            Console.WriteLine(newWorkdir);
        }

        [ListNo("List 9-memo 파일속성")]
        public void FileAttributesSample()
        {
            var fi = new FileInfo(@"C:\\Example\\Greeting.txt");
            if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                Console.WriteLine("ReadOnly 파일입니다.");
            }
            if ((fi.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                Console.WriteLine("System 파일입니다.");
            }
        }

        [ListNo("List 9-54 경로 이름을 구성 요소로 분할한다")]
        public void SplitPath()
        {
            var path = @"C:\Program Files\Microsoft Office\Office16\EXCEL.EXE";
            var directoryName = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(path);
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var pathRoot = Path.GetPathRoot(path);

            Console.WriteLine("DirectoryName : {0}", directoryName);
            Console.WriteLine("FileName : {0}", fileName);
            Console.WriteLine("Extension : {0}", extension);
            Console.WriteLine("FilenameWithoutExtension : {0}", filenameWithoutExtension);
            Console.WriteLine("PathRoot : {0}", pathRoot);
        }

        [ListNo("List 9-55 상대 경로로부터 절대 경로를 구한다")]
        public void GetFullPath()
        {
            var fullPath = Path.GetFullPath(@"..\Greeting.txt");
            Console.WriteLine(fullPath);
        }


        [ListNo("List 9-56 경로를 구성한다")]
        public void BuildPath()
        {
            var dir = @"C:\Example\Temp";
            var fname = "Greeting.txt";
            var path = Path.Combine(dir, fname);
            Console.WriteLine(path);
        }

        [ListNo("List 9-58 여러 개의 요소를 사용해 경로를 구성한다.")]
        public void BuildPath2()
        {
            var topdir = @"C:\Example\";
            var subdir = @"Temp";
            var fname = "Greeting.txt";
            var path = Path.Combine(topdir, subdir, fname);
            Console.WriteLine(path);
        }

        [ListNo("List 9-59 임시 파일을 생성")]
        public void GetTempFileName()
        {
            var tempFileName = Path.GetTempFileName();
            Console.WriteLine(tempFileName);
        }

        [ListNo("List 9-60 임시 폴더의 경로를 구한다")]
        public void GetTempPath()
        {
            var tempPath = Path.GetTempPath();
            Console.WriteLine(tempPath);
        }

        [ListNo("List 9-61 특수 폴더의 경로를 구한다")]
        public void SpecialFolders()
        {
            // "바탕 화면" 폴더를 구한다
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Console.WriteLine(desktopPath);
            // "내 문서" 폴더를 구한다
            var myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Console.WriteLine(myDocumentsPath);
            // 프로그램 파일 폴더를 구한다
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            Console.WriteLine(programFilesPath);
            // Windows 폴더를 구한다
            var windowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            Console.WriteLine(windowsPath);
            // 시스템 폴더를 구한다
            var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            Console.WriteLine(systemPath);
        }
    }
}
