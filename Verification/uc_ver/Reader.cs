using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Verification.uc_ver
{
    internal class Reader
    {
        private readonly Dictionary<string, Element> elements;
        private readonly List<Mistake> mistakes;
        private readonly Diagram diagram;


        public Reader(Dictionary<string, Element> elements, Diagram diagram)
        {
            this.elements = elements;
            mistakes = diagram.Mistakes;
            this.diagram = diagram;
        }

        public void ReadData(XmlElement root)
        {
            foreach (XmlNode childnode in root.FirstChild.ChildNodes)
            {
                if (childnode.Name == "packageImport" || childnode.Name == "xmi:Extension")
                    continue;

                string type = getType(childnode),
                parent = getParent(childnode),
                name = getName(childnode),
                id = getId(childnode);

                if (ElementTypes.List.Contains(type))
                {
                    switch (type)
                    {
                        case ElementTypes.Association:
                            {
                                // обработка ассоциации
                                string from = childnode.ChildNodes[1].Attributes.GetNamedItem("type")?.Value,
                                    to = childnode.ChildNodes[2].Attributes.GetNamedItem("type")?.Value;
                                elements.Add(id, new Arrow(id, type, name, parent, from, to));
                                break;
                            }
                        case ElementTypes.Actor:
                            {
                                ReadActor(childnode);
                                var actor = new Element(id, type, name, parent);
                                elements.Add(id, actor);
                                if (!string.IsNullOrWhiteSpace(name))
                                    diagram.Actors.Add(actor);

                                break;
                            }
                        case ElementTypes.Package:
                            {
                                ReadPackage(childnode);
                                elements.Add(id, new Element(id, type, name, parent));
                                break;
                            }
                        case ElementTypes.Precedent:
                            {
                                elements.Add(id, new Element(id, type, name, parent));
                                ReadPrecedent(childnode);
                                break;
                            }
                        default:
                            {
                                if (childnode.Name == ElementTypes.Comment)
                                {
                                    type = ElementTypes.Comment;
                                    string to = childnode.Attributes.GetNamedItem("annotatedElement")?.Value;
                                    name = childnode.Attributes.GetNamedItem("body")?.Value;
                                    elements.Add(id, new Arrow(id, type, name, parent, null, to));
                                }
                                else
                                    mistakes.Add(UCMistakeFactory.Create(
                                        MistakesTypes.ERROR,
                                        $"Элемент находится за пределами системы: {type} - {name}"));
                                break;
                            }
                    }
                }
                else
                    mistakes.Add(UCMistakeFactory.Create(
                        MistakesTypes.ERROR,
                        $"Недопустимый элемент: {type} - {name}"));
            }

            if (diagram.Image != null)
                if (!ReadCoordinates(root))
                    mistakes.Add(UCMistakeFactory.Create(MistakesTypes.WARNING, "Координаты отсутствуют"));
        }

        private bool ReadCoordinates(XmlElement root)
        {
            XmlNode coordRoot;
            try
            {
                coordRoot = root.GetElementsByTagName("plane")[0];
            }
            catch (NullReferenceException)
            {
                return false;
            }

            int minX = int.MaxValue, minY = int.MaxValue;

            foreach (XmlNode node in coordRoot.ChildNodes)
            {
                //if (node.Attributes["xsi:type"] == null) continue;

                string id = node.Attributes["modelElement"]?.Value,
                x = node.Attributes["x"]?.Value,
                y = node.Attributes["y"]?.Value,
                w = node.Attributes["width"]?.Value,
                h = node.Attributes["height"]?.Value;

                int intX = ConvertCoordinates(x);
                int intY = ConvertCoordinates(y);
                minX = minX > intX ? intX : minX;
                minY = minY > intY ? intY : minY;

                if (id == null || !elements.ContainsKey(id)) continue;

                var element = elements[id];
                element.X = intX;
                element.Y = intY;
                element.W = ConvertCoordinates(w);
                element.H = ConvertCoordinates(h);
            }

            var (realMinX, realMinY) = MinCoordinates.Compute(diagram.Image);
            var diffX = minX >= 0 ? realMinX - minX - 10 : Math.Abs(minX - realMinX) / 2;
            var diffY = realMinY - minY;
            foreach (var element in elements)
            {
                element.Value.X += diffX;
                element.Value.Y += diffY;
            }

            return true;
        }

        private int ConvertCoordinates(string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception)
            {
                return int.MaxValue;
            }
        }

        private void ReadPackage(XmlNode package)
        {
            foreach (XmlNode childnode in package.ChildNodes)
            {
                if (childnode.Name == "xmi:Extension")
                    continue;

                string type = getType(childnode),
                    parent = getParent(childnode),
                    name = getName(childnode),
                    id = getId(childnode);

                switch (type)
                {
                    case ElementTypes.Association:
                        {
                            // обработка ассоциации
                            string from = childnode.ChildNodes[1].Attributes.GetNamedItem("type")?.Value,
                                to = childnode.ChildNodes[2].Attributes.GetNamedItem("type")?.Value;
                            elements.Add(id, new Arrow(id, type, name, parent, from, to));
                            break;
                        }
                    case ElementTypes.Precedent:
                        {
                            elements.Add(id, new Element(id, type, name, parent));
                            ReadPrecedent(childnode);
                            break;
                        }
                    default:
                        {
                            if (childnode.Name == ElementTypes.Comment)
                            {
                                type = ElementTypes.Comment;
                                string to = childnode.Attributes.GetNamedItem("annotatedElement")?.Value;
                                name = childnode.Attributes.GetNamedItem("body")?.Value;
                                elements.Add(id, new Arrow(id, type, name, parent, null, to));
                            }
                            else
                                mistakes.Add(UCMistakeFactory.Create(
                                    MistakesTypes.ERROR,
                                    $"Недопустимый элемент внутри системы {getName(package)}: {id} - {name}"));
                            break;
                        }
                }
            }
        }

        private void ReadPrecedent(XmlNode precedent)
        {
            foreach (XmlNode childnode in precedent.ChildNodes)
            {
                if (childnode.Name == "xmi:Extension")
                    continue;

                string type = childnode.Name,
                    parent = getParent(childnode),
                    name = getName(childnode),
                    id = getId(childnode);

                switch (type)
                {
                    case ElementTypes.Include:
                        {
                            string from = childnode.Attributes.GetNamedItem("includingCase")?.Value,
                                to = childnode.Attributes.GetNamedItem("addition")?.Value;
                            elements.Add(id, new Arrow(id, type, name, parent, from, to));
                            break;
                        }
                    case ElementTypes.Extend:
                        {
                            string from = childnode.Attributes.GetNamedItem("extension")?.Value,
                                to = childnode.Attributes.GetNamedItem("extendedCase")?.Value;
                            elements.Add(id, new Arrow(id, type, name, parent, from, to));
                            break;
                        }
                    case ElementTypes.ExtensionPoint:
                        {
                            string to = childnode.Attributes.GetNamedItem("useCase")?.Value;
                            elements.Add(id, new Arrow(id, type, name, parent, null, to));
                            break;
                        }
                    default:
                        {
                            mistakes.Add(UCMistakeFactory.Create(
                                MistakesTypes.ERROR,
                                $"Недопустимый элемент элемент внутри системы {getName(precedent.ParentNode)}: {type} - {name}"));
                            break;
                        }
                }
            }
        }

        private void ReadActor(XmlNode actor)
        {
            foreach (XmlNode childnode in actor.ChildNodes)
            {
                if (childnode.Name == "xmi:Extension")
                    continue;

                string type = getType(childnode),
                parent = getParent(childnode),
                name = getName(childnode),
                id = getId(childnode);

                if (childnode.Name == ElementTypes.Generalization)
                {
                    type = ElementTypes.Generalization;
                    string from = childnode.Attributes.GetNamedItem("specific")?.Value,
                        to = childnode.Attributes.GetNamedItem("general")?.Value;

                    elements.Add(id, new Arrow(id, type, name, parent, from, to));
                }
                else
                    mistakes.Add(UCMistakeFactory.Create(MistakesTypes.ERROR, $"Недопустимый элемент: {type} - {name}"));
            }
        }

        private string getId(XmlNode item)
        {
            return item.Attributes.GetNamedItem("xmi:id")?.Value;
        }

        private string getType(XmlNode item)
        {
            return item.Attributes.GetNamedItem("xsi:type")?.Value;
        }
        private string getName(XmlNode item)
        {
            return item.Attributes.GetNamedItem("name")?.Value;
        }

        private string getParent(XmlNode item)
        {
            return item.ParentNode.Attributes.GetNamedItem("xmi:id")?.Value;
        }
    }
}
