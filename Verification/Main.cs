using System;
using System.Drawing;
using System.Windows.Forms;
using Verification.ad_ver;
using Verification.package_ver;
using Verification.type_definer;
using Verification.uc_ver;
using System.Collections.Generic;

namespace Verification
{
	public partial class Main : Form
	{
		public Distribution Distribution;
		private Helper helperForm;
		private bool isClearingRows;
		private Random rnd = new Random();

		public Main()
		{
			InitializeComponent();
			Distribution = new Distribution();
			Distribution.NewDiagramAdded += AddDiagram;
			Distribution.SomethingChanged += UpdateDiagramOnGUI;

			helperForm = null;
			isClearingRows = false;

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

		/// <summary>
		/// Проверка согласованности диаграмм
		/// </summary>
		private void checkDiffDiagrams()
		{
			// имя файла = тип_диаграммы.xml (тип_диаграммы = uc\ad\cd)
			if (Distribution.AllDiagrams.Count > 3)
			{
				ShowMsg("Загружено более трех диаграмм", "Exception");
				return;
			}
			Diagram uc = null, ad = null, cd = null;
			Distribution.AllDiagrams.TryGetValue("uc", out uc);
			Distribution.AllDiagrams.TryGetValue("ad", out ad);
			Distribution.AllDiagrams.TryGetValue("cd", out cd);
			var mistakes = new List<Mistake>();         // список ошибок для вывода
			ConsistencyVerifier.Verify(uc, ad, cd, mistakes);
		}

		// Запуск прогресс бара

		// Кнопка "верифицировать"
		private void btVerify_Click(object sender, EventArgs e)
		{
			var progressBar = new ProgressBar();
			var diagramsCount = Distribution.AllDiagrams.Count;
			progressBar.SetStep(100 / diagramsCount);
			progressBar.Show();

			var verificatedNames = new List<string>();
			var keys = new string[diagramsCount];
			Distribution.AllDiagrams.Keys.CopyTo(keys, 0);
			foreach (var key in keys)
			{
				var curDiagram = Distribution.AllDiagrams[key];
				if (!curDiagram.Verificated)
				{
					Verificate(curDiagram);
					verificatedNames.Add(key);
				}
				progressBar.PerformProgress();
			}
			UpdateDiagramOnGUI(verificatedNames);

			progressBar.SetValue(100);
			progressBar.Close();
			progressBar.Dispose();
		}

		private void Verificate(Diagram diagram)
		{
			var type = diagram.EType;
			diagram.Mistakes.Clear();

			switch (type)
			{
				case EDiagramTypes.AD:
					{
						StartADVer(diagram);
						break;
					}
				case EDiagramTypes.UCD:
					{
						StartUCDVer(diagram);
						break;
					}
				case EDiagramTypes.CD:
					{
						StartCDVer(diagram);
						break;
					}
				case EDiagramTypes.UNDEF:
					{
						ShowMsg("Тип диаграммы не определен", "Сообщение");
						return;
					}
			}
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
			ADVerifier.Verify(diagram);
			diagram.Verificated = true;
			Distribution.AllDiagrams[diagram.Name] = diagram;
		}

		private void StartUCDVer(Diagram diagram)
		{
			var vetificatorUC = new VerificatorUC(diagram);
			vetificatorUC.Verificate();
			diagram.Verificated = true;
			Distribution.AllDiagrams[diagram.Name] = diagram;
		}

		private void StartCDVer(Diagram diagram)
		{
			diagram.Mistakes.Add(new Mistake(1, $"Имя класса начинается с маленькой буквы", new BoundingBox(30, 30, 254, 174)));
			diagram.Mistakes.Add(new Mistake(1, $"Имя аттрибута начинается с большой буквы", new BoundingBox(30, 30, 254, 174)));

			diagram.Verificated = true;
			Distribution.AllDiagrams[diagram.Name] = diagram;
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

			var curRow = diagramsGV.CurrentRow;
			var selectedName = curRow.Cells[0].Value.ToString();
			Distribution.AllDiagrams.Remove(selectedName);
			diagramsGV.Rows.RemoveAt(curRow.Index);

			if (diagramsGV.Rows.Count == 0)
			{
				btDelete.Enabled = btOutput.Enabled = false;
			}
		}

		// Обновление выделенной диаграммы
		private void diagramsGV_SelectionChanged(object sender, EventArgs e)
		{
			var name = diagramsGV.Rows.Count == 0 ? null : new List<string>() { diagramsGV.CurrentCell.Value.ToString() };
			UpdateDiagramOnGUI(name);
		}

		// Обновление выделенной ошибки
		private void errorsGV_SelectionChanged(object sender, EventArgs e)
		{
			if (isClearingRows)
				return;

			UpdateMistakesOnGUI();
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

		private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// create or focus help form 
			if (helperForm != null && !helperForm.Disposing && helperForm.Text != "")
			{
				helperForm.Focus();
			}
			else
			{
				helperForm = new Helper();
				helperForm.Show();
			}

		}
	}
}
