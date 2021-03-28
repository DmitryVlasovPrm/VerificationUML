﻿using ActivityDiagramVer;
using ActivityDiagramVer.parser;
using ActivityDiagramVer.result;
using ActivityDiagramVer.verification.lexical;
using ActivityDiagramVer.verification.syntax;
using System.Linq;

namespace Verification.ad_ver
{
    internal class ADVerifier
    {
        public static void Verify(Diagram diagram)
        {
            ADNodesList adNodesList = new ADNodesList();
            XmiParser parser = new XmiParser(adNodesList);
            ADMistakeFactory.diagram = diagram;

            parser.Parse(diagram);
            adNodesList.connect();
            // adNodesList.print();


            LexicalAnalizator lexicalAnalizator = new LexicalAnalizator();
            lexicalAnalizator.setDiagramElements(adNodesList);
            lexicalAnalizator.check();

            SyntaxAnalizator syntaxAnalizator = new SyntaxAnalizator();
            syntaxAnalizator.setDiagramElements(adNodesList);
            syntaxAnalizator.check();


            if (!diagram.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL))
            {
                PetriNet petriNet = new PetriNet();
                petriNet.petriCheck(adNodesList);
            }
        }
    }
}
