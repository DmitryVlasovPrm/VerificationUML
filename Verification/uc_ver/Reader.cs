using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Verification.uc_ver
{
    class Reader
    {
        private Dictionary<string, Element> elements;

        public Reader(Dictionary<string, Element> elements)
        {
            this.elements = elements;
        }

        public void ReadData(XmlNode root)
        {
            XmlNode coordinates = null;
            foreach (XmlNode childnode in root.ChildNodes)
            {
                if (childnode.Name == "packageImport")
                    continue;

                if (childnode.Name == "xmi:Extension")
                    coordinates = childnode;

                string type = getType(childnode),
                parent = getParent(childnode),
                name = getName(childnode),
                id = getId(childnode);

                if (Types.List.Contains(type))
                {
                    switch (type)
                    {
                        case Types.Association:
                            {
                                // обработка ассоциации
                                string from = childnode.ChildNodes[1].Attributes.GetNamedItem("type")?.Value,
                                    to = childnode.ChildNodes[2].Attributes.GetNamedItem("type")?.Value;
                                elements.Add(id, new Arrow(id, type, name, parent, from, to));
                                break;
                            }
                        case Types.Actor:
                            {
                                ReadActor(childnode);
                                elements.Add(id, new Element(id, type, name, parent));
                                break;
                            }
                        case Types.Package:
                            {
                                ReadPackage(childnode);
                                elements.Add(id, new Element(id, type, name, parent));
                                break;
                            }
                        case Types.Precedent:
                            {
                                elements.Add(id, new Element(id, type, name, parent));
                                ReadPrecedent(childnode);
                                break;
                            }
                        default:
                            {
                                if (childnode.Name == Types.Comment)
                                {
                                    type = Types.Comment;
                                    string to = childnode.Attributes.GetNamedItem("annotatedElement")?.Value;
                                    name = childnode.Attributes.GetNamedItem("body")?.Value;
                                    elements.Add(id, new Arrow(id, type, name, parent, null, to));
                                }
                                else
                                    Console.WriteLine($"Элемент находится за пределами системы: {type} - {name}");
                                break;
                            }
                    }
                }
                else
                    Console.WriteLine($"Недопустимый элемент: {type} - {name}");
            }

            if (coordinates != null)
                ReadCoordinates(coordinates);
        }

        private void ReadCoordinates(XmlNode coordinates)
        {
            XmlNode eAnnotations = coordinates.FirstChild,
                contents = null;
            foreach (XmlNode node in eAnnotations.ChildNodes)
                if (node.Name == "contens")
                    contents = node;

            if(contents != null)
            {
                foreach(XmlNode node in contents.ChildNodes)
                {

                }
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
                    case Types.Association:
                        {
                            // обработка ассоциации
                            string from = childnode.ChildNodes[1].Attributes.GetNamedItem("type")?.Value,
                                to = childnode.ChildNodes[2].Attributes.GetNamedItem("type")?.Value;
                            elements.Add(id, new Arrow(id, type, name, parent, from, to));
                            break;
                        }
                    case Types.Precedent:
                        {
                            elements.Add(id, new Element(id, type, name, parent));
                            ReadPrecedent(childnode);
                            break;
                        }
                    default:
                        {
                            if (childnode.Name == Types.Comment)
                            {
                                type = Types.Comment;
                                string to = childnode.Attributes.GetNamedItem("annotatedElement")?.Value;
                                name = childnode.Attributes.GetNamedItem("body")?.Value;
                                elements.Add(id, new Arrow(id, type, name, parent, null, to));
                            }
                            else
                                Console.WriteLine($"Недопустимый элемент внутри системы {getName(package)}: {id} - {name}");
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
                    case Types.Include:
                        {
                            string from = childnode.Attributes.GetNamedItem("includingCase")?.Value,
                                to = childnode.Attributes.GetNamedItem("addition")?.Value;
                            elements.Add(id, new Arrow(id, type, name, parent, from, to));
                            break;
                        }
                    case Types.Extend:
                        {
                            string from = childnode.Attributes.GetNamedItem("extension")?.Value,
                                to = childnode.Attributes.GetNamedItem("extendedCase")?.Value;
                            elements.Add(id, new Arrow(id, type, name, parent, from, to));
                            break;
                        }
                    case Types.ExtensionPoint:
                        {
                            string to = childnode.Attributes.GetNamedItem("useCase")?.Value;
                            elements.Add(id, new Arrow(id, type, name, parent, null, to));
                            break;
                        }
                    default:
                        {
                            Console.WriteLine($"Недопустимый элемент элемент внутри системы {getName(precedent.ParentNode)}: {type} - {name}");
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

                if (childnode.Name == Types.Generalization)
                {
                    type = Types.Generalization;
                    string from = childnode.Attributes.GetNamedItem("specific")?.Value,
                        to = childnode.Attributes.GetNamedItem("general")?.Value;

                    elements.Add(id, new Arrow(id, type, name, parent, from, to));
                }
                else
                    Console.WriteLine($"Недопустимый элемент: {type} - {name}");
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
