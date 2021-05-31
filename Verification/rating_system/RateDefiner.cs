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
        private static IDictionary<string, double> gradesAD = new Dictionary<string, double>() {
            { "", 2 },
            {"", 3 }
        };
        private static IDictionary<string, double> gradesCD = new Dictionary<string, double>() {
            { "", 0 }
        };
        private static IDictionary<string, double> gradesUCD = new Dictionary<string, double>() {
            { "", 0 }
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
        private static Tuple<double,string> getGrade(Diagram diagram, IDictionary<string, double> grades) {
            double curGrade = maxGrade;
            foreach (var mistake in diagram.Mistakes) {
                curGrade -= grades[mistake.Text];           //TODO(сменить поле TEXT)
                if (curGrade < minGrade)
                    return new Tuple<double, string>(curGrade, minGradeMsg);
            }
            return new Tuple<double, string>(curGrade, "");            
        }
    }

}
