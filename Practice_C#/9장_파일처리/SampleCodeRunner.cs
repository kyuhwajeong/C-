using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gushwell.CsBook {
    // プロジェクトに定義されたメソッドを順に呼び出していくクラス。
    // 各プロジェクトから参照追加して利用する。
    // 本書では説明していないリフレクションおよび属性という機能を使っている。
    // 詳しく知りたい場合には、リフレクションおよび属性について書かれた資料を参照してほしい。
    public class SampleCodeRunner {
        public static void Run() {
            var asm = Assembly.GetEntryAssembly();
            var types = asm.GetTypes()
                           .OrderBy(x => x.Name);
            foreach (var type in types) {
                var attr = type.GetCustomAttribute(typeof(SampleCodeAttribute)) as SampleCodeAttribute;
                if (attr == null)
                    continue;
                Console.WriteLine("\n{0}",attr.Chapter);
                var obj = Activator.CreateInstance(type);
                CallMethods(obj);
            }
            Console.ReadLine();

        }

        // obj의 인스턴스 메서드 중에 public void 메서드(인수 없음)를 차례로 호출한다
        public static void CallMethods(Object obj)
        {
            var type = obj.GetType();

            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var mi in methods)
            {
                if (mi.Name == "Finalize" || mi.Name == "MemberwiseClone" ||
                    mi.Name.StartsWith("set_") || mi.Name.StartsWith("get_"))
                    continue;
                if (mi.GetParameters().Length > 0 || mi.ReturnParameter.ParameterType.Name != "Void")
                    continue;
                var listNoAttr = mi.GetCustomAttribute(typeof(ListNoAttribute)) as ListNoAttribute;
                var listNo = listNoAttr.ListNumber;

                if (listNo == null)
                    Console.WriteLine("\n- {0} method",mi.Name);
                else
                    Console.WriteLine("\n- {0} method ({1})",mi.Name, listNo);
                //var listNo = listNoAttr?.ListNumber;

                //if (listNo == null)
                //    Console.WriteLine($"\n- {mi.Name} method");
                //else
                //    Console.WriteLine($"\n- {mi.Name} method ({listNo})");
                mi.Invoke(obj, new object[] { });
                Thread.Sleep(10);
            }
        }
    }

    // メソッドに付加する属性。リスト番号を指定する。
    [AttributeUsage(AttributeTargets.Method)]
    public class ListNoAttribute : Attribute {
        public string ListNumber { get; set; }

        public ListNoAttribute(string listNumber) {
            ListNumber = listNumber;
        }
    }

    // public void 메서드를 실행하기 위한 마커 속
    // 실행하려는 클래스에 이 속성을 부가한다
    [AttributeUsage(AttributeTargets.Class)]
    public class SampleCodeAttribute : Attribute
    {
        public string Chapter { get; private set; }

        public SampleCodeAttribute(string chapter)
        {
            Chapter = chapter;
        }
    }
}
