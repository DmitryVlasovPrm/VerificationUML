using ActivityDiagramVer;
using ActivityDiagramVer.parser;
using ActivityDiagramVer.result;
using ActivityDiagramVer.verification.lexical;
using ActivityDiagramVer.verification.syntax;
using System.Linq;
using Verification.ad_ver.verification;

namespace Verification.ad_ver {
    /// <summary>
    /// Точка входа для модуля верификации AD
    /// </summary>
    internal class ADVerifier {
        public static void Verify(Diagram diagram) {
            ADNodesList adNodesList = new ADNodesList();
            XmiParser parser = new XmiParser(adNodesList);
            ADMistakeFactory.diagram = diagram;
            changeMistakeSeriousness(ActivityDiagramVer.verification.Level.FATAL);

            bool hasJoinOrFork = false;
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
        }
        private static void changeMistakeSeriousness(ActivityDiagramVer.verification.Level level) {
            System.Console.WriteLine("has fork/join");
            MistakesSeriousness.mistakes[MISTAKES.NO_FINAL] =
            MistakesSeriousness.mistakes[MISTAKES.NO_INITIAL] =
            MistakesSeriousness.mistakes[MISTAKES.NO_ACTIVITIES] =
            MistakesSeriousness.mistakes[MISTAKES.MORE_THAN_ONE_OUT] =
            MistakesSeriousness.mistakes[MISTAKES.DO_NOT_HAVE_ALT] =
            MistakesSeriousness.mistakes[MISTAKES.OUT_NOT_IN_ACT] = level;
        }
    }

}
