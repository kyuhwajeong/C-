using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextFileProcessor;

namespace LineCounter {

    // List 17-9
    class Program {
        static void Main(string[] args) {
            TextProcessor.Run<LineCounterProcessor>(args[0]);// .\17장_템플릿메서드_텍스트파일처리.exe .\LineCounterProcessor.cs
        }
    }
}