using Verification.rating_system;
using System.Linq;
using Verification.cd_ver.Entities;

namespace Verification.cd_ver
{
	public static class Functions
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
					Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType} \"{elementName}\" начинается с цифры", elementBox, ALL_MISTAKES.CD_DIGIT));

				if (!char.IsUpper(elementName[0]))
					Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType} \"{elementName}\" начинается с маленькой буквы", elementBox, ALL_MISTAKES.CD_SMALL_LETTER));

				if (!elementName.All(char.IsLetterOrDigit))
					Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя {elementType} \"{elementName}\" содержит недопустимые символы", elementBox, ALL_MISTAKES.CD_IMPOSSIBLE_SYMBOLS));
			}
		}

		public static (bool, bool) CheckOperationName(string operationName, string elementName, string elementType, BoundingBox elementBox)
		{
			var constr = false;
			var destr = false;

			if (operationName != "")
			{
				// Формирование имени типа элемента
				if (elementType == "класс" || elementType == "интерфейс")
					elementType += "а";

				// Конструктор
				if (elementName.ToLower() == operationName.ToLower())
				{
					if (char.IsLower(operationName[0]))
						Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя конструктора \"{operationName}\" начинается с маленькой буквы ({elementType} \"{elementName}\")",
							elementBox, ALL_MISTAKES.CD_CONSTRUCTOR_HAS_SMALL_LETTER));
					constr = true;
				}
				// Деструктор
				else if (operationName[0] == '~')
				{
					if (char.IsLower(operationName.Substring(1).TrimStart()[0]))
						Analysis.Diagram.Mistakes.Add(new Mistake(1, $"Имя деструктора \"{operationName}\" начинается с маленькой буквы ({elementType} \"{elementName}\")",
							elementBox, ALL_MISTAKES.CD_DESTRUCTOR_HAS_SMALL_LETTER));
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
