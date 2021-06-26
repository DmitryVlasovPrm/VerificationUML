using ActivityDiagramVer.entities;
using ActivityDiagramVer.verification;
using Verification;
using Verification.rating_system;

namespace ActivityDiagramVer.result {
    public class ADMistakeFactory {
        private static readonly int noCoordinates = -1;
        public static Diagram diagram;


        /**
     * Создание ошибки, содержащей элемент диаграммы
     */

        public static void createMistake(Level level, string mistake, ADNodesList.ADNode element, ALL_MISTAKES type) {
            if (diagram == null) return;
            var tmp = (DiagramElement)element.getValue();
            string descr = tmp is DecisionNode ? ((DecisionNode)tmp).getQuestion() : tmp.getDescription();

            var bbox = new BoundingBox(tmp.X, tmp.Y, tmp.Width, tmp.Height);
            descr = descr==""?descr:(" '" + descr + "'");
            diagram.Mistakes.Add(new Mistake(levelAdapter(level), ElementTypeAdapter.toString(tmp.getType()) +descr+ ": " + mistake, bbox, type));

        }

        /**
         * Созадние ошибки, не содержащей ссылки не на какой элемент
         */
        public static void createMistake(Level level, string mistake, ALL_MISTAKES type) {
            if (diagram == null) return;
            var bbox = new BoundingBox(noCoordinates, noCoordinates, noCoordinates, noCoordinates);
            diagram.Mistakes.Add(new Mistake(levelAdapter(level), mistake, bbox, type));
        }

        /**
         * Ошибки для переходов и для дорожек
         */
        public static void createMistake(Level level, string mistake, BaseNode element, ALL_MISTAKES type) {
            if (diagram == null) return;
            if (element is Swimlane)
                mistake = "Дорожка участника '" + ((Swimlane)element).getName() + "': " + mistake;
            if (element is ControlFlow)
                mistake = "Переход '" + ((ControlFlow)element).getText() + "': " + mistake;


            var bbox = new BoundingBox(element.X, element.Y, element.Width, element.Height);
            diagram.Mistakes.Add(new Mistake(levelAdapter(level), mistake, bbox, type));
        }
        private static int levelAdapter(Level level) {
            switch (level) {
                case Level.EASY: return MistakesTypes.WARNING;
                case Level.HARD: return MistakesTypes.ERROR;
                default: return MistakesTypes.FATAL;
            }
        }
    }
}
