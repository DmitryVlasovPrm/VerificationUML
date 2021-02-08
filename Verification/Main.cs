using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Verification
{
    public partial class Main : Form
    {
        public Distribution Distribution;

        public Main()
        {
            InitializeComponent();
            Distribution = new Distribution();
            Distribution.NewDiagramAdded += AddDiagram;
            Distribution.SomethingChanged += UpdateGUIState;
            dataGridView1.Font = new Font("Microsoft Sans Serif", 14);
        }

        // Выбор файлов
        private void выбратьToolStripMenuItem_Click(object sender, EventArgs e)
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

        // Сохранение результата
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Кнопка "пакетная обработка"
        private void button1_Click(object sender, EventArgs e)
        {

        }

        // Кнопка "верифицировать"
        private void button2_Click(object sender, EventArgs e)
        {

        }

        // Добавление новой диаграммы в GUI
        private void AddDiagram(string name)
        {
            if (dataGridView1.Columns.Count == 0)
                dataGridView1.Columns.Add("diagramName", "");
            dataGridView1.Rows.Add(name);
        }

        // Обновление картинок и списка ошибок
        private void UpdateGUIState()
        {
            if (dataGridView1.SelectedCells.Count !=0)
            {
                var selectedName = dataGridView1.SelectedCells[0].Value.ToString();
                var selectedDiagram = Distribution.AllDiagrams.Find(a => a.Name == selectedName);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = selectedDiagram.Image.Bitmap;
            }
        }

        // Обновление выделенной диаграммы
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateGUIState();
        }

        // При закрытии формы
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dialogResult = MessageBox.Show("Вы уверены, что хотите выйти?", "Верификация диаграмм UML",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = dialogResult == DialogResult.Yes ? false : true;
        }
    }
}
