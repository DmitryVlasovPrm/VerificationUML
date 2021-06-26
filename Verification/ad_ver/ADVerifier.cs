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

            bool hasJoinOrFork = false;
            parser.Parse(diagram, ref hasJoinOrFork);
            // находим предшествующие\последующие элементы для каждого объекта
            if (!diagram.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL)) {
                adNodesList.connect();
            } else return;
            // adNodesList.print();

            // осуществляем проверку без использования графа
            ADModelVerifier verificationWithoutGraph = new ADModelVerifier(new LexicalAnalizator());
            verificationWithoutGraph.setDiagramElements(adNodesList);
            verificationWithoutGraph.check();

            // осуществляем проверку с использованием графа
            if (hasJoinOrFork && !diagram.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL)) {
                GraphVerifier graphVerifier = new GraphVerifier();
                graphVerifier.check(adNodesList);
            }
        }
    }

}
