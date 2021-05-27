using ActivityDiagramVer;
using ActivityDiagramVer.parser;
using ActivityDiagramVer.result;
using ActivityDiagramVer.verification.lexical;
using ActivityDiagramVer.verification.syntax;
using System.Diagnostics;
using System.Linq;
using Verification.ad_ver.verification;

namespace Verification.ad_ver {
    /// <summary>
    /// Точка входа для модуля верификации AD
    /// </summary>
    internal class ADVerifier {
        private static double d_count = 0;
        private static long allTime = 0;
        public static void Verify(Diagram diagram) {
            d_count++;
            ADNodesList adNodesList = new ADNodesList();
            XmiParser parser = new XmiParser(adNodesList);
            ADMistakeFactory.diagram = diagram;
            changeMistakeSeriousness(ActivityDiagramVer.verification.Level.FATAL);

            bool hasJoinOrFork = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            parser.Parse(diagram, ref hasJoinOrFork);
            if (hasJoinOrFork)
                changeMistakeSeriousness(ActivityDiagramVer.verification.Level.HARD);
            if (!diagram.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL)) {
                adNodesList.connect();
            } else return;
            // adNodesList.print();

            ADModelVerifier syntaxAnalizator = new ADModelVerifier(new LexicalAnalizator());
            syntaxAnalizator.setDiagramElements(adNodesList);
            syntaxAnalizator.check();

            if (hasJoinOrFork && !diagram.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL)) {
                GraphVerifier petriNet = new GraphVerifier();
                petriNet.check(adNodesList);
            }
            sw.Stop();
            var _time = sw.ElapsedMilliseconds;
            allTime += _time;
            System.Console.WriteLine($"[{d_count}]:{_time}ms");
            System.Console.WriteLine($"[{d_count}]: avg ={(allTime / d_count)}");
        }
        private static void changeMistakeSeriousness(ActivityDiagramVer.verification.Level level) {
            
            MistakesSeriousness.mistakes[MISTAKES.NO_FINAL] =
            MistakesSeriousness.mistakes[MISTAKES.NO_INITIAL] =
            MistakesSeriousness.mistakes[MISTAKES.NO_ACTIVITIES] =
            MistakesSeriousness.mistakes[MISTAKES.MORE_THAN_ONE_OUT] =
            MistakesSeriousness.mistakes[MISTAKES.DO_NOT_HAVE_ALT] =
            MistakesSeriousness.mistakes[MISTAKES.OUT_NOT_IN_ACT] = level;
        }
    }

}
