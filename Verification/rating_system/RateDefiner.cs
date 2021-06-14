using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification.rating_system {
    class RateDefiner {
        private static double minGrade = 0;
        private static double maxGrade = 0;
        private static string minGradeMsg = "Балл меньше проходного";


        // ошибка-снижаемый балл
        private static IDictionary<ALL_MISTAKES, double> gradesAD = new Dictionary<ALL_MISTAKES, double>() {
            { ALL_MISTAKES.MORE_THAN_ONE_INIT, 1.0 },
            { ALL_MISTAKES.NO_FINAL, 1.0 },
            { ALL_MISTAKES.NO_INITIAL, 1.0 },
            { ALL_MISTAKES.NO_ACTIVITIES, 1.0 },
            { ALL_MISTAKES.MORE_THAN_ONE_OUT, 1.0 },
            { ALL_MISTAKES.DO_NOT_HAVE_ALT, 1.0 },
            { ALL_MISTAKES.ONLY_ONE_ALT, 1.0 },
            { ALL_MISTAKES.NO_OUT, 1.0 }, //1
            { ALL_MISTAKES.NO_IN, 1.0 }, //1
            { ALL_MISTAKES.NO_PARTION, 1.0 },
            { ALL_MISTAKES.REPEATED_NAME, 1.0 },
            { ALL_MISTAKES.SAME_TARGET, 1.0 },
            { ALL_MISTAKES.OUT_NOT_IN_ACT, 1.0 },
            { ALL_MISTAKES.NEXT_DECISION, 1.0 },
            { ALL_MISTAKES.HAS_1_IN, 1.0 },

            { ALL_MISTAKES.FORBIDDEN_ELEMENT, 1.0 },//1
            { ALL_MISTAKES.NO_SWIMLANE, 1.0 },//1
            { ALL_MISTAKES.SMALL_LETTER, 1.0 },
            { ALL_MISTAKES.NO_NAME, 1.0 },
            { ALL_MISTAKES.NOT_NOUN, 1.0 },
            { ALL_MISTAKES.END_WITH_QUEST, 1.0 },
            { ALL_MISTAKES.HAVE_NOT_QUEST, 1.0 },
            { ALL_MISTAKES.REPEATED_ALT, 1.0 },
            { ALL_MISTAKES.HAVE_EMPTY_ALT, 1.0 },
            { ALL_MISTAKES.HAVE_MARK, 1.0 },
            { ALL_MISTAKES.STRANGE_SYMBOL, 1.0 },
            { ALL_MISTAKES.EMPTY_SWIMLANE, 1.0 },

            { ALL_MISTAKES.TWO_TOKENS, 1.0 },
            { ALL_MISTAKES.DEAD_ROAD, 1.0 },
            { ALL_MISTAKES.MANY_TOKENS_IN_END, 1.0 },
            { ALL_MISTAKES.COULD_NOT_REACH_FINAL, 1.0 },
            { ALL_MISTAKES.FINAL_COLOR_TOKEN, 1.0 }
        };
        private static IDictionary<ALL_MISTAKES, double> gradesCD = new Dictionary<ALL_MISTAKES, double>() {
            {ALL_MISTAKES.CD_NO_LINK, 1.0},
            {ALL_MISTAKES.CD_NO_CHILDREN, 1.0},
            {ALL_MISTAKES.CD_INCORRECT_NAME, 1.0},
            {ALL_MISTAKES.CD_EMPTY_CLASS, 1.0},
            {ALL_MISTAKES.CD_MUST_BE_ATTRIB, 1.0},
            {ALL_MISTAKES.CD_HAS_OUTPUT_TYPE, 1.0},
            {ALL_MISTAKES.CD_HAS_NOT_OUTPUT_TYPE, 1.0},
            {ALL_MISTAKES.CD_NO_AVAILABLE_LINKS, 1.0},
            {ALL_MISTAKES.CD_RESTRICTION_HAS_NO_BRACKETS, 1.0},
            {ALL_MISTAKES.CD_NO_PACKAGE, 1.0},
            {ALL_MISTAKES.CD_NO_CONTAINER, 1.0},
            {ALL_MISTAKES.CD_LESS_ZERO, 1.0},
            {ALL_MISTAKES.CD_WRONG_RANGE, 1.0},
            {ALL_MISTAKES.CD_DUPLICATE_NAME, 1.0},
            {ALL_MISTAKES.CD_IMPOSSIBLE_ELEMENT, 1.0},
            {ALL_MISTAKES.CD_IMPOSSIBLE_TYPE, 1.0},
            {ALL_MISTAKES.CD_AGGREG_COMPOS_CYCLE, 1.0},
            {ALL_MISTAKES.CD_SETTER_WITHOUT_PARAMS, 1.0},
            {ALL_MISTAKES.CD_GETTER_WITH_PARAMS, 1.0}
        };
        private static IDictionary<ALL_MISTAKES, double> gradesUCD = new Dictionary<ALL_MISTAKES, double>() {
            {ALL_MISTAKES.UCREPEAT, 1.0},
            {ALL_MISTAKES.UCNOUN, 1.0},
            {ALL_MISTAKES.UCNOLINK, 1.0},
            {ALL_MISTAKES.UCNOTEXT, 1.0},
            {ALL_MISTAKES.UCNOBORDER, 1.0},
            {ALL_MISTAKES.UCNONAME, 1.0},
            {ALL_MISTAKES.UCNOTEXTINPRECEDENT, 1.0},
            {ALL_MISTAKES.UCREPETEDNAME, 1.0},
            {ALL_MISTAKES.UCBIGLETTER, 1.0},
            {ALL_MISTAKES.UCASSOSIATION, 1.0},
            {ALL_MISTAKES.UCNOPRECEDENTDOT, 1.0},
            {ALL_MISTAKES.UCONLYONEPRECEDENT, 1.0},
            {ALL_MISTAKES.UCINCLUDE, 1.0},
            {ALL_MISTAKES.UCBEHINDBORDER, 1.0},
            {ALL_MISTAKES.UCNOAVALABELELEMENT, 1.0},
            {ALL_MISTAKES.UCNOCOORDINATE, 1.0},
            {ALL_MISTAKES.UCNOAVALABELELEMENTINSYSTEM, 1.0}
        };

        /// <summary>
        /// Рассчитать балл переданной диаграмме
        /// </summary>
        /// <param name="diagram"></param>
        /// <returns>Полученный балл и сообщение о непрохождении проходного балла или пустую строку</returns>
        public static Tuple<double, string> defineGrade(Diagram diagram, double max, double min) {
            maxGrade = max;
            minGrade = min;
            switch (diagram.EType) {
                case type_definer.EDiagramTypes.UCD: return getGrade(diagram, gradesUCD);
                case type_definer.EDiagramTypes.AD: return getGrade(diagram, gradesAD); ;
                case type_definer.EDiagramTypes.CD: return getGrade(diagram, gradesCD); ;
                default: throw new Exception("Тип диаграммы не определен");
            }
        }

        /// <summary>
        /// Рассчитать балл 
        /// </summary>
        /// <param name="diagram">Диаграмма, для кот необходимо определить балл</param>
        /// <param name="grades">Перечень ошибок и вычитаемых баллов</param>
        /// <returns>Полученный балл и сообщение о непрохождении проходного балла или пустую строку</returns>
        private static Tuple<double,string> getGrade(Diagram diagram, IDictionary<ALL_MISTAKES, double> grades) {
            double curGrade = maxGrade;
            foreach (var mistake in diagram.Mistakes) {
                curGrade -= grades.ContainsKey(mistake.type)? grades[mistake.type]:0;           //TODO(сменить поле TEXT)
                if (curGrade < minGrade)
                    return new Tuple<double, string>(curGrade, minGradeMsg);
            }
            return new Tuple<double, string>(curGrade, "");            
        }
    }

}
