using Emgu.CV;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Verification
{
    public partial class Main
    {
        // Функция выбора файлов
        private void ChooseFiles()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите xmi, png или jpeg файлы",
                Multiselect = true,
                Filter = "Файлы xmi, png или jpeg|*.xmi; *.png; *.jpeg"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var allFiles = openFileDialog.FileNames.ToList();
                Distribution.CreateDiagrams(allFiles);
            }
        }

        // Добавление новой диаграммы в GUI
        private void AddDiagram(string name)
        {
            // Если таблица диаграмм строится впервые
            if (diagramsGV.Columns.Count == 0)
                diagramsGV.Columns.Add("diagramName", "");

            var diagramsRowsCount = diagramsGV.Rows.Count;
            for (var i = 0; i < diagramsRowsCount; i++)
            {
                var rowValue = diagramsGV.Rows[i].Cells[0].Value.ToString();
                if (rowValue == name)
                {
                    UpdateDiagramOnGUI(new List<string>() { name });
                    return;
                }
            }

            diagramsGV.Rows.Add(name);
            btVerify.Enabled = btDelete.Enabled = btOutput.Enabled = true;
        }

        // Обновление картинки при выборе диаграммы
        private void UpdateDiagramOnGUI(List<string> diagramNamesToUpdate)
        {
            // Случай, когда все диаграммы удалили, и нужно очистить GUI элементы
            if (diagramNamesToUpdate == null)
            {
                diagramPicture.Image = null;
                isClearingRows = true;
                errorsGV.Rows.Clear();
                isClearingRows = false;
                errorsGB.Text = "Ошибки";
                return;
            }

            // Случай, когда произошло обновление неактивной диаграммы
            var currentName = diagramsGV.CurrentCell.Value.ToString();
            if (!diagramNamesToUpdate.Contains(currentName))
                return;

            // Произошло обновление активной диаграммы, меняем информацию о ней на GUI элементах
            Distribution.AllDiagrams.TryGetValue(currentName, out Diagram selectedDiagram);
            if (selectedDiagram != null)
            {
                diagramPicture.Image = selectedDiagram.Image != null ? selectedDiagram.Image.Bitmap : diagramPicture.Image = null;
                ShowDiagramMistakes(selectedDiagram);
            }
        }

        // Показ всех ошибок в таблице
        private void ShowDiagramMistakes(Diagram diagram)
        {
            isClearingRows = true;
            errorsGV.Rows.Clear();
            isClearingRows = false;

            var typeStr = "";
            switch (diagram.EType)
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
            errorsGB.Text = string.Format($"Ошибки (Тип диаграммы: {typeStr})");

            if (!diagram.Verificated)
                return;

            // Если таблица ошибок строится впервые
            if (errorsGV.Columns.Count == 0)
            {
                var column = new DataGridViewColumn
                {
                    Name = "id",
                    HeaderText = "",
                    Visible = false,
                    CellTemplate = new DataGridViewTextBoxCell()
                };
                errorsGV.Columns.Add(column);

                column = new DataGridViewColumn
                {
                    Name = "seriousness"
                };
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                column.HeaderText = "Серьзность";
                column.CellTemplate = new DataGridViewTextBoxCell();
                errorsGV.Columns.Add(column);

                column = new DataGridViewColumn
                {
                    Name = "text",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    HeaderText = "Текст",
                    CellTemplate = new DataGridViewTextBoxCell()
                };
                errorsGV.Columns.Add(column);
            }

            var mistakes = diagram.Mistakes.OrderByDescending(a => a.Seriousness).ThenBy(a => a.Text).ToList();
            var mistakesCount = mistakes.Count;
            for (var i = 0; i < mistakesCount; i++)
            {
                var curMistake = mistakes[i];
                errorsGV.Rows.Add(curMistake.Id, curMistake.Seriousness, curMistake.Text);
                var color = Color.White;
                switch (curMistake.Seriousness)
                {
                    case 0:
                        color = Color.FromArgb(255, 240, 157);
                        break;
                    case 1:
                        color = Color.FromArgb(255, 157, 157);
                        break;
                    case 2:
                        color = Color.FromArgb(255, 50, 50);
                        break;
                }
                errorsGV.Rows[errorsGV.Rows.Count - 1].DefaultCellStyle.BackColor = color;
            }
        }

        // Обновление отображния картинок
        private void UpdateMistakesOnGUI()
        {
            if (errorsGV.Rows.Count == 0)
            {
                return;
            }

            var selectedDiagramName = diagramsGV.CurrentCell.Value.ToString();
            Distribution.AllDiagrams.TryGetValue(selectedDiagramName, out Diagram selectedDiagram);

            if (!selectedDiagram.Verificated)
            {
                diagramPicture.Image = selectedDiagram.Image.Bitmap;
                return;
            }

            if (selectedDiagram.Image != null)
            {
                var selectedMistakeId = errorsGV.CurrentRow.Cells[0].Value.ToString();
                errorsGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                errorsGV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                var mistake = selectedDiagram.Mistakes.Find(x => x.Id.ToString() == selectedMistakeId);

                if (mistake.Bbox != null)
                {
                    var bbox = mistake.Bbox;
                    var curImage = selectedDiagram.Image.Copy();
                    CvInvoke.Rectangle(curImage, new Rectangle(bbox.X, bbox.Y, bbox.W, bbox.H), new MCvScalar(0, 0, 255, 255), 2);
                    diagramPicture.Image = curImage.Bitmap;
                }
                else
                {
                    diagramPicture.Image = selectedDiagram.Image.Bitmap;
                }
            }
        }
    }
}
