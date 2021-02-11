using System;
using System.Drawing;
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
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // Выбор файлов
        private void выбратьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseFiles();
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

        // Кнопка "добавить" диаграмму
        private void button4_Click(object sender, EventArgs e)
        {
            ChooseFiles();
        }

        // Кнопка "удалить" диаграмму
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show($"Необходимо выбрать диаграмму", "Верификация диаграмм UML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                var selectedName = row.Cells[0].Value.ToString();
                Distribution.AllDiagrams.RemoveAll(a => a.Name == selectedName);
                dataGridView1.Rows.RemoveAt(row.Index);
            }

            if (dataGridView1.Rows.Count == 0)
                button3.Enabled = false;
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
