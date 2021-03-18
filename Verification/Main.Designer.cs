
namespace Verification
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выбратьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.diagramPicture = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.diagramsGV = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.errorsGV = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btOutput = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btPackage = new System.Windows.Forms.Button();
            this.btVerify = new System.Windows.Forms.Button();
            this.menu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagramPicture)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagramsGV)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorsGV)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1199, 27);
            this.menu.TabIndex = 0;
            this.menu.Text = "menu";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выбратьToolStripMenuItem,
            this.сохранитьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(53, 23);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // выбратьToolStripMenuItem
            // 
            this.выбратьToolStripMenuItem.Name = "выбратьToolStripMenuItem";
            this.выбратьToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.выбратьToolStripMenuItem.Text = "Добавить файл(ы)";
            this.выбратьToolStripMenuItem.Click += new System.EventHandler(this.выбратьToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.сохранитьToolStripMenuItem.Text = "Сохранить результат";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(74, 23);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.03771F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.962288F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1199, 712);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.30344F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.69656F));
            this.tableLayoutPanel2.Controls.Add(this.diagramPicture, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1199, 641);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // diagramPicture
            // 
            this.diagramPicture.BackColor = System.Drawing.Color.White;
            this.diagramPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.diagramPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramPicture.Location = new System.Drawing.Point(3, 3);
            this.diagramPicture.Name = "diagramPicture";
            this.diagramPicture.Size = new System.Drawing.Size(788, 635);
            this.diagramPicture.TabIndex = 0;
            this.diagramPicture.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(794, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.2987F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.7013F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(405, 641);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.diagramsGV);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 306);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 284);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Диаграммы";
            // 
            // diagramsGV
            // 
            this.diagramsGV.AllowUserToAddRows = false;
            this.diagramsGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.diagramsGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.diagramsGV.BackgroundColor = System.Drawing.Color.White;
            this.diagramsGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.diagramsGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramsGV.Location = new System.Drawing.Point(3, 19);
            this.diagramsGV.MultiSelect = false;
            this.diagramsGV.Name = "diagramsGV";
            this.diagramsGV.ReadOnly = true;
            this.diagramsGV.RowHeadersVisible = false;
            this.diagramsGV.RowHeadersWidth = 51;
            this.diagramsGV.RowTemplate.Height = 24;
            this.diagramsGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.diagramsGV.Size = new System.Drawing.Size(393, 262);
            this.diagramsGV.TabIndex = 0;
            this.diagramsGV.SelectionChanged += new System.EventHandler(this.diagramsGV_SelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.errorsGV);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 300);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ошибки";
            // 
            // errorsGV
            // 
            this.errorsGV.AllowUserToAddRows = false;
            this.errorsGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.errorsGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.errorsGV.BackgroundColor = System.Drawing.Color.White;
            this.errorsGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.errorsGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsGV.Location = new System.Drawing.Point(3, 19);
            this.errorsGV.MultiSelect = false;
            this.errorsGV.Name = "errorsGV";
            this.errorsGV.ReadOnly = true;
            this.errorsGV.RowHeadersVisible = false;
            this.errorsGV.RowHeadersWidth = 51;
            this.errorsGV.RowTemplate.Height = 24;
            this.errorsGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.errorsGV.Size = new System.Drawing.Size(393, 278);
            this.errorsGV.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.11111F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.08642F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.04938F));
            this.tableLayoutPanel5.Controls.Add(this.btOutput, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btDelete, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btAdd, 2, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 590);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(405, 51);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // btOutput
            // 
            this.btOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOutput.Enabled = false;
            this.btOutput.Location = new System.Drawing.Point(7, 8);
            this.btOutput.Margin = new System.Windows.Forms.Padding(7, 8, 3, 0);
            this.btOutput.Name = "btOutput";
            this.btOutput.Size = new System.Drawing.Size(115, 43);
            this.btOutput.TabIndex = 2;
            this.btOutput.Text = "Экспорт ошибок";
            this.btOutput.UseVisualStyleBackColor = true;
            this.btOutput.Click += new System.EventHandler(this.btOutput_Click);
            // 
            // btDelete
            // 
            this.btDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btDelete.Enabled = false;
            this.btDelete.Location = new System.Drawing.Point(132, 8);
            this.btDelete.Margin = new System.Windows.Forms.Padding(7, 8, 3, 0);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(123, 43);
            this.btDelete.TabIndex = 0;
            this.btDelete.Text = "Удалить";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btAdd
            // 
            this.btAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btAdd.Location = new System.Drawing.Point(261, 8);
            this.btAdd.Margin = new System.Windows.Forms.Padding(3, 8, 6, 0);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(138, 43);
            this.btAdd.TabIndex = 1;
            this.btAdd.Text = "Добавить";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.71059F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.60217F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.77064F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 382F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.btPackage, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btVerify, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 641);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1199, 71);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // btPackage
            // 
            this.btPackage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btPackage.Enabled = false;
            this.btPackage.Location = new System.Drawing.Point(643, 12);
            this.btPackage.Margin = new System.Windows.Forms.Padding(12, 12, 7, 12);
            this.btPackage.Name = "btPackage";
            this.btPackage.Size = new System.Drawing.Size(251, 47);
            this.btPackage.TabIndex = 0;
            this.btPackage.Text = "Пакетная обработка";
            this.btPackage.UseVisualStyleBackColor = true;
            this.btPackage.Click += new System.EventHandler(this.btPackage_Click);
            // 
            // btVerify
            // 
            this.btVerify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btVerify.Enabled = false;
            this.btVerify.Location = new System.Drawing.Point(908, 12);
            this.btVerify.Margin = new System.Windows.Forms.Padding(7, 12, 6, 12);
            this.btVerify.Name = "btVerify";
            this.btVerify.Size = new System.Drawing.Size(285, 47);
            this.btVerify.TabIndex = 1;
            this.btVerify.Text = "Верифицировать";
            this.btVerify.UseVisualStyleBackColor = true;
            this.btVerify.Click += new System.EventHandler(this.btVerify_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 739);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "Верификация диаграмм UML";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagramPicture)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagramsGV)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorsGV)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выбратьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox diagramPicture;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btPackage;
        private System.Windows.Forms.Button btVerify;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView diagramsGV;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView errorsGV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btOutput;
    }
}

