using System.Windows.Forms;
using System.Linq;

namespace Verification
{
    public partial class Main
    {
        // Функция выбора файлов
        private void ChooseFiles()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите xmi и png файлы";
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Файлы xmi и png|*.xmi; *.png";

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
            diagramsGV.Rows.Add(name);
            btVerify.Enabled = true;
            btDelete.Enabled = true;
        }

        // Обновление картинок и списка ошибок
        private void UpdateGUIState()
        {
            if (diagramsGV.Rows.Count == 0)
            {
                diagramPicture.Image = null;
                return;
            }

            if (diagramsGV.SelectedCells.Count == 0)
                diagramsGV.SelectedCells[0].Selected = true;

            var selectedName = diagramsGV.SelectedCells[0].Value.ToString();
            Distribution.AllDiagrams.TryGetValue(selectedName, out Diagram selectedDiagram);
            if (selectedDiagram != null && selectedDiagram.Image != null)
                diagramPicture.Image = selectedDiagram.Image.Bitmap;
            else
                diagramPicture.Image = null;
        }
    }
}
