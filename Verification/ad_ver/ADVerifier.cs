﻿using ActivityDiagramVer;
using ActivityDiagramVer.parser;
using ActivityDiagramVer.result;
using ActivityDiagramVer.verification.lexical;
using ActivityDiagramVer.verification.syntax;
using System.Linq;

namespace Verification.ad_ver
{
    /// <summary>
    /// Точка входа для модуля верификации AD
    /// </summary>
    internal class ADVerifier
    {
        public static void Verify(Diagram diagram)
        {
            ADNodesList adNodesList = new ADNodesList();
            XmiParser parser = new XmiParser(adNodesList);
            ADMistakeFactory.diagram = diagram;

            parser.Parse(diagram);
            if (!diagram.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL)) {
                adNodesList.connect();
            } else return;
            // adNodesList.print();

            ADModelVerifier syntaxAnalizator = new ADModelVerifier(new LexicalAnalizator());
            syntaxAnalizator.setDiagramElements(adNodesList);
            syntaxAnalizator.check();


            if (!diagram.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL))
            {
                GraphVerifier petriNet = new GraphVerifier();
                petriNet.check(adNodesList);
            }
        }
    }
}
