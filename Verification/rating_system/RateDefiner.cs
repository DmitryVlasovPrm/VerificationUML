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
                curGrade -= grades.ContainsKey(mistake.type)? grades[mistake.type].Item1:0;
                if (curGrade < minGrade)
                    return new Tuple<double, string>(curGrade, minGradeMsg);
            }
            return new Tuple<double, string>(curGrade, "");            
        }
    }

}
