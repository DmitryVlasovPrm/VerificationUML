using System;
using System.Windows.Forms;
using Verification.cd_ver.Entities;
using Verification.rating_system;

namespace Verification.cd_ver
{
    public static class Analysis
    {
        // Лексический анализ с элементами семантики (для простоты кода)
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
                    var curItemId = curItem.Id;
                    var elementType = curItem.IsInterface ? "интерфейс" : "класс";
                    var upperElementType = elementType.Substring(0, 1).ToUpper() + elementType.Substring(1);
                    var curItemName = curItem.Name;

                    // Проверка наличия связей (семантика)
                    if (curItem.GeneralClassesIdxs.Count == 0)
                    {
                        var connection = allElements.Connections.Find(a => a.OwnedElementId1 == curItemId || a.OwnedElementId2 == curItemId);
                        if (connection == null)
                        {
                            var dependency = allElements.Dependences.Find(a => a.ClientElementId == curItemId || a.SupplierElementId == curItemId);
                            if (dependency == null)
                            {
                                var parent = allElements.Classes.Find(a => a.GeneralClassesIdxs.Contains(curItemId));
                                if (parent == null)
                                    diagram.Mistakes.Add(new Mistake(2, $"{upperElementType} \"{curItemName}\" не имеет связей", curItem.Box, ALL_MISTAKES.CDNOLINK));
                            }
                        }
                    }

                    // Если есть protected, смотрим, есть ли потомки
                    if (curItem.Attributes.Find(a => a.Visibility == Visibility.@protected) != null ||
                        curItem.Operations.Find(a => a.Visibility == Visibility.@protected) != null)
                    {
                        if (allElements.Classes.Find(a => a.GeneralClassesIdxs.Contains(curItemId)) == null)
                            diagram.Mistakes.Add(new Mistake(2, $"{upperElementType} \"{curItemName}\" с элементами \"protected\" не имеет потомков", curItem.Box, ALL_MISTAKES.CDNOCHILDREN));
                    }

                    // Проверка названий
                    if (!char.IsUpper(curItemName[0]))
                        diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType}а \"{curItemName}\" начинается с маленькой буквы", curItem.Box, ALL_MISTAKES.CDSMALLLETTER));
                    if (curItemName.Contains(" "))
                        diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType}а \"{curItemName}\" содержит пробелы", curItem.Box, ALL_MISTAKES.CDGETBLANKS));

                    // Проверка наличия атрибутов и операторов
                    var attributes = curItem.Attributes;
                    var attributesCount = attributes.Count;
                    var operations = curItem.Operations;
                    var operationsCount = operations.Count;
                    if (attributesCount == 0 && operationsCount == 0)
                    {
                        diagram.Mistakes.Add(new Mistake(2, $"{upperElementType} \"{curItemName}\" должен иметь хотя бы один параметр или операцию", curItem.Box, ALL_MISTAKES.CDMUSTBEOPER));
                        continue;
                    }

                    // Рассматриваем атрибуты
                    for (var j = 0; j < attributesCount; j++)
                    {
                        var curAttribute = attributes[j];
                        var curAttributeName = curAttribute.Name;

                        if (curAttributeName != "")
                        {
                            if (char.IsUpper(curAttributeName[0]))
                                diagram.Mistakes.Add(new Mistake(1, $"Имя атрибута \"{curAttributeName}\" начинается с большой буквы ({elementType} \"{curItemName}\")", curItem.Box, ALL_MISTAKES.CDBIGLETTER));
                            if (curAttributeName.Contains(" "))
                                diagram.Mistakes.Add(new Mistake(1, $"Имя атрибута \"{curAttributeName}\" содержит пробелы ({elementType} \"{curItemName}\")", curItem.Box, ALL_MISTAKES.CDGETBLANKS2));
                        }
                        // Проверяем типы
                        if (curAttribute.TypeId == "primitive")
                            continue;
                    }

                    // Рассматриваем операции
                    for (var j = 0; j < operationsCount; j++)
                    {
                        var curOperation = operations[j];
                        var curOperationName = curOperation.Name;
                        var constr = false;
                        var destr = false;

                        if (curOperationName != "")
                        {
                            // Конструктор
                            if (curItemName.ToLower() == curOperationName.ToLower())
                            {
                                if (char.IsLower(curOperationName[0]))
                                    diagram.Mistakes.Add(new Mistake(1, $"Имя конструктора \"{curOperationName}\" начинается с маленькой буквы ({elementType} \"{curItemName}\")", curItem.Box, ALL_MISTAKES.CDCONSTRUCTORHASSMALLLETTER));
                                constr = true;
                            }
                            // Деструктор
                            else if (curOperationName[0] == '~')
                            {
                                if (char.IsLower(curOperationName.Substring(1).TrimStart()[0]))
                                    diagram.Mistakes.Add(new Mistake(1, $"Имя деструктора \"{curOperationName}\" начинается с маленькой буквы ({elementType} \"{curItemName}\")", curItem.Box, ALL_MISTAKES.CDDESTRUCTORHASSMALLLETTER));
                                destr = true;
                            }
                            // Остальные типы
                            else
                            {
                                if (char.IsUpper(curOperationName[0]))
                                    diagram.Mistakes.Add(new Mistake(1, $"Имя операции \"{curOperationName}\" начинается с большой буквы ({elementType} \"{curItemName}\")", curItem.Box, ALL_MISTAKES.CDOPERSTARTWITHBIGLETTER));
                                if (curOperationName.Contains(" "))
                                    diagram.Mistakes.Add(new Mistake(1, $"Имя операции \"{curOperationName}\" содержит пробелы ({elementType} \"{curItemName}\")", curItem.Box, ALL_MISTAKES.CDOPERHASBLANKS));
                            }
                        }

                        // Проверяем типы параметров
                        var paramsCount = curOperation.Parameters.Count;
                        for (var k = 0; k < paramsCount; k++)
                        {
                            var curParam = curOperation.Parameters[k];
                            if (curParam.DataTypeId == "primitive")
                                continue;
                        }

                        // Синтаксическая часть
                        // Возвращаемое значение
                        if ((constr || destr) && curOperation.ReturnDataTypeId != "")
                        {
                            var typeStr = constr ? "Конструктор" : "Деструктор";
                            diagram.Mistakes.Add(new Mistake(2, $"{typeStr} \"{curOperationName}\" имеет возвращаемый тип ({elementType} \"{curItemName}\")", curItem.Box, ALL_MISTAKES.CDHASOUTPUTTYPE));
                        }
                        if (!constr && !destr && curOperation.ReturnDataTypeId == "")
                            diagram.Mistakes.Add(new Mistake(1, $"Не указан возвращаемый тип операции \"{curOperationName}\" ({elementType} \"{curItemName}\")", curItem.Box, ALL_MISTAKES.CDPOINTOUTPUTOPERATIONTYPE));
                    }
                }

                // Для перечислений
                var enumElements = allElements.Enumerations;
                elementsCount = enumElements.Count;
                for (var i = 0; i < elementsCount; i++)
                {
                    var curItem = allElements.Enumerations[i];
                    var curItemId = curItem.Id;
                    var curItemName = curItem.Name;

                    // Проверка наличия связей (семантика)
                    var dependency = allElements.Dependences.Find(a => a.ClientElementId == curItemId || a.SupplierElementId == curItemId);
                    if (dependency == null)
                        diagram.Mistakes.Add(new Mistake(2, $"Перечисление \"{curItemName}\" не имеет допустимых связей", curItem.Box, ALL_MISTAKES.CDNOAVAILABLELINKS));

                    if (!char.IsUpper(curItemName[0]))
                        diagram.Mistakes.Add(new Mistake(1, $"Имя перечисления \"{curItemName}\" начинается с маленькой буквы", curItem.Box, ALL_MISTAKES.CDENUMSTARTWITHSMALLLETTER));
                    if (curItemName.Contains(" "))
                        diagram.Mistakes.Add(new Mistake(1, $"Имя перечисления \"{curItemName}\" содержит пробелы", curItem.Box, ALL_MISTAKES.CDENUMHASBLANKS));
                }

                // Комментарии в скобках {}
                var commentsCount = allElements.Comments.Count;
                for (var i = 0; i < commentsCount; i++)
                {
                    var curComment = allElements.Comments[i];
                    var body = curComment.Body;
                    if (body != "" && (body[0] != '{' || body[body.Length - 1] != '}'))
                        diagram.Mistakes.Add(new Mistake(0, "Ограничение должно записываться в скобках {}", curComment.Box, ALL_MISTAKES.CDRESTRICTIONHASNOTBRANKETS));
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
                // Наличие элемента Package
                if (allElements.Packages.Count == 0)
                    diagram.Mistakes.Add(new Mistake(0, "Отсутствует элемент Package", null, ALL_MISTAKES.CDNOPACKAGE));

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
                        if (mainClass == null || subordinateClass == null)
                            continue;
                        CheckSyntacticMistake1(allElements, mainClass, subordinateClass, ref diagram);
                    }

                    if (curConnection.ConnectionType2 == ConnectionType.CompositeAggregation ||
                        curConnection.ConnectionType2 == ConnectionType.SharedAggregation)
                    {
                        var mainClass = allElements.Classes.Find(a => a.Id == curConnection.OwnedElementId1);
                        var subordinateClass = allElements.Classes.Find(a => a.Id == curConnection.OwnedElementId2);
                        if (mainClass == null || subordinateClass == null)
                            continue;
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

                if (curType != null && curType.IsContainer(subordinateClass.Name))
                {
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                var mainElementType = mainClass.IsInterface ? "Интерфейс" : "Класс";
                var subordinateElementType = subordinateClass.IsInterface ? "интерфейс" : "класс";
                diagram.Mistakes.Add(new Mistake(0, $"{mainElementType} \"{mainClass.Name}\" не имеет контейнера для объектов {subordinateElementType}а \"{subordinateClass.Name}\"", mainClass.Box, ALL_MISTAKES.CDNOCONTAINER));
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
                        var num1 = numbers[0] == "*" ? int.MaxValue : int.Parse(numbers[0]);
                        var num2 = numbers[1] == "*" ? int.MaxValue : int.Parse(numbers[1]);

                        if (num1 < 0 || num2 < 0)
                            diagram.Mistakes.Add(new Mistake(1, $"Значение кратности {multiplicity} меньше нуля", bbox, ALL_MISTAKES.CDLESSZERO));
                        if (num1 > num2)
                            diagram.Mistakes.Add(new Mistake(1, $"Диапазон {multiplicity} записан неверно", bbox, ALL_MISTAKES.CDWRONGDIAPOSON));
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