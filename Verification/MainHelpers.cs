using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Verification
{
    public partial class Main
    {
        // Функция выбора файлов
        private void ChooseFiles()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите xmi, png или jpeg файлы";
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Файлы xmi, png или jpeg|*.xmi; *.png; *.jpeg";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var allFiles = openFileDialog.FileNames.ToList();
                Distribution.CreateDiagrams(allFiles);
            }
        }

        // Добавление новой диаграммы в GUI
        private void AddDiagram(string name)
        {
            if (diagramsGV.Columns.Count == 0)
                diagramsGV.Columns.Add("diagramName", "");

            for (int i = 0; i < diagramsGV.Rows.Count; i++)
            {
                var row = diagramsGV.Rows[i];
                var cell = diagramsGV.Rows[i].Cells[0];
                if (cell.Value.ToString() == name)
                    diagramsGV.Rows.Remove(row);
            }

            diagramsGV.Rows.Add(name);
            btVerify.Enabled = btDelete.Enabled = btOutput.Enabled = true;
        }

        // Обновление картинок и списка ошибок
        private void UpdateGUIState()
        {
            if (diagramsGV.Rows.Count == 0)
            {
                diagramPicture.Image = null;
                errorsGV.Rows.Clear();
                return;
            }

            if (diagramsGV.SelectedCells.Count == 0)
                diagramsGV.SelectedCells[0].Selected = true;

            var selectedName = diagramsGV.SelectedCells[0].Value.ToString();
            Distribution.AllDiagrams.TryGetValue(selectedName, out Diagram selectedDiagram);
            if (selectedDiagram != null)
            {
                diagramPicture.Image = selectedDiagram.Image != null ? selectedDiagram.Image.Bitmap : diagramPicture.Image = null;
                ShowDiagramMistakes(selectedDiagram.Mistakes, selectedDiagram.EType);
            }
        }

        // Показ всех ошибок в таблице
        private void ShowDiagramMistakes(List<Mistake> mistakes, type_definer.EDiagramTypes type)
		{
            errorsGV.Rows.Clear();
            string typeStr = "";
            switch (type)
            {
                case type_definer.EDiagramTypes.AD:
                    typeStr = "Диаграмма активностей";
                    break;
                case type_definer.EDiagramTypes.CD:
                    typeStr = "Диаграмма классов";
                    break;
                case type_definer.EDiagramTypes.UCD:
                    typeStr = "Диаграмма прецедентов";
                    break;
                case type_definer.EDiagramTypes.UNDEF:
                    typeStr = "Неопределен";
                    break;
            }
            groupBox1.Text = string.Format("Ошибки (Тип диаграммы: {0})", typeStr);

            if (errorsGV.Columns.Count == 0)
                errorsGV.Columns.Add("text", "");

            mistakes.Add(new Mistake(2, "Class1", new BoundingBox(0, 0, 0, 0)));
            mistakes.Add(new Mistake(1, "Class2", new BoundingBox(0, 0, 0, 0)));
            for (int i = 0; i < mistakes.Count; i++)
            {
                errorsGV.Rows.Add(mistakes[i].Text);
            }
        }
    }
}
