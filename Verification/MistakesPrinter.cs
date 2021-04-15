using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Verification
{
    internal static class MistakesPrinter
    {
        public static void Print(List<Mistake> mistakes)
        {
            Print(mistakes, "Mistakes.txt");
        }

        public static void Print(List<Mistake> mistakes, string path, string diagramName) {
            using (var sw = new StreamWriter(path, true, Encoding.Unicode)) {
                sw.WriteLine("--------------------------");
                sw.WriteLine("-- Диаграмма "+diagramName+": --");
                mistakes
                    .ForEach(mistake => sw.WriteLine(@mistake.Text));
            }
        }
        public static void Print(List<Mistake> mistakes, string path)
        {
            using (var sw = new StreamWriter(path, true, Encoding.Unicode))
            {
                mistakes
                    .ForEach(mistake => sw.WriteLine(mistake.Text));
            }
        }
    }
}
