using System;
using System.Windows.Forms;
using Verification.cd_ver.Entities;
using Verification.rating_system;

namespace Verification.cd_ver
{
    public static class Analysis
    {
        public static Diagram Diagram;
        // Лексический анализ с элементами семантики (для простоты кода)
        public static void LexicalAnalysis(Elements allElements, ref Diagram diagram)
        {
            Diagram = diagram;
            
            try
            {
                // Для классов и интерфейсов
                var curElements = allElements.Classes;
                var elementsCount = curElements.Count;
                for (var i = 0; i < elementsCount; i++)
                {
                    var element = curElements[i];
                    var elementId = element.Id;
                    var elementName = element.Name;
                    var elementType = element.IsInterface ? "интерфейс" : "класс";
                    var upperElementType = elementType.Substring(0, 1).ToUpper() + elementType.Substring(1);

                    // Проверка наличия связей (семантика)
                    Checkers.CheckConnectionExistence(allElements, element, upperElementType);

                    // Если есть protected, смотрим, есть ли потомки
                    if (element.Attributes.Find(a => a.Visibility == Visibility.@protected) != null ||
                        element.Operations.Find(a => a.Visibility == Visibility.@protected) != null)
                    {
                        if (allElements.Classes.Find(a => a.GeneralClassesIdxs.Contains(elementId)) == null)
                            diagram.Mistakes.Add(new Mistake(2, $"{upperElementType} \"{elementName}\" с элементами \"protected\" не имеет потомков", element.Box, ALL_MISTAKES.CD_NO_CHILDREN));
                    }

                    // Проверка имени класса (интерфейса)
                    Checkers.CheckName(elementName, elementType, element.Box);

                    // Проверка наличия атрибутов и операторов
                    var attributes = element.Attributes;
                    var attributesCount = attributes.Count;
                    var operations = element.Operations;
                    var operationsCount = operations.Count;
                    if (attributesCount == 0)
                    {
                        if (operationsCount == 0)
                        {
                            diagram.Mistakes.Add(new Mistake(2, $"{upperElementType} \"{elementName}\" должен иметь хотя бы один атрибут или операцию", element.Box, ALL_MISTAKES.CD_EMPTY_CLASS));
                            continue;
                        }
                        else
                        {
                            // Нет раздела с атрибутами
                            diagram.Mistakes.Add(new Mistake(0, $"{upperElementType} \"{elementName}\" не имеет раздела с атрибутами", element.Box, ALL_MISTAKES.CD_MUST_BE_ATTRIB));
                        }
                    }

                    // Рассматриваем атрибуты
                    for (var j = 0; j < attributesCount; j++)
                    {
                        var attribute = attributes[j];
                        // Проверка имени атрибута
                        Checkers.CheckName(attribute.Name, "атрибут", element.Box);
                        // Проверка типа атрибута
                        Checkers.CheckType(attribute.DataTypeId);
                    }

                    // Рассматриваем операции
                    for (var j = 0; j < operationsCount; j++)
                    {
                        var operation = operations[j];
                        var operationName = operation.Name;

                        // Проверяем название операции
                        var isConstrDestr = Checkers.CheckOperationName(operation, elementName, elementType, element.Box);

                        // Проверяем типы параметров
                        var paramsCount = operation.Parameters.Count;
                        for (var k = 0; k < paramsCount; k++)
                            Checkers.CheckType(operation.Parameters[k].DataTypeId);

                        // Синтаксическая часть
                        // Возвращаемое значение
                        if ((isConstrDestr.Item1 || isConstrDestr.Item2) && operation.ReturnDataTypeId != "")
                        {
                            var typeStr = isConstrDestr.Item1 ? "Конструктор" : "Деструктор";
                            diagram.Mistakes.Add(new Mistake(2, $"{typeStr} \"{operationName}\" имеет возвращаемый тип ({elementType} \"{elementName}\")", element.Box, ALL_MISTAKES.CD_HAS_OUTPUT_TYPE));
                        }
                        if (!isConstrDestr.Item1 && !isConstrDestr.Item2 && operation.ReturnDataTypeId == "")
                            diagram.Mistakes.Add(new Mistake(1, $"Не указан возвращаемый тип операции \"{operationName}\" ({elementType} \"{elementName}\")", element.Box, ALL_MISTAKES.CD_HAS_NOT_OUTPUT_TYPE));
                    }
                }

                // Для перечислений
                var enumElements = allElements.Enumerations;
                elementsCount = enumElements.Count;
                for (var i = 0; i < elementsCount; i++)
                {
                    var enumeration = allElements.Enumerations[i];
                    var enumerationId = enumeration.Id;
                    var enumerationName = enumeration.Name;

                    // Проверка наличия связей (семантика)
                    var dependency = allElements.Dependences.Find(a => a.ClientElementId == enumerationId || a.SupplierElementId == enumerationId);
                    if (dependency == null)
                        diagram.Mistakes.Add(new Mistake(2, $"Перечисление \"{enumerationName}\" не имеет допустимых связей", enumeration.Box, ALL_MISTAKES.CD_NO_AVAILABLE_LINKS));

                    Checkers.CheckName(enumerationName, "перечисление", enumeration.Box);
                }

                // Комментарии в скобках {}
                var commentsCount = allElements.Comments.Count;
                for (var i = 0; i < commentsCount; i++)
                    Checkers.CheckComment(allElements.Comments[i]);
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
                // Наличие элемента Package
                if (allElements.Packages.Count == 0)
                    diagram.Mistakes.Add(new Mistake(0, "Отсутствует элемент Package", null, ALL_MISTAKES.CD_NO_PACKAGE));

                var connectionsCount = allElements.Connections.Count;
                for (var i = 0; i < connectionsCount; i++)
                {
                    var connection = allElements.Connections[i];

                    if (connection.ConnectionType1 == ConnectionType.CompositeAggregation ||
                        connection.ConnectionType1 == ConnectionType.SharedAggregation)
                    {
                        var mainClass = allElements.Classes.Find(a => a.Id == connection.OwnedElementId2);
                        var subordinateClass = allElements.Classes.Find(a => a.Id == connection.OwnedElementId1);
                        if (mainClass == null || subordinateClass == null)
                            continue;

                        // Проверка композиции или агрегации в главном элементе
                        Checkers.CheckCompositionOrAggregation(allElements, mainClass, subordinateClass);
                    }

                    if (connection.ConnectionType2 == ConnectionType.CompositeAggregation ||
                        connection.ConnectionType2 == ConnectionType.SharedAggregation)
                    {
                        var mainClass = allElements.Classes.Find(a => a.Id == connection.OwnedElementId1);
                        var subordinateClass = allElements.Classes.Find(a => a.Id == connection.OwnedElementId2);
                        if (mainClass == null || subordinateClass == null)
                            continue;

                        // Проверка композиции или агрегации в главном элементе
                        Checkers.CheckCompositionOrAggregation(allElements, mainClass, subordinateClass);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SyntacticAnalysis: " + ex.Message);
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
                    var connection = allElements.Connections[i];
                    for (var j = 0; j < 2; j++)
                    {
                        var multiplicity = j == 0 ? connection.Multiplicity1 : connection.Multiplicity2;
                        var bbox = j == 0 ? connection.Box1 : connection.Box2;
                        var numbers = multiplicity.Split(new string[] { ".." }, StringSplitOptions.None);
                        var num1 = numbers[0] == "*" ? int.MaxValue : int.Parse(numbers[0]);
                        var num2 = numbers[1] == "*" ? int.MaxValue : int.Parse(numbers[1]);

                        if (num1 < 0 || num2 < 0)
                            diagram.Mistakes.Add(new Mistake(1, $"Значение кратности {multiplicity} меньше нуля", bbox, ALL_MISTAKES.CD_LESS_ZERO));
                        if (num1 > num2)
                            diagram.Mistakes.Add(new Mistake(1, $"Диапазон {multiplicity} записан неверно", bbox, ALL_MISTAKES.CD_WRONG_RANGE));
                    }
                }

                /*
                // Проверка циклов для обобщений
                var generalizationGraph = new Graph();
                var curElements = allElements.Classes;
                var elementsCount = curElements.Count;

                for (var i = 0; i < elementsCount; i++)
                {
                    var element = curElements[i];
                    var elementId = element.Id;

                    // Строим граф для поиска циклов в обобщениях
                    var generalClassesCount = element.GeneralClassesIdxs.Count;
                    for (var j = 0; j < generalClassesCount; j++)
                        generalizationGraph.AddVertex(elementId, element.GeneralClassesIdxs[j]);
                }

                if (generalizationGraph.IsCycleExist())
                    diagram.Mistakes.Add(new Mistake(2, $"Цикл между обобщениями", null, ALL_MISTAKES.CD_GENERALIZATION_CYCLE));
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SemanticAnalysis: " + ex.Message);
            }
        }
    }
}