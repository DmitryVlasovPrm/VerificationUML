using ActivityDiagramVer.entities;
using ActivityDiagramVer.result;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.verification.lexical
{
    /**
 * Этап лексического анализа
 */
    class LexicalAnalizator
    {
        private ADNodesList diagramElements;
        public void setDiagramElements(ADNodesList diagramElements)
        {
            this.diagramElements = diagramElements;
        }

        public void check()
        {
            for (int i = 0; i < diagramElements.size(); i++)
            {
                switch (diagramElements.get(i).getType())
                {
                    case ElementType.FLOW:
                        checkFlow((ControlFlow)diagramElements.get(i));
                        break;
                    case ElementType.FORK:
                        break;
                    case ElementType.JOIN:
                        break;
                    case ElementType.MERGE:
                        break;
                    case ElementType.ACTIVITY:
                        checkActivity((ActivityNode)diagramElements.get(i), diagramElements.getNode(i));
                        break;
                    case ElementType.DECISION:
                        checkDecision((DecisionNode)diagramElements.get(i), diagramElements.getNode(i));
                        break;
                    case ElementType.SWIMLANE:
                        checkSwimlane((Swimlane)diagramElements.get(i));
                        break;
                    case ElementType.UNKNOWN:
                        break;
                }
            }
        }

        private void checkFlow(ControlFlow flow)
        {
            bool notCondButHaveMark = false;
            bool isCond = false;
            // если это не условие, проверяем подпись
            if (diagramElements.get(flow.getTarget()).getType() != ElementType.DECISION)
            {
                if (!flow.getText().Equals(""))
                {
                    notCondButHaveMark = true;
                }
            }
            else isCond = true;

            if (diagramElements.get(flow.getSrc()).getType() != ElementType.DECISION)
            {
                if (!flow.getText().Equals(""))
                {
                    if (!isCond) ADMistakeFactory.createMistake(Level.HARD, MistakesAdapter.toString(MISTAKES.HAVE_MARK) + " - \"" + flow.getText() + "\"", flow);
                }
                else if (notCondButHaveMark) ADMistakeFactory.createMistake(Level.HARD, MistakesAdapter.toString(MISTAKES.HAVE_MARK) + " - \"" + flow.getText() + "\"", flow);//writeMistake(Level.HARD.toString(), flow.getType().toString(), "", MISTAKES.HAVE_MARK.toString() + " - \"" + flow.getText() + "\"");
            }
        }

        private void checkSwimlane(Swimlane swimlane)
        {
            // проверка на заглавную букву
            if ((!swimlane.getName().Substring(0, 1).ToUpper().Equals(swimlane.getName().Substring(0, 1))))
            {
                ADMistakeFactory.createMistake(Level.HARD, MistakesAdapter.toString(MISTAKES.SMALL_LETTER), swimlane);
            }
        }
        private void checkActivity(ActivityNode activity, ADNodesList.ADNode node)
        {
            // проверка на заглавную букву
            if ((!activity.getName().Substring(0, 1).ToUpper().Equals(activity.getName().Substring(0, 1))))
            {
                ADMistakeFactory.createMistake(Level.HARD, MistakesAdapter.toString(MISTAKES.SMALL_LETTER), node);
            }
            // получаем первое слово существительного и проверяем, что оно не заканчивается на ь или т
            String firstWord = activity.getName().Split(' ')[0];
            //Console.WriteLine(firstWord);

            if (firstWord.EndsWith("ь") && !firstWord.EndsWith("ль") || firstWord.EndsWith("т"))
                ADMistakeFactory.createMistake(Level.EASY, MistakesAdapter.toString(MISTAKES.NOT_NOUN), node);
        }
        private void checkDecision(DecisionNode decision, ADNodesList.ADNode node)
        {
            // добавляем вопрос для перехода
            BaseNode flowIn = diagramElements.get(decision.getInId(0));
            String quest = ((ControlFlow)flowIn).getText();
            decision.setQuestion(quest.Trim());

            // добавляем альтернативы -> проходим по всем выходящим переходам и получаем подписи
            for (int i = 0; i < decision.outSize(); i++)
            {
                BaseNode flow = diagramElements.get(decision.getOutId(i));
                decision.addAlternative(((ControlFlow)flow).getText());
            }

            // проверяем подписи альтернатив, если их больше одной
            bool checkAlt = decision.alternativeSize() >= 2;

            // поиск совпадающих названий
            if (checkAlt)
                decision.findEqualAlternatives().ForEach(x => ADMistakeFactory.createMistake(Level.HARD, MistakesAdapter.toString(MISTAKES.REPEATED_ALT) + " - " + x, node));

            // проверка на альтернативу без подписи
            if (checkAlt)
                if (decision.findEmptyAlternative())
                    ADMistakeFactory.createMistake(Level.HARD, MistakesAdapter.toString(MISTAKES.HAVE_EMPTY_ALT), node);

            // проверка, что альтернативы начинаются с заглавных букв
            if (checkAlt)
                for (int i = 0; i < decision.alternativeSize(); i++)
                {
                    String alter = decision.getAlternative(i);
                    if (!alter.Equals(""))
                        if (!alter.Substring(0, 1).ToUpper().Equals(alter.Substring(0, 1)))
                            ADMistakeFactory.createMistake(Level.EASY,  " альтернатива \"" + alter + "\"" + MistakesAdapter.toString(MISTAKES.SMALL_LETTER), node);
                }


            bool checkQuest = true;
            // проверка, что имеется условие
            if (decision.getQuestion().Equals(""))
            {
                ADMistakeFactory.createMistake(Level.HARD,  MistakesAdapter.toString(MISTAKES.HAVE_NOT_QUEST), node);
                checkQuest = false; // дальнейшие проверки условия не требуются (его нет)
            }

            // проверка на заглавную букву
            if (checkQuest)
                if ((!decision.getQuestion().Substring(0, 1).ToUpper().Equals(decision.getQuestion().Substring(0, 1))))
                {
                    ADMistakeFactory.createMistake(Level.EASY,  MistakesAdapter.toString(MISTAKES.SMALL_LETTER), node);
                }
            // заканчивается на знак вопроса
            if (checkQuest)
                if ((!decision.getQuestion().EndsWith("?")))
                    ADMistakeFactory.createMistake(Level.EASY,  MistakesAdapter.toString(MISTAKES.END_WITH_QUEST), node);
        }
        /**
         * Ошибки, которые могут возникнуть на данном этапе
         */
        private enum MISTAKES
        {
            SMALL_LETTER,
            NOT_NOUN,
            END_WITH_QUEST,
            HAVE_NOT_QUEST,
            REPEATED_ALT,
            HAVE_EMPTY_ALT,
            HAVE_MARK
        }
        private class MistakesAdapter
        {
            public static String toString(MISTAKES mistake)
            {
                switch (mistake)
                {
                    case MISTAKES.SMALL_LETTER:
                        return "имя начинается с маленькой буквы";
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
                    default:
                        throw new ArgumentException();
                }

            }
        }
    }
}
