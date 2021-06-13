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
        //private static IDictionary<ALL_MISTAKES, double> gradesAD = new Dictionary<ALL_MISTAKES, double>() {
        //    { ALL_MISTAKES.MORE_THAN_ONE_INIT, 1.0 },
        //    { ALL_MISTAKES.NO_FINAL, 1.0 },
        //    { ALL_MISTAKES.NO_INITIAL, 1.0 },
        //    { ALL_MISTAKES.NO_ACTIVITIES, 1.0 },
        //    { ALL_MISTAKES.MORE_THAN_ONE_OUT, 1.0 },
        //    { ALL_MISTAKES.DO_NOT_HAVE_ALT, 1.0 },
        //    { ALL_MISTAKES.ONLY_ONE_ALT, 1.0 },
        //    { ALL_MISTAKES.NO_OUT, 1.0 }, //1
        //    { ALL_MISTAKES.NO_IN, 1.0 }, //1
        //    { ALL_MISTAKES.NO_PARTION, 1.0 },
        //    { ALL_MISTAKES.REPEATED_NAME, 1.0 },
        //    { ALL_MISTAKES.SAME_TARGET, 1.0 },
        //    { ALL_MISTAKES.OUT_NOT_IN_ACT, 1.0 },
        //    { ALL_MISTAKES.NEXT_DECISION, 1.0 },
        //    { ALL_MISTAKES.HAS_1_IN, 1.0 },

        //    { ALL_MISTAKES.FORBIDDEN_ELEMENT, 1.0 },//1
        //    { ALL_MISTAKES.NO_SWIMLANE, 1.0 },//1
        //    { ALL_MISTAKES.SMALL_LETTER, 1.0 },
        //    { ALL_MISTAKES.NO_NAME, 1.0 },
        //    { ALL_MISTAKES.NOT_NOUN, 1.0 },
        //    { ALL_MISTAKES.END_WITH_QUEST, 1.0 },
        //    { ALL_MISTAKES.HAVE_NOT_QUEST, 1.0 },
        //    { ALL_MISTAKES.REPEATED_ALT, 1.0 },
        //    { ALL_MISTAKES.HAVE_EMPTY_ALT, 1.0 },
        //    { ALL_MISTAKES.HAVE_MARK, 1.0 },
        //    { ALL_MISTAKES.STRANGE_SYMBOL, 1.0 },
        //    { ALL_MISTAKES.EMPTY_SWIMLANE, 1.0 },

        //    { ALL_MISTAKES.TWO_TOKENS, 1.0 },
        //    { ALL_MISTAKES.DEAD_ROAD, 1.0 },
        //    { ALL_MISTAKES.MANY_TOKENS_IN_END, 1.0 },
        //    { ALL_MISTAKES.COULD_NOT_REACH_FINAL, 1.0 },
        //    { ALL_MISTAKES.FINAL_COLOR_TOKEN, 1.0 }
        //};
        //private static IDictionary<ALL_MISTAKES, double> gradesCD = new Dictionary<ALL_MISTAKES, double>() {
        //    {ALL_MISTAKES.CDNOLINK, 1.0},
        //    {ALL_MISTAKES.CDNOCHILDREN, 1.0},
        //    {ALL_MISTAKES.CDSMALLLETTER, 1.0},
        //    {ALL_MISTAKES.CDGETBLANKS, 1.0},
        //    {ALL_MISTAKES.CDMUSTBEOPER, 1.0},
        //    {ALL_MISTAKES.CDBIGLETTER, 1.0},
        //    {ALL_MISTAKES.CDGETBLANKS2, 1.0},
        //    {ALL_MISTAKES.CDCONSTRUCTORHASSMALLLETTER, 1.0},
        //    {ALL_MISTAKES.CDDESTRUCTORHASSMALLLETTER, 1.0},
        //    {ALL_MISTAKES.CDOPERSTARTWITHBIGLETTER, 1.0},
        //    {ALL_MISTAKES.CDOPERHASBLANKS, 1.0},
        //    {ALL_MISTAKES.CDHASOUTPUTTYPE, 1.0},
        //    {ALL_MISTAKES.CDPOINTOUTPUTOPERATIONTYPE, 1.0},
        //    {ALL_MISTAKES.CDNOAVAILABLELINKS, 1.0},
        //    {ALL_MISTAKES.CDENUMSTARTWITHSMALLLETTER, 1.0},
        //    {ALL_MISTAKES.CDENUMHASBLANKS, 1.0},
        //    {ALL_MISTAKES.CDRESTRICTIONHASNOTBRANKETS, 1.0},
        //    {ALL_MISTAKES.CDNOPACKAGE, 1.0},
        //    {ALL_MISTAKES.CDNOCONTAINER, 1.0},
        //    {ALL_MISTAKES.CDLESSZERO, 1.0},
        //    {ALL_MISTAKES.CDWRONGDIAPOSON, 1.0},
        //    {ALL_MISTAKES.CDHASNAME, 1.0},
        //    {ALL_MISTAKES.CDENUMHASNAME, 1.0},
        //    {ALL_MISTAKES.CDIMPROPRATETYPE, 1.0}
        //};
        //private static IDictionary<ALL_MISTAKES, double> gradesUCD = new Dictionary<ALL_MISTAKES, double>() {
        //    {ALL_MISTAKES.UCREPEAT, 1.0},
        //    {ALL_MISTAKES.UCNOUN, 1.0},
        //    {ALL_MISTAKES.UCNOLINK, 1.0},
        //    {ALL_MISTAKES.UCNOTEXT, 1.0},
        //    {ALL_MISTAKES.UCNOBORDER, 1.0},
        //    {ALL_MISTAKES.UCNONAME, 1.0},
        //    {ALL_MISTAKES.UCNOTEXTINPRECEDENT, 1.0},
        //    {ALL_MISTAKES.UCREPETEDNAME, 1.0},
        //    {ALL_MISTAKES.UCBIGLETTER, 1.0},
        //    {ALL_MISTAKES.UCASSOSIATION, 1.0},
        //    {ALL_MISTAKES.UCNOPRECEDENTDOT, 1.0},
        //    {ALL_MISTAKES.UCONLYONEPRECEDENT, 1.0},
        //    {ALL_MISTAKES.UCINCLUDE, 1.0},
        //    {ALL_MISTAKES.UCBEHINDBORDER, 1.0},
        //    {ALL_MISTAKES.UCNOAVALABELELEMENT, 1.0},
        //    {ALL_MISTAKES.UCNOCOORDINATE, 1.0},
        //    {ALL_MISTAKES.UCNOAVALABELELEMENTINSYSTEM, 1.0}
        //};

        /// <summary>
        /// Рассчитать балл переданной диаграмме
        /// </summary>
        /// <param name="diagram"></param>
        /// <returns>Полученный балл и сообщение о непрохождении проходного балла или пустую строку</returns>
        public static Tuple<double, string> defineGrade(Diagram diagram, double max, double min) {
            maxGrade = max;
            minGrade = min;
            return getGrade(diagram, settings.MistakeModel.mistakes);
        }

        /// <summary>
        /// Рассчитать балл 
        /// </summary>
        /// <param name="diagram">Диаграмма, для кот необходимо определить балл</param>
        /// <param name="grades">Перечень ошибок и вычитаемых баллов</param>
        /// <returns>Полученный балл и сообщение о непрохождении проходного балла или пустую строку</returns>
        private static Tuple<double,string> getGrade(Diagram diagram, IDictionary<ALL_MISTAKES, Tuple<double, string>> grades) {
            double curGrade = maxGrade;
            foreach (var mistake in diagram.Mistakes) {
                curGrade -= grades.ContainsKey(mistake.type)? grades[mistake.type].Item1:0;           //TODO(сменить поле TEXT)
                if (curGrade < minGrade)
                    return new Tuple<double, string>(curGrade, minGradeMsg);
            }
            return new Tuple<double, string>(curGrade, "");            
        }
    }

}
