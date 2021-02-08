using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Verification
{
    public class Distribution
    {
        public List<Diagram> AllDiagrams;
        public Action<string> NewDiagramAdded;
        public Action SomethingChanged;

        public Distribution()
        {
            AllDiagrams = new List<Diagram>();
        }

        public void CreateDiagrams(List<string> files)
        {
            try
            {
                var isSomethingChanged = false;
                var xmiFiles = files.FindAll(a => Path.GetExtension(a) == ".xmi");
                files.RemoveAll(a => Path.GetExtension(a) == ".xmi");

                // Добавляем или заменяем xmi файлы
                var xmiFilesCount = xmiFiles.Count;
                for (var i = 0; i < xmiFilesCount; i++)
                {
                    var pathToXmi = xmiFiles[i];
                    var name = Path.GetFileNameWithoutExtension(pathToXmi);
                    var diagramId = AllDiagrams.FindIndex(a => a.Name == name);
                    if (diagramId != -1)
                    {
                        var dialogResult = MessageBox.Show($"Диаграмма c именем {name} уже существует.\nПерезаписать ее?", "Верификация диаграмм UML",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.No)
                        {
                            continue;
                        }
                        else
                        {
                            AllDiagrams.RemoveAt(diagramId);
                        }
                    }
                    var pathToPng = files.Find(a => Path.GetFileNameWithoutExtension(a) == name);
                    files.Remove(pathToPng);

                    var doc = new XmlDocument();
                    doc.Load(pathToXmi);
                    var root = doc.DocumentElement;

                    Image<Bgra, byte> image = null;
                    if (pathToPng != null)
                        image = new Image<Bgra, byte>(pathToPng);
                    var diagram = new Diagram(name, root, image);
                    AllDiagrams.Add(diagram);

                    NewDiagramAdded?.Invoke(name);
                    isSomethingChanged = true;
                }

                // Добавляем к xmi файлам новые рисунки
                var pngFilesCount = files.Count;
                for (var i = 0; i < pngFilesCount; i++)
                {
                    var pathToFile = files[i];
                    var name = Path.GetFileNameWithoutExtension(pathToFile);
                    var diagramId = AllDiagrams.FindIndex(a => a.Name == name);
                    if (diagramId != -1)
                    {
                        if (AllDiagrams[diagramId].Image == null)
                        {
                            Image<Bgra, byte> image = new Image<Bgra, byte>(pathToFile);
                            AllDiagrams[diagramId].Image = image;
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
                                Image<Bgra, byte> image = new Image<Bgra, byte>(pathToFile);
                                AllDiagrams[diagramId].Image = image;
                            }
                        }
                        isSomethingChanged = true;
                    }
                }

                if (isSomethingChanged)
                    SomethingChanged?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка в TypeDefinition: {ex.Message}", "Верификация диаграмм UML",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
