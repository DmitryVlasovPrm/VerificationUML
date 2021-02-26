using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Verification.type_definer;
using Verification.uc_ver;
using System.Linq;
using System.ComponentModel;
using System.Threading;

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
            diagramsGV.Font = new Font("Microsoft Sans Serif", 14);
            diagramPicture.SizeMode = PictureBoxSizeMode.Zoom;
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
        private void btPackage_Click(object sender, EventArgs e)
        {

        }

        // Кнопка "верифицировать"
        private void btVerify_Click(object sender, EventArgs e)
        {
            var selectedKey = diagramsGV.CurrentCell.Value.ToString();
            var curDiagram = Distribution.AllDiagrams[selectedKey];

            ShowMsg("Определяем тип диаграммы", "Сообщение");
            var type = TypeDefiner.DefineDiagramType(curDiagram.XmlInfo);


            string waitingFormMsg = "";
            BackgroundWorker bw = new BackgroundWorker();
            switch (type)
            {
                case EDiagramTypes.AD:
                    {
                        waitingFormMsg = "Верификация ДА";
                        bw.DoWork += (obj, ex) => StartADVer(curDiagram);
                        break;
                    }
                case EDiagramTypes.UCD:
                    {
                        waitingFormMsg = "Верификация ДП";
                        bw.DoWork += (obj, ex) => StartUCDVer(curDiagram);
                        break;
                    }
                case EDiagramTypes.CD:
                    {
                        waitingFormMsg = "Верификация ДК";
                        bw.DoWork += (obj, ex) => StartCDVer(curDiagram);
                        break;
                    }
                case EDiagramTypes.UNDEF:
                    {
                        ShowMsg("Тип диаграммы не определен", "Сообщение");
                        return;
                    }
            }
            WaitingForm waitingForm = new WaitingForm();
            waitingForm.InitializationWaitingForm(this, waitingFormMsg); // инициализация прогресс бара
            waitingForm.show();
            bw.RunWorkerCompleted += waitingForm.bw_RunWorkerCompleted;
            bw.RunWorkerAsync();


        }
        private void ShowMsg(string msg, string title)
        {
            DialogResult result = MessageBox.Show(
            msg,
            title,
            MessageBoxButtons.OK,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);
        }
        private void StartADVer(Diagram diagram) { }
        private void StartUCDVer(Diagram diagram)
        {
            var vetificatorUC = new VerificatorUC(diagram);
            vetificatorUC.Verificate();
        }
        private void StartCDVer(Diagram diagram) { }


        // Кнопка "добавить" диаграмму
        private void btAdd_Click(object sender, EventArgs e)
        {
            ChooseFiles();
        }

        // Кнопка "удалить" диаграмму
        private void btDelete_Click(object sender, EventArgs e)
        {
            if (diagramsGV.SelectedCells.Count == 0)
            {
                MessageBox.Show($"Необходимо выбрать диаграмму", "Верификация диаграмм UML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in diagramsGV.SelectedRows)
            {
                var selectedName = row.Cells[0].Value.ToString();
                Distribution.AllDiagrams.Remove(selectedName);
                diagramsGV.Rows.RemoveAt(row.Index);
            }

            if (diagramsGV.Rows.Count == 0)
                btDelete.Enabled = false;
        }

        // Обновление выделенной диаграммы
        private void diagramsGV_SelectionChanged(object sender, EventArgs e)
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
