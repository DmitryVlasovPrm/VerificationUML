using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verification.type_definer
{
    class TypeDefiner
    {

        /*
            // Тестирование
            var definer = new TypeDefiner();
            definer.define(@"C:\Diagram.xmi");
         */


        /**
        * Быстрое пределение типа диаграммы
        */
        class TypeDefiner
        {
            /**
             * Проверка, что в списке переданных элементов есть хотя бы один с атрибутом равным значению type
             */
            private XmlNode findActivePackageEl(XmlNodeList xPackagedList, string type)
            {
                foreach (XmlNode node in xPackagedList)
                {
                    var attr = node.Attributes["xsi:type"];
                    if (attr == null) continue;
                    if (attr.Value.Equals(type))
                        return node;
                }
                return null;
            }


            /**
             * Определение типа диаграммы
             * return тип диаграммы или UNDEF, если тип не определен
             */
            public EDiagramTypes define(string path)
            {
                XmlDocument xmlFile = null;
                XmlElement root = null;
                ADNodesList adNodesList;
                if (!File.Exists(path))
                {
                    Debug.println("[x] File is not exist");
                    return EDiagramTypes.UNDEF;
                }
                xmlFile = new XmlDocument();
                xmlFile.Load(path);

                // получение всех packagedElement
                XmlNode xRoot = null;
                XmlNodeList xPackagedList;
                try
                {
                    xPackagedList = xmlFile.GetElementsByTagName("packagedElement");
                }
                catch (NullReferenceException e)
                {
                    Debug.println("[x] Тег packagedElement не найден");
                    return EDiagramTypes.UNDEF;
                }

                // проверка на AD
                xRoot = findActivePackageEl(xPackagedList, "uml:Activity");
                if (xRoot != null)
                {
                    Debug.println("[x] Вид диаграммы AD");
                    return EDiagramTypes.AD;
                }

                // проверка на UCD
                xRoot = findActivePackageEl(xPackagedList, "uml:UseCase");
                if (xRoot != null)
                {
                    Debug.println("[x] Вид диаграммы UCD");
                    return EDiagramTypes.UCD;
                }

                // проверка на CD
                xRoot = findActivePackageEl(xPackagedList, "uml:Class");
                if (xRoot != null)
                {
                    Debug.println("[x] Вид диаграммы CD");
                    return EDiagramTypes.CD;
                }
                return EDiagramTypes.UNDEF;
            }
        }
    }
}
