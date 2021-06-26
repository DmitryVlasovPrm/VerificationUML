using ActivityDiagramVer.verification;
using System;
using System.Collections.Generic;

namespace Verification.ad_ver.verification {
    /// <summary>
    /// Перечисление возможных ошибок
    /// </summary>
    internal static class MistakesSeriousness {
        public static Dictionary<MISTAKES, Level> mistakes = new Dictionary<MISTAKES, Level> {
            { MISTAKES.MORE_THAN_ONE_INIT, Level.FATAL },
            { MISTAKES.NO_FINAL, Level.FATAL },
            { MISTAKES.NO_INITIAL, Level.FATAL },
            { MISTAKES.NO_ACTIVITIES, Level.FATAL },
            { MISTAKES.MORE_THAN_ONE_OUT, Level.FATAL },
            { MISTAKES.DO_NOT_HAVE_ALT, Level.FATAL },
            { MISTAKES.ONLY_ONE_ALT, Level.HARD },
            { MISTAKES.NO_OUT, Level.FATAL }, //1
            { MISTAKES.NO_IN, Level.FATAL }, //1
            { MISTAKES.NO_PARTION, Level.EASY },
            { MISTAKES.REPEATED_NAME, Level.HARD },
            { MISTAKES.SAME_TARGET, Level.HARD },
            { MISTAKES.OUT_NOT_IN_ACT, Level.FATAL },
            { MISTAKES.NEXT_DECISION, Level.HARD },
            { MISTAKES.HAS_1_IN, Level.HARD },

            { MISTAKES.FORBIDDEN_ELEMENT, Level.FATAL },//1
            { MISTAKES.NO_SWIMLANE, Level.HARD },//1
            { MISTAKES.SMALL_LETTER, Level.HARD },
            { MISTAKES.NO_NAME, Level.HARD },
            { MISTAKES.NOT_NOUN, Level.EASY },
            { MISTAKES.END_WITH_QUEST, Level.EASY },
            { MISTAKES.HAVE_NOT_QUEST, Level.HARD },
            { MISTAKES.REPEATED_ALT, Level.HARD },
            { MISTAKES.HAVE_EMPTY_ALT, Level.HARD },
            { MISTAKES.HAVE_MARK, Level.HARD },
            { MISTAKES.STRANGE_SYMBOL, Level.HARD },
            { MISTAKES.EMPTY_SWIMLANE, Level.EASY },

            { MISTAKES.TWO_TOKENS, Level.FATAL },
            { MISTAKES.DEAD_ROAD, Level.FATAL },
            { MISTAKES.MANY_TOKENS_IN_END, Level.FATAL },
            { MISTAKES.COULD_NOT_REACH_FINAL, Level.FATAL },
            { MISTAKES.FINAL_COLOR_TOKEN, Level.FATAL }
        };
    }
    internal enum MISTAKES {
        // синтаксис 1
        MORE_THAN_ONE_INIT,
        NO_FINAL,
        NO_INITIAL,
        NO_ACTIVITIES,
        MORE_THAN_ONE_OUT,
        DO_NOT_HAVE_ALT,
        ONLY_ONE_ALT,
        NO_OUT,
        NO_IN,
        NO_PARTION,
        REPEATED_NAME,
        SAME_TARGET,
        OUT_NOT_IN_ACT,
        NEXT_DECISION,
        HAS_1_IN,
        // лексические
        FORBIDDEN_ELEMENT,
        NO_SWIMLANE,
        SMALL_LETTER,
        NO_NAME,
        NOT_NOUN,
        END_WITH_QUEST,
        HAVE_NOT_QUEST,
        HAVE_EMPTY_ALT,
        HAVE_MARK,
        STRANGE_SYMBOL,
        EMPTY_SWIMLANE,
        // синтаксис 2
        TWO_TOKENS,
        DEAD_ROAD,
        MANY_TOKENS_IN_END,
        COULD_NOT_REACH_FINAL,
        FINAL_COLOR_TOKEN,

        // семантические
        REPEATED_ALT
    }
    internal class MistakeAdapter {
        public static string toString(MISTAKES mistake) {
            switch (mistake) {
                case MISTAKES.MORE_THAN_ONE_INIT:
                    return "В диаграмме больше одного начального состояния";
                case MISTAKES.NO_FINAL:
                    return "В диаграмме отсутствует финальное состояние";
                case MISTAKES.NO_INITIAL:
                    return "В диаграмме отсутствует начальное состояние";
                case MISTAKES.NO_ACTIVITIES:
                    return "В диаграмме отсутствуют активности";
                case MISTAKES.MORE_THAN_ONE_OUT:
                    return "больше одного выходящего перехода";
                case MISTAKES.DO_NOT_HAVE_ALT:
                    return "не имеет альтернатив";
                case MISTAKES.ONLY_ONE_ALT:
                    return "всего одна альтернатива";
                case MISTAKES.HAS_1_IN:
                    return "всего один входной переход";
                case MISTAKES.NO_IN:
                    return "отсутствует входящий переход";
                case MISTAKES.NO_OUT:
                    return "отсутствует выходящий переход";
                case MISTAKES.NO_PARTION:
                    return "не принадлежит никакому участнику";
                case MISTAKES.REPEATED_NAME:
                    return "имя не уникально";
                case MISTAKES.SAME_TARGET:
                    return "альтернативы ведут в один и тот же элемент";
                case MISTAKES.OUT_NOT_IN_ACT:
                    return "переход ведет не в активность, разветвитель, узел слияние или в условный переход";
                case MISTAKES.NEXT_DECISION:
                    return "альтернатива ведет в условный переход";

                // лексические
                case MISTAKES.FORBIDDEN_ELEMENT:
                    return "Использовался недопустимый элемент";
                case MISTAKES.NO_SWIMLANE:
                    return "Отсутствуют акторы или используется не верный элемент";
                case MISTAKES.SMALL_LETTER:
                    return "имя начинается с маленькой буквы";
                case MISTAKES.NO_NAME:
                    return "отсутствует имя";
                case MISTAKES.NOT_NOUN:
                    return "первое слово возможно не является именем существительным";
                case MISTAKES.END_WITH_QUEST:
                    return "нет знака вопроса";
                case MISTAKES.HAVE_NOT_QUEST:
                    return "отсутствует условие";
                case MISTAKES.REPEATED_ALT:
                    return "повторяется альтернатива";
                case MISTAKES.HAVE_EMPTY_ALT:
                    return "неподписанная альтернатива";
                case MISTAKES.HAVE_MARK:
                    return "имеет подпись, не являясь условием или альтернативой";
                case MISTAKES.STRANGE_SYMBOL:
                    return "название имеет специальный символ";
                case MISTAKES.EMPTY_SWIMLANE:
                    return "не содержит элементов";


                // синтаксис 2
                // просто пересечение двух токенов
                // Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором\разветвителем
                case MISTAKES.TWO_TOKENS: return "Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем";// "в элементе пересеклись токены. Возможно отсутствие синхронизатора";
                case MISTAKES.DEAD_ROAD: return "Тупик. Невозможно активировать синхронизатор";        // на определенном шаге не был передвинут ни один токен
                                                                // возможно пересечение двух токенов в конечном состоянии из-за отсутствия синхронизатора
                case MISTAKES.MANY_TOKENS_IN_END: return "Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем";//"при достижении конечного состояния остались токены";
                case MISTAKES.COULD_NOT_REACH_FINAL:
                    return "недостижимое конечное состояние. Возможно имеется синхронизатор, " +
"который невозможно активировать";       // проверьте, что все переходы, ведущие в синхронизаторы могут быть активны одновременно 
                case MISTAKES.FINAL_COLOR_TOKEN: return "Отсутствует синхронизатор или существует поток, выходящий из зоны между синхронизатором-разветвителем";//"достигли конечное состояние с цветным токеном. Отсутствует парный синхронизатор";
                default:
                    throw new ArgumentException();
            }
        }
    }
}
