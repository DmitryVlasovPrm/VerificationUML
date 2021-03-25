using ActivityDiagramVer.entities;
using ActivityDiagramVer.result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Verification;
using Verification.ad_ver.entities;

namespace ActivityDiagramVer.parser
{

    class XmiParser
    {
        private XmlDocument xmlFile = null;
        private XmlElement root = null;
        private ADNodesList adNodesList;
        private List<BaseNode> unknownNodes = new List<BaseNode>();
        private (int, int) coordMin;

        public XmiParser(ADNodesList adNodesList)
        {
            this.adNodesList = adNodesList;
        }

        private XmlNode findActivePackageEl(XmlNodeList xPackagedList)
        {
            foreach (XmlNode node in xPackagedList)
            {
                var attr = node.Attributes["xsi:type"];
                if (attr == null) continue;
                if (attr.Value.Equals("uml:Activity"))
                    return node;
            }
            return null;
        }
        public bool Parse(Diagram diagram)
        {
            this.xmlFile = diagram.doc;

            // получим корневой элемент
            XmlNode xRoot = null;
            XmlNodeList xPackagedList;
            try
            {
                xPackagedList = xmlFile.GetElementsByTagName("packagedElement");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("[x] Тег packagedElement не найден");
                return false;
            }


            xRoot = findActivePackageEl(xPackagedList);
            if (xRoot == null)
            {
                Console.WriteLine("[x] Вид диаграммы не AD");
                return false;
            }

            var attr = xRoot.Attributes["xsi:type"];
            if (attr == null)
            {
                Console.WriteLine("[x] Не удалось распарсить xmi файл");
                return false;
            }
            if (!attr.Value.Equals("uml:Activity"))
            {
                Console.WriteLine("[x] Вид диаграммы не AD");
                return false;
            }

            foreach (XmlNode node in xRoot.ChildNodes)
            {
                var elAttr = node.Attributes["xsi:type"];
                if (elAttr == null) continue;

                Console.WriteLine(elAttr.Value);

                if (elAttr.Value == "uml:OpaqueAction" || elAttr.Value == "uml:InitialNode" || elAttr.Value == "uml:ActivityFinalNode" ||
                    elAttr.Value == "uml:FlowFinalNode" || elAttr.Value == "uml:DecisionNode" || elAttr.Value == "uml:MergeNode" ||
                    elAttr.Value == "uml:ForkNode" || elAttr.Value == "uml:JoinNode")
                {
                    DiagramElement nodeFromXMI = null;
                    switch (elAttr.Value)
                    {
                        // активность
                        case "uml:OpaqueAction":
                            nodeFromXMI = new ActivityNode(node.Attributes["xmi:id"].Value,
                                    attrAdapter(node.Attributes["inPartition"]), attrAdapter(node.Attributes["name"]));
                            nodeFromXMI.setType(ElementType.ACTIVITY);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // узел инициализации
                        case "uml:InitialNode":
                            nodeFromXMI = new InitialNode(node.Attributes["xmi:id"].Value, attrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.INITIAL_NODE);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // конечное состояние
                        case "uml:ActivityFinalNode":
                        case "uml:FlowFinalNode":
                            nodeFromXMI = new FinalNode(node.Attributes["xmi:id"].Value, attrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.FINAL_NODE);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // условный переход
                        case "uml:DecisionNode":
                            nodeFromXMI = new DecisionNode(node.Attributes["xmi:id"].Value, attrAdapter(node.Attributes["inPartition"]), attrAdapter(node.Attributes["question"]));
                            nodeFromXMI.setType(ElementType.DECISION);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // узел слияния
                        case "uml:MergeNode":
                            nodeFromXMI = new MergeNode(node.Attributes["xmi:id"].Value, attrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.MERGE);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // разветвитель
                        case "uml:ForkNode":
                            nodeFromXMI = new ForkNode(node.Attributes["xmi:id"].Value, attrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.FORK);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // синхронизатор
                        case "uml:JoinNode":
                            nodeFromXMI = new JoinNode(node.Attributes["xmi:id"].Value, attrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.JOIN);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                    }
                    // добавляем ид входящих и выходящих переходов
                    if (nodeFromXMI != null)
                    {
                        String idsIn = node.Attributes["incoming"]?.Value;
                        String idsOut = node.Attributes["outgoing"]?.Value;
                        nodeFromXMI.addIn(idsIn == null ? "" : idsIn);
                        nodeFromXMI.addOut(idsOut == null ? "" : idsOut);
                    }
                }
                // создаем переход
                else if (node.Attributes["xsi:type"].Value.Equals("uml:ControlFlow"))
                {
                    // находим подпись перехода
                    var markNode = node.ChildNodes[1];
                    String mark = markNode.Attributes["value"].Value.Trim();        // если подпись является "yes", значит это подпись по умолчанию

                    ControlFlow temp = new ControlFlow(node.Attributes["xmi:id"].Value, mark.Equals("true") ? "" : mark);
                    temp.setType(ElementType.FLOW);
                    temp.setSrc(attrAdapter(node.Attributes["source"]));
                    temp.setTarget(attrAdapter(node.Attributes["target"]));
                    adNodesList.addLast(temp);
                }
                // создаем дорожку
                else if (node.Attributes["xsi:type"].Value.Equals("uml:ActivityPartition"))
                {
                    Swimlane temp = new Swimlane(node.Attributes["xmi:id"].Value, attrAdapter(node.Attributes["name"]));
                    temp.ChildCount = node.Attributes["node"] == null ? 0 : node.Attributes["node"].Value.Split().Length;
                    temp.setType(ElementType.SWIMLANE);
                    if(temp.Name!="") diagram.Actors.Add(temp);
                    adNodesList.addLast(temp);

                }
                // неизвестный элемент
                else
                {
                    var unknownNode = new UnknownNode(node.Attributes["xmi:id"].Value);
                    unknownNode.setType(ElementType.UNKNOWN);
                    unknownNodes.Add(unknownNode);
                }
            }

            XmlNode coordRoot = null;
            try
            {
                coordRoot = xmlFile.GetElementsByTagName("plane")[0];
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("[x] Тег packagedElement не найден");
                return false;
            }

            if (coordRoot == null) return true;
            findCoordinates(coordRoot);

            return true;
        }

        private String attrAdapter(XmlAttribute attr)
        {
            return attr == null ? "" : attr.Value.Trim();
        }
        /**
         * Добавляет координаты к элементам
         * @param packagedElement
         */
        private void findCoordinates(XmlNode packagedElement)
        {
            int xMin = int.MaxValue, yMin= int.MaxValue;
            foreach (XmlNode nodeCh in packagedElement.ChildNodes)
            {
                var attr = nodeCh.Attributes["xsi:type"];
                if (attr == null) continue;     // если эл-т не имеет атрибут type, он нас не интересует 
                String id = nodeCh.Attributes["modelElement"]?.Value;
                String xStr = nodeCh.Attributes["x"]?.Value;
                String yStr = nodeCh.Attributes["y"]?.Value;
                String widthStr = nodeCh.Attributes["width"]?.Value;
                String heightStr = nodeCh.Attributes["height"]?.Value;
                int x = 0, y = 0, width = 0, height = 0;
                bool noCoord = true;
                if (xStr != null)
                {
                    x = int.Parse(xStr);
                    noCoord = false;
                }
                if (yStr != null)
                {
                    y = int.Parse(yStr);
                    noCoord = false;
                }
                if (widthStr != null)
                {
                    width = int.Parse(widthStr);
                    noCoord = false;
                }
                if (heightStr != null)
                {
                    height = int.Parse(heightStr);
                    noCoord = false;
                }
                if (noCoord)
                {
                    x = y = width = height = -1;
                }


                // ищем эл-т по ид
                BaseNode node = adNodesList.get(id);
                if (node == default)
                {
                    node = unknownNodes.Find(n => n.getId().Equals(id));
                    if (node == default) continue;
                    ADMistakeFactory.createMistake(verification.Level.FATAL, "Используется недопустимый элемент");
                }
                node.X = x;
                node.Y = y;
                node.Width = width;
                node.Height = height;

                // ищем минимальный 
                if (x != -1) {
                    xMin = Math.Min(x, xMin);
                    yMin = Math.Min(y, yMin);
                }
            }

            // нормализация координат
            if (xMin == int.MaxValue) return;
            for (int i = 0; i < adNodesList.size(); i++) {
                adNodesList.get(i).X -= xMin;
                adNodesList.get(i).Y-= yMin;
                Console.WriteLine($"[x] xmin={xMin}, yMin={yMin}, x={adNodesList.get(i).X}, y={adNodesList.get(i).Y}");
            }

        }
    }
}
