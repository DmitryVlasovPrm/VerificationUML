using System;

namespace ActivityDiagramVer.entities
{
    public enum ElementType
    {
        UNKNOWN,
        FLOW,
        ACTIVITY,
        FORK,
        JOIN,
        DECISION,
        MERGE,
        INITIAL_NODE,
        FINAL_NODE,
        SWIMLANE
    }

    public class ElementTypeAdapter
    {
        public static String toString(ElementType type)
        {
            switch (type)
            {
                case ElementType.DECISION: return "Условный переход";
                case ElementType.ACTIVITY: return "Активность";
                case ElementType.MERGE: return "Узел слияния";
                case ElementType.JOIN: return "Синхронизатор";
                case ElementType.FORK: return "Разветвитель";
                case ElementType.FLOW: return "Переход";
                case ElementType.FINAL_NODE: return "Конечное состояние";
                case ElementType.INITIAL_NODE: return "Начальное состояние";
                case ElementType.SWIMLANE: return "Дорожка участника";
                case ElementType.UNKNOWN: return "";
                default: throw new ArgumentException();
            }
        }
    }
}


