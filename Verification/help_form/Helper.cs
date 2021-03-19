using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Verification.help_form;

namespace Verification {
    public partial class Helper : Form {
        DataGridView dgvMistakes;
        
        private const int MISTAKE_COL_NUM = 6;
        private const int Panel1MaxWidth = 200;

        public Helper() {
            InitializeComponent();
            InitializeTreeView();
            splitContainer1.SplitterDistance = Panel1MaxWidth;
            
            createTable();
        }
        private void createTable() {
            dgvMistakes = new DataGridView();
            splitContainer1.Panel2.Controls.Add(dgvMistakes);
            dgvMistakes.Parent = splitContainer1.Panel2;
            dgvMistakes.Dock = DockStyle.Fill;
            dgvMistakes.ReadOnly = true;
            dgvMistakes.Visible = false;
            customizeTable();
            
        }
        private void customizeTable() {
            dgvMistakes.ColumnHeadersDefaultCellStyle.Font =
                new Font(dgvMistakes.Font.FontFamily, 7f, FontStyle.Bold);


            dgvMistakes.ColumnCount = MISTAKE_COL_NUM;
            dgvMistakes.Columns[0].Name = "Номер";
            dgvMistakes.Columns[1].Name = "Серьезность";
            dgvMistakes.Columns[2].Name = "Ошибка";
            dgvMistakes.Columns[3].Name = "Описание";
            dgvMistakes.Columns[4].Name = "Причины";
            dgvMistakes.Columns[5].Name = "Этап";

            dgvMistakes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvMistakes.Font = new Font(dgvMistakes.Font.FontFamily, 7.5f);


            dgvMistakes.Columns[0].Width = 40;
            dgvMistakes.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMistakes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvMistakes.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            

        }

        private void InitializeTreeView() {
            treevHelp.BeginUpdate();
            treevHelp.Nodes.Add("Общие сведения");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Tag = TreeTags.GENERAL;

            treevHelp.Nodes.Add("Работа с программой");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Tag = TreeTags.PROGRAM;

            treevHelp.Nodes[1].Nodes.Add("Верификация одной диаграммы");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[0].Tag = TreeTags.VER1;

            treevHelp.Nodes[1].Nodes.Add("Верификация нескольких диаграммы");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[1].Tag = TreeTags.VER2;

            treevHelp.Nodes[1].Nodes.Add("Верификация пакета");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Nodes[2].Tag = TreeTags.VER_PACK;

            treevHelp.Nodes.Add("Выводимые ошибки");
            treevHelp.Nodes[treevHelp.Nodes.Count - 1].Tag = TreeTags.MISTAKES;
            treevHelp.EndUpdate();
        }

        private void changeVisible(bool isRBVisible) {
            rbHelp.Visible = isRBVisible;
            dgvMistakes.Visible = !isRBVisible;
        }
        private void fillTable() {
            dgvMistakes.Rows.Clear();
            var mistakesLst = Properties.Resources.ADMistakes.Split('|');
            // для всех строк
            for (int i = 0; i < mistakesLst.Length; i+=MISTAKE_COL_NUM) {
                dgvMistakes.Rows.Add(new object[] { mistakesLst[i], mistakesLst[i+1], 
                    mistakesLst[i+2], mistakesLst[i+3], mistakesLst[i+4], mistakesLst[i+5] });
            }
        }

        private void showInfo() {
            //1 | СЕРЬЕЗНОСТЬ | ОШИБКА | ОПИСАНИЕ | ПРИЧИНЫ ОШИБКИ | ЭТАП ПРОВЕРКИ 
            switch ((TreeTags)treevHelp.SelectedNode.Tag) {
                case TreeTags.GENERAL:
                    changeVisible(true);
                    rbHelp.Clear();
                    rbHelp.Rtf = @"{\rtf1\utf-8"+Properties.Resources.HelpGeneral+"}";
                    break;
                case TreeTags.PROGRAM:
                    changeVisible(true); 
                    rbHelp.Clear();
                    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpGeneral + "}";
                    break;
                case TreeTags.VER1:
                    changeVisible(true);
                    rbHelp.Clear();
                    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpGeneral + "}";
                    break;
                case TreeTags.VER2:
                    changeVisible(true);
                    rbHelp.Clear();
                    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpGeneral + "}";
                    break;
                case TreeTags.VER_PACK:
                    changeVisible(true);
                    rbHelp.Clear();
                    rbHelp.Rtf = @"{\rtf1\utf-8" + Properties.Resources.HelpGeneral + "}";
                    break;
                case TreeTags.MISTAKES:
                    changeVisible(false);
                    fillTable();
                    break;
                default:
                    break;
            }
        }

        private void treevHelp_AfterSelect(object sender, TreeViewEventArgs e) {
            if (treevHelp.SelectedNode == null) return;
            if (treevHelp.SelectedNode.Tag == null) return;
            showInfo();
        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e) {

        }
        
        private void splitContainer1_SizeChanged(object sender, EventArgs e) {
            if (splitContainer1.Panel1.Width > Panel1MaxWidth) {
                splitContainer1.SplitterDistance = Panel1MaxWidth;
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) {
            if (splitContainer1.Panel1.Width > Panel1MaxWidth) {
                splitContainer1.SplitterDistance = Panel1MaxWidth;
            }
        }
    }
}
