namespace ADB
{
    partial class SelectForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectForm));
			this._btnSelect = new System.Windows.Forms.Button();
			this.treeView1 = new ADB.AutoSelectTreeView();
			this.SuspendLayout();
			// 
			// _btnSelect
			// 
			this._btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this._btnSelect, "_btnSelect");
			this._btnSelect.Name = "_btnSelect";
			this._btnSelect.UseVisualStyleBackColor = true;
			// 
			// treeView1
			// 
			this.treeView1.CheckBoxes = true;
			this.treeView1.ItemHeight = 16;
			resources.ApplyResources(this.treeView1, "treeView1");
			this.treeView1.Name = "treeView1";
			// 
			// SelectForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this._btnSelect);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SelectForm";
			this.Load += new System.EventHandler(this.SelectOption_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnSelect;
        private ADB.AutoSelectTreeView treeView1;
    }
}