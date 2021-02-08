using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        // При закрытии формы
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dialogResult = MessageBox.Show("Вы уверены, что хотите выйти?", "Верификация диаграмм UML",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = dialogResult == DialogResult.Yes ? false : true;
        }
    }
}
