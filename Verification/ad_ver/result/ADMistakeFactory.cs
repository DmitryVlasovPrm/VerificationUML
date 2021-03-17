using ActivityDiagramVer.entities;
using ActivityDiagramVer.verification;
using System;
using Verification;

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
            var bbox = new BoundingBox(tmp.X, tmp.Y, tmp.Width, tmp.Height);
            diagram.Mistakes.Add(new Mistake(levelAdapter(level), ElementTypeAdapter.toString(tmp.getType()) + " '" + descr + "': " + mistake, bbox));

        }

        /**
         * Созадние ошибки, не содержащей ссылки не на какой элемент
         * @param level
         * @param mistake
         */
        public static void createMistake(Level level, String mistake)
        {
            if (diagram == null) return;
            var bbox = new BoundingBox(noCoordinates, noCoordinates, noCoordinates, noCoordinates);
            diagram.Mistakes.Add(new Mistake(levelAdapter(level), mistake, bbox));
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
                mistake = "Дорожка участника '" + ((Swimlane)element).getName() + "': " + mistake;
            if (element is ControlFlow)
                mistake = "Переход '" + ((ControlFlow)element).getText() + "': " + mistake;
            var bbox = new BoundingBox(element.X, element.Y, element.Width, element.Height);
            diagram.Mistakes.Add(new Mistake(levelAdapter(level), mistake, bbox));
        }
        private static int levelAdapter(Level level)
        {
            switch (level)
            {
                case Level.EASY: return MistakesTypes.WARNING;
                case Level.HARD: return MistakesTypes.ERROR;
                default: return MistakesTypes.FATAL;
            }
        }
    }
}
