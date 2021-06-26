using ActivityDiagramVer.entities;
using ActivityDiagramVer.result;
using ActivityDiagramVer.verification.lexical;
using System;
using Verification.ad_ver.verification;
using Verification.rating_system;

namespace ActivityDiagramVer.verification.syntax {
    /// <summary>
    /// Класс, отвечающий за проверку диаграммы без графа
    /// </summary>
    internal class ADModelVerifier {
        private ADNodesList diagramElements;
        private int initialCount = 0;
        private int finalCount = 0;
        private int activityCount = 0;
        private readonly LexicalAnalizator lexicalAnalizator;

        public ADModelVerifier(LexicalAnalizator lexicalAnalizator) {
            this.lexicalAnalizator = lexicalAnalizator ?? throw new ArgumentNullException(nameof(lexicalAnalizator));
        }

        public void setDiagramElements(ADNodesList diagramElements) {
            this.diagramElements = diagramElements;
        }

        public void check() {
            lexicalAnalizator.setDiagramElements(diagramElements);

            // прохождение по всем элементам и их проверка
            for (int i = 0; i < diagramElements.size(); i++) {
                BaseNode currentNode = diagramElements.get(i);
                switch (diagramElements.get(i).getType()) {
                    case ElementType.FLOW:
                        lexicalAnalizator.checkFlow((ControlFlow)diagramElements.get(i));
                        break;
                    case ElementType.INITIAL_NODE:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        if (((DiagramElement)currentNode).outSize() == 0)
                            ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_OUT], MistakeAdapter.toString(MISTAKES.NO_OUT), diagramElements.getNode(i), ALL_MISTAKES.NO_OUT);
                        checkInitial();
                        break;
                    case ElementType.FINAL_NODE:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        if (((DiagramElement)currentNode).inSize() == 0)
                            ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_OUT], MistakeAdapter.toString(MISTAKES.NO_IN), diagramElements.getNode(i), ALL_MISTAKES.NO_IN);
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
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_FINAL], MistakeAdapter.toString(MISTAKES.NO_FINAL), ALL_MISTAKES.NO_FINAL);
            if (initialCount == 0)
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_INITIAL], MistakeAdapter.toString(MISTAKES.NO_INITIAL), ALL_MISTAKES.NO_INITIAL);
            if (activityCount == 0)
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_ACTIVITIES], MistakeAdapter.toString(MISTAKES.NO_ACTIVITIES), ALL_MISTAKES.NO_ACTIVITIES);
            if (lexicalAnalizator.swimlaneCount == 0)
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_SWIMLANE], MistakeAdapter.toString(MISTAKES.NO_SWIMLANE), ALL_MISTAKES.NO_SWIMLANE);
        }

        /**
         * Проверка, что элемент принадлежит какому-либо участнику
         */
        private void checkIfInPartion(DiagramElement currentNode, string name, ADNodesList.ADNode node) {
            if (currentNode.getInPartition().Equals(""))
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_PARTION], MistakeAdapter.toString(MISTAKES.NO_PARTION), node, ALL_MISTAKES.NO_PARTION);
        }

        /**
         * Проверка, что имеется хотя бы один входящий\выходящий переход
         */
        private void checkInOut(DiagramElement currentNode, string name, ADNodesList.ADNode node) {
            if (currentNode is MergeNode || currentNode is JoinNode)
                if ((currentNode).inSize() == 1)
                    ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.HAS_1_IN], MistakeAdapter.toString(MISTAKES.HAS_1_IN), node, ALL_MISTAKES.HAS_1_IN);
            if ((currentNode).inSize() == 0)
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_IN], MistakeAdapter.toString(MISTAKES.NO_IN), node, ALL_MISTAKES.NO_IN);
            if ((currentNode).outSize() == 0)
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_OUT], MistakeAdapter.toString(MISTAKES.NO_OUT), node, ALL_MISTAKES.NO_OUT);
        }

        private void checkFork(ForkNode fork, ADNodesList.ADNode node) {
            for (int i = 0; i < fork.outSize(); i++) {
                ElementType elementType = diagramElements.get(((ControlFlow)diagramElements.get(fork.getOutId(i))).getTarget()).getType();
                Console.WriteLine("elementType="+elementType);
                if (elementType != ElementType.ACTIVITY && elementType != ElementType.DECISION && elementType != ElementType.FORK && elementType != ElementType.MERGE) {
                    ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.OUT_NOT_IN_ACT], MistakeAdapter.toString(MISTAKES.OUT_NOT_IN_ACT), node, ALL_MISTAKES.OUT_NOT_IN_ACT);
                }
            }
        }

        private void checkInitial() {
            initialCount++;
            if (initialCount > 1) ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.MORE_THAN_ONE_INIT], MistakeAdapter.toString(MISTAKES.MORE_THAN_ONE_INIT), ALL_MISTAKES.MORE_THAN_ONE_INIT);

        }
        private void checkFinal() {
            finalCount++;
        }
        private void checkActivity(ActivityNode activity, ADNodesList.ADNode node) {
            activityCount++;
            // активность имеет больше одного выходящего перехода
            if (activity.outSize() >= 2)
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.MORE_THAN_ONE_OUT], MistakeAdapter.toString(MISTAKES.MORE_THAN_ONE_OUT), node, ALL_MISTAKES.MORE_THAN_ONE_OUT);
        }

        private void checkDecision(DecisionNode decision, ADNodesList.ADNode node) {
            bool checkAlt = true;
            // проверка, что альтернативы есть
            if (decision.alternativeSize() == 0) {
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.DO_NOT_HAVE_ALT], MistakeAdapter.toString(MISTAKES.DO_NOT_HAVE_ALT), node, ALL_MISTAKES.DO_NOT_HAVE_ALT);
                checkAlt = false;
            }

            // проверка, что альтернатив больше одной
            if (checkAlt)
                if (decision.alternativeSize() == 1) {
                    ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.ONLY_ONE_ALT], MistakeAdapter.toString(MISTAKES.ONLY_ONE_ALT), node, ALL_MISTAKES.ONLY_ONE_ALT);
                }

            // проверка, что альтернативы не ведут в один и тот же элемент
            if (checkAlt) {
                for (int i = 0; i < decision.outSize() - 1; i++) {
                    string targetId = ((ControlFlow)diagramElements.get(decision.getOutId(i))).getTarget();
                    for (int j = i + 1; j < decision.outSize(); j++) {
                        if (targetId.Equals(((ControlFlow)diagramElements.get(decision.getOutId(j))).getTarget()))
                            ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.SAME_TARGET], MistakeAdapter.toString(MISTAKES.SAME_TARGET), node, ALL_MISTAKES.SAME_TARGET);

                    }
                    if (diagramElements.get(targetId).getType() == ElementType.DECISION)
                        ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NEXT_DECISION], MistakeAdapter.toString(MISTAKES.NEXT_DECISION), node, ALL_MISTAKES.NEXT_DECISION);
                }
                // проверка на последовательность условных операторов
                string targetId2 = ((ControlFlow)diagramElements.get(decision.getOutId(decision.outSize() - 1))).getTarget();
                if (diagramElements.get(targetId2).getType() == ElementType.DECISION)
                    ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NEXT_DECISION], MistakeAdapter.toString(MISTAKES.NEXT_DECISION), node, ALL_MISTAKES.NEXT_DECISION);

            }

        }
    }
}
