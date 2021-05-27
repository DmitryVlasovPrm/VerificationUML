using ActivityDiagramVer.entities;
using ActivityDiagramVer.result;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using Verification;
using Verification.ad_ver.entities;
using Verification.ad_ver.verification;

namespace ActivityDiagramVer.parser {
    internal class XmiParser {
        private XmlDocument xmlFile = null;
        private readonly ADNodesList adNodesList;
        private readonly List<BaseNode> unknownNodes = new List<BaseNode>();
        private int elCounts = 0;
        private static int d_number = 0;

        public XmiParser(ADNodesList adNodesList) {
            this.adNodesList = adNodesList;
        }

        private XmlNode FindActivePackageEl(XmlNodeList xPackagedList) {
            foreach (XmlNode node in xPackagedList) {
                var attr = node.Attributes["xsi:type"];
                if (attr == null) continue;
                if (attr.Value.Equals("uml:Activity"))
                    return node;
            }
            return null;
        }
        public bool Parse(Diagram diagram, ref bool hasJoinOrFork) {
            d_number++;
            xmlFile = diagram.doc;
            XmlNodeList xPackagedList;
            try {
                xPackagedList = xmlFile.GetElementsByTagName("packagedElement");
            } catch (NullReferenceException) {
                Console.WriteLine("[x] Тег packagedElement не найден");
                return false;
            }


            // получим корневой элемент
            XmlNode xRoot = FindActivePackageEl(xPackagedList);
            if (xRoot == null) {
                Console.WriteLine("[x] Вид диаграммы не AD");
                return false;
            }

            var attr = xRoot.Attributes["xsi:type"];
            if (attr == null) {
                Console.WriteLine("[x] Не удалось распарсить xmi файл");
                return false;
            }
            if (!attr.Value.Equals("uml:Activity")) {
                Console.WriteLine("[x] Вид диаграммы не AD");
                return false;
            }

            foreach (XmlNode node in xRoot.ChildNodes) {
                var elAttr = node.Attributes["xsi:type"];
                if (elAttr == null) continue;

                elCounts++;
                if (elAttr.Value == "uml:OpaqueAction" || elAttr.Value == "uml:InitialNode" || elAttr.Value == "uml:ActivityFinalNode" ||
                    elAttr.Value == "uml:FlowFinalNode" || elAttr.Value == "uml:DecisionNode" || elAttr.Value == "uml:MergeNode" ||
                    elAttr.Value == "uml:ForkNode" || elAttr.Value == "uml:JoinNode") {
                    DiagramElement nodeFromXMI = null;
                    switch (elAttr.Value) {
                        // активность
                        case "uml:OpaqueAction":
                            nodeFromXMI = new ActivityNode(node.Attributes["xmi:id"].Value,
                                    AttrAdapter(node.Attributes["inPartition"]), AttrAdapter(node.Attributes["name"]));
                            nodeFromXMI.setType(ElementType.ACTIVITY);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // узел инициализации
                        case "uml:InitialNode":
                            nodeFromXMI = new InitialNode(node.Attributes["xmi:id"].Value, AttrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.INITIAL_NODE);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // конечное состояние
                        case "uml:ActivityFinalNode":
                        case "uml:FlowFinalNode":
                            nodeFromXMI = new FinalNode(node.Attributes["xmi:id"].Value, AttrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.FINAL_NODE);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // условный переход
                        case "uml:DecisionNode":
                            nodeFromXMI = new DecisionNode(node.Attributes["xmi:id"].Value, AttrAdapter(node.Attributes["inPartition"]), AttrAdapter(node.Attributes["question"]));
                            nodeFromXMI.setType(ElementType.DECISION);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // узел слияния
                        case "uml:MergeNode":
                            nodeFromXMI = new MergeNode(node.Attributes["xmi:id"].Value, AttrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.MERGE);
                            adNodesList.addLast(nodeFromXMI);
                            break;
                        // разветвитель
                        case "uml:ForkNode":
                            nodeFromXMI = new ForkNode(node.Attributes["xmi:id"].Value, AttrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.FORK);
                            adNodesList.addLast(nodeFromXMI);
                            hasJoinOrFork = true;
                            break;
                        // синхронизатор
                        case "uml:JoinNode":
                            nodeFromXMI = new JoinNode(node.Attributes["xmi:id"].Value, AttrAdapter(node.Attributes["inPartition"]));
                            nodeFromXMI.setType(ElementType.JOIN);
                            adNodesList.addLast(nodeFromXMI);
                            hasJoinOrFork = true;
                            break;
                    }
                    // добавляем ид входящих и выходящих переходов
                    if (nodeFromXMI != null) {
                        string idsIn = node.Attributes["incoming"]?.Value;
                        string idsOut = node.Attributes["outgoing"]?.Value;
                        nodeFromXMI.addIn(idsIn ?? "");
                        nodeFromXMI.addOut(idsOut ?? "");
                        
                    }
                }
                // создаем переход
                else if (node.Attributes["xsi:type"].Value.Equals("uml:ControlFlow")) {
                    // находим подпись перехода
                    var markNode = node.ChildNodes[1];
                    string mark = markNode.Attributes["value"].Value.Trim();        // если подпись является "yes", значит это подпись по умолчанию

                    ControlFlow temp = new ControlFlow(node.Attributes["xmi:id"].Value, mark.Equals("true") ? "" : mark);
                    temp.setType(ElementType.FLOW);
                    temp.setSrc(AttrAdapter(node.Attributes["source"]));
                    temp.setTarget(AttrAdapter(node.Attributes["target"]));
                    adNodesList.addLast(temp);
                    elCounts--;
                }
                // создаем дорожку
                else if (node.Attributes["xsi:type"].Value.Equals("uml:ActivityPartition")) {
                    Swimlane temp = new Swimlane(node.Attributes["xmi:id"].Value, AttrAdapter(node.Attributes["name"])) {
                        ChildCount = node.Attributes["node"] == null ? 0 : node.Attributes["node"].Value.Split().Length
                    };
                    temp.setType(ElementType.SWIMLANE);
                    if (temp.Name != "") diagram.Actors.Add(temp);
                    adNodesList.addLast(temp);

                }
                // неизвестный элемент
                else {
                    var unknownNode = new UnknownNode(node.Attributes["xmi:id"].Value);
                    unknownNode.setType(ElementType.UNKNOWN);
                    unknownNodes.Add(unknownNode);
                    elCounts--;
                }
            }

            XmlNode coordRoot = null;
            try {
                coordRoot = xmlFile.GetElementsByTagName("plane")[0];
            } catch (NullReferenceException) {
                Console.WriteLine("[x] Тег packagedElement не найден");
            }

            if (coordRoot != null)
                FindCoordinates(coordRoot, diagram);
            for (int i = 0; i < adNodesList.size(); i++) {
                var node = adNodesList.get(i);
                if (node is DiagramElement) {
                    var nodeFromXMI = (DiagramElement)node;
                    switch (nodeFromXMI.getType()) {
                        case ElementType.FINAL_NODE:
                            if (nodeFromXMI.inSize() == 0) {
                                // ошибка
                                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_IN], MistakeAdapter.toString(MISTAKES.NO_IN), new ADNodesList.ADNode(nodeFromXMI));
                            }
                            break;
                        case ElementType.INITIAL_NODE:
                            if (nodeFromXMI.outSize() == 0) {
                                // ошибка
                                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_OUT], MistakeAdapter.toString(MISTAKES.NO_OUT), new ADNodesList.ADNode(nodeFromXMI));
                            }
                            break;
                        default:
                            if (nodeFromXMI.inSize() == 0 || nodeFromXMI.outSize() == 0) {
                                // ошибка
                                if (nodeFromXMI.inSize() == 0) ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_IN], MistakeAdapter.toString(MISTAKES.NO_IN), new ADNodesList.ADNode(nodeFromXMI));
                                if (nodeFromXMI.outSize() == 0) ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.NO_OUT], MistakeAdapter.toString(MISTAKES.NO_OUT), new ADNodesList.ADNode(nodeFromXMI));
                            }
                            break;
                    }
                }
            }
            foreach (var node in unknownNodes) {
                ADMistakeFactory.createMistake(MistakesSeriousness.mistakes[MISTAKES.FORBIDDEN_ELEMENT], MistakeAdapter.toString(MISTAKES.FORBIDDEN_ELEMENT), node);
            }

            System.Console.WriteLine($"[{d_number}]: {elCounts}");
            return true;
        }

        private string AttrAdapter(XmlAttribute attr) {
            return attr == null ? "" : Regex.Replace(attr.Value.Trim(), @"\s+", " ");
        }
        /**
         * Добавляет координаты к элементам
         * @param packagedElement
         */
        private void FindCoordinates(XmlNode packagedElement, Diagram diagram) {
            if (diagram.Image == null) return;
            int xMin = int.MaxValue, yMin = int.MaxValue;
            (int, int) coordMin = (0, 0);
            coordMin = MinCoordinates.Compute(diagram.Image);

            foreach (XmlNode nodeCh in packagedElement.ChildNodes) {
                var attr = nodeCh.Attributes["xsi:type"];
                if (attr == null) continue;     // если эл-т не имеет атрибут type, он нас не интересует 
                string id = nodeCh.Attributes["modelElement"]?.Value;
                string xStr = nodeCh.Attributes["x"]?.Value;
                string yStr = nodeCh.Attributes["y"]?.Value;
                string widthStr = nodeCh.Attributes["width"]?.Value;
                string heightStr = nodeCh.Attributes["height"]?.Value;
                int x = 0, y = 0, width = 0, height = 0;
                bool noCoord = true;
                if (xStr != null) {
                    x = int.Parse(xStr);
                    noCoord = false;
                }
                if (yStr != null) {
                    y = int.Parse(yStr);
                    noCoord = false;
                }
                if (widthStr != null) {
                    width = int.Parse(widthStr);
                    noCoord = false;
                }
                if (heightStr != null) {
                    height = int.Parse(heightStr);
                    noCoord = false;
                }
                if (noCoord) {
                    x = y = width = height = -1;
                }


                // ищем эл-т по ид
                BaseNode node = adNodesList.get(id);
                if (node == default) {
                    node = unknownNodes.Find(n => n.getId().Equals(id));
                    if (node == default) continue;
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
                adNodesList.get(i).X += coordMin.Item1;

                adNodesList.get(i).Y -= yMin;
                adNodesList.get(i).Y += coordMin.Item2;
            }

        }
    }
}
