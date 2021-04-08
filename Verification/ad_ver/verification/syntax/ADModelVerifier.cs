using ActivityDiagramVer.entities;
using ActivityDiagramVer.result;
using ActivityDiagramVer.verification.lexical;
using System;
using System.Collections.Generic;
using Verification.ad_ver.verification;

namespace ActivityDiagramVer.verification.syntax
{
    /// <summary>
    /// Класс, отвечающий за проверку диаграммы без графа
    /// </summary>
    internal class ADModelVerifier
    {
        private ADNodesList diagramElements;
        private int initialCount = 0;
        private int finalCount = 0;
        private int activityCount = 0;
        private LexicalAnalizator lexicalAnalizator;

        public ADModelVerifier(LexicalAnalizator lexicalAnalizator) {
            this.lexicalAnalizator = lexicalAnalizator ?? throw new ArgumentNullException(nameof(lexicalAnalizator));
        }

        public void setDiagramElements(ADNodesList diagramElements)
        {
            this.diagramElements = diagramElements;
        }

        public void check()
        {
            lexicalAnalizator.setDiagramElements(diagramElements);

            for (int i = 0; i < diagramElements.size(); i++)
            {
                BaseNode currentNode = diagramElements.get(i);
                switch (diagramElements.get(i).getType())
                {
                    case ElementType.FLOW:
                        lexicalAnalizator.checkFlow((ControlFlow)diagramElements.get(i));
                        break;
                    case ElementType.INITIAL_NODE:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        if (((DiagramElement)currentNode).outSize() == 0)
                            ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NO_OUT), diagramElements.getNode(i));
                        checkInitial();
                        break;
                    case ElementType.FINAL_NODE:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        if (((DiagramElement)currentNode).inSize() == 0)
                            ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NO_IN), diagramElements.getNode(i));
                        checkFinal();
                        break;
                    case ElementType.FORK:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        checkInOut((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        checkFork((ForkNode)currentNode, diagramElements.getNode(i));
                        break;
                    case ElementType.JOIN:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        checkInOut((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        break;
                    case ElementType.MERGE:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        checkInOut((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        break;
                    case ElementType.ACTIVITY:
                        lexicalAnalizator.checkActivity((ActivityNode)diagramElements.get(i), diagramElements.getNode(i));

                        checkIfInPartion((DiagramElement)currentNode, ((ActivityNode)currentNode).getName(), diagramElements.getNode(i));
                        checkInOut((DiagramElement)currentNode, ((ActivityNode)currentNode).getName(), diagramElements.getNode(i));
                        checkActivity((ActivityNode)diagramElements.get(i), diagramElements.getNode(i));
                        break;
                    case ElementType.DECISION:
                        lexicalAnalizator.checkDecision((DecisionNode)diagramElements.get(i), diagramElements.getNode(i));

                        checkIfInPartion((DiagramElement)currentNode, ((DecisionNode)currentNode).getQuestion(), diagramElements.getNode(i));
                        checkInOut((DiagramElement)currentNode, ((DecisionNode)currentNode).getQuestion(), diagramElements.getNode(i));
                        checkDecision((DecisionNode)diagramElements.get(i), diagramElements.getNode(i));
                        break;
                    case ElementType.SWIMLANE:
                        lexicalAnalizator.checkSwimlane((Swimlane)diagramElements.get(i));
                        break;
                    case ElementType.UNKNOWN:
                        break;
                }
            }
            if (finalCount == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_FINAL));
            if (initialCount == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_INITIAL));
            if (activityCount == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_ACTIVITIES));

            // проверка, что имена активностей уникальны
            List<ActivityNode> activities = diagramElements.getAllActivities();
            for (int i = 0; i < activities.Count - 1; i++)
            {
                for (int j = i + 1; j < activities.Count; j++)
                {
                    if (activities[i].getName().Equals(activities[j].getName()))
                        ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.REPEATED_ACT), diagramElements.getNode(activities[i].getId()));
                }
            }
        }

        /**
         * Проверка, что элемент принадлежит какому-либо участнику
         * @param currentNode
         * @param name
         */
        private void checkIfInPartion(DiagramElement currentNode, string name, ADNodesList.ADNode node)
        {
            if (currentNode.getInPartition().Equals(""))
                ADMistakeFactory.createMistake(Level.EASY, MistakeAdapter.toString(MISTAKES.NO_PARTION), node);
        }

        /**
         * Проверка, что имеется хотя бы один входящий\выходящий переход
         * @param currentNode
         * @param name
         */
        private void checkInOut(DiagramElement currentNode, string name, ADNodesList.ADNode node)
        {
            if (currentNode is MergeNode)
                if ((currentNode).inSize() == 1)
                    ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.MERGE_HAS_1_IN), node);
            if ((currentNode).inSize() == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_IN), node);
            if ((currentNode).outSize() == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_OUT), node);
        }

        private void checkFork(ForkNode fork, ADNodesList.ADNode node)
        {
            for (int i = 0; i < fork.outSize(); i++)
            {
                ElementType elementType = diagramElements.get(((ControlFlow)diagramElements.get(fork.getOutId(i))).getTarget()).getType();
                if (elementType != ElementType.ACTIVITY && elementType != ElementType.DECISION && elementType != ElementType.FORK)
                    ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.OUT_NOT_IN_ACT), node);
            }
        }

        private void checkInitial()
        {
            initialCount++;
            if (initialCount > 1) ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.MORE_THAN_ONE_INIT));

        }
        private void checkFinal()
        {
            finalCount++;
        }
        private void checkActivity(ActivityNode activity, ADNodesList.ADNode node)
        {
            activityCount++;
            // активность имеет больше одного выходящего перехода
            if (activity.outSize() >= 2)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.MORE_THAN_ONE_OUT), node);
        }

        private void checkDecision(DecisionNode decision, ADNodesList.ADNode node)
        {
            bool checkAlt = true;
            // проверка, что альтернативы есть
            if (decision.alternativeSize() == 0)
            {
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.DO_NOT_HAVE_ALT), node);
                checkAlt = false;
            }

            // проверка, что альтернатив больше одной
            if (checkAlt)
                if (decision.alternativeSize() == 1)
                {
                    ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.ONLY_ONE_ALT), node);
                }

            // проверка, что альтернативы не ведут в один и тот же элемент
            if (checkAlt)
            {
                for (int i = 0; i < decision.outSize() - 1; i++)
                {
                    string targetId = ((ControlFlow)diagramElements.get(decision.getOutId(i))).getTarget();
                    for (int j = i + 1; j < decision.outSize(); j++)
                    {
                        if (targetId.Equals(((ControlFlow)diagramElements.get(decision.getOutId(j))).getTarget()))
                            ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.SAME_TARGET), node);

                    }
                    if (diagramElements.get(targetId).getType() == ElementType.DECISION)
                        ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NEXT_DECISION), node);
                }
                // проверка на последовательность условных операторов
                string targetId2 = ((ControlFlow)diagramElements.get(decision.getOutId(decision.outSize() - 1))).getTarget();
                if (diagramElements.get(targetId2).getType() == ElementType.DECISION)
                    ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NEXT_DECISION), node);

            }

        }
    }
}
