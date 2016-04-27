namespace SPConverter.GUI
{
    partial class CategoriesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoriesForm));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btLeaveBlank = new System.Windows.Forms.Button();
            this.btChoose = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btSkip = new System.Windows.Forms.Button();
            this.cbSaveChoise = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelEdit = new System.Windows.Forms.Panel();
            this.btAdd = new System.Windows.Forms.Button();
            this.btRename = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btEditClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panelEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(12, 72);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(523, 369);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // btLeaveBlank
            // 
            this.btLeaveBlank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btLeaveBlank.Location = new System.Drawing.Point(425, 6);
            this.btLeaveBlank.Name = "btLeaveBlank";
            this.btLeaveBlank.Size = new System.Drawing.Size(98, 23);
            this.btLeaveBlank.TabIndex = 1;
            this.btLeaveBlank.Text = "Без категории";
            this.btLeaveBlank.UseVisualStyleBackColor = true;
            this.btLeaveBlank.Click += new System.EventHandler(this.btLeaveBlank_Click);
            // 
            // btChoose
            // 
            this.btChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btChoose.Location = new System.Drawing.Point(233, 6);
            this.btChoose.Name = "btChoose";
            this.btChoose.Size = new System.Drawing.Size(75, 23);
            this.btChoose.TabIndex = 2;
            this.btChoose.Text = "Выбрать";
            this.btChoose.UseVisualStyleBackColor = true;
            this.btChoose.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(12, 13);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(526, 53);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // btSkip
            // 
            this.btSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSkip.Location = new System.Drawing.Point(314, 6);
            this.btSkip.Name = "btSkip";
            this.btSkip.Size = new System.Drawing.Size(75, 23);
            this.btSkip.TabIndex = 6;
            this.btSkip.Text = "Пропустить";
            this.btSkip.UseVisualStyleBackColor = true;
            this.btSkip.Click += new System.EventHandler(this.btSkip_Click);
            // 
            // cbSaveChoise
            // 
            this.cbSaveChoise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbSaveChoise.AutoSize = true;
            this.cbSaveChoise.Checked = true;
            this.cbSaveChoise.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSaveChoise.Location = new System.Drawing.Point(3, 9);
            this.cbSaveChoise.Name = "cbSaveChoise";
            this.cbSaveChoise.Size = new System.Drawing.Size(117, 17);
            this.cbSaveChoise.TabIndex = 7;
            this.cbSaveChoise.Text = "Запомнить выбор";
            this.cbSaveChoise.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbSaveChoise);
            this.panel1.Controls.Add(this.btSkip);
            this.panel1.Controls.Add(this.btLeaveBlank);
            this.panel1.Controls.Add(this.btChoose);
            this.panel1.Location = new System.Drawing.Point(12, 447);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(526, 34);
            this.panel1.TabIndex = 8;
            // 
            // panelEdit
            // 
            this.panelEdit.Controls.Add(this.btDelete);
            this.panelEdit.Controls.Add(this.btRename);
            this.panelEdit.Controls.Add(this.btAdd);
            this.panelEdit.Location = new System.Drawing.Point(12, 22);
            this.panelEdit.Name = "panelEdit";
            this.panelEdit.Size = new System.Drawing.Size(523, 44);
            this.panelEdit.TabIndex = 9;
            this.panelEdit.Visible = false;
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(6, 9);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(89, 23);
            this.btAdd.TabIndex = 0;
            this.btAdd.Text = "Добавить";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btRename
            // 
            this.btRename.Location = new System.Drawing.Point(106, 9);
            this.btRename.Name = "btRename";
            this.btRename.Size = new System.Drawing.Size(119, 23);
            this.btRename.TabIndex = 1;
            this.btRename.Text = "Переименовать";
            this.btRename.UseVisualStyleBackColor = true;
            // 
            // btDelete
            // 
            this.btDelete.Location = new System.Drawing.Point(233, 9);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(75, 23);
            this.btDelete.TabIndex = 2;
            this.btDelete.Text = "Удалить";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btEditClose
            // 
            this.btEditClose.Location = new System.Drawing.Point(454, 453);
            this.btEditClose.Name = "btEditClose";
            this.btEditClose.Size = new System.Drawing.Size(75, 23);
            this.btEditClose.TabIndex = 10;
            this.btEditClose.Text = "Закрыть";
            this.btEditClose.UseVisualStyleBackColor = true;
            this.btEditClose.Visible = false;
            // 
            // CategoriesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 493);
            this.Controls.Add(this.btEditClose);
            this.Controls.Add(this.panelEdit);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.treeView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CategoriesForm";
            this.Text = "Каталог";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelEdit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btLeaveBlank;
        private System.Windows.Forms.Button btChoose;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btSkip;
        private System.Windows.Forms.CheckBox cbSaveChoise;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelEdit;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btRename;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btEditClose;
    }
}