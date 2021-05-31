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
    // Если возникла ошибка [NonComVisibleBaseClass]
    // https://stackoverflow.com/questions/1049742/noncomvisiblebaseclass-was-detected-how-do-i-fix-this
    public partial class SettingsForm : Form {
        private SettingsController controller;
        public SettingsForm(SettingsController controller) {
            InitializeComponent();
            new ToolTip().SetToolTip(tbMax, "Положительное целое или действительное число через запятую");
            new ToolTip().SetToolTip(tbMin, "Положительное целое или действительное число через запятую");

            this.controller = controller;
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

        internal void fillFormByDefault(string min, string max, int cbIndex) {
            tbMax.Text = max;
            tbMin.Text = min;
            cbMeassure.SelectedIndex = cbIndex;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            controller.onOk(tbMin.Text, tbMax.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            controller.onCancel();
        }

        private void cbMeassure_SelectedIndexChanged(object sender, EventArgs e) {
            controller.onCbChanged(cbMeassure.SelectedIndex);
        }

        private void tb_onlyNumbers(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&(e.KeyChar != ',')) {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1)) {
                e.Handled = true;
            }
        }

        private void btnExport_Click(object sender, EventArgs e) {
            controller.export(tbMin.Text, tbMax.Text);
        }

        private void btnImport_Click(object sender, EventArgs e) {
            controller.import();
        }
    }
}
