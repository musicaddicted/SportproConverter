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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btLeaveBlank = new System.Windows.Forms.Button();
            this.btChoose = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btSkip = new System.Windows.Forms.Button();
            this.cbSaveChoise = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(12, 72);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(524, 330);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // btLeaveBlank
            // 
            this.btLeaveBlank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btLeaveBlank.Location = new System.Drawing.Point(438, 419);
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
            this.btChoose.Location = new System.Drawing.Point(243, 419);
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
            this.btSkip.Location = new System.Drawing.Point(324, 419);
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
            this.cbSaveChoise.Location = new System.Drawing.Point(12, 423);
            this.cbSaveChoise.Name = "cbSaveChoise";
            this.cbSaveChoise.Size = new System.Drawing.Size(117, 17);
            this.cbSaveChoise.TabIndex = 7;
            this.cbSaveChoise.Text = "Запомнить выбор";
            this.cbSaveChoise.UseVisualStyleBackColor = true;
            // 
            // CategoriesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 454);
            this.Controls.Add(this.cbSaveChoise);
            this.Controls.Add(this.btSkip);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btChoose);
            this.Controls.Add(this.btLeaveBlank);
            this.Controls.Add(this.treeView1);
            this.Name = "CategoriesForm";
            this.ShowIcon = false;
            this.Text = "Каталог";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btLeaveBlank;
        private System.Windows.Forms.Button btChoose;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btSkip;
        private System.Windows.Forms.CheckBox cbSaveChoise;
    }
}