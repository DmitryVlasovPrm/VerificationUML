
namespace Verification.settings {
    partial class SettingsForm {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbMax = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gboxMin = new System.Windows.Forms.GroupBox();
            this.tbMin = new System.Windows.Forms.TextBox();
            this.cbMeassure = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.gboxMin.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbMax);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tbMax
            // 
            this.tbMax.Location = new System.Drawing.Point(42, 38);
            this.tbMax.MaxLength = 6;
            this.tbMax.Name = "tbMax";
            this.tbMax.Size = new System.Drawing.Size(46, 20);
            this.tbMax.TabIndex = 2;
            this.tbMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_onlyNumbers);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Max:";
            // 
            // gboxMin
            // 
            this.gboxMin.Controls.Add(this.tbMin);
            this.gboxMin.Controls.Add(this.cbMeassure);
            this.gboxMin.Controls.Add(this.label1);
            this.gboxMin.Location = new System.Drawing.Point(0, 106);
            this.gboxMin.Name = "gboxMin";
            this.gboxMin.Size = new System.Drawing.Size(180, 100);
            this.gboxMin.TabIndex = 0;
            this.gboxMin.TabStop = false;
            // 
            // tbMin
            // 
            this.tbMin.Location = new System.Drawing.Point(38, 46);
            this.tbMin.MaxLength = 6;
            this.tbMin.Name = "tbMin";
            this.tbMin.Size = new System.Drawing.Size(46, 20);
            this.tbMin.TabIndex = 3;
            this.tbMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_onlyNumbers);
            // 
            // cbMeassure
            // 
            this.cbMeassure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMeassure.FormattingEnabled = true;
            this.cbMeassure.Items.AddRange(new object[] {
            "Балл",
            "%"});
            this.cbMeassure.Location = new System.Drawing.Point(111, 46);
            this.cbMeassure.Name = "cbMeassure";
            this.cbMeassure.Size = new System.Drawing.Size(63, 21);
            this.cbMeassure.TabIndex = 2;
            this.cbMeassure.SelectedIndexChanged += new System.EventHandler(this.cbMeassure_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(206, 241);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(105, 37);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(38, 241);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 37);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(197, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(125, 24);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Экспорт настроек";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(197, 53);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(125, 24);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Импорт настроек";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 295);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gboxMin);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsForm";
            this.Text = "Настройки";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gboxMin.ResumeLayout(false);
            this.gboxMin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gboxMin;
        private System.Windows.Forms.TextBox tbMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMin;
        private System.Windows.Forms.ComboBox cbMeassure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
    }
}