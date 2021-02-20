using ActivityDiagramVer.entities;
using ActivityDiagramVer.verification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.result
{
    public class MistakeFactory
    {
        /**
     * Создание ошибки, содержащей элемент диаграммы
     * @param level
     * @param mistake
     * @param element
     */
        public static void createMistake(Level level, String mistake, ADNodesList.ADNode element)
        {
            //Mistakes.mistakes.Add(new ElementMistake(mistake, level, element, Mistakes.mistakes.Count));
            var tmp = (DiagramElement)element.getValue();
            string descr = tmp is DecisionNode ? ((DecisionNode)tmp).getQuestion() : tmp.getDescription();
            Mistakes.mistakes.Add(new Mistake(tmp.getType()+" '"+descr+"': "+mistake, level, Mistakes.mistakes.Count, tmp.x, tmp.y));
        }

        /**
         * Созадние ошибки, не содержащей ссылки не на какой элемент
         * @param level
         * @param mistake
         */
        public static void createMistake(Level level, String mistake)
        {
            //Mistakes.mistakes.Add(new GeneralMistake(mistake, level, Mistakes.mistakes.Count));
            Mistakes.mistakes.Add(new Mistake(mistake, level, Mistakes.mistakes.Count));
        }

        /**
         * Ошибки для переходов и для дорожек
         * @param level
         * @param mistake
         * @param element
         */
        public static void createMistake(Level level, String mistake, BaseNode element)
        {
            //Mistakes.mistakes.Add(new NotADNodeMistakes(mistake, level, element, Mistakes.mistakes.Count));
            if (element is Swimlane)
                mistake = "Дорожка участника '"+((Swimlane)element).getName() + "': " + mistake;
            if (element is ControlFlow)
                mistake = "Переход '" + ((ControlFlow)element).getText() + "': " + mistake;
            Mistakes.mistakes.Add(new Mistake(mistake, level, Mistakes.mistakes.Count, element.x, element.y));
        }
    }
}
