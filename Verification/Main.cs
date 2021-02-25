using System;
using System.Drawing;
using System.Windows.Forms;
using ActivityDiagramVer;
using ActivityDiagramVer.parser;
using ActivityDiagramVer.result;
using ActivityDiagramVer.verification.lexical;
using ActivityDiagramVer.verification.syntax;
using Emgu.CV;
using Emgu.CV.Structure;
using Verification.type_definer;
using System.Linq;
using ActivityDiagramVer.verification;
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

            //showMsg("Определяем тип диаграммы", "Сообщение");
            var type = TypeDefiner.DefineDiagramType(curDiagram.XmlInfo);


            string waitingFormMsg = "";
            BackgroundWorker bw = new BackgroundWorker();
            switch (type)
            {
                case EDiagramTypes.AD:
                    {
                        //showMsg("Верификация диаграммы активностей начата", "Сообщение");
                        ADMistakeFactory.diagram = curDiagram;
                        waitingFormMsg = "Верификация ДА";
                        bw.DoWork += (obj, ex) => startADVer(curDiagram);
                        break;
                    }
                case EDiagramTypes.UCD:
                    {
                        waitingFormMsg = "Верификация ДП";
                        bw.DoWork += (obj, ex) => startUCDVer(curDiagram);
                        break;
                    }
                case EDiagramTypes.CD:
                    {
                        waitingFormMsg = "Верификация ДК";
                        bw.DoWork += (obj, ex) => startCDVer(curDiagram);
                        break;
                    }
                case EDiagramTypes.UNDEF:
                    {
                        showMsg("Тип диаграммы не определен", "Сообщение");
                        return;
                    }
            }
            WaitingForm waitingForm = new WaitingForm();
            waitingForm.InitializationWaitingForm(this, waitingFormMsg);        // инициализация прогресс бара
            waitingForm.show();
            bw.RunWorkerCompleted += waitingForm.bw_RunWorkerCompleted;
            bw.RunWorkerAsync();


        }
        private void showMsg(string msg, string title)
        {
            DialogResult result = MessageBox.Show(
            msg,
            title,
            MessageBoxButtons.OK,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);

        }
        private void startADVer(Diagram diagram)
        {
            ADNodesList adNodesList = new ADNodesList();
            XmiParser parser = new XmiParser(adNodesList);

            var isSuccess = parser.Parse(@"C:\Users\DocGashe\Documents\Лекции\ДиПломная\Тестирование\С координатами\Условный перед join.xmi");
            Thread.Sleep(10000);
            return;
            //parser.Parse(diagram.Name);     //TODO: путь до xmi

            //Console.WriteLine("----------------------");
            //for (int i = 0; i < adNodesList.size(); i++)
            //{
            //    Console.WriteLine(adNodesList.get(i).getId() + " " + adNodesList.get(i).getType());
            //}
            //Console.WriteLine("----------------------");
            if (!isSuccess)
            {
                showMsg("Не удалось получить диаграмму активности из xmi файла: \n" + diagram.Name, "Сообщение");
                return;
            }
            adNodesList.connect();
            // adNodesList.print();


            LexicalAnalizator lexicalAnalizator = new LexicalAnalizator(Level.EASY);
            lexicalAnalizator.setDiagramElements(adNodesList);
            lexicalAnalizator.check();

            SyntaxAnalizator syntaxAnalizator = new SyntaxAnalizator(Level.EASY);
            syntaxAnalizator.setDiagramElements(adNodesList);
            syntaxAnalizator.check();


            if (!diagram.Mistakes.Any(x => x.Seriousness == (int)Level.FATAL))
            {
                PetriNet petriNet = new PetriNet();
                petriNet.petriCheck(adNodesList);
            }
            showMsg("Верификация завершена", "Сообщение");
        }
        private void startUCDVer(Diagram diagram) { }
        private void startCDVer(Diagram diagram) { }


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
