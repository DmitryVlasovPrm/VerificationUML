using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Verification.cd_ver.Entities;
using Verification.rating_system;
using Attribute = Verification.cd_ver.Entities.Attribute;

namespace Verification.cd_ver
{
	public static class ExtractElements
	{
		private static bool isThereImage = true;

		public static void Extract(XmlElement root, ref Elements allElements, ref Diagram diagram)
		{
			// Id, type, box
			List<Tuple<string, string, BoundingBox>> graphicInfo = new List<Tuple<string, string, BoundingBox>>();
			if (diagram.Image != null)
			{
				var (realMinX, realMinY) = (30, 30);
				var minX = 10000;
				var minY = 10000;

				// Для отрисовки на png
				var graphics = root.GetElementsByTagName("plane");
				var graphicsCount = graphics.Count;

				for (var i = 0; i < graphicsCount; i++)
				{
					try
					{
						var curContent = graphics[i];
						var parentNode = curContent.ParentNode;
						if (parentNode.Attributes["xsi:type"].Value != "com.genmymodel.graphic.uml:ClassDiagram")
							continue;

						var graphicElements = curContent.SelectNodes("ownedDiagramElements");
						var graphicElementsCount = graphicElements.Count;
						for (var j = 0; j < graphicElementsCount; j++)
						{
							var curElement = graphicElements[j];
							var elementType = curElement.Attributes["xsi:type"].Value;

							var elementId = "";
							if (curElement.Attributes["modelElement"] != null)
								elementId = curElement.Attributes["modelElement"].Value;

							var curX = 0;
							var curY = 0;
							if (curElement.Attributes["x"] != null)
								curX = int.Parse(curElement.Attributes["x"].Value);
							if (curElement.Attributes["y"] != null)
								curY = int.Parse(curElement.Attributes["y"].Value);
							// Для нормировки
							if (curX < minX)
								minX = curX;
							if (curY < minY)
								minY = curY;

							var curW = 0;
							var curH = 0;
							var isVisible = false;
							if (curElement.Attributes["width"] != null)
							{
								curW = int.Parse(curElement.Attributes["width"].Value);
								isVisible = true;
							}
							if (curElement.Attributes["height"] != null)
							{
								curH = int.Parse(curElement.Attributes["height"].Value);
								isVisible = true;
							}
							if (!isVisible)
								continue;

							var bbox = new BoundingBox(curX, curY, curW, curH);
							var item = new Tuple<string, string, BoundingBox>(elementId, elementType, bbox);
							graphicInfo.Add(item);
						}
					}
					catch (Exception ex)
					{
						Main.MainFormInstance.Invoke(new Action(() => { MessageBox.Show("Ошибка",
							"Ошибка в экспорте координат элементов: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error); }));
					}
				}

				// Нормировка
				for (var i = 0; i < graphicInfo.Count; i++)
				{
					var offsetX = realMinX - minX;
					var offsetY = realMinY - minY;
					graphicInfo[i].Item3.X += offsetX;
					graphicInfo[i].Item3.Y += offsetY;
				}
			}
			else
			{
				isThereImage = false;
			}

			// Сами элементы диаграммы
			var xmlElements = root.GetElementsByTagName("packagedElement");
			var elementsCount = xmlElements.Count;

			for (var i = 0; i < elementsCount; i++)
			{
				try
				{
					var curElement = xmlElements[i];
					var elementType = curElement.Attributes["xsi:type"].Value;
					var elementId = curElement.Attributes["xmi:id"].Value;

					var nameAttribute = curElement.Attributes["name"];
					var elementName = nameAttribute != null ? nameAttribute.Value : "";

					switch (elementType)
					{
						case "uml:Package":
							var elementGraphicInfo = graphicInfo.Find(a => a.Item1 == elementId && a.Item2 == "com.genmymodel.graphic.uml:PackageWidget");
							if (elementGraphicInfo == null && isThereImage)
								break;
							var box = elementGraphicInfo?.Item3;
							allElements.Packages.Add(new Package(elementId, elementName, box));
							break;

						case "uml:Class":
						case "uml:Interface":
							var xsiType = "com.genmymodel.graphic.uml:ClassWidget";
							var russianName = "Класс";
							var isInterface = false;
							if (elementType == "uml:Interface")
							{
								xsiType = "com.genmymodel.graphic.uml:InterfaceWidget";
								russianName = "Интерфейс";
								isInterface = true;
							}

							elementGraphicInfo = graphicInfo.Find(a => a.Item1 == elementId && a.Item2 == xsiType);
							if (elementGraphicInfo == null && isThereImage)
								break;
							box = elementGraphicInfo?.Item3;

							var attributes = new List<Attribute>();
							var operations = new List<Operation>();

							// Считываем атрибуты
							var attributeElements = curElement.SelectNodes("ownedAttribute");
							var attribElementsCount = attributeElements.Count;
							for (var k = 0; k < attribElementsCount; k++)
							{
								var curAttrib = attributeElements[k];
								var attribId = curAttrib.Attributes["xmi:id"].Value;
								var attribName = curAttrib.Attributes["name"].Value;

								var attribVisibility = Visibility.none;
								if (curAttrib.Attributes["visibility"] != null)
									attribVisibility = (Visibility)Enum.Parse(typeof(Visibility), curAttrib.Attributes["visibility"].Value, true);

								var attribDataTypeId = GetDataType(curAttrib, box, ref diagram);
								attributes.Add(new Attribute(attribId, attribName, attribVisibility, attribDataTypeId));
							}

							// Считываем операции
							var operationElements = curElement.SelectNodes("ownedOperation");
							var operationElementsCount = operationElements.Count;
							for (var k = 0; k < operationElementsCount; k++)
							{
								var curOperation = operationElements[k];
								var operationId = curOperation.Attributes["xmi:id"].Value;
								var operationName = curOperation.Attributes["name"].Value;

								var returnDataTypeId = "";
								var parameters = new List<Parameter>();
								var parametersElements = curOperation.SelectNodes("ownedParameter");
								var parametersCount = parametersElements.Count;
								for (var t = 0; t < parametersCount; t++)
								{
									var curParameter = parametersElements[t];
									var paramId = curParameter.Attributes["xmi:id"].Value;
									var paramName = curParameter.Attributes["name"].Value;

									// Для возвращаемого значения операции
									if (curParameter.Attributes["direction"] != null &&
										curParameter.Attributes["direction"].Value == "return")
									{
										returnDataTypeId = GetDataType(curParameter, box, ref diagram);
									}
									else
									{
										// Обычный параметр
										var paramDataType = GetDataType(curParameter, box, ref diagram);
										parameters.Add(new Parameter(paramId, paramName, paramDataType));
									}
								}

								var operationVisibility = Visibility.@public;
								if (curOperation.Attributes["visibility"] != null)
									operationVisibility = (Visibility)Enum.Parse(typeof(Visibility),
										curOperation.Attributes["visibility"].Value, true);

								operations.Add(new Operation(operationId, operationName, parameters,
									operationVisibility, returnDataTypeId));
							}

							// Смотрим обобщение (наследование)
							var generalClassesIdxs = new List<string>();
							var generalizationElements = curElement.SelectNodes("generalization");
							var genElementsCount = generalizationElements.Count;
							for (var k = 0; k < genElementsCount; k++)
							{
								var curGeneralization = generalizationElements[k];
								generalClassesIdxs.Add(curGeneralization.Attributes["general"].Value);
							}

							// Смотрим интерфейсы
							var interfaceSuppliersIdxs = new List<string>();
							var interfaceElements = curElement.SelectNodes("interfaceRealization");
							var interfaceElementsCount = interfaceElements.Count;
							for (var k = 0; k < interfaceElementsCount; k++)
							{
								var curRealization = interfaceElements[k];
								interfaceSuppliersIdxs.Add(curRealization.Attributes["supplier"].Value);
							}

							var newClass = new Class(elementId, elementName, box, attributes, operations, generalClassesIdxs, isInterface, interfaceSuppliersIdxs);

							// Проверим на дублирование
							var isExist = allElements.Classes.Exists(a => a.Name == elementName);
							if (isExist)
							{
								diagram.Mistakes.Add(new Mistake(2, $"{russianName} с таким именем уже существует", box, ALL_MISTAKES.CD_DUPLICATE_NAME));
								break;
							}

							allElements.Classes.Add(newClass);
							break;

						case "uml:Association":
							string[] navigableEndIdxs = null;
							if (curElement.Attributes["navigableOwnedEnd"] != null)
								navigableEndIdxs = curElement.Attributes["navigableOwnedEnd"].Value.Split(' ');

							var ownedEnds = curElement.SelectNodes("ownedEnd");
							string ownedElementId1 = "",
								role1 = "",
								mult1 = "",
								ownedElementId2 = "",
								role2 = "",
								mult2 = "";
							BoundingBox box1 = null, box2 = null;
							bool navigalable1 = false, navigalable2 = false;
							ConnectionType connType1 = ConnectionType.Association,
								connType2 = ConnectionType.Association;

							// Смотрим каждый из концов связи
							for (var j = 0; j < 2; j++)
							{
								var curOwnedEnd = ownedEnds[j];
								var curOwnedEndId = curOwnedEnd.Attributes["xmi:id"].Value;
								string curRole = "";
								if (curOwnedEnd.Attributes["name"] != null)
									curRole = curOwnedEnd.Attributes["name"].Value;
								// id элемента, к которому идет данная связь
								var curOwnedElementId = curOwnedEnd.Attributes["type"].Value;

								var curConnectionType = ConnectionType.Association;
								if (curOwnedEnd.Attributes["aggregation"] != null)
									curConnectionType = (ConnectionType)Array.IndexOf(ReservedNames.ConnectionTypes,
										curOwnedEnd.Attributes["aggregation"].Value);

								var curNavigation = false;
								if (navigableEndIdxs != null)
									curNavigation = navigableEndIdxs.Contains(curOwnedEndId);

								var curLowerValue = "0";
								if (curOwnedEnd.SelectSingleNode("lowerValue").Attributes["value"] != null)
									curLowerValue = curOwnedEnd.SelectSingleNode("lowerValue").Attributes["value"].Value;
								var curUpperValue = curOwnedEnd.SelectSingleNode("upperValue").Attributes["value"].Value;
								var curMult = curLowerValue + ".." + curUpperValue;

								elementGraphicInfo = graphicInfo.Find(a => a.Item1 == curOwnedEndId && a.Item2 == "com.genmymodel.graphic.uml:MemberEndMultiplicityWidget");
								var curBox = elementGraphicInfo?.Item3;

								if (j == 0)
								{
									ownedElementId1 = curOwnedElementId;
									role1 = curRole;
									mult1 = curMult;
									box1 = curBox;
									navigalable1 = curNavigation;
									connType1 = curConnectionType;
								}
								else
								{
									ownedElementId2 = curOwnedElementId;
									role2 = curRole;
									mult2 = curMult;
									box2 = curBox;
									navigalable2 = curNavigation;
									connType2 = curConnectionType;
								}
							}

							allElements.Connections.Add(new Connection(elementId, elementName,
								ownedElementId1, role1, mult1, box1, navigalable1, connType1,
								ownedElementId2, role2, mult2, box2, navigalable2, connType2));
							break;

						case "uml:DataType":
							allElements.Types.Add(new DataType(elementId, elementName));
							break;

						case "uml:Usage":
							var clientId = curElement.Attributes["client"].Value;
							var supplierId = curElement.Attributes["supplier"].Value;
							allElements.Dependences.Add(new Dependence(elementId, "use", clientId, supplierId, DependenceType.Usage));
							break;

						case "uml:Dependency":
							clientId = curElement.Attributes["client"].Value;
							supplierId = curElement.Attributes["supplier"].Value;
							allElements.Dependences.Add(new Dependence(elementId, elementName, clientId, supplierId, DependenceType.Dependency));
							break;

						case "uml:Enumeration":
							elementGraphicInfo = graphicInfo.Find(a => a.Item1 == elementId && a.Item2 == "com.genmymodel.graphic.uml:EnumerationWidget");
							if (elementGraphicInfo == null && isThereImage)
								break;
							box = elementGraphicInfo?.Item3;

							var literals = new List<Literal>();
							var literalElements = curElement.SelectNodes("ownedLiteral");
							var literalElementsCount = literalElements.Count;
							for (var k = 0; k < literalElementsCount; k++)
							{
								var curLiteral = literalElements[k];
								var literalId = curLiteral.Attributes["xmi:id"].Value;
								var literalName = curLiteral.Attributes["name"].Value;
								literals.Add(new Literal(literalId, literalName));
							}

							// Проверим на дублирование
							isExist = allElements.Enumerations.Exists(a => a.Name == elementName);
							if (isExist)
							{
								diagram.Mistakes.Add(new Mistake(2, "Перечисление с таким именем уже существует", box, ALL_MISTAKES.CD_DUPLICATE_NAME));
								break;
							}

							allElements.Enumerations.Add(new Enumeration(elementId, elementName, box, literals));
							break;

						default:
							elementGraphicInfo = graphicInfo.Find(a => a.Item1 == elementId);
							if (elementGraphicInfo == null && isThereImage)
								break;
							box = elementGraphicInfo.Item3;
							diagram.Mistakes.Add(new Mistake(2, "Недопустимый элемент", box, ALL_MISTAKES.CD_IMPOSSIBLE_ELEMENT));
							break;
					}
				}
				catch (Exception ex)
				{
					Main.MainFormInstance.Invoke(new Action(() => { MessageBox.Show("Ошибка",
						"Ошибка в экспорте элементов: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error); }));
				}
			}

			// Проверяем комментарии (ограничения)
			var xmlComments = root.GetElementsByTagName("ownedComment");

			var commentsCount = xmlComments.Count;
			for (var i = 0; i < commentsCount; i++)
			{
				try
				{
					var curComment = xmlComments[i];
					var commentId = curComment.Attributes["xmi:id"].Value;
					var elementGraphicInfo = graphicInfo.Find(a => a.Item1 == commentId);
					if (elementGraphicInfo == null && isThereImage)
						break;
					var box = elementGraphicInfo?.Item3;

					var annotatedElementId = "";
					if (curComment.Attributes["annotatedElement"] != null)
						annotatedElementId = curComment.Attributes["annotatedElement"].Value;
					var body = "";
					if (curComment.Attributes["body"] != null)
						body = curComment.Attributes["body"].Value.TrimStart().TrimEnd();

					allElements.Comments.Add(new Comment(commentId, body, box, annotatedElementId));
				}
				catch (Exception ex)
				{
					Main.MainFormInstance.Invoke(new Action(() => { MessageBox.Show("Ошибка",
						"Ошибка в экспорте комментариев: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error); }));
				}
			}
		}

		private static string GetDataType(XmlNode node, BoundingBox box, ref Diagram diagram)
		{
			var dataTypeId = "";
			if (node.Attributes["type"] != null)
			{
				dataTypeId = node.Attributes["type"].Value;
			}
			else
			{
				var typeNode = node.SelectSingleNode("type");
				if (typeNode != null)
				{
					var nameInfo = typeNode.Attributes["href"].Value;
					var name = nameInfo.Substring(nameInfo.LastIndexOf("//") + 2);
					diagram.Mistakes.Add(new Mistake(1, $"Имя типа \"{name}\" не соответствует целевому языку программирования", box, ALL_MISTAKES.CD_IMPOSSIBLE_TYPE));
					dataTypeId = "primitiveType";
				}
			}
			return dataTypeId;
		}
	}
}