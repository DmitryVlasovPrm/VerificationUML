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
            if (dataGridView1.Columns.Count == 0)
                dataGridView1.Columns.Add("diagramName", "");
            dataGridView1.Rows.Add(name);
            button2.Enabled = true;
            button3.Enabled = true;
        }

        // Обновление картинок и списка ошибок
        private void UpdateGUIState()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                pictureBox1.Image = null;
                return;
            }

            if (dataGridView1.SelectedCells.Count == 0)
                dataGridView1.SelectedCells[0].Selected = true;

            var selectedName = dataGridView1.SelectedCells[0].Value.ToString();
            var selectedDiagram = Distribution.AllDiagrams.Find(a => a.Name == selectedName);
            if (selectedDiagram.Image != null)
                pictureBox1.Image = selectedDiagram.Image.Bitmap;
            else
                pictureBox1.Image = null;
        }
    }
}
