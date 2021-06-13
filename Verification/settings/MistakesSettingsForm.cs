using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Verification.rating_system;

namespace Verification.settings {
    public partial class MistakesSettingsForm : Form {
        MistakesSettingsController controller;
        MistakeModel model;
        public MistakesSettingsForm(MistakesSettingsController controller, MistakeModel model) {
            InitializeComponent();
            this.controller = controller;
            this.model = model;
            customizeTable();
        }
        private void customizeTable() {
            dgvMistakes.ColumnHeadersDefaultCellStyle.Font =
                new Font(dgvMistakes.Font.FontFamily, 7f, FontStyle.Bold);
            dgvMistakes.ColumnCount = 2;
            dgvMistakes.Columns[0].Name = "Ошибка";
            dgvMistakes.Columns[1].Name = "Балл";

            dgvMistakes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvMistakes.Font = new Font(dgvMistakes.Font.FontFamily, 7.5f);

            dgvMistakes.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMistakes.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMistakes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvMistakes.Columns[1].Width = 80;

            dgvMistakes.Columns[0].ReadOnly = true;
        }
        internal void ShowMsg(string msg, string title) {
            MessageBox.Show(
                msg,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }
        public bool validateValues() {
            for (int i = 0; i<dgvMistakes.RowCount; i++) {
                if (dgvMistakes.Rows[i].Cells[1].ErrorText != "")
                    return false;
            }
            return true;
        }

        public void fillForm() {
           dgvMistakes.Rows.Clear();
            
            foreach (ALL_MISTAKES key in MistakeModel.mistakes.Keys) {
                dgvMistakes.Rows.Add(new object[] { MistakeModel.mistakes[key].Item2, MistakeModel.mistakes[key].Item1});
                dgvMistakes.Rows[dgvMistakes.Rows.Count - 1].Tag = key;
            }
        }
        public void updateModel() {
            for(int i = 0; i<dgvMistakes.RowCount; i++) {
                var row = dgvMistakes.Rows[i];
                model.setValue(row.Tag.ToString(), row.Cells[1].Value.ToString());                
            }
        }
        private void btnExportClick(object sender, EventArgs e) {
            controller.export();
        }

        private void btnImportClick(object sender, EventArgs e) {
            controller.import();
        }

        private void btnOkClick(object sender, EventArgs e) {
            controller.onOk();
        }

        private void bntCancelClick(object sender, EventArgs e) {
            this.Dispose();
        }
        private void dgvMistakes_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
            if (e.ColumnIndex==0) return;
            try {
                Double.Parse(e.FormattedValue.ToString());
                dgvMistakes.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
            } catch(Exception) {
                dgvMistakes.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "старое значение: "+dgvMistakes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            }
        }
    }
}
