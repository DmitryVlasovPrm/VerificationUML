
namespace Verification.settings {
    partial class MistakesSettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.dgvMistakes = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMistakes)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMistakes
            // 
            this.dgvMistakes.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvMistakes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMistakes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMistakes.GridColor = System.Drawing.SystemColors.Control;
            this.dgvMistakes.Location = new System.Drawing.Point(0, 0);
            this.dgvMistakes.Name = "dgvMistakes";
            this.dgvMistakes.Size = new System.Drawing.Size(800, 450);
            this.dgvMistakes.TabIndex = 0;
            // 
            // MistakesSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvMistakes);
            this.Name = "MistakesSettingsForm";
            this.Text = "MistakesSettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMistakes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMistakes;
    }
}