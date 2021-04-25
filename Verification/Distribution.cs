using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Verification.type_definer;

namespace Verification
{
    public class Distribution
    {
        public Dictionary<string, Diagram> AllDiagrams;
        public Action<string> NewDiagramAdded;
        public Action<List<string>> SomethingChanged;
        private readonly List<string> ChangedDiagramNames;

        public Distribution()
        {
            AllDiagrams = new Dictionary<string, Diagram>();
            ChangedDiagramNames = new List<string>();
        }

        public void CreateDiagrams(List<string> files)
        {
            try
            {
                ChangedDiagramNames.Clear();
                var xmiFiles = files.FindAll(a => Path.GetExtension(a) == ".xmi");
                files.RemoveAll(a => Path.GetExtension(a) == ".xmi");

                // Добавляем или заменяем xmi файлы
                var xmiFilesCount = xmiFiles.Count;
                for (var i = 0; i < xmiFilesCount; i++)
                {
                    Image<Bgra, byte> image = null;
                    var pathToXmi = xmiFiles[i];
                    var name = Path.GetFileNameWithoutExtension(pathToXmi);
                    if (AllDiagrams.ContainsKey(name))
                    {
                        // Если такая диаграмма уже существует
                        var dialogResult = MessageBox.Show($"Диаграмма c именем {name} уже существует.\nПерезаписать ее?", "Верификация диаграмм UML",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.No)
                        {
                            continue;
                        }
                        else
                        {
                            image = AllDiagrams[name].Image;
                            AllDiagrams.Remove(name);
                        }
                    }
                    var pathToPng = files.Find(a => Path.GetFileNameWithoutExtension(a) == name);
                    if (pathToPng != null)
                        image = new Image<Bgra, byte>(pathToPng);
                    files.Remove(pathToPng);

                    var doc = new XmlDocument();
                    doc.Load(pathToXmi);
                    var root = doc.DocumentElement;
                    var type = TypeDefiner.DefineDiagramType(root);
                    var diagram = new Diagram(name, root, image, type, doc);
                    AllDiagrams.Add(name, diagram);

                    NewDiagramAdded?.Invoke(name);
                    ChangedDiagramNames.Add(name);
                }

                // Добавляем к xmi файлам новые рисунки
                var pngFilesCount = files.Count;
                for (var i = 0; i < pngFilesCount; i++)
                {
                    var pathToFile = files[i];
                    var name = Path.GetFileNameWithoutExtension(pathToFile);

                    if (AllDiagrams.ContainsKey(name))
                    {
                        if (AllDiagrams[name].Image == null)
                        {
                            Image<Bgra, byte> image = new Image<Bgra, byte>(pathToFile);
                            AllDiagrams[name].Image = image;
                            AllDiagrams[name].Verificated = false;
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
                                AllDiagrams[name].Image = image;
                                AllDiagrams[name].Verificated = false;
                            }
                        }
                        ChangedDiagramNames.Add(name);
                    }
                }

                if (ChangedDiagramNames.Count != 0)
                    SomethingChanged.Invoke(ChangedDiagramNames);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка в TypeDefinition: {ex.Message}", "Верификация диаграмм UML",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
