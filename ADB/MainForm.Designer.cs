namespace ADB
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._btnBuild = new System.Windows.Forms.Button();
            this._tbTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._cbBuilderType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this._btnSelect = new System.Windows.Forms.Button();
            this._btnAddAssambly = new System.Windows.Forms.Button();
            this._btnDeleteAssembly = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.assemblyView = new ADB.AssemblyView();
            this.panel5 = new System.Windows.Forms.Panel();
            this._cbShowMembers = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._lbXMLList = new ADB.XMLListBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this._btnAddXML = new System.Windows.Forms.Button();
            this._btnDeleteXMLFile = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGenerateSolution = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSolution = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExtendTag = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExtendSection = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCustomStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCustomBuilder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuADBHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnBuild
            // 
            resources.ApplyResources(this._btnBuild, "_btnBuild");
            this._btnBuild.Name = "_btnBuild";
            this._btnBuild.UseVisualStyleBackColor = true;
            this._btnBuild.Click += new System.EventHandler(this._btnBuild_Click);
            // 
            // _tbTitle
            // 
            resources.ApplyResources(this._tbTitle, "_tbTitle");
            this._tbTitle.Name = "_tbTitle";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // _cbBuilderType
            // 
            resources.ApplyResources(this._cbBuilderType, "_cbBuilderType");
            this._cbBuilderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbBuilderType.FormattingEnabled = true;
            this._cbBuilderType.Name = "_cbBuilderType";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageList2, "imageList2");
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _btnSelect
            // 
            resources.ApplyResources(this._btnSelect, "_btnSelect");
            this._btnSelect.Name = "_btnSelect";
            this._btnSelect.UseVisualStyleBackColor = true;
            this._btnSelect.Click += new System.EventHandler(this._btnSelect_Click);
            // 
            // _btnAddAssambly
            // 
            resources.ApplyResources(this._btnAddAssambly, "_btnAddAssambly");
            this._btnAddAssambly.Name = "_btnAddAssambly";
            this._btnAddAssambly.UseVisualStyleBackColor = true;
            this._btnAddAssambly.Click += new System.EventHandler(this._btnAddAssambly_Click);
            // 
            // _btnDeleteAssembly
            // 
            resources.ApplyResources(this._btnDeleteAssembly, "_btnDeleteAssembly");
            this._btnDeleteAssembly.Name = "_btnDeleteAssembly";
            this._btnDeleteAssembly.UseVisualStyleBackColor = true;
            this._btnDeleteAssembly.Click += new System.EventHandler(this._btnDeleteAssembly_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.assemblyView);
            this.groupBox1.Controls.Add(this.panel5);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // assemblyView
            // 
            resources.ApplyResources(this.assemblyView, "assemblyView");
            this.assemblyView.Name = "assemblyView";
            this.assemblyView.ViewMode = ADB.ViewMode.TypesAndMembers;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this._cbShowMembers);
            this.panel5.Controls.Add(this._btnAddAssambly);
            this.panel5.Controls.Add(this._btnDeleteAssembly);
            this.panel5.Controls.Add(this._btnSelect);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // _cbShowMembers
            // 
            resources.ApplyResources(this._cbShowMembers, "_cbShowMembers");
            this._cbShowMembers.Name = "_cbShowMembers";
            this._cbShowMembers.UseVisualStyleBackColor = true;
            this._cbShowMembers.CheckedChanged += new System.EventHandler(this._cbShowMembers_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._lbXMLList);
            this.groupBox2.Controls.Add(this.panel6);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // _lbXMLList
            // 
            resources.ApplyResources(this._lbXMLList, "_lbXMLList");
            this._lbXMLList.FormattingEnabled = true;
            this._lbXMLList.Name = "_lbXMLList";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this._btnAddXML);
            this.panel6.Controls.Add(this._btnDeleteXMLFile);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // _btnAddXML
            // 
            resources.ApplyResources(this._btnAddXML, "_btnAddXML");
            this._btnAddXML.Name = "_btnAddXML";
            this._btnAddXML.UseVisualStyleBackColor = true;
            this._btnAddXML.Click += new System.EventHandler(this._btnAddXML_Click);
            // 
            // _btnDeleteXMLFile
            // 
            resources.ApplyResources(this._btnDeleteXMLFile, "_btnDeleteXMLFile");
            this._btnDeleteXMLFile.Name = "_btnDeleteXMLFile";
            this._btnDeleteXMLFile.UseVisualStyleBackColor = true;
            this._btnDeleteXMLFile.Click += new System.EventHandler(this._btnDeleteXMLFile_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.menuStrip1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Controls.Add(this.splitter1);
            this.panel4.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this._btnBuild);
            this.panel3.Controls.Add(this.label3);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this._cbBuilderType);
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Name = "panel7";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this._tbTitle);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuLanguage,
            this.menuGenerateSolution,
            this.menuHelp});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.menuFile.Name = "menuFile";
            resources.ApplyResources(this.menuFile, "menuFile");
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // menuLanguage
            // 
            this.menuLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem1});
            this.menuLanguage.Name = "menuLanguage";
            resources.ApplyResources(this.menuLanguage, "menuLanguage");
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Tag = "zh-CN";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.menuLanguage_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Tag = "en-US";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.menuLanguage_Click);
            // 
            // menuGenerateSolution
            // 
            this.menuGenerateSolution.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSolution});
            this.menuGenerateSolution.Name = "menuGenerateSolution";
            resources.ApplyResources(this.menuGenerateSolution, "menuGenerateSolution");
            // 
            // menuSolution
            // 
            this.menuSolution.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuExtendTag,
            this.menuExtendSection,
            this.menuCustomStyle,
            this.menuCustomBuilder});
            this.menuSolution.Name = "menuSolution";
            resources.ApplyResources(this.menuSolution, "menuSolution");
            // 
            // menuExtendTag
            // 
            this.menuExtendTag.Name = "menuExtendTag";
            resources.ApplyResources(this.menuExtendTag, "menuExtendTag");
            this.menuExtendTag.Click += new System.EventHandler(this.menuExtendTag_Click);
            // 
            // menuExtendSection
            // 
            this.menuExtendSection.Name = "menuExtendSection";
            resources.ApplyResources(this.menuExtendSection, "menuExtendSection");
            this.menuExtendSection.Click += new System.EventHandler(this.menuExtendSection_Click);
            // 
            // menuCustomStyle
            // 
            this.menuCustomStyle.Name = "menuCustomStyle";
            resources.ApplyResources(this.menuCustomStyle, "menuCustomStyle");
            this.menuCustomStyle.Click += new System.EventHandler(this.menuCustomStyle_Click);
            // 
            // menuCustomBuilder
            // 
            this.menuCustomBuilder.Name = "menuCustomBuilder";
            resources.ApplyResources(this.menuCustomBuilder, "menuCustomBuilder");
            this.menuCustomBuilder.Click += new System.EventHandler(this.menuCustomBuilder_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.menuADBHelp,
            this.toolStripSeparator1,
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            resources.ApplyResources(this.menuHelp, "menuHelp");
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Checked = true;
            this.toolStripMenuItem5.CheckOnClick = true;
            this.toolStripMenuItem5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            this.toolStripMenuItem5.CheckedChanged += new System.EventHandler(this.toolStripMenuItem5_CheckedChanged);
            // 
            // menuADBHelp
            // 
            this.menuADBHelp.Name = "menuADBHelp";
            resources.ApplyResources(this.menuADBHelp, "menuADBHelp");
            this.menuADBHelp.Click += new System.EventHandler(this.menuADBHelp_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            resources.ApplyResources(this.menuAbout, "menuAbout");
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.groupBox1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button _btnBuild;
        private System.Windows.Forms.TextBox _tbTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _cbBuilderType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ImageList imageList2;
		private System.Windows.Forms.Button _btnSelect;
		private System.Windows.Forms.Button _btnAddAssambly;
        private System.Windows.Forms.Button _btnDeleteAssembly;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuLanguage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem menuADBHelp;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Button _btnAddXML;
		private System.Windows.Forms.Button _btnDeleteXMLFile;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.ToolStripMenuItem menuGenerateSolution;
		private System.Windows.Forms.ToolStripMenuItem menuSolution;
		private System.Windows.Forms.ToolStripMenuItem menuExtendTag;
		private System.Windows.Forms.ToolStripMenuItem menuCustomStyle;
		private System.Windows.Forms.ToolStripMenuItem menuCustomBuilder;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuExtendSection;
		private XMLListBox _lbXMLList;
		private System.Windows.Forms.CheckBox _cbShowMembers;
		private AssemblyView assemblyView;
    }
}

