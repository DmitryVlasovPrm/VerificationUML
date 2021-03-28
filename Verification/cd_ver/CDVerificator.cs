using Verification.cd_ver.Entities;

namespace Verification.cd_ver
{
    class CDVerificator
    {
        public static Elements AllElements;
        public static Diagram Diagram;

        public CDVerificator(Diagram diagram)
		{
            AllElements = new Elements();
            Diagram = diagram;
        }

        public void Verify()
		{
            ExtractElements.Extract(Diagram.XmlInfo, ref AllElements, ref Diagram);
            // Лексический анализ
            Analysis.LexicalAnalysis(AllElements, ref Diagram);
            // Синтаксический анализ
            Analysis.SyntacticAnalysis(AllElements, ref Diagram);
            // Семантический анализ
            Analysis.SemanticAnalysis(AllElements, ref Diagram);
        }
    }
}