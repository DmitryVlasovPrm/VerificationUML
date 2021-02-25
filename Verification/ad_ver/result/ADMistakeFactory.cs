using ActivityDiagramVer.entities;
using ActivityDiagramVer.verification;
using System;
using System.Collections.Generic;
using System.Text;
using Verification;
using Verification.type_definer;

namespace ActivityDiagramVer.result
{
    public class ADMistakeFactory
    {
        private static int noCoordinates = -1;
        public static Diagram diagram;

        /**
     * Создание ошибки, содержащей элемент диаграммы
     * @param level
     * @param mistake
     * @param element
     */
        
        public static void createMistake(Level level, String mistake, ADNodesList.ADNode element)
        {
            if (diagram == null) return;
            var tmp = (DiagramElement)element.getValue();
            string descr = tmp is DecisionNode ? ((DecisionNode)tmp).getQuestion() : tmp.getDescription();
            diagram.Mistakes.Add(new Mistake(EDiagramTypes.AD, (int)level, tmp.getType() + " '" + descr + "': " + mistake, tmp.X, tmp.Y, tmp.Width, tmp.Height));
            
        }

        /**
         * Созадние ошибки, не содержащей ссылки не на какой элемент
         * @param level
         * @param mistake
         */
        public static void createMistake(Level level, String mistake)
        {
            if (diagram == null) return;
            diagram.Mistakes.Add(new Mistake(EDiagramTypes.AD, (int)level, mistake, noCoordinates, noCoordinates, noCoordinates, noCoordinates));
        }

        /**
         * Ошибки для переходов и для дорожек
         * @param level
         * @param mistake
         * @param element
         */
        public static void createMistake(Level level, String mistake, BaseNode element)
        {
            if (diagram == null) return;
            if (element is Swimlane)
                mistake = "Дорожка участника '"+((Swimlane)element).getName() + "': " + mistake;
            if (element is ControlFlow)
                mistake = "Переход '" + ((ControlFlow)element).getText() + "': " + mistake;
            diagram.Mistakes.Add(new Mistake(EDiagramTypes.AD, (int)level, mistake, element.X, element.Y, element.Width, element.Height));
        }
    }
}
