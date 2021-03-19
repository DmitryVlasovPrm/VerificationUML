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
        public Action SomethingChanged;

        public Distribution()
        {
            AllDiagrams = new Dictionary<string, Diagram>();
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
                    if (AllDiagrams.ContainsKey(name))
                    {// Если такая диаграмма уже существует
                        var dialogResult = MessageBox.Show($"Диаграмма c именем {name} уже существует.\nПерезаписать ее?", "Верификация диаграмм UML",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.No)
                        {
                            continue;
                        }
                        else
                        {
                            AllDiagrams.Remove(name);
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
                    var type = TypeDefiner.DefineDiagramType(root);
                    var diagram = new Diagram(name, root, image, type, doc);
                    AllDiagrams.Add(name, diagram);

                    NewDiagramAdded?.Invoke(name);
                    isSomethingChanged = true;
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
