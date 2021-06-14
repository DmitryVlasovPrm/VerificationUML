using Verification.rating_system;
using System.Linq;
using Verification.cd_ver.Entities;

namespace Verification.cd_ver
{
	public static class Checkers
	{
		public static void CheckName(string elementName, string elementType, BoundingBox elementBox)
		{
			if (elementName != "")
			{
				// Формирование имени типа элемента
				if (elementType == "класс" || elementType == "интерфейс" || elementType == "атрибут")
					elementType += "а";
				if (elementType == "операция")
					elementType = "операции";
				if (elementType == "перечисление")
					elementType = "перечисления";

				if (char.IsDigit(elementName[0]))
					Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType} \"{elementName}\" начинается с цифры", elementBox, ALL_MISTAKES.CD_INCORRECT_NAME));

				if (!char.IsUpper(elementName[0]))
					Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType} \"{elementName}\" начинается с маленькой буквы", elementBox, ALL_MISTAKES.CD_INCORRECT_NAME));

				if (!elementName.All(char.IsLetterOrDigit))
					Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType} \"{elementName}\" содержит недопустимые символы", elementBox, ALL_MISTAKES.CD_INCORRECT_NAME));
			}
		}

		public static (bool, bool) CheckOperationName(Operation operation, string elementName, string elementType, BoundingBox elementBox)
		{
			var constr = false;
			var destr = false;
			var operationName = operation.Name;

			if (operationName != "")
			{
				// Проверка setter'а
				if (operationName.TrimStart().StartsWith("set"))
				{
					if (operation.Parameters.Count == 0)
						Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Setter \"{operationName}\" не имеет ни один параметр ({elementType} \"{elementName}\")",
							elementBox, ALL_MISTAKES.CD_SETTER_WITHOUT_PARAMS));
				}

				// Проверка getter'а
				if (operationName.TrimStart().StartsWith("get"))
				{
					if (operation.Parameters.Count != 0)
						Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Getter \"{operationName}\" имеет параметр(ы) ({elementType} \"{elementName}\")",
							elementBox, ALL_MISTAKES.CD_GETTER_WITH_PARAMS));
				}

				// Конструктор
				if (elementName.ToLower() == operationName.ToLower())
				{
					if (char.IsLower(operationName[0]))
						Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя конструктора \"{operationName}\" начинается с маленькой буквы ({elementType} \"{elementName}\")",
							elementBox, ALL_MISTAKES.CD_INCORRECT_NAME));
					constr = true;
				}
				// Деструктор
				else if (operationName[0] == '~')
				{
					if (char.IsLower(operationName.Substring(1).TrimStart()[0]))
						Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя деструктора \"{operationName}\" начинается с маленькой буквы ({elementType} \"{elementName}\")",
							elementBox, ALL_MISTAKES.CD_INCORRECT_NAME));
					destr = true;
				}
				// Остальные типы
				else
				{
					CheckName(operationName, "операция", elementBox);
				}
			}

			return (constr, destr);
		}

		public static void CheckType(string typeId)
		{
			// Analysis.Diagram.Mistakes.Add(new Mistake(0, null, null, ALL_MISTAKES.CD_IMPOSSIBLE_TYPE));
		}

		public static void CheckComment(Comment comment)
		{
			var body = comment.Body;
			if (body != "" && (body[0] != '{' || body[body.Length - 1] != '}'))
				Analysis.Diagram.Mistakes.Add(new Mistake(0, "Ограничение должно записываться в скобках {}", comment.Box, ALL_MISTAKES.CD_RESTRICTION_HAS_NO_BRACKETS));
		}

		// Поиск каких любой связей элемента
		public static void CheckConnectionExistence(Elements allElements, Class element, string upperElementType)
		{
			if (element.GeneralClassesIdxs.Count == 0)
			{
				var connection = allElements.Connections.Find(a => a.OwnedElementId1 == element.Id || a.OwnedElementId2 == element.Id);
				if (connection == null)
				{
					var dependency = allElements.Dependences.Find(a => a.ClientElementId == element.Id || a.SupplierElementId == element.Id);
					if (dependency == null)
					{
						var parent = allElements.Classes.Find(a => a.GeneralClassesIdxs.Contains(element.Id));
						if (parent == null)
							Analysis.Diagram.Mistakes.Add(new Mistake(2, $"{upperElementType} \"{element.Name}\" не имеет связей", element.Box, ALL_MISTAKES.CD_NO_LINK));
					}
				}
			}
		}

		// Проверка композиции или агрегации в главном элементе
		public static void CheckCompositionOrAggregation(Elements allElements, Class mainClass, Class subordinateClass)
		{
			var mainAttributes = mainClass.Attributes;
			var mainAttributesCount = mainAttributes.Count;

			var isExist = false;
			for (var i = 0; i < mainAttributesCount; i++)
			{
				var attribute = mainAttributes[i];
				var dataTypeId = attribute.DataTypeId;
				var type = allElements.Types.Find(a => a.Id == dataTypeId);

				if (type != null && type.IsContainer(subordinateClass.Name))
				{
					isExist = true;
					break;
				}

				if (type == null)
				{
					var typeName = allElements.Classes.Find(a => a.Id == dataTypeId);
					if (typeName != null && typeName.Name == subordinateClass.Name)
					{
						isExist = true;
						break;
					}
				}
			}

			if (!isExist)
			{
				var mainElementType = mainClass.IsInterface ? "Интерфейс" : "Класс";
				var subordinateElementType = subordinateClass.IsInterface ? "интерфейс" : "класс";
				Analysis.Diagram.Mistakes.Add(new Mistake(0, $"{mainElementType} \"{mainClass.Name}\" не имеет контейнера для объектов {subordinateElementType}а \"{subordinateClass.Name}\"", mainClass.Box, ALL_MISTAKES.CD_NO_CONTAINER));
			}
		}
	}
}
