using System;
using System.Windows.Forms;
using System.Linq;
using Verification.cd_ver.Entities;

namespace Verification.cd_ver
{
    public static class Analysis
    {
        private static string[] AllTypes =
        {
            "int", "string", "float", "double", "bool", "List<T>", "List<string>",
            "List<Variable>", "List<Domain>", "List<Box>"
        };

        // Лексический анализ
        public static void LexicalAnalysis(Elements allElements, ref Diagram diagram)
        {
            try
            {
                // Для классов и интерфейсов
                var curElements = allElements.Classes;
                var elementsCount = curElements.Count;
                for (var i = 0; i < elementsCount; i++)
                {
                    var curItem = curElements[i];
                    var elementType = curItem.IsInterface ? "интерфейс" : "класс";
                    var curItemName = curItem.Name;
                    
                    if (!char.IsUpper(curItemName[0]))
                        diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType}а начинается с маленькой буквы: \"{curItemName}\"", curItem.Box));
                    if (curItemName.Contains(" "))
                        diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType}а содержит пробелы: \"{curItemName}\"", curItem.Box));

                    var attributes = curItem.Attributes;
                    var attributesCount = attributes.Count;
                    for (var j = 0; j < attributesCount; j++)
                    {
                        var curAttribute = attributes[j];
                        var curAttributeName = curAttribute.Name;
                        if (char.IsUpper(curAttributeName[0]))
                            diagram.Mistakes.Add(new Mistake(1, $"Имя атрибута начинается с большой буквы ({elementType} \"{curItemName}\"): \"{curAttributeName}\"", curItem.Box));
                        if (curAttributeName.Contains(" "))
                            diagram.Mistakes.Add(new Mistake(1, $"Имя атрибута содержит пробелы ({elementType} \"{curItemName}\"): \"{curAttributeName}\"", curItem.Box));
                    
                        var curType = allElements.Types.Find(a => a.Id == curAttribute.TypeId);
                        if (curType != null && !AllTypes.Contains(curType.Name) && allElements.Classes.FindIndex(a => a.Id == curAttribute.TypeId) == -1)
                            diagram.Mistakes.Add(new Mistake(1, $"Неверное имя типа ({elementType} \"{curItemName}\"): \"{curType.Name}\"", curItem.Box));
                    }
                }

                // Для перечислений
                var enumElements = allElements.Enumerations;
                elementsCount = enumElements.Count;
                for (var i = 0; i < elementsCount; i++)
                {
                    var curItem = allElements.Enumerations[i];
                    var curItemName = curItem.Name;

                    if (!char.IsUpper(curItemName[0]))
                        diagram.Mistakes.Add(new Mistake(1, $"Имя перечисления начинается с маленькой буквы: \"{curItemName}\"", curItem.Box));
                    if (curItemName.Contains(" "))
                        diagram.Mistakes.Add(new Mistake(1, $"Имя перечисления содержит пробелы: \"{curItemName}\"", curItem.Box));
                }

                // Комментарии в скобках {}
                var commentsCount = allElements.Comments.Count;
                for (var i = 0; i < commentsCount; i++)
				{
                    var curComment = allElements.Comments[i];
                    var body = curComment.Body;
                    if (body != "" && (body[0] != '{' || body[body.Length - 1] != '}'))
                        diagram.Mistakes.Add(new Mistake(0, "Ограничение должно записываться в скобках {}", curComment.Box));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in LexicalAnalysis: " + ex.Message);
            }
        }

        // Синтаксический анализ
        public static void SyntacticAnalysis(Elements allElements, ref Diagram diagram)
        {
            try
            {
                var connectionsCount = allElements.Connections.Count;
                for (var i = 0; i < connectionsCount; i++)
                {
                    var curConnection = allElements.Connections[i];

                    // Проверка композиции или агрегации в главном элементе
                    if (curConnection.ConnectionType1 == ConnectionType.CompositeAggregation ||
                        curConnection.ConnectionType1 == ConnectionType.SharedAggregation)
                    {
                        var mainClass = allElements.Classes.Find(a => a.Id == curConnection.OwnedElementId2);
                        var subordinateClass = allElements.Classes.Find(a => a.Id == curConnection.OwnedElementId1);
                        CheckSyntacticMistake1(allElements, mainClass, subordinateClass, ref diagram);
                    }

                    if (curConnection.ConnectionType2 == ConnectionType.CompositeAggregation ||
                        curConnection.ConnectionType2 == ConnectionType.SharedAggregation)
                    {
                        var mainClass = allElements.Classes.Find(a => a.Id == curConnection.OwnedElementId1);
                        var subordinateClass = allElements.Classes.Find(a => a.Id == curConnection.OwnedElementId2);
                        CheckSyntacticMistake1(allElements, mainClass, subordinateClass, ref diagram);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SyntacticAnalysis: " + ex.Message);
            }
        }

        // Проверка композиции или агрегации в главном элементе
        private static void CheckSyntacticMistake1(Elements allElements, Class mainClass, Class subordinateClass, ref Diagram diagram)
		{
            var mainAttributes = mainClass.Attributes;
            var mainAttributesCount = mainAttributes.Count;

            var isExist = false;
            for (var i = 0; i < mainAttributesCount; i++)
            {
                var curAttribute = mainAttributes[i];
                var curTypeId = curAttribute.TypeId;
                var curType = allElements.Types.Find(a => a.Id == curTypeId);

                if (curType != null && curType.IsContainer && curType.Name.Contains(subordinateClass.Name))
                {
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                var mainElementType = mainClass.IsInterface ? "Интерфейс" : "Класс";
                var subordinateElementType = subordinateClass.IsInterface ? "интерфейс" : "класс";
                diagram.Mistakes.Add(new Mistake(0, $"{mainElementType} \"{mainClass.Name}\" не имеет контейнера для объектов {subordinateElementType}а \"{subordinateClass.Name}\"", mainClass.Box));
            }
        }

        // Семантический анализ
        public static void SemanticAnalysis(Elements allElements, ref Diagram diagram)
        {
            try
            {
                var connectionsCount = allElements.Connections.Count;
                for (var i = 0; i < connectionsCount; i++)
				{
                    var curConnection = allElements.Connections[i];
                    for (var j = 0; j < 2; j++)
                    {
                        var multiplicity = j == 0 ? curConnection.Multiplicity1 : curConnection.Multiplicity2;
                        var bbox = j == 0 ? curConnection.Box1 : curConnection.Box2;
                        var numbers = multiplicity.Split(new string[] { ".." }, StringSplitOptions.None);
                        var num1 = numbers[0] == "*" ? Int32.MaxValue : int.Parse(numbers[0]);
                        var num2 = numbers[1] == "*" ? Int32.MaxValue : int.Parse(numbers[1]);

                        if (num1 < 0 || num2 < 0)
                            diagram.Mistakes.Add(new Mistake(1, "Значение кратности меньше нуля", bbox));
                        if (num1 > num2)
                            diagram.Mistakes.Add(new Mistake(1, "Диапазон записан неверно", bbox));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SemanticAnalysis: " + ex.Message);
            }
        }
    }
}