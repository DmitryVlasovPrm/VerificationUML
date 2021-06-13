﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verification.rating_system;

namespace Verification.settings {
    [Serializable]
    public class MistakeModel {
        public static IDictionary<ALL_MISTAKES, Tuple<double, String>> mistakes = new Dictionary<ALL_MISTAKES, Tuple<double, String>>() {
                { ALL_MISTAKES.MORE_THAN_ONE_INIT, new Tuple<double, string>(1.0,"dБолее одного начального состояния") },
                { ALL_MISTAKES.NO_FINAL, new Tuple<double, string>(1.0,"Более одного конечного состояния") },
                { ALL_MISTAKES.NO_INITIAL, new Tuple<double, string>(1.0,"Отсутствует начальное состояние") },
                { ALL_MISTAKES.NO_ACTIVITIES, new Tuple<double, string>(1.0,"Отсутствуют активности") },
                { ALL_MISTAKES.MORE_THAN_ONE_OUT, new Tuple<double, string>(1.0,"Элемент имеет более одного ") },
                { ALL_MISTAKES.DO_NOT_HAVE_ALT, new Tuple<double, string>(1.0,"Условный переход не имеет альтернатив") },
                { ALL_MISTAKES.ONLY_ONE_ALT, new Tuple<double, string>(1.0,"Условный переход имеет всего одну альтернативу") },
                { ALL_MISTAKES.NO_OUT, new Tuple<double, string>(1.0,"У элемента (кроме конечного состояния) отсутствует выходящий переход") },
                { ALL_MISTAKES.NO_IN, new Tuple<double, string>(1.0,"У элемента (кроме начального состояния) отсутствует входящий переход	") },
                { ALL_MISTAKES.NO_PARTION, new Tuple<double, string>(1.0,"Элемент не принадлежит никакому участнику") },
                { ALL_MISTAKES.REPEATED_NAME, new Tuple<double, string>(1.0,"Имя участника или альтернатива не уникальны") },
                { ALL_MISTAKES.SAME_TARGET, new Tuple<double, string>(1.0,"Альтернативы ведут в один и тот же элемент") },
                { ALL_MISTAKES.OUT_NOT_IN_ACT, new Tuple<double, string>(1.0,"Переход ведет не в активность, разветвитель или в условный переход ") },
                { ALL_MISTAKES.NEXT_DECISION, new Tuple<double, string>(1.0,"Альтернатива ведет в условный переход") },
                { ALL_MISTAKES.HAS_1_IN, new Tuple<double, string>(1.0,"Разветвитель или слияние имеют всего один входной переход") },

                { ALL_MISTAKES.FORBIDDEN_ELEMENT, new Tuple<double, string>(1.0,"В диаграмме используется недопустимый элемент") },
                { ALL_MISTAKES.NO_SWIMLANE, new Tuple<double, string>(0.0,"Отсутствуют акторы или используется не верный элемент") },
                { ALL_MISTAKES.SMALL_LETTER, new Tuple<double, string>(1.0,"Имя активности или участника начинается с маленькой буквы") },
                { ALL_MISTAKES.NO_NAME, new Tuple<double, string>(1.0,"Актор или активность не имееют подписи") },
                { ALL_MISTAKES.NOT_NOUN, new Tuple<double, string>(1.0,"Первое слово в имени активности, возможно, не является именем существительным") },
                { ALL_MISTAKES.END_WITH_QUEST, new Tuple<double, string>(1.0,"В условии отсутствует знак вопроса") },
                { ALL_MISTAKES.HAVE_NOT_QUEST, new Tuple<double, string>(1.0,"Отсутствует подпись в условии") },
                { ALL_MISTAKES.REPEATED_ALT, new Tuple<double, string>(1.0,"Повторяется альтернатива у одного условного перехода") },
                { ALL_MISTAKES.HAVE_EMPTY_ALT, new Tuple<double, string>(1.0,"Условный переход имеет не подписанную альтернативу") },
                { ALL_MISTAKES.HAVE_MARK, new Tuple<double, string>(1.0,"Переход имеет подпись, не являясь условием или альтернативой") },
                { ALL_MISTAKES.STRANGE_SYMBOL, new Tuple<double, string>(1.0,"В имени участника используется специальный символ") },
                { ALL_MISTAKES.EMPTY_SWIMLANE, new Tuple<double, string>(0.0,"Дорожка участника не содержит элементов") },

                { ALL_MISTAKES.TWO_TOKENS, new Tuple<double, string>(1.0,"Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем") },
                { ALL_MISTAKES.DEAD_ROAD, new Tuple<double, string>(1.0,"Тупик. Невозможно активировать синхронизатор") },
                { ALL_MISTAKES.MANY_TOKENS_IN_END, new Tuple<double, string>(1.0,"Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем") },
                { ALL_MISTAKES.COULD_NOT_REACH_FINAL, new Tuple<double, string>(1.0,"Недостижимое конечное состояние. Возможно имеется синхронизатор, который невозможно активировать") },
                { ALL_MISTAKES.FINAL_COLOR_TOKEN, new Tuple<double, string>(1.0,"Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем") },

                {ALL_MISTAKES.CDNOLINK, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDNOCHILDREN, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDSMALLLETTER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDGETBLANKS, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDMUSTBEOPER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDBIGLETTER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDGETBLANKS2, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDCONSTRUCTORHASSMALLLETTER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDDESTRUCTORHASSMALLLETTER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDOPERSTARTWITHBIGLETTER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDOPERHASBLANKS, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDHASOUTPUTTYPE, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDPOINTOUTPUTOPERATIONTYPE, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDNOAVAILABLELINKS, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDENUMSTARTWITHSMALLLETTER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDENUMHASBLANKS, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDRESTRICTIONHASNOTBRANKETS, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDNOPACKAGE, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDNOCONTAINER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDLESSZERO, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDWRONGDIAPOSON, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDHASNAME, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDENUMHASNAME, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.CDIMPROPRATETYPE, new Tuple<double, string>(1.0,"")},

                {ALL_MISTAKES.UCREPEAT, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOUN, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOLINK, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOTEXT, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOBORDER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNONAME, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOTEXTINPRECEDENT, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCREPETEDNAME, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCBIGLETTER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCASSOSIATION, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOPRECEDENTDOT, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCONLYONEPRECEDENT, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCINCLUDE, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCBEHINDBORDER, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOAVALABELELEMENT, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOCOORDINATE, new Tuple<double, string>(1.0,"")},
                {ALL_MISTAKES.UCNOAVALABELELEMENTINSYSTEM, new Tuple<double, string>(1.0,"")}
            };

        public IDictionary<ALL_MISTAKES, Tuple<double, string>> Mistakes { get => mistakes; set => mistakes = value; }

        public void setValue(string key, string value) {
            var text = mistakes[(ALL_MISTAKES)Enum.Parse(typeof(ALL_MISTAKES), key)].Item2;
            mistakes[(ALL_MISTAKES)Enum.Parse(typeof(ALL_MISTAKES), key)] = new Tuple<double, string>(Double.Parse(value), text);
        }
    }
}
