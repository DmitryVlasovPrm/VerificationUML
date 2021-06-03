using ActivityDiagramVer.entities;
using ActivityDiagramVer.result;
using System.Collections.Generic;
using Verification.ad_ver.verification;
using Verification.rating_system;

namespace ActivityDiagramVer.verification.lexical
{
    /// <summary>
    /// Класс, содержащий методы проверки лексических ошибок
    /// </summary>
    internal class LexicalAnalizator
    {
        private ADNodesList diagramElements;
        private ISet<string> activityNames = new HashSet<string>();
        private ISet<string> participantNames = new HashSet<string>();
        public int swimlaneCount=0;
        public void setDiagramElements(ADNodesList diagramElements)
        {
            this.diagramElements = diagramElements;
        }

        public void checkFlow(ControlFlow flow)
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
                    if (!isCond) ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.HAVE_MARK) + " - \"" + flow.getText() + "\"", flow, ALL_MISTAKES.HAVE_MARK);
                }
                else if (notCondButHaveMark) ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.HAVE_MARK) + " - \"" + flow.getText() + "\"", flow, ALL_MISTAKES.HAVE_MARK);
            }
        }

        public void checkSwimlane(Swimlane swimlane)
        {
            swimlaneCount++;
            // проверка на уникальность имени
            if (participantNames.Contains(swimlane.getName().ToLower())) {
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.REPEATED_NAME], MistakeAdapter.toString(MISTAKES.REPEATED_NAME), swimlane, ALL_MISTAKES.REPEATED_NAME);
                return;
            } else {
                participantNames.Add(swimlane.getName().ToLower());
            }
            // проверка на заглавную букву
            if ((!swimlane.getName().Substring(0, 1).ToUpper().Equals(swimlane.getName().Substring(0, 1))))
            {
                ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.SMALL_LETTER), swimlane, ALL_MISTAKES.SMALL_LETTER);
            }
            // проверка на колво дочерних элементов
            if (swimlane.ChildCount == 0)
            {
                ADMistakeFactory.createMistake(Level.EASY, MistakeAdapter.toString(MISTAKES.EMPTY_SWIMLANE), swimlane, ALL_MISTAKES.EMPTY_SWIMLANE);
            }
            // проверка на спец символ
            if (hasSpecialSymbol(swimlane.getName()))
                ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.STRANGE_SYMBOL), swimlane, ALL_MISTAKES.STRANGE_SYMBOL);
        }
        private bool hasSpecialSymbol(string str)
        {
            // проверка на спец символ
            char firstLetter = str.Substring(0, 1).ToCharArray()[0];
            char lastLetter = str.Substring(str.Length - 1, 1).ToCharArray()[0];
            if ((firstLetter > 'a' && firstLetter < 'я' || firstLetter < 'a' || firstLetter > 'z') && (lastLetter > 'a' && lastLetter < 'я' || lastLetter < 'a' || lastLetter > 'z'))
            {
                return false;
            }
            return true;
        }

        public void checkActivity(ActivityNode activity, ADNodesList.ADNode node)
        {
            if (activity.getName().Length == 0)
                ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NO_NAME), node, ALL_MISTAKES.NO_NAME);
            else
            {
                // проверка на уникальность имени
                if (activityNames.Contains(activity.getName().ToLower())) {
                    ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.REPEATED_NAME], MistakeAdapter.toString(MISTAKES.REPEATED_NAME), diagramElements.getNode(activity.getId()), ALL_MISTAKES.REPEATED_NAME);
                    return;
                } else {
                    activityNames.Add(activity.getName().ToLower());
                }

                // проверка на заглавную букву
                if ((!activity.getName().Substring(0, 1).ToUpper().Equals(activity.getName().Substring(0, 1))))
                {
                    ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.SMALL_LETTER), node, ALL_MISTAKES.SMALL_LETTER);
                }
                // получаем первое слово существительного и проверяем, что оно не заканчивается на ь или т
                string firstWord = activity.getName().Split(' ')[0];
                //Console.WriteLine(firstWord);

                if (firstWord.EndsWith("ь") && !firstWord.EndsWith("ль") || firstWord.EndsWith("т"))
                    ADMistakeFactory.createMistake(Level.EASY, MistakeAdapter.toString(MISTAKES.NOT_NOUN), node, ALL_MISTAKES.NOT_NOUN);
            }
        }
        public void checkDecision(DecisionNode decision, ADNodesList.ADNode node)
        {
            // добавляем вопрос для перехода
            BaseNode flowIn = diagramElements.get(decision.getInId(0));
            string quest = ((ControlFlow)flowIn).getText();
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
                decision.findEqualAlternatives().ForEach(x => ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.REPEATED_ALT) + " - " + x, node, ALL_MISTAKES.REPEATED_ALT));

            // проверка на альтернативу без подписи
            if (checkAlt)
                if (decision.findEmptyAlternative())
                    ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.HAVE_EMPTY_ALT), node, ALL_MISTAKES.HAVE_EMPTY_ALT);

            // проверка, что альтернативы начинаются с заглавных букв
            //if (checkAlt)
            //    for (int i = 0; i < decision.alternativeSize(); i++)
            //    {
            //        String alter = decision.getAlternative(i);
            //        if (!alter.Equals(""))
            //            if (!alter.Substring(0, 1).ToUpper().Equals(alter.Substring(0, 1)))
            //                ADMistakeFactory.createMistake(Level.EASY,  " альтернатива \"" + alter + "\"" + MistakeAdapter.toString(MISTAKES.SMALL_LETTER), node);
            //    }


            bool checkQuest = true;
            // проверка, что имеется условие
            if (decision.getQuestion().Equals(""))
            {
                ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.HAVE_NOT_QUEST), node, ALL_MISTAKES.HAVE_NOT_QUEST);
                checkQuest = false; // дальнейшие проверки условия не требуются (его нет)
            }

            // проверка на заглавную букву
            if (checkQuest)
                if ((!decision.getQuestion().Substring(0, 1).ToUpper().Equals(decision.getQuestion().Substring(0, 1))))
                {
                    ADMistakeFactory.createMistake(Level.EASY, MistakeAdapter.toString(MISTAKES.SMALL_LETTER), node, ALL_MISTAKES.SMALL_LETTER);
                }
            // заканчивается на знак вопроса
            if (checkQuest)
                if ((!decision.getQuestion().EndsWith("?")))
                    ADMistakeFactory.createMistake(Level.EASY, MistakeAdapter.toString(MISTAKES.END_WITH_QUEST), node, ALL_MISTAKES.END_WITH_QUEST);
        }
    }
}
