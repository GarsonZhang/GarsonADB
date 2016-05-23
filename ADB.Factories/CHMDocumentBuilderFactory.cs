using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ADB.Factories;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace ADB.Factories
{
    using Properties;
    using ContentTree = ContentTreeItem;

    /// <summary>
    /// 表示一个生成CHM文档的文档生成器的基类
    /// </summary>
    /// <remarks>
    /// CHMDocumentBuilderFactory是一个抽象类，其责职是将CreateNamespacePage（生成命名空间文档页面），CreateTypePage（生成类型如类，接口等的页面）和CreateMemberPage（生成类型成员如方法，属性等的页面）生成的页面编译为CHM文档，<span style="color: #ff0000">因此，当你想编写一个具有个性化风格样式（比如MSDN2008样式）的文档时，只需继承此类并重写CreateNamespacePage，CreateTypePage和CreateMemberPage三个方法即可，而不需要关心编译CHM的细节。</span>
    /// </remarks>
    public abstract class CHMDocumentBuilderFactory : ADB.Factories.DocumentBuilder
    {
        #region HTML模板

        static TextTemplate tempPage = new TextTemplate(Templates.Temp_Page);
        static TextTemplate tempSection = new TextTemplate(Templates.Temp_Section);
        static TextTemplate tempTypeTable = new TextTemplate(Templates.Temp_TypeTable);
        static TextTemplate tempTypeTable_Row = new TextTemplate(Templates.Temp_TypeTable_Row);
        static TextTemplate tempExceptionTable = new TextTemplate(Templates.Temp_ExceptionTable);
        static TextTemplate tempExample_Description = new TextTemplate(Templates.Temp_Example_Description);
        static TextTemplate tempExample_Code = new TextTemplate(Templates.Temp_Example_Code);
        static TextTemplate tempExceptionTable_Row = new TextTemplate(Templates.Temp_ExceptionTable_Row);
        static TextTemplate tempDeclaration = new TextTemplate(Templates.Temp_Declaration);
        static TextTemplate tempParamsBlock = new TextTemplate(Templates.Temp_ParamsBlock);
        static TextTemplate tempParam = new TextTemplate(Templates.Temp_Param);
        static TextTemplate tempRemarks = new TextTemplate(Templates.Temp_Remarks);
        static TextTemplate tempImg = new TextTemplate(Templates.Temp_Img);
        static TextTemplate tempHHC = new TextTemplate(Templates.HHC);
        static TextTemplate tempHHP = new TextTemplate(Templates.HHP);
        static TextTemplate tempHHP_NoIndex = new TextTemplate(Templates.HHP_NoIndex);
        static TextTemplate tempUL = new TextTemplate(Templates.UL);
        static TextTemplate tempLI = new TextTemplate(Templates.LI);
        static TextTemplate tempLI_Link = new TextTemplate(Templates.LI_Link);

        #endregion

        BuildProgress _buildProgress = null;

        Hashtable _memberFileName = new Hashtable();

		Hashtable _namespaceFileName = new Hashtable();

		Encoding _targetEncoding = Encoding.GetEncoding(Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage);

		/// <summary>
		/// 
		/// </summary>
		public Encoding TargetEncoding
		{
			get { return CHM_OptionDialog.Encoding; }
		}

        int _total = 0;

        // 目录树的根项
        ContentTree _root = new ContentTree("root", "");

        readonly static string AssemblyBasePath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

        private readonly string _defaultHHCPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\HTML Help Workshop\\hhc.exe";

        /// <summary>
        /// HHC路径
        /// </summary>
        public string HHCPath
        {
            get
            {
                return string.IsNullOrEmpty(Settings.Default.HHCPath) ? _defaultHHCPath : Settings.Default.HHCPath;
            }
            set
            {
                Settings.Default.HHCPath = value;
                Settings.Default.Save();
            }
        }


        bool _isCanceled = false, _isFinish = false;
        Process _hhcProcess = null;
        AutoResetEvent _exitEvent = null;

        string _target = null;
        string _title = null;

        readonly static Hashtable NameToTypeDictionary = new Hashtable();
        readonly static Hashtable NameToTitleDictionary = new Hashtable();

        static CHMDocumentBuilderFactory()
        {
            NameToTitleDictionary.Add("Constructor", Resources.strConstructor);
            NameToTitleDictionary.Add("PublicMethod", Resources.strPublicMethod);
            NameToTitleDictionary.Add("PrivateMethod", Resources.strPrivateMethod);
            NameToTitleDictionary.Add("ProtectedMethod", Resources.strProtectedMethod);
            NameToTitleDictionary.Add("PublicProperty", Resources.strPublicProperty);
            NameToTitleDictionary.Add("PrivateProperty", Resources.strPrivateProperty);
            NameToTitleDictionary.Add("ProtectedProperty", Resources.strProtectedProperty);
            NameToTitleDictionary.Add("PublicField", Resources.strPublicField);
            NameToTitleDictionary.Add("PrivateField", Resources.strPrivateField);
            NameToTitleDictionary.Add("ProtectedField", Resources.strProtectedField);
            NameToTitleDictionary.Add("Event", Resources.strEvent);
            NameToTitleDictionary.Add("Class", Resources.strClass);
            NameToTitleDictionary.Add("Interface", Resources.strInterface);
            NameToTitleDictionary.Add("Delegate", Resources.strDelegate);
            NameToTitleDictionary.Add("Enumeration", Resources.strEnumeration);
            NameToTitleDictionary.Add("Structure", Resources.strStructure);

            NameToTypeDictionary.Add("Constructor", Resources.strConstructor);
            NameToTypeDictionary.Add("PublicMethod", Resources.strMethod);
            NameToTypeDictionary.Add("PrivateMethod", Resources.strMethod);
            NameToTypeDictionary.Add("ProtectedMethod", Resources.strMethod);
            NameToTypeDictionary.Add("PublicProperty", Resources.strProperty);
            NameToTypeDictionary.Add("PrivateProperty", Resources.strProperty);
            NameToTypeDictionary.Add("ProtectedProperty", Resources.strProperty);
            NameToTypeDictionary.Add("PublicField", Resources.strField);
            NameToTypeDictionary.Add("PrivateField", Resources.strField);
            NameToTypeDictionary.Add("ProtectedField", Resources.strField);
            NameToTypeDictionary.Add("Event", Resources.strEvent);
            NameToTypeDictionary.Add("Class", Resources.strClass);
            NameToTypeDictionary.Add("Interface", Resources.strInterface);
            NameToTypeDictionary.Add("Delegate", Resources.strDelegate);
            NameToTypeDictionary.Add("Enumeration", Resources.strEnumeration);
            NameToTypeDictionary.Add("Structure", Resources.strStructure);
        }

        /// <summary>
        /// 初始化CHMDocumentBuilderFactory实例
        /// </summary>
        /// <param name="data"></param>
        /// <param name="interact"></param>
        public CHMDocumentBuilderFactory(IGetData data, IInteract interact)
            : base(data, interact)
        {
        }

        /// <summary>
        /// 目标文件路径
        /// </summary>
        public string Target
        {
            get { return _target; }
        }

        private string _targetDirectory = "";

        /// <summary>
        /// 文档标题
        /// </summary>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// 是否已经取消
        /// </summary>
        public bool IsCanceled
        {
            get { return _isCanceled; }
        }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool IsFinish
        {
            get { return _isFinish; }
        }

        /// <summary>
        /// 取消生成过程
        /// </summary>
        public void Cancel()
        {
            _isCanceled = true;
            if (_hhcProcess != null && !_hhcProcess.HasExited)
            {
                _hhcProcess.Kill();
            }
        }

        /// <summary>
        /// 等待直到完成或取消
        /// </summary>
        public void WaitForExit()
        {
            _exitEvent.WaitOne();
        }

        /// <summary>
        /// 生成文档
        /// </summary>
        /// <param name="title">文档标题</param>
        /// <param name="target">文档文件路径</param>
        public override void Build(string title, string target)
        {
            #region 获取HHC的路径
            if (!File.Exists(HHCPath))
            {
                string hhcPath;
                do
                {
                    hhcPath = (string)Interact.Invoke(new GetHHCPathDelegate(this.GetHHCPathMehtod));
                } while (!string.IsNullOrEmpty(hhcPath) && !File.Exists(hhcPath));
                if (string.IsNullOrEmpty(hhcPath))
                    return;
                else
                    HHCPath = hhcPath;
            }
            #endregion

            if (!IsFinish)
            {
                #region 初始化与Build方法相关的变量
                FileInfo targetFileInfo = new FileInfo(target);
                if (string.Compare(targetFileInfo.Extension, ".chm", true) != 0) target += ".chm";
                _title = title;
                _targetDirectory = targetFileInfo.DirectoryName;
                _exitEvent = new AutoResetEvent(false);
                _isCanceled = false;
                _isFinish = false;
                _hhcProcess = null;
                _target = target;
                #endregion

                Interact.Invoke(new BeforeBuildDelegate(BeforeBuild));
                try
                {
                    CreateContentTree();
                    CreatePages();
                    if (!IsCanceled) CreateCHM(CHM_OptionDialog.BuildMode);
                }
                finally
                {
                    _isFinish = true;
                    _hhcProcess = null;
					if (IsCanceled)
						_buildProgress.InvokeNotify(NotifyType.Canceled, null);
					else
					{
						if (CHM_OptionDialog.BuildMode == BuildMode.HTMLAndCHM)
							_buildProgress.InvokeNotify(NotifyType.Finished, target);
						else
							_buildProgress.InvokeNotify(NotifyType.FinishedNoCHM, null);
					}
                    //Interact.Invoke(new AfterBuildDelegate(AfterBuild), IsCanceled ? null : target);
                    _exitEvent.Set();
                }
            }
            else
            {
                throw new Exception(Resources.String1);
            }
        }

        private delegate string GetHHCPathDelegate();

        private string GetHHCPathMehtod()
        {
            GetHHCPathDialog pathDialog = new GetHHCPathDialog();
            if (pathDialog.ShowDialog(Interact.MainForm) == System.Windows.Forms.DialogResult.OK)
                return pathDialog.HHCPath;
            else
                return null;
        }

        private void CreateCHM(BuildMode buildMode)
        {
            if (_root.Count > 0)
            {
                _outputCache.Remove(0, _outputCache.Length);
                StringBuilder links = new StringBuilder();
                StringBuilder files = new StringBuilder();
                foreach (ContentTreeItem nsItem in _root.GetSubItemsCollection())
                {
                    if (nsItem.Name != null)
                    {
                        files.Append("\r\n");
                        files.Append(RelativeHtmlFilePath + "\\" + nsItem.FileName);
                        links.Append(tempLI_Link.Render(nsItem.Name, RelativeHtmlFilePath + "\\" + nsItem.FileName, "1"));
                    }
                    foreach (ContentTreeItem nsSubItem in nsItem.GetSubItemsCollection())
                    {
                        StringBuilder nsSubLinks = new StringBuilder();
                        foreach (ContentTreeItem typeItem in nsSubItem.GetSubItemsCollection())
                        {
                            files.Append("\r\n");
                            files.Append(RelativeHtmlFilePath + "\\" + typeItem.FileName);
                            nsSubLinks.Append(tempLI_Link.Render(typeItem.Name + " " + NameToTypeDictionary[nsSubItem.Name], RelativeHtmlFilePath + "\\" + typeItem.FileName, "1"));
                            StringBuilder typeSubItems = new StringBuilder();
                            foreach (ContentTreeItem subTypeItem in typeItem.GetSubItemsCollection())
                            {
                                if (subTypeItem.GetSubItemsCollection().Count > 0)
                                {
                                    typeSubItems.Append(tempLI_Link.Render(NameToTitleDictionary[subTypeItem.Name], RelativeHtmlFilePath + "\\" + subTypeItem.FileName, "1"));
                                    StringBuilder memLinks = new StringBuilder();
                                    foreach (ContentTreeItem memItem in subTypeItem.GetSubItemsCollection())
                                    {
                                        memLinks.Append(tempLI_Link.Render(memItem.Name + " " + NameToTypeDictionary[subTypeItem.Name], RelativeHtmlFilePath + "\\" + memItem.FileName, "11"));
                                    }
                                    if (memLinks.Length > 0)
                                    {
                                        typeSubItems.Append("<UL>");
                                        typeSubItems.Append(memLinks);
                                        typeSubItems.Append("\r\n</UL>");
                                    }
                                }
                            }
                            if (typeSubItems.Length > 0)
                            {
                                nsSubLinks.Append("<UL>");
                                nsSubLinks.Append(typeSubItems);
                                nsSubLinks.Append("\r\n</UL>");
                            }
                        }
                        if (nsSubLinks.Length > 0)
                        {
                            if (nsItem.Name != null)
                            {
                                links.Append("<UL>");
                                links.Append(nsSubLinks);
                                links.Append("\r\n</UL>");
                            }
                            else
                                links.Append(nsSubLinks);
                        }
                    }
                }
                if (string.Compare(new FileInfo(_target).Extension, ".chm") != 0) _target += ".chm";
                tempHHP_NoIndex.SaveAs(_targetDirectory + "\\Document.hhp", TargetEncoding, _target, "Document.hhc", _title, "Document.hhc", files);
				tempHHC.SaveAs(_targetDirectory + "\\Document.hhc", TargetEncoding, links);
                if (CHM_OptionDialog.BuildMode == BuildMode.HTMLAndCHM)
                {
                    _hhcProcess = new Process();
                    _hhcProcess.StartInfo.UseShellExecute = false;
                    _hhcProcess.StartInfo.CreateNoWindow = true;
                    _hhcProcess.StartInfo.RedirectStandardOutput = true;
                    _hhcProcess.StartInfo.RedirectStandardInput = true;
                    _hhcProcess.StartInfo.FileName = _defaultHHCPath;
                    _hhcProcess.StartInfo.Arguments = string.Format("\"{0}\\Document.hhp\"", _targetDirectory);
                    _hhcProcess.OutputDataReceived += new DataReceivedEventHandler(this.hhc_OutputDataReceived);
                    _hhcProcess.Start();
                    try
                    {
                        _hhcProcess.BeginOutputReadLine();
                    }
                    finally
                    {
                        _hhcProcess.WaitForExit();
                    }

                    if (_outputCache.Length > 0)
                        _buildProgress.BeginInvokeNotify(NotifyType.OutputDataReceived, _outputCache.ToString());
                }
            }
        }

        StringBuilder _outputCache = new StringBuilder(_cacheLength);
        static readonly int _cacheLength = 4096;

        private void hhc_OutputDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine.Data != null)
            {
                string data = outLine.Data + "\r\n";
                if (_outputCache.Length + data.Length > _cacheLength)
                {
                    _buildProgress.BeginInvokeNotify(NotifyType.OutputDataReceived, _outputCache.ToString());
                    _outputCache.Remove(0, _outputCache.Length);
                }
                else
                {
                    _outputCache.Append(data);
                }
            }
        }

        /// <summary>
        /// 获取文件名筛选器字符串，该字符串决定对话框的“另存为文件类型”框中出现的选择内容。
        /// </summary>
        public override string Filter
        {
            get { return "CHM|*.chm"; }
        }

        /// <summary>
        /// 创建目录树，并确定命名空间，类型，成员对应的页面的文件名
        /// </summary>
        /// <returns></returns>
        private void CreateContentTree()
        {
            int pageCount = 0;
			foreach (Type type in DataProvider.GetSelectedTypes())
			{
				ContentTreeItem nsItem = null, typeParentItem = null;
				string typeParentName = "";
				nsItem = _root.Find(type.Namespace);
				if (nsItem == null)
				{
					string nsPageFileName = (++pageCount).ToString("000000") + Resources.Extension;
					_root.Add(nsItem = new ContentTreeItem(type.Namespace, nsPageFileName));
					if (!string.IsNullOrEmpty(type.Namespace)) _namespaceFileName.Add(type.Namespace, nsPageFileName);
				}
				if (type.IsClass)
					typeParentName = "Class";
				else if (type.IsEnum)
					typeParentName = "Enumeration";
				else if (type.IsValueType)
					typeParentName = "Structure";
				else if (type.IsInterface)
					typeParentName = "Interface";
				else
					typeParentName = "Delegate";
				typeParentItem = nsItem.Find(typeParentName);
				if (typeParentItem == null) nsItem.Add(typeParentItem = new ContentTreeItem(typeParentName, ""));
                ContentTreeItem typeItem = new ContentTreeItem(DocumentBuilderUtility.GetTypeDefinition(type, false), (++pageCount).ToString("000000") + Resources.Extension, type);
				_memberFileName.Add(DocumentBuilderUtility.GetMemberID(type), typeItem.FileName);
				typeParentItem.Add(typeItem);
				foreach (MemberInfo memberInfo in DataProvider.GetTypeSelectedMembers(type))
				{
					string parentItemName = "";
					#region 添加成员
					switch (memberInfo.MemberType)
					{
					case MemberTypes.Constructor:
						{
							parentItemName = "Constructor";
							break;
						}
					case MemberTypes.Method:
						{
							MethodInfo method = memberInfo as MethodInfo;
							if (!method.IsSpecialName)
							{
								if (method.IsPublic)
									parentItemName = "PublicMethod";
								else if (method.IsPrivate)
									parentItemName = "PrivateMethod";
								else
									parentItemName = "ProtectedMethod";
							}
							break;
						}
					case MemberTypes.Field:
						{
							FieldInfo field = memberInfo as FieldInfo;
							if (field.IsPublic)
								parentItemName = "PublicField";
							else if (field.IsPrivate)
								parentItemName = "PrivateField";
							else
								parentItemName = "ProtectedField";
							break;
						}
					case MemberTypes.Property:
						{
							PropertyInfo property = memberInfo as PropertyInfo;
							MethodInfo pm = property.GetGetMethod(true);
							if (pm == null) pm = property.GetSetMethod(true);
							if (pm != null)
							{
								if (pm.IsPublic)
									parentItemName = "PublicProperty";
								else if (pm.IsPrivate)
									parentItemName = "PrivateProperty";
								else
									parentItemName = "ProtectedProperty";
							}
							break;
						}
					case MemberTypes.Event:
						{
							parentItemName = "Event";
							break;
						}
					}
					if (!string.IsNullOrEmpty(parentItemName))
					{
						ContentTreeItem parentItem = typeItem.Find(parentItemName);
						if (parentItem == null)
						{
							parentItem = new ContentTreeItem(parentItemName, typeItem.FileName + "#" + parentItemName);
							typeItem.Add(parentItem);
						}
						if (memberInfo.MemberType == MemberTypes.Constructor)
						{
							string fileName = (++pageCount).ToString("000000") + Resources.Extension;
                            parentItem.Add(new ContentTreeItem(DocumentBuilderUtility.GetTypeDefinition(memberInfo.ReflectedType, false), fileName, memberInfo));
						}
						else
						{
							string fileName = _memberFileName[DocumentBuilderUtility.GetMemberID(memberInfo)] as string;
							if (string.IsNullOrEmpty(fileName))
							{
								fileName = (++pageCount).ToString("000000") + Resources.Extension;
								_memberFileName.Add(DocumentBuilderUtility.GetMemberID(memberInfo), fileName);
							}
							parentItem.Add(new ContentTreeItem(memberInfo.Name, fileName, memberInfo));
						}
					}
					#endregion
				}
			}
            _root.Sort();
            _total = pageCount;
        }

        private bool CreatePages()
        {
            if (_total > 0)
            {
                Hashtable htmlFiles = new Hashtable();
                int pageCmp = 0;
                foreach (ContentTreeItem ns in _root.GetSubItemsCollection())
                {
                    if (!IsCanceled)
                    {
                        if (!string.IsNullOrEmpty(ns.Name))
                            CreateNamespacePage(ns, ns);
                        pageCmp++;
                    }
                    else
                        return false;
                    foreach (ContentTreeItem nsSubItem in ns.GetSubItemsCollection())
                        foreach (ContentTreeItem type in nsSubItem.GetSubItemsCollection())
                        {
                            if (!IsCanceled)
                            {
                                CreateTypePage(type, type);
                                pageCmp++;
                            }
                            else
                                return false;
                            foreach (ContentTreeItem subTypeItem in type.GetSubItemsCollection())
                                foreach (ContentTreeItem member in subTypeItem.GetSubItemsCollection())
                                {
                                    if (!IsCanceled)
                                    {
                                        if (!htmlFiles.ContainsKey(member.FileName))
                                        {
                                            CreateMemberPage(member);
                                            htmlFiles.Add(member.FileName, 0);
                                            pageCmp++;
                                        }
                                    }
                                    else
                                        return false;
                                }
                            _buildProgress.BeginInvokeNotify(NotifyType.ShowProgress, pageCmp * 100 / _total);
                        }
                }
            }
            return true;
        }

        delegate void BeforeBuildDelegate();

        private void BeforeBuild()
        {
            _buildProgress = new BuildProgress(this);
            _buildProgress.Show(Interact.MainForm);
        }

        delegate void AfterBuildDelegate(string target);

        private void AfterBuild(string target)
        {
            _buildProgress.Notify(NotifyType.Finished, target);
        }

        /// <summary>
        /// 获取类型或成员的页面文件的文件名
        /// </summary>
        /// <param name="id"></param>
        /// <returns>指定类型或成员的页面文件的文件名</returns>
        protected string GetFileName(string id)
        {
            return (string)_memberFileName[id];
        }

        /// <summary>
        /// 获取命名空间对应的页面文件的文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns>指定命名空间对应的页面文件的文件名</returns>
        protected string GetNamespaceFileName(string name)
        {
            return (string)_namespaceFileName[name];
        }

        /// <summary>
        /// 获取所有的命名空间的名称
        /// </summary>
        /// <returns>所有的命名空间的名称</returns>
        protected string[] GetAllNamespaces()
        {
            string[] nss = new string[_namespaceFileName.Count];
            int i = 0;
            foreach (DictionaryEntry ent in _namespaceFileName) nss[i++] = ent.Key as string;
            return nss;
        }

        /// <summary>
        /// 生成命名空间页面
        /// </summary>
        /// <param name="typeItems">命名空间中的所有类型</param>
        /// <param name="ns">命名空间包含信息（名称，页面文件名等）</param>
        protected abstract void CreateNamespacePage(IGetTypes typeItems, ContentTreeItem ns);

        /// <summary>
        /// 生成类型页面
        /// </summary>
        /// <param name="memberItems">该类型包含的所有成员</param>
        /// <param name="type">类型包含信息（名称，页面文件名等）</param>
        protected abstract void CreateTypePage(IGetMembers memberItems, ContentTreeItem type);

        /// <summary>
        /// 创建成员页面
        /// </summary>
        /// <param name="member">成员的信息</param>
        protected abstract void CreateMemberPage(ContentTreeItem member);

        /// <summary>
        /// 存放页面的目录路径（相对于CHM文件所在的目录）
        /// </summary>
        protected string RelativeHtmlFilePath
        {
            get
            {
                return "pages";
            }
		}
		CHM_OptionDialog _optionDialog = new CHM_OptionDialog();

		/// <summary>
		/// 
		/// </summary>
		public sealed override Form OptionDialog
		{
			get { return (Form)CHM_OptionDialog; }
		}

		public virtual CHM_OptionDialog CHM_OptionDialog
		{
			get { return _optionDialog; }
		}
    }

    /// <summary>
    /// 目录树中的项，用于在存储命名空间，类型，类型成员的信息
    /// </summary>
    public class ContentTreeItem : IGetTypes, IGetMembers
    {
        static readonly MemberItemInfoComparer Comparer = new MemberItemInfoComparer();
        string _name;
        string _fileName;
        List<ContentTreeItem> _subItems;
        MemberInfo _info;
        object _tag = null;
        ContentTreeItem _parent;

        /// <summary>
        /// 初始化ContentTreeItem实例
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="fileName">对应页面的文件名</param>
        public ContentTreeItem(string name, string fileName)
        {
            _name = name;
            _fileName = fileName;
            _subItems = new List<ContentTreeItem>();
            _info = null;
        }
        /// <summary>
        /// 初始化ContentTreeItem实例
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="fileName">对应页面的文件名</param>
        /// <param name="info">项对应的成员或类型的信息</param>
        public ContentTreeItem(string name, string fileName, MemberInfo info)
        {
            _name = name;
            _fileName = fileName;
            _subItems = new List<ContentTreeItem>();
            _info = info;
        }

        /// <summary>
        /// 项在目录树中的父项
        /// </summary>
        public ContentTreeItem Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// 与该项相关联的附加数据
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// 成员信息
        /// </summary>
        public MemberInfo Info
        {
            get { return _info; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 对应的页面文件的文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// 获取一个枚举子项的接口
        /// </summary>
        /// <returns></returns>
        public ICollection<ContentTreeItem> GetSubItemsCollection()
        {
            return _subItems;
        }

        /// <summary>
        /// 添加一个子项
        /// </summary>
        /// <param name="item"></param>
        public void Add(ContentTreeItem item)
        {
            this._subItems.Add(item);
            item._parent = this;
        }

        /// <summary>
        /// 对子项排序
        /// </summary>
        public void Sort()
        {
            _subItems.Sort(Comparer);
            foreach (ContentTreeItem item in _subItems) item.Sort();
        }

        /// <summary>
        /// 包含的子项的数目
        /// </summary>
        public int Count
        {
            get { return _subItems.Count; }
        }

        /// <summary>
        /// 将所有子项拷贝到一个数组
        /// </summary>
        /// <returns>包含所有子项的数组</returns>
        public ContentTreeItem[] GetAllSubItem()
        {
            return _subItems.ToArray();
        }

        /// <summary>
        /// 查找名称为name的子项
        /// </summary>
        /// <param name="name">要查找的子项的名称</param>
        /// <returns>名称为name的子项</returns>
        public ContentTreeItem Find(string name)
        {
            foreach (ContentTreeItem item in _subItems)
            {
                if (string.Compare(item.Name, name, true) == 0) return item;
            }
            return null;
        }

        #region 实现IGetTypes

        ContentTreeItem[] IGetTypes.GetMembers(string category)
        {
            ContentTreeItem categoryItem = Find(category);
            if (categoryItem != null)
                return categoryItem.GetAllSubItem();
            else
                return null;
        }

        string[] IGetTypes.GetAllCategory()
        {
            List<string> categorys = new List<string>();
            foreach (ContentTreeItem item in GetSubItemsCollection())
            {
                categorys.Add(item.Name);
            }
            return categorys.ToArray();
        }

        #endregion

        #region 实现IGetMembers

        ContentTreeItem[] IGetMembers.GetMembers(string category)
        {
            ContentTreeItem categoryItem = Find(category);
            if (categoryItem != null)
                return categoryItem.GetAllSubItem();
            else
                return null;
        }

        string[] IGetMembers.GetAllCategory()
        {
            List<string> categorys = new List<string>();
            foreach (ContentTreeItem item in GetSubItemsCollection())
            {
                categorys.Add(item.Name);
            }
            return categorys.ToArray();
        }

        #endregion
    }

    internal class MemberItemInfoComparer : IComparer<ContentTreeItem>
    {
        public int Compare(ContentTreeItem item1, ContentTreeItem item2)
        {
            return string.Compare(item1.Name, item2.Name, true);
        }
    }

    /// <summary>
    /// 提供一个获取按类别获取命名空间包含的类型的接口
    /// </summary>
    public interface IGetTypes
    {
        /// <summary>
        /// 获取某一类别的成员，如GetMembers("PublicMethod")获取所有公共方法，对应关系如下:
        /// Class       类
        /// Structure   结构
        /// Enumeration 枚举
        /// Delegate    委托
        /// Interface   接口
        /// </summary>
        /// <param name="category">类别名</param>
        /// <returns>属于指定类别的所有成员</returns>
        ContentTreeItem[] GetMembers(string category);

        /// <summary>
        /// 获取包含的所有类别
        /// </summary>
        /// <returns></returns>
        string[] GetAllCategory();
    }

    /// <summary>
    /// 提供一个获取按类别获取类型成员的接口
    /// </summary>
    public interface IGetMembers
    {
        /// <summary>
        /// 获取某一类别的成员，如GetMembers("PublicMethod")获取所有公共方法，对应关系如下:
        /// Constructor       构造方法
        /// PublicMethod      公共方法
        /// ProtectedMethod   受保护方法
        /// PrivateMethod     私有方法
        /// PublicProperty    公共属性
        /// ProtectedProperty 受保护属性
        /// PrivateProperty   私有方法
        /// PublicField       公共字段
        /// ProtectedField    受保护字段
        /// PrivateField      私有字段
        /// Event             事件
        /// </summary>
        /// <param name="category">类别名</param>
        /// <returns>属于指定类别的所有成员</returns>
        ContentTreeItem[] GetMembers(string category);

        /// <summary>
        /// 获取包含的所有类别
        /// </summary>
        /// <returns>包含的所有类别</returns>
        string[] GetAllCategory();
    }
}
