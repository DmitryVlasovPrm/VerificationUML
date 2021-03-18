using ActivityDiagramVer;
using ActivityDiagramVer.parser;
using ActivityDiagramVer.result;
using ActivityDiagramVer.verification.lexical;
using ActivityDiagramVer.verification.syntax;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Verification.type_definer;
using Verification.uc_ver;

namespace Verification
{
    public partial class Main : Form
    {
        public Distribution Distribution;
        private Helper helperForm = null;

        public Main()
        {
            InitializeComponent();
            Distribution = new Distribution();
            Distribution.NewDiagramAdded += AddDiagram;
            Distribution.SomethingChanged += UpdateGUIState;
            errorsGV.Font = new Font("Microsoft Sans Serif", 10);
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
            Verificate(curDiagram);
        }

        private void Verificate(Diagram diagram)
        {
            //ShowMsg("Определяем тип диаграммы", "Сообщение");
            var type = TypeDefiner.DefineDiagramType(diagram.XmlInfo);

            string waitingFormMsg = "";
            var bw = new BackgroundWorker();
            diagram.Mistakes.Clear();

            switch (type)
            {
                case EDiagramTypes.AD:
                    {
                        waitingFormMsg = "Верификация ДА";
                        bw.DoWork += (obj, ex) => StartADVer(diagram);
                        break;
                    }
                case EDiagramTypes.UCD:
                    {
                        waitingFormMsg = "Верификация ДП";
                        bw.DoWork += (obj, ex) => StartUCDVer(diagram);
                        break;
                    }
                case EDiagramTypes.CD:
                    {
                        waitingFormMsg = "Верификация ДК";
                        bw.DoWork += (obj, ex) => StartCDVer(diagram);
                        break;
                    }
                case EDiagramTypes.UNDEF:
                    {
                        ShowMsg("Тип диаграммы не определен", "Сообщение");
                        return;
                    }
            }
            var waitingForm = new WaitingForm();
            waitingForm.InitializationWaitingForm(this, waitingFormMsg); // инициализация прогресс бара
            waitingForm.show();
            bw.RunWorkerCompleted += waitingForm.bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }
        private void ShowMsg(string msg, string title)
        {
            MessageBox.Show(
                msg,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }
        private void StartADVer(Diagram diagram)
        {
            //if (!File.Exists(diagram.Name)) {
            //    ShowMsg($"Файл\n{diagram.Name}\n не существует","Сообщение");
            //    return;
            //}
            ADNodesList adNodesList = new ADNodesList();
            XmiParser parser = new XmiParser(adNodesList);
            ADMistakeFactory.diagram = diagram;

            //var isSuccess = parser.Parse(@"C:\Users\DocGashe\Documents\Лекции\ДиПломная\Тестирование\С координатами\Ошибка синхронизатора3.xmi");
            parser.Parse(diagram);     //TODO: путь до xmi
            if (!isSuccess)
            {
                ShowMsg("Не удалось получить диаграмму активности из xmi файла: \n" + diagram.Name, "Сообщение");
                return;
            }
            adNodesList.connect();
            // adNodesList.print();


            LexicalAnalizator lexicalAnalizator = new LexicalAnalizator();
            lexicalAnalizator.setDiagramElements(adNodesList);
            lexicalAnalizator.check();

            SyntaxAnalizator syntaxAnalizator = new SyntaxAnalizator();
            syntaxAnalizator.setDiagramElements(adNodesList);
            syntaxAnalizator.check();


            if (!diagram.Mistakes.Any(x => x.Seriousness == MistakesTypes.FATAL))
            {
                PetriNet petriNet = new PetriNet();
                petriNet.petriCheck(adNodesList);
            }
            diagram.Verificated = true;
            diagram.Mistakes.ForEach(x => Console.WriteLine(x.Text));
            ShowMsg("Верификация завершена", "Сообщение");
        }
        private void StartUCDVer(Diagram diagram)
        {
            var vetificatorUC = new VerificatorUC(diagram);
            vetificatorUC.Verificate();
            diagram.Verificated = true;
        }
        private void StartCDVer(Diagram diagram)
        {
            diagram.Verificated = true;
        }

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
            {
                btDelete.Enabled = btOutput.Enabled = false;
            }
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
            e.Cancel = dialogResult != DialogResult.Yes;
        }

        private void btOutput_Click(object sender, EventArgs e)
        {
            var selectedKey = diagramsGV.CurrentCell.Value.ToString();
            var curDiagram = Distribution.AllDiagrams[selectedKey];

            if (curDiagram.Verificated)
            {
                var saveDialog = new SaveFileDialog
                {
                    Title = "Сохранение списка ошибок",
                    FileName = "Mistakes.txt",
                    Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*"
                };
                if (saveDialog.ShowDialog() == DialogResult.OK)
                    MistakesPrinter.Print(curDiagram.Mistakes, saveDialog.FileName);
            }
            else
            {
                var result = MessageBox.Show(
                    "Диаграмма не прошла верификацию.\nВерифицировать?",
                    "Верификация диаграмм UML",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    Verificate(curDiagram);
            }
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e) {
            // create or focus help form 
            if (helperForm != null && !helperForm.Disposing && helperForm.Text!="") {
                helperForm.Focus();
            } else {
                helperForm = new Helper();
                helperForm.Show();
            }


        }
        private bool CheckOpened(string name) {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc) {
                if (frm.Text == name) {
                    return true;
                }
            }
            return false;
        }
    }
}
