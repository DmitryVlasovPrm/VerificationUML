using ActivityDiagramVer.entities;
using ActivityDiagramVer.verification;
using Verification;

namespace ActivityDiagramVer.result
{
    public class ADMistakeFactory
    {
        private static readonly int noCoordinates = -1;
        public static Diagram diagram;
        private static (int, int) coordMin = (0, 0);
        private static bool minCoordFound = false;

        /**
     * Создание ошибки, содержащей элемент диаграммы
     */

        public static void createMistake(Level level, string mistake, ADNodesList.ADNode element)
        {
            if (diagram == null) return;
            var tmp = (DiagramElement)element.getValue();
            string descr = tmp is DecisionNode ? ((DecisionNode)tmp).getQuestion() : tmp.getDescription();

            if (!minCoordFound)
            {
                if (diagram.Image != null)
                    coordMin = MinCoordinates.Compute(diagram.Image);
                minCoordFound = true;
            }

            var bbox = new BoundingBox(tmp.X + coordMin.Item1, tmp.Y + coordMin.Item2, tmp.Width, tmp.Height);
            diagram.Mistakes.Add(new Mistake(levelAdapter(level), ElementTypeAdapter.toString(tmp.getType()) + " '" + descr + "': " + mistake, bbox));

        }

        /**
         * Созадние ошибки, не содержащей ссылки не на какой элемент
         */
        public static void createMistake(Level level, string mistake)
        {
            if (diagram == null) return;
            var bbox = new BoundingBox(noCoordinates, noCoordinates, noCoordinates, noCoordinates);
            diagram.Mistakes.Add(new Mistake(levelAdapter(level), mistake, bbox));
        }

        /**
         * Ошибки для переходов и для дорожек
         */
        public static void createMistake(Level level, string mistake, BaseNode element)
        {
            if (diagram == null) return;
            if (element is Swimlane)
                mistake = "Дорожка участника '" + ((Swimlane)element).getName() + "': " + mistake;
            if (element is ControlFlow)
                mistake = "Переход '" + ((ControlFlow)element).getText() + "': " + mistake;

            if (!minCoordFound)
            {
                if (diagram.Image != null)
                    coordMin = MinCoordinates.Compute(diagram.Image);
                minCoordFound = true;
            }
            var bbox = new BoundingBox(element.X + coordMin.Item1, element.Y + coordMin.Item2, element.Width, element.Height);
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
