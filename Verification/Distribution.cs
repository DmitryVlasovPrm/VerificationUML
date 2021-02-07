using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;

namespace Verification
{
    public class Distribution
    {
        public List<Diagram> AllDiagrams;
        public Action<string> NewDiagramAdded;

        public Distribution()
        {
            AllDiagrams = new List<Diagram>();
        }

        public void CreateDiagrams(List<string> files)
        {
            try
            {
                var xmiFiles = files.FindAll(a => Path.GetExtension(a) == ".xmi");
                files.RemoveAll(a => Path.GetExtension(a) == ".xmi");

                var xmiFilesCount = xmiFiles.Count;
                for (var i = 0; i < xmiFilesCount; i++)
                {
                    var isNew = true;
                    var pathToXmi = xmiFiles[i];
                    var curXmiName = Path.GetFileNameWithoutExtension(pathToXmi);
                    if (AllDiagrams.Exists(a => a.Name == curXmiName))
                    {
                        var dialogResult = MessageBox.Show($"Диаграмма c именем {curXmiName} уже существует.\nПерезаписать ее?", "Верификация диаграмм UML",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.No)
                        {
                            continue;
                        }
                        else
                        {
                            isNew = false;
                            AllDiagrams.RemoveAll(a => a.Name == curXmiName);
                        }
                    }
                    var pathToPng = files.Find(a => Path.GetFileNameWithoutExtension(a) == curXmiName);
                    files.Remove(pathToPng);

                    var doc = new XmlDocument();
                    doc.Load(pathToXmi);
                    var root = doc.DocumentElement;

                    var diagram = new Diagram(curXmiName, pathToXmi, pathToPng, root);
                    AllDiagrams.Add(diagram);
                    if (isNew)
                        NewDiagramAdded?.Invoke(curXmiName);
                }

                var pngFilesCount = files.Count;
                for (var i = 0; i < pngFilesCount; i++)
                {
                    var pathToFile = files[i];
                    var name = Path.GetFileNameWithoutExtension(pathToFile);
                    var diagramId = AllDiagrams.FindIndex(a => a.Name == name);
                    if (diagramId != -1)
                    {
                        if (AllDiagrams[diagramId].PathToPng == null)
                        {
                            AllDiagrams[diagramId].PathToPng = pathToFile;
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show($"У диаграммы с именем {name} уже существует png файл.\nПерезаписать его?", "Верификация диаграмм UML",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.No)
                            {
                                continue;
                            }
                            else
                            {
                                AllDiagrams[diagramId].PathToPng = pathToFile;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка в TypeDefinition: {ex.Message}", "Верификация диаграмм UML",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
