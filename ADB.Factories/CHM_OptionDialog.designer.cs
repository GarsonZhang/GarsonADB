namespace ADB.Factories
{
    partial class CHM_OptionDialog
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CHM_OptionDialog));
			this.rbHTMLAndCHM = new System.Windows.Forms.RadioButton();
			this.rbHTMOnly = new System.Windows.Forms.RadioButton();
			this.button1 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._cbAllCodePages = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// rbHTMLAndCHM
			// 
			this.rbHTMLAndCHM.AccessibleDescription = null;
			this.rbHTMLAndCHM.AccessibleName = null;
			resources.ApplyResources(this.rbHTMLAndCHM, "rbHTMLAndCHM");
			this.rbHTMLAndCHM.BackgroundImage = null;
			this.rbHTMLAndCHM.Checked = true;
			this.rbHTMLAndCHM.Font = null;
			this.rbHTMLAndCHM.Name = "rbHTMLAndCHM";
			this.rbHTMLAndCHM.TabStop = true;
			this.rbHTMLAndCHM.UseVisualStyleBackColor = true;
			// 
			// rbHTMOnly
			// 
			this.rbHTMOnly.AccessibleDescription = null;
			this.rbHTMOnly.AccessibleName = null;
			resources.ApplyResources(this.rbHTMOnly, "rbHTMOnly");
			this.rbHTMOnly.BackgroundImage = null;
			this.rbHTMOnly.Font = null;
			this.rbHTMOnly.Name = "rbHTMOnly";
			this.rbHTMOnly.TabStop = true;
			this.rbHTMOnly.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.AccessibleDescription = null;
			this.button1.AccessibleName = null;
			resources.ApplyResources(this.button1, "button1");
			this.button1.BackgroundImage = null;
			this.button1.Font = null;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// panel1
			// 
			this.panel1.AccessibleDescription = null;
			this.panel1.AccessibleName = null;
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.BackgroundImage = null;
			this.panel1.Controls.Add(this.button1);
			this.panel1.Font = null;
			this.panel1.Name = "panel1";
			// 
			// groupBox1
			// 
			this.groupBox1.AccessibleDescription = null;
			this.groupBox1.AccessibleName = null;
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackgroundImage = null;
			this.groupBox1.Controls.Add(this.rbHTMLAndCHM);
			this.groupBox1.Controls.Add(this.rbHTMOnly);
			this.groupBox1.Font = null;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.AccessibleDescription = null;
			this.groupBox2.AccessibleName = null;
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackgroundImage = null;
			this.groupBox2.Controls.Add(this._cbAllCodePages);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Font = null;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// _cbAllCodePages
			// 
			this._cbAllCodePages.AccessibleDescription = null;
			this._cbAllCodePages.AccessibleName = null;
			resources.ApplyResources(this._cbAllCodePages, "_cbAllCodePages");
			this._cbAllCodePages.BackgroundImage = null;
			this._cbAllCodePages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cbAllCodePages.Font = null;
			this._cbAllCodePages.FormattingEnabled = true;
			this._cbAllCodePages.Name = "_cbAllCodePages";
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Font = null;
			this.label1.Name = "label1";
			// 
			// CHM_OptionDialog
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.panel1);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = null;
			this.Name = "CHM_OptionDialog";
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbHTMLAndCHM;
        private System.Windows.Forms.RadioButton rbHTMOnly;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox _cbAllCodePages;
		private System.Windows.Forms.Label label1;
    }
}