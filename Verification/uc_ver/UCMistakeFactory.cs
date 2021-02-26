using System;
using System.Collections.Generic;
using System.Text;
using Verification;
using Verification.type_definer;

namespace Verification.uc_ver
{
    public class UCMistakeFactory
    {
        private static int noCoordinates = -1;
        public static Diagram diagram;

        /**
     * Создание ошибки, содержащей элемент диаграммы
     * @param level
     * @param mistake
     * @param element
     */

        //public static void createMistake(ErrorTypes eType, string mistake, ADNodesList.ADNode element)
        //{
        //    if (diagram == null) return;
        //    //Mistakes.mistakes.Add(new ElementMistake(mistake, level, element, Mistakes.mistakes.Count));
        //    var tmp = (DiagramElement)element.getValue();
        //    string descr = tmp is DecisionNode ? ((DecisionNode)tmp).getQuestion() : tmp.getDescription();
        //    // Mistakes.mistakes.Add(new Mistake(tmp.getType()+" '"+descr+"': "+mistake, level, Mistakes.mistakes.Count, tmp.x, tmp.y));
        //    //Mistakes.mistakes.Add(new Mistake(EDiagramTypes.AD, level, tmp.getType() + " '" + descr + "': " + mistake, tmp.X, tmp.Y, tmp.Width, tmp.Height);
        //    var m = new Mistake(EDiagramTypes.UCD, eType, )
        //    diagram.Mistakes.Add(new Mistake(EDiagramTypes.UCD, (int)level, tmp.getType() + " '" + descr + "': " + mistake, tmp.X, tmp.Y, tmp.Width, tmp.Height));

        //}

        /**
         * Созадние ошибки, не содержащей ссылки не на какой элемент
         * @param level
         * @param mistake
         */
        //public static void createMistake(Level level, String mistake)
        //{
        //    if (diagram == null) return;
        //    //Mistakes.mistakes.Add(new GeneralMistake(mistake, level, Mistakes.mistakes.Count));
        //    //Mistakes.mistakes.Add(new Mistake(mistake, level, Mistakes.mistakes.Count));
        //    diagram.Mistakes.Add(new Mistake(EDiagramTypes.AD, (int)level, mistake, noCoordinates, noCoordinates, noCoordinates, noCoordinates));
        //}

        /**
         * Ошибки для переходов и для дорожек
         * @param level
         * @param mistake
         * @param element
         */
        //public static void createMistake(Level level, String mistake, BaseNode element)
        //{
        //    if (diagram == null) return;
        //    //Mistakes.mistakes.Add(new NotADNodeMistakes(mistake, level, element, Mistakes.mistakes.Count));
        //    if (element is Swimlane)
        //        mistake = "Дорожка участника '" + ((Swimlane)element).getName() + "': " + mistake;
        //    if (element is ControlFlow)
        //        mistake = "Переход '" + ((ControlFlow)element).getText() + "': " + mistake;
        //    //Mistakes.mistakes.Add(new Mistake(mistake, level, Mistakes.mistakes.Count, element.x, element.y));
        //    diagram.Mistakes.Add(new Mistake(EDiagramTypes.AD, (int)level, mistake, element.X, element.Y, element.Width, element.Height));
        //}
    }
}
