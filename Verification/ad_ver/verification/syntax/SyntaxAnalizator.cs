using ActivityDiagramVer.entities;
using ActivityDiagramVer.result;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityDiagramVer.verification.syntax
{
    class SyntaxAnalizator
    {
        private Level level;
        private ADNodesList diagramElements;
        private int initialCount = 0;
        private int finalCount = 0;
        private int activityCount = 0;

        public SyntaxAnalizator(Level level)
        {
            this.level = level;
        }

        //region Getter-Setter

        public Level getLevel()
        {
            return level;
        }

        public void setLevel(Level level)
        {
            this.level = level;
        }

        public ADNodesList getDiagramElements()
        {
            return diagramElements;
        }

        public void setDiagramElements(ADNodesList diagramElements)
        {
            this.diagramElements = diagramElements;
        }

        //endregion

        public void check()
        {
            for (int i = 0; i < diagramElements.size(); i++)
            {
                BaseNode currentNode = diagramElements.get(i);
                switch (diagramElements.get(i).getType())
                {
                    case ElementType.FLOW:
                        break;
                    case ElementType.INITIAL_NODE:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        if (((DiagramElement)currentNode).outSize() == 0)
                            ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NO_OUT), diagramElements.getNode(i));
                        //                        writeMistake(Level.HARD.toString(), currentNode.getType().toString(), "", MISTAKES.NO_OUT.toString());
                        checkInitial();
                        break;
                    case ElementType.FINAL_NODE:
                        checkIfInPartion((DiagramElement)currentNode, "", diagramElements.getNode(i));
                        if (((DiagramElement)currentNode).inSize() == 0)
                            ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NO_IN), diagramElements.getNode(i));
                        //                        writeMistake(Level.HARD.toString(), currentNode.getType().toString(), "", MISTAKES.NO_IN.toString());
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
                        checkIfInPartion((DiagramElement)currentNode, ((ActivityNode)currentNode).getName(), diagramElements.getNode(i));
                        checkInOut((DiagramElement)currentNode, ((ActivityNode)currentNode).getName(), diagramElements.getNode(i));
                        checkActivity((ActivityNode)diagramElements.get(i), diagramElements.getNode(i));
                        break;
                    case ElementType.DECISION:
                        checkIfInPartion((DiagramElement)currentNode, ((DecisionNode)currentNode).getQuestion(), diagramElements.getNode(i));
                        checkInOut((DiagramElement)currentNode, ((DecisionNode)currentNode).getQuestion(), diagramElements.getNode(i));
                        checkDecision((DecisionNode)diagramElements.get(i), diagramElements.getNode(i));
                        break;
                    case ElementType.STRANGE:
                        break;
                }
            }
            if (finalCount == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_FINAL));
            //            writeMistake(Level.HARD.toString()+" "+MISTAKES.NO_FINAL.toString());
            if (initialCount == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_INITIAL));
            //            writeMistake(Level.HARD.toString()+" "+MISTAKES.NO_INITIAL.toString());
            if (activityCount == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_ACTIVITIES));
            //            writeMistake(Level.HARD.toString()+" "+MISTAKES.NO_ACTIVITIES.toString());

            // проверка, что имена активностей уникальны
            List<ActivityNode> activities = diagramElements.getAllActivities();
            for (int i = 0; i < activities.Count - 1; i++)
            {
                for (int j = i + 1; j < activities.Count; j++)
                {
                    if (activities[i].getName().Equals(activities[j].getName()))
                        ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.REPEATED_ACT), diagramElements.getNode(activities[i].getId()));
                    //                    writeMistake(Level.HARD.toString(), activities[i].getType().toString(), activities[i].getName(), MISTAKES.REPEATED_ACT.toString());
                }
            }
        }

        /**
         * Проверка, что элемент принадлежит какому-либо участнику
         * @param currentNode
         * @param name
         */
        private void checkIfInPartion(DiagramElement currentNode, String name, ADNodesList.ADNode node)
        {
            if (currentNode.getInPartition().Equals(""))
                ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NO_PARTION), node);
            //            writeMistake(Level.HARD.toString(), currentNode.getType().toString(), name, MISTAKES.NO_PARTION.toString());
        }

        /**
         * Проверка, что имеется хотя бы один входящий\выходящий переход
         * @param currentNode
         * @param name
         */
        private void checkInOut(DiagramElement currentNode, String name, ADNodesList.ADNode node)
        {
            if (currentNode is MergeNode)
                if ((currentNode).inSize() == 1)
                    ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.MERGE_HAS_1_IN), node);
            if ((currentNode).inSize() == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_IN), node);
            //            writeMistake(Level.HARD.toString(), currentNode.getType().toString(), name, MISTAKES.NO_IN.toString());
            if ((currentNode).outSize() == 0)
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.NO_OUT), node);
            //            writeMistake(Level.HARD.toString(), currentNode.getType().toString(), name, MISTAKES.NO_OUT.toString());
        }

        private void checkFork(ForkNode fork, ADNodesList.ADNode node)
        {
            for (int i = 0; i < fork.outSize(); i++)
            {
                ElementType elementType = diagramElements.get(((ControlFlow)diagramElements.get(fork.getOutId(i))).getTarget()).getType();
                if (elementType != ElementType.ACTIVITY && elementType != ElementType.DECISION && elementType != ElementType.FORK)
                    ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.OUT_NOT_IN_ACT), node);
                //                writeMistake(level.toString(), fork.getType().toString(), "", MISTAKES.OUT_NOT_IN_ACT.toString());
            }
        }

        private void checkInitial()
        {
            initialCount++;
            if (initialCount > 1) ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.MORE_THAN_ONE_INIT));//writeMistake("["+Level.HARD.toString()+"] "+MISTAKES.MORE_THAN_ONE_INIT.toString());

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
                ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.MORE_THAN_ONE_OUT), node);
            //            writeMistake(Level.HARD.toString(), activity.getType().toString(), activity.getName(), MISTAKES.MORE_THAN_ONE_OUT.toString());
        }

        private void checkDecision(DecisionNode decision, ADNodesList.ADNode node)
        {
            bool checkAlt = true;
            // проверка, что альтернативы есть
            if (decision.alternativeSize() == 0)
            {
                ADMistakeFactory.createMistake(Level.FATAL, MistakeAdapter.toString(MISTAKES.DO_NOT_HAVE_ALT), node);
                //            writeMistake(Level.HARD.toString(), decision.getType().toString(), decision.getQuestion(), MISTAKES.DO_NOT_HAVE_ALT.toString());
                checkAlt = false;
            }

            // проверка, что альтернатив больше одной
            if (checkAlt)
                if (decision.alternativeSize() == 1)
                {
                    ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.ONLY_ONE_ALT), node);
                    //            writeMistake(Level.HARD.toString(), decision.getType().toString(), decision.getQuestion(), MISTAKES.ONLY_ONE_ALT.toString());
                }

            // проверка, что альтернативы не ведут в один и тот же элемент
            if (checkAlt)
            {
                for (int i = 0; i < decision.outSize() - 1; i++)
                {
                    String targetId = ((ControlFlow)diagramElements.get(decision.getOutId(i))).getTarget();
                    for (int j = i + 1; j < decision.outSize(); j++)
                    {
                        if (targetId.Equals(((ControlFlow)diagramElements.get(decision.getOutId(j))).getTarget()))
                            ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.SAME_TARGET), node);
                        //                        writeMistake(Level.HARD.toString(), decision.getType().toString(), decision.getQuestion(), MISTAKES.SAME_TARGET.toString());
                        
                    }
                    if (diagramElements.get(targetId).getType() == ElementType.DECISION)
                        ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NEXT_DECISION), node);
                    //                        writeMistake(level.toString(), decision.getType().toString(), decision.getQuestion(), MISTAKES.NEXT_DECISION.toString());
                }
                // проверка на последовательность условных операторов
                String targetId2 = ((ControlFlow)diagramElements.get(decision.getOutId(decision.outSize() - 1))).getTarget();
                if (diagramElements.get(targetId2).getType() == ElementType.DECISION)
                    ADMistakeFactory.createMistake(Level.HARD, MistakeAdapter.toString(MISTAKES.NEXT_DECISION), node);
                //                writeMistake(level.toString(), decision.getType().toString(), decision.getQuestion(), MISTAKES.NEXT_DECISION.toString());

            }

        }

        /**
         * Ошибки, которые могут возникнуть на данном этапе
         */
        private enum MISTAKES
        {
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
            REPEATED_ACT,
            SAME_TARGET,
            OUT_NOT_IN_ACT,
            NEXT_DECISION, 
            MERGE_HAS_1_IN
        }
        private class MistakeAdapter{
           
        public static String toString(MISTAKES mistake)
        {
            switch (mistake)
            {
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
                case MISTAKES.MERGE_HAS_1_IN:
                    return "всего один входной переход";
                    case MISTAKES.NO_IN:
                    return "отсутствует входящий переход";
                case MISTAKES.NO_OUT:
                    return "отсутствует выходящий переход";
                case MISTAKES.NO_PARTION:
                    return "не принадлежит никакому участнику";
                case MISTAKES.REPEATED_ACT:
                    return "имя не уникально";
                case MISTAKES.SAME_TARGET:
                    return "альтернативы ведут в один и тот же элемент";
                case MISTAKES.OUT_NOT_IN_ACT:
                    return "переход ведет не в активность или в разветвитель";
                case MISTAKES.NEXT_DECISION:
                    return "альтернатива ведет в условный переход";
                default:
                    throw new ArgumentException();
            }
        }
    }
}
}
