using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verification.settings {
    public partial class MistakesSettingsForm : Form {
        MistakesSettingsController controller;
        public MistakesSettingsForm(MistakesSettingsController controller) {
            InitializeComponent();
            this.controller = controller;
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
            dgvMistakes.Columns[1].Width = 40;
        }
    }
}
