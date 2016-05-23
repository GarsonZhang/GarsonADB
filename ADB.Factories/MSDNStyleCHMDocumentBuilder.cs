using System;
using System.Collections.Generic;
using System.Text;
using ADB.Factories;
using System.Collections;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;
using System.Xml;
using System.Threading;
using System.Diagnostics;

namespace ADB.Factories
{
    using Properties;

    /// <summary>
    /// 表示一个生成MSDN2005样式的CHM文档的文档生成器的类
    /// </summary>
    /// <remarks>
    /// MSDNStyleCHMDocumentBuilder继承于CHMDocumentBuilderFactory，并重写了CreateNamespacePage，CreateTypePage和CreateMemberPage以生成MSDN2005样式的文档页面，MSDNStyleCHMDocumentBuilder的GetTag方法为虚函数，<span style="color: #ff0000">如果要扩展注释标志，只需继承该类，并重写GetTag方法即可。</span>
    /// </remarks>
    /// <example>
    /// 以下例子通过继承MSDNStyleCHMDocumentBuilder并重写GetTag扩展&lt;image&gt;标志
    /// 
    /// <file src="files\DemoCustomBuilder.rar">工程文件</file>
    /// 
    /// <code language="C#" src="codes\DemoCustomBuilder.cs"/>
    /// </example>
    public class MSDNStyleCHMDocumentBuilder : CHMDocumentBuilderFactory
    {
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
        static TextTemplate tempEditIndex = new TextTemplate(Templates.EditIndex);
        static TextTemplate tempEditIndex_NS = new TextTemplate(Templates.EditIndex_NS);

        readonly static Hashtable NameToTitleDictionary = new Hashtable();
        readonly static Hashtable NameToTypeDictionary = new Hashtable();

        readonly static string _assemblyBasePath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

        /// <summary>
        /// 程序集所在的路径
        /// </summary>
        public static string AssemblyBasePath
        {
            get { return MSDNStyleCHMDocumentBuilder._assemblyBasePath; }
        }

        private string _targetDirectory;

        readonly string _htmlTempDirectory = _assemblyBasePath + "\\Template";

        private string _htmlFileDirectory = "";

        /// <summary>
        /// 获取存放页面模板的文件夹的路径
        /// </summary>
        public string HtmlTempDirectory
        {
            get { return _htmlTempDirectory; }
        }

        static MSDNStyleCHMDocumentBuilder()
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

            _typePageSections = new PageSection[13];
            _typePageSections[0] = new PageSection(Resources.String42, PageSectionType.Remarks, null);
            _typePageSections[1] = new PageSection(Resources.String43, PageSectionType.Example, null);
            _typePageSections[2] = new PageSection(Resources.strConstructor, PageSectionType.Constructor, null);
            _typePageSections[3] = new PageSection(Resources.strPublicMethod, PageSectionType.PublicMethod, null);
            _typePageSections[4] = new PageSection(Resources.strProtectedMethod, PageSectionType.ProtectedMethod, null);
            _typePageSections[5] = new PageSection(Resources.strPrivateMethod, PageSectionType.PrivateMethod, null);
            _typePageSections[6] = new PageSection(Resources.strPublicProperty, PageSectionType.PublicProperty, null);
            _typePageSections[7] = new PageSection(Resources.strProtectedProperty, PageSectionType.ProtectedProperty, null);
            _typePageSections[8] = new PageSection(Resources.strPrivateProperty, PageSectionType.PrivateProperty, null);
            _typePageSections[9] = new PageSection(Resources.strPublicField, PageSectionType.PublicField, null);
            _typePageSections[10] = new PageSection(Resources.strProtectedField, PageSectionType.ProtectedField, null);
            _typePageSections[11] = new PageSection(Resources.strPrivateField, PageSectionType.PrivateField, null);
            _typePageSections[12] = new PageSection(Resources.strEvent, PageSectionType.Event, null);

            _memberPageSections = new PageSection[4];
            _memberPageSections[0] = new PageSection(Resources.String42, PageSectionType.Remarks, null);
            _memberPageSections[1] = new PageSection(Resources.String44, PageSectionType.Syntax, null);
            _memberPageSections[2] = new PageSection(Resources.String45, PageSectionType.Exception, null);
            _memberPageSections[3] = new PageSection(Resources.String43, PageSectionType.Example, null);

        }

        /// <summary>
        /// 初始化MSDNStyleCHMDocumentBuilder
        /// </summary>
        /// <param name="data">用于获取成员及其XML注释的接口</param>
        /// <param name="interact">用于与主界面交互的接口</param>
        public MSDNStyleCHMDocumentBuilder(IGetData data, IInteract interact)
            : base(data, interact)
        {
        }

        /// <summary>
        /// 保存页面文件的文件夹的路径
        /// </summary>
        protected string HtmlFileDirectory
        {
            get { return _htmlFileDirectory; }
        }

        /// <summary>
        /// 生成文档
        /// </summary>
        /// <param name="title">文档的标题</param>
        /// <param name="target">文档的路径</param>
        public override void Build(string title, string target)
        {
            FileInfo targetInfo = new FileInfo(target);
            _targetDirectory = targetInfo.DirectoryName;
            _htmlFileDirectory = targetInfo.DirectoryName + "\\" + RelativeHtmlFilePath;
            if (!Directory.Exists(HtmlFileDirectory)) FileSystem.CreateDirectory(HtmlFileDirectory);
            FileSystem.CopyDirectory(_htmlTempDirectory + "\\files", HtmlFileDirectory + "\\files", true);
            base.Build(title, target);
        }

        private string CreateTableSection(string id, string title, string allRows)
        {
            Hashtable sectionValues = new Hashtable();
            sectionValues["ID"] = id;
            sectionValues["Title"] = title;

            Hashtable tableValues = new Hashtable();
            tableValues["Column1"] = title;
            tableValues["AllRows"] = allRows;
            tableValues["Desc"] = Resources.strDesc;
            sectionValues["Content"] = tempTypeTable.Render(tableValues);

            return tempSection.Render(sectionValues);
        }

        private bool IsStatic(MemberInfo info)
        {
            switch (info.MemberType)
            {
                case MemberTypes.Method:
                    return (info as MethodInfo).IsStatic;
                case MemberTypes.Field:
                    return (info as FieldInfo).IsStatic;
                case MemberTypes.Constructor:
                    return (info as ConstructorInfo).IsStatic;
                case MemberTypes.Property:
                    {
                        MethodInfo pm = (info as PropertyInfo).GetGetMethod(true);
                        if (pm == null) pm = (info as PropertyInfo).GetSetMethod(true);
                        if (pm != null) return pm.IsStatic; else return false;
                    }
                default:
                    return false;
            }
        }

        private string CreateMemberDeclaration(MemberInfo info, MemberXmlElement memberData)
        {
            StringBuilder declaration = new StringBuilder();
            switch (info.MemberType)
            {
                case MemberTypes.Method:
                    {
                        MethodInfo method = info as MethodInfo;
                        if (method.IsStatic) declaration.Append("static ");
                        if (method.IsVirtual) declaration.Append("virtual ");
                        declaration.Append(DocumentBuilderUtility.GetTypeDefinition(method.ReturnType) + " " + method.Name);

                        //泛型方法处理
                        //if (method.IsGenericMethod == true)
                        //if (method.IsGenericMethod == true)
                        //{
                        if (memberData != null && memberData.Data != null)
                        {
                            XmlNodeList lst = memberData.Data.GetElementsByTagName("typeparam");
                            if (lst.Count > 0)
                            {
                                declaration.Append("<");
                                declaration.Append((lst[0] as XmlElement).GetAttribute("name"));
                                for (int i = 1; i < lst.Count; i++)
                                {
                                    declaration.Append("," + (lst[i] as XmlElement).GetAttribute("name"));
                                }
                                declaration.Append(">");
                            }
                        }
                        //}

                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length > 0)
                        {
                            string strParams = "";
                            foreach (ParameterInfo param in parameters)
                            {
                                if (string.IsNullOrEmpty(strParams))
                                    strParams = "    " + DocumentBuilderUtility.GetTypeDefinition(param.ParameterType) + " " + param.Name;
                                else
                                    strParams += ",\r\n    " + DocumentBuilderUtility.GetTypeDefinition(param.ParameterType) + " " + param.Name;
                            }
                            declaration.Append("(\r\n" + strParams + "\r\n);");
                        }
                        else
                            declaration.Append("();");
                        break;
                    }
                case MemberTypes.Property:
                    {
                        PropertyInfo property = info as PropertyInfo;
                        MethodInfo method = property.GetGetMethod(true);
                        if (method == null) method = property.GetSetMethod(true);
                        if (method.IsStatic) declaration.Append("static ");
                        if (method.IsVirtual) declaration.Append("virtual ");
                        declaration.Append(DocumentBuilderUtility.GetTypeDefinition(method.ReturnType) + " " + property.Name);
                        if (method != null)
                        {
                            ParameterInfo[] parameters = method.GetParameters();
                            if (parameters.Length > 0)
                            {
                                string strParams = "";
                                foreach (ParameterInfo param in parameters)
                                {
                                    if (string.IsNullOrEmpty(strParams))
                                        strParams = "    " + DocumentBuilderUtility.GetTypeDefinition(param.ParameterType) + " " + param.Name;
                                    else
                                        strParams += ",\r\n    " + DocumentBuilderUtility.GetTypeDefinition(param.ParameterType) + " " + param.Name;
                                }
                                declaration.Append("[\r\n" + strParams + "\r\n]");
                            }
                            declaration.ToString();
                        }
                        declaration.Append("{");
                        if (property.GetGetMethod(true) != null) declaration.Append(" get;");
                        if (property.GetSetMethod(true) != null) declaration.Append(" set;");
                        declaration.Append(" }");
                        break;
                    }
                case MemberTypes.Constructor:
                    {
                        ConstructorInfo ctor = info as ConstructorInfo;
                        declaration.Append(DocumentBuilderUtility.GetTypeDefinition(ctor.DeclaringType, false));
                        ParameterInfo[] parameters = ctor.GetParameters();
                        if (parameters.Length > 0)
                        {
                            string strParams = "";
                            foreach (ParameterInfo param in parameters)
                            {
                                if (string.IsNullOrEmpty(strParams))
                                    strParams = "    " + DocumentBuilderUtility.GetTypeDefinition(param.ParameterType) + " " + param.Name;
                                else
                                    strParams += ",\r\n    " + DocumentBuilderUtility.GetTypeDefinition(param.ParameterType) + " " + param.Name;
                            }
                            declaration.Append("(\r\n" + strParams + "\r\n);");
                        }
                        else
                            declaration.Append("();");
                        break;
                    }
                case MemberTypes.Field:
                    {
                        FieldInfo field = info as FieldInfo;
                        if (field.IsStatic) declaration.Append("static ");
                        declaration.Append(field.ToString());
                        break;
                    }
                case MemberTypes.Event:
                    {
                        EventInfo ent = info as EventInfo;
                        declaration.Append(ent.ToString());
                        break;
                    }
            }
            return declaration.ToString();
        }

        /// <summary>
        /// 生成命名空间页面
        /// </summary>
        /// <param name="typeItems">命名空间中的所有类型</param>
        /// <param name="ns">命名空间包含信息（名称，页面文件名等）</param>
        protected override void CreateNamespacePage(IGetTypes typeItems, ContentTreeItem ns)
        {
            HtmlText content = new HtmlText();
            foreach (string category in typeItems.GetAllCategory())
            {
                string title = NameToTitleDictionary[category] as string;
                HtmlText secContent = new HtmlText();
                foreach (ContentTreeItem typeItem in typeItems.GetMembers(category))
                {
                    Type type = typeItem.Info as Type;
                    MemberXmlElement data = DataProvider.GetMemberXmlNode(type);
                    string summary;
                    summary = GetChildNodeInnerText(data, "summary");
                    if (string.IsNullOrEmpty(summary)) summary = "&nbsp;";
                    string strRow = tempTypeTable_Row.Render(
                        tempImg.Render(NameToTitleDictionary[category], category + ".png"),
                        typeItem.FileName,
                        HtmlText.HtmlFormat(typeItem.Name),
                        summary
                    );
                    secContent.AppendPre(strRow);
                }
                content.AppendPre(CreateTableSection(title, title, secContent.ToString()));
            }

            Hashtable values = new Hashtable();
            values["CollapseAll"] = Resources.strCollapseAll;
            values["ExpandAll"] = Resources.strExpandAll;
            values["PIC"] = Resources.strPic;
            values["Title"] = HtmlText.HtmlFormat(ns.Name);
            values["Summary"] = "";
            values["Content"] = content.ToString();
            values["Encoding"] = TargetEncoding.HeaderName;
            tempPage.SaveAs(HtmlFileDirectory + "\\" + ns.FileName, TargetEncoding, values);
        }

        /// <summary>
        /// 生成类型页面
        /// </summary>
        /// <param name="memberItems">该类型包含的所有成员</param>
        /// <param name="typeItem">类型包含信息（名称，页面文件名等）</param>
        protected override void CreateTypePage(IGetMembers memberItems, ContentTreeItem typeItem)
        {
            HtmlText pageContent = new HtmlText();
            Type type = typeItem.Info as Type;

            MemberXmlElement typeData = DataProvider.GetMemberXmlNode(type);
            int secID = 0;
            foreach (PageSection section in TypePageSections)
            {
                string secText = null;
                switch (section.SectionType)
                {
                    case PageSectionType.Constructor:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "Constructor");
                        break;
                    case PageSectionType.PublicMethod:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "PublicMethod");
                        break;
                    case PageSectionType.ProtectedMethod:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "ProtectedMethod");
                        break;
                    case PageSectionType.PrivateMethod:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "PrivateMethod");
                        break;
                    case PageSectionType.PublicProperty:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "PublicProperty");
                        break;
                    case PageSectionType.ProtectedProperty:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "ProtectedProperty");
                        break;
                    case PageSectionType.PrivateProperty:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "PrivateProperty");
                        break;
                    case PageSectionType.PublicField:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "PublicField");
                        break;
                    case PageSectionType.ProtectedField:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "ProtectedField");
                        break;
                    case PageSectionType.PrivateField:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "PrivateField");
                        break;
                    case PageSectionType.Event:
                        secText = GetTypeMemberCategorySection(memberItems, type, typeData, "Event");
                        break;
                    case PageSectionType.Remarks:
                        {
                            if (typeData != null)
                            {
                                HtmlText remarks = new HtmlText();
                                remarks.AppendPre(GetChildNodeInnerText(typeData, "remarks"));
                                if (remarks.Length > 0)
                                {
                                    Hashtable sectionValues = new Hashtable();
                                    sectionValues["ID"] = "remarks";
                                    sectionValues["Title"] = Resources.String42;
                                    sectionValues["Content"] = tempRemarks.Render(remarks);
                                    secText = tempSection.Render(sectionValues);
                                }
                            }
                            break;
                        }
                    case PageSectionType.Example:
                        {
                            if (typeData != null)
                            {
                                HtmlText example = new HtmlText();
                                example.AppendPre(GetChildNodeInnerText(typeData, "example"));
                                if (example.Length > 0)
                                {
                                    Hashtable sectionValues = new Hashtable();
                                    sectionValues["ID"] = "example";
                                    sectionValues["Title"] = Resources.String43;
                                    sectionValues["Content"] = tempRemarks.Render(example);
                                    secText = tempSection.Render(sectionValues);
                                }
                            }
                            break;
                        }
                    case PageSectionType.FromXML:
                        {
                            if (typeData != null)
                            {
                                string text = GetChildNodeInnerText(typeData, section.XmlSource);
                                if (!string.IsNullOrEmpty(text))
                                {
                                    Hashtable sectionValues = new Hashtable();
                                    sectionValues["ID"] = "SEC" + secID.ToString("00");
                                    sectionValues["Title"] = section.Name;
                                    sectionValues["Content"] = text;
                                    secText = tempSection.Render(sectionValues);
                                }
                            }
                            break;
                        }
                }
                if (!string.IsNullOrEmpty(secText)) pageContent.AppendPre(secText);
                secID++;
            }

            Hashtable values = new Hashtable();
            values["CollapseAll"] = Resources.strCollapseAll;
            values["ExpandAll"] = Resources.strExpandAll;
            values["PIC"] = Resources.strPic;
            values["Title"] = HtmlText.HtmlFormat(DocumentBuilderUtility.GetTypeDefinition(type, false)) + " " + NameToTypeDictionary[typeItem.Parent.Name];
            values["Summary"] = GetChildNodeInnerText(typeData, "summary");
            values["Content"] = pageContent;
            values["Encoding"] = TargetEncoding.HeaderName;
            tempPage.SaveAs(
                HtmlFileDirectory + "\\" + typeItem.FileName,
                TargetEncoding,
                values
                );
        }

        private string GetTypeMemberCategorySection(IGetMembers memberItems, Type type, MemberXmlElement typeData, string category)
        {
            if (memberItems.GetMembers(category) != null)
            {
                string secText = null;
                string sectionTitle = NameToTitleDictionary[category] as string;
                HtmlText sectionContent = new HtmlText();
                foreach (ContentTreeItem memberItem in memberItems.GetMembers(category))
                {
                    MemberXmlElement memberData = DataProvider.GetMemberXmlNode(memberItem.Info);
                    string summary = "";
                    summary = GetChildNodeInnerText(memberData, "summary");
                    if (memberItem.Info.DeclaringType != type)
                        summary += string.Format(Resources.String56, memberItem.Info.DeclaringType.Name);
                    if (string.IsNullOrEmpty(summary)) summary = "&nbsp;";
                    HtmlText imgs = new HtmlText();
                    imgs.AppendPre(tempImg.Render(NameToTitleDictionary[category], category + ".bmp"));
                    if (IsStatic(memberItem.Info))
                    {
                        imgs.AppendPre("&nbsp;");
                        imgs.AppendPre(tempImg.Render("static", "static.bmp"));
                    }

                    string mname = "";
                    mname += HtmlText.HtmlFormat(memberItem.Name);
                    MethodInfo method = memberItem.Info as MethodInfo;
                    //if (method != null)
                    if (method != null)
                    {
                        //泛型方法
                        //if (method.IsGenericMethod == true)
                        //{

                        if (memberData != null && memberData.Data != null)
                        {
                            XmlNodeList lst = memberData.Data.GetElementsByTagName("typeparam");

                            if (lst.Count > 0)
                            {
                                mname += HtmlText.HtmlFormat("<");
                                mname += (lst[0] as XmlElement).GetAttribute("name");
                                for (int i = 1; i < lst.Count; i++)
                                {
                                    mname += ("," + (lst[i] as XmlElement).GetAttribute("name"));
                                }
                                //泛型方法处理
                                //name = name.Replace("``" + elem.GetElementsByTagName("typeparam").Count, "");
                                mname += HtmlText.HtmlFormat(">");
                            }
                        }
                        //}

                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length > 0)
                        {
                            string strParams = "";
                            foreach (ParameterInfo param in parameters)
                            {
                                if (string.IsNullOrEmpty(strParams))
                                    strParams = "" + SimpleParamName(DocumentBuilderUtility.GetTypeDefinition(param.ParameterType)) + "";
                                else
                                    strParams += "," + SimpleParamName(DocumentBuilderUtility.GetTypeDefinition(param.ParameterType)) + "";
                            }
                            mname += ("(" + strParams + ")");
                        }
                        else
                            mname += ("()");
                    }


                    string strRow = tempTypeTable_Row.Render(
                        imgs,
                        memberItem.FileName,
                        //HtmlText.HtmlFormat(memberItem.Name),
                        mname,
                        summary
                        );
                    sectionContent.AppendPre(strRow);
                }
                if (sectionContent.Length > 0)
                    secText = CreateTableSection(category, sectionTitle, sectionContent.ToString());
                return secText;
            }
            else
                return null;
        }

        private string SimpleParamName(string name)
        {
            return name.Replace("System.", "").Replace("GZFramework.DB.Core.", "");
        }

        /// <summary>
        /// 创建成员页面
        /// </summary>
        /// <param name="memberItem">成员的信息</param>
        protected override void CreateMemberPage(ContentTreeItem memberItem)
        {

            MemberInfo info = memberItem.Info;
            MemberXmlElement memberData = DataProvider.GetMemberXmlNode(info);

            HtmlText content = new HtmlText();
            int secID = 0;
            foreach (PageSection section in MemberPageSections)
            {
                string secText = null;
                switch (section.SectionType)
                {
                    case PageSectionType.Syntax:
                        {
                            string parameters = "";
                            string ret = "";
                            string val = "";

                            if (memberData != null)
                            {
                                XmlNodeList ns;

                                ns = memberData.Data.GetElementsByTagName("param");
                                foreach (XmlElement p in ns)
                                {
                                    parameters += tempParam.Render(
                                        HtmlText.HtmlFormat(p.GetAttribute("name")),
                                        GetInnerTags(p, memberData.Owner)
                                    );
                                }

                                ret = GetChildNodeInnerText(memberData, "returns");
                                val = GetChildNodeInnerText(memberData, "value");
                            }

                            HtmlText secContent = new HtmlText(), declaration = new HtmlText();

                            declaration.AppendPre(HtmlText.HtmlFormat(CreateMemberDeclaration(info, memberData)));

                            secContent.AppendPre(tempDeclaration.Render("C#", declaration));

                            switch (info.MemberType)
                            {
                                case MemberTypes.Constructor:
                                case MemberTypes.Method:
                                    {
                                        if (!string.IsNullOrEmpty(parameters))
                                            secContent.AppendPre(tempParamsBlock.Render(Resources.String46, parameters));
                                        if (!string.IsNullOrEmpty(ret))
                                            secContent.AppendPre(tempParamsBlock.Render(Resources.String47, ret));
                                        break;
                                    }
                                case MemberTypes.Property:
                                    {
                                        if (!string.IsNullOrEmpty(val))
                                            secContent.AppendPre(tempParamsBlock.Render(Resources.String48, val));
                                        break;
                                    }
                                default:
                                    break;
                            }

                            Hashtable sectionValues = new Hashtable();
                            sectionValues["ID"] = "Syntex";
                            sectionValues["Title"] = section.Name;
                            sectionValues["Content"] = secContent.ToString();
                            secText = tempSection.Render(sectionValues);
                            break;
                        }
                    case PageSectionType.Remarks:
                        {
                            if (memberData != null)
                            {
                                string remarks = GetChildNodeInnerText(memberData, "remarks");
                                if (!string.IsNullOrEmpty(remarks))
                                {
                                    Hashtable sectionValues = new Hashtable();
                                    sectionValues["ID"] = "Remarks";
                                    sectionValues["Title"] = section.Name;
                                    sectionValues["Content"] = tempRemarks.Render(remarks);
                                    secText = tempSection.Render(sectionValues);
                                }
                            }
                            break;
                        }
                    case PageSectionType.Exception:
                        {
                            if (memberData != null)
                            {
                                XmlNodeList ns;
                                ns = memberData.Data.GetElementsByTagName("exception");
                                HtmlText ecx = new HtmlText();
                                foreach (XmlElement n in ns)
                                {
                                    int index = n.GetAttribute("cref").LastIndexOfAny(new char[] { ':', '.', '+' });
                                    ecx.AppendPre(
                                        tempExceptionTable_Row.Render(
                                            n.GetAttribute("cref").Substring(index + 1),
                                            n.InnerText
                                        )
                                    );
                                }
                                if (ecx.Length > 0)
                                {
                                    Hashtable sectionValues = new Hashtable();
                                    sectionValues["ID"] = "Exception";
                                    sectionValues["Title"] = section.Name;
                                    sectionValues["Content"] = tempExceptionTable.Render(ecx);
                                    secText = tempSection.Render(sectionValues);
                                }
                            }
                            break;
                        }
                    case PageSectionType.Example:
                        {
                            if (memberData != null)
                            {
                                string exampleText = GetChildNodeInnerText(memberData, "example");
                                if (!string.IsNullOrEmpty(exampleText))
                                {
                                    Hashtable sectionValues = new Hashtable();
                                    sectionValues["ID"] = "Exception";
                                    sectionValues["Title"] = section.Name;
                                    sectionValues["Content"] = exampleText;
                                    secText = tempSection.Render(sectionValues);
                                }
                            }
                            break;
                        }
                    case PageSectionType.FromXML:
                        {
                            if (memberData != null)
                            {
                                string text = GetChildNodeInnerText(memberData, section.XmlSource);
                                if (!string.IsNullOrEmpty(text))
                                {
                                    Hashtable sectionValues = new Hashtable();
                                    sectionValues["ID"] = "SEC" + secID.ToString("00");
                                    sectionValues["Title"] = section.Name;
                                    sectionValues["Content"] = text;
                                    secText = tempSection.Render(sectionValues);
                                }
                            }
                            break;
                        }
                }
                if (!string.IsNullOrEmpty(secText))
                    content.AppendPre(secText);
                secID++;
            }

            string summary = GetChildNodeInnerText(memberData, "summary");

            Hashtable values = new Hashtable();
            values["CollapseAll"] = Resources.strCollapseAll;
            values["ExpandAll"] = Resources.strExpandAll;
            values["PIC"] = Resources.strPic;
            if (info.MemberType == MemberTypes.Constructor)
                values["Title"] = HtmlText.HtmlFormat(memberItem.Name) + " " + NameToTypeDictionary[memberItem.Parent.Name];
            else
                values["Title"] = HtmlText.HtmlFormat(info.DeclaringType.Name + "." + info.Name) + " " + NameToTypeDictionary[memberItem.Parent.Name];
            values["Summary"] = summary;
            values["Content"] = content;
            values["Encoding"] = TargetEncoding.HeaderName;

            tempPage.SaveAs(
                HtmlFileDirectory + "\\" + memberItem.FileName,
                TargetEncoding,
                values
                );

        }

        private string GetChildNodeInnerText(MemberXmlElement elem, string name)
        {
            if (elem != null)
            {
                XmlNodeList nodes = elem.Data.GetElementsByTagName(name);
                if (nodes != null && nodes.Count > 0 && nodes[0].NodeType == XmlNodeType.Element)
                {
                    return GetTag(nodes[0] as XmlElement, elem.OwnerDirectory);
                }
                else
                    return "";
            }
            else
                return "";
        }

        /// <summary>
        /// 将XML注释所有子级转换成HTML
        /// </summary>
        /// <param name="node">指定的XML注释</param>
        /// <param name="xmlFile">elem所在的xml文件的路径</param>
        /// <returns>所有子级的html</returns>
        protected string GetInnerTags(XmlElement node, string xmlFile)
        {
            HtmlText strText = new HtmlText();

            foreach (XmlNode subNode in node.ChildNodes)
            {
                switch (subNode.NodeType)
                {
                    case XmlNodeType.Text:
                        {
                            strText.AppendParagraphAndTrim(subNode.InnerText);
                            break;
                        }
                    case XmlNodeType.Element:
                        {
                            strText.AppendPre(GetTag(subNode as XmlElement, xmlFile));
                            break;
                        }
                }
            }
            return strText.ToString();
        }

        /// <summary>
        /// 获取XML元素的开始标志
        /// </summary>
        /// <param name="elem">要获取开始标志的XML元素</param>
        /// <returns>一个表示XML元素开始标志的字符串</returns>
        protected string GetElementBeginTag(XmlElement elem)
        {
            StringBuilder tag = new StringBuilder();
            tag.Append("<");
            tag.Append(elem.Name);
            foreach (XmlAttribute attr in elem.Attributes)
            {
                tag.AppendFormat(" {0}=\"{1}\"", attr.Name, attr.Value);
            }
            tag.Append(">");
            return tag.ToString();
        }

        /// <summary>
        /// 获取XML元素的结束标志
        /// </summary>
        /// <param name="elem">要获取结束标志的XML元素</param>
        /// <returns>一个表示XML元素结束标志的字符串</returns>
        protected string GetElementEndTag(XmlElement elem)
        {
            return string.Format("</{0}>", elem.Name);
        }

        /// <summary>
        /// 将XML注释转换成页面上的HTML代码，继承类可以重写此方法已扩展XML注释标志
        /// </summary>
        /// <param name="elem">要转成HTML的XML元素</param>
        /// <param name="xmlFile">elem所在的XML文件的路径</param>
        /// <returns>XML注释对应的的HTML代码</returns>
        protected virtual string GetTag(XmlElement elem, String xmlFile)
        {
            HtmlText strText = new HtmlText();
            switch (elem.Name)
            {
                case "code":
                    {
                        if (elem.HasAttribute("src"))
                        {
                            string encoding = "";
                            if (elem.HasAttribute("encoding"))
                                encoding = elem.GetAttribute("encoding");
                            else
                                encoding = "GBK";
                            StreamReader sr = null;
                            try
                            {
                                sr = new StreamReader(
                                    xmlFile + "\\" + elem.GetAttribute("src"),
                                    Encoding.GetEncoding(encoding)
                                    );
                            }
                            catch
                            {
                                break;
                            }
                            try
                            {
                                HtmlText code = new HtmlText(sr.ReadToEnd());
                                string language = elem.GetAttribute("language");
                                if (string.IsNullOrEmpty(language)) language = "C#";
                                strText.AppendPre(
                                    tempExample_Code.Render(
                                        language,
                                        code
                                    )
                                );
                            }
                            finally
                            {
                                sr.Close();
                            }
                        }
                        else
                        {
                            HtmlText code = new HtmlText();
                            code.AppendParagraphAndTrim(elem.InnerText);
                            strText.AppendPre(
                                tempExample_Code.Render(
                                    elem.GetAttribute("language"),
                                    code
                                )
                            );
                        }
                        break;
                    }
                case "see":
                    {
                        string cref = elem.GetAttribute("cref");
                        if (cref != null)
                        {
                            string href = GetFileName(cref);
                            int index = cref.LastIndexOfAny(new char[] { '.', ':' });
                            if (string.IsNullOrEmpty(href))
                            {
                                strText.Append(cref.Substring(index + 1));
                            }
                            else
                            {
                                strText.AppendPre(string.Format("<a target=\"_self\" href=\"{0}\">{1}</a>", href, cref.Substring(index + 1)));
                            }
                        }
                        break;
                    }
                case "para":
                    {
                        strText.AppendPre("<p>");
                        strText.AppendPre(GetInnerTags(elem as XmlElement, xmlFile));
                        strText.AppendPre("</p>");
                        break;
                    }
                case "value":
                case "returns":
                case "summary":
                case "param":
                case "remarks":
                    {
                        strText.AppendPre(GetInnerTags(elem, xmlFile));
                        break;
                    }
                default:
                    {
                        if (!string.IsNullOrEmpty(elem.InnerXml))
                        {
                            strText.AppendPre(GetElementBeginTag(elem));
                            strText.AppendPre(GetInnerTags(elem as XmlElement, xmlFile));
                            strText.AppendPre(GetElementEndTag(elem));
                        }
                        else
                        {
                            strText.AppendPre(elem.OuterXml);
                        }
                        break;
                    }
            }
            return strText.ToString();
        }

        static PageSection[] _typePageSections;
        /// <summary>
        /// 
        /// </summary>
        public virtual PageSection[] TypePageSections
        {
            get { return _typePageSections; }
        }

        static PageSection[] _memberPageSections;

        /// <summary>
        /// 
        /// </summary>
        public virtual PageSection[] MemberPageSections
        {
            get { return _memberPageSections; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PageSectionType
    {
        /// <summary>
        /// 
        /// </summary>
        Summary,
        /// <summary>
        /// 
        /// </summary>
        Remarks,
        /// <summary>
        /// 
        /// </summary>
        Exception,
        /// <summary>
        /// 
        /// </summary>
        Syntax,
        /// <summary>
        /// 
        /// </summary>
        Example,

        /// <summary>
        /// 
        /// </summary>
        Constructor,
        /// <summary>
        /// 
        /// </summary>
        PublicMethod,
        /// <summary>
        /// 
        /// </summary>
        PrivateMethod,
        /// <summary>
        /// 
        /// </summary>
        ProtectedMethod,
        /// <summary>
        /// 
        /// </summary>
        PublicProperty,
        /// <summary>
        /// 
        /// </summary>
        PrivateProperty,
        /// <summary>
        /// 
        /// </summary>
        ProtectedProperty,
        /// <summary>
        /// 
        /// </summary>
        PublicField,
        /// <summary>
        /// 
        /// </summary>
        PrivateField,
        /// <summary>
        /// 
        /// </summary>
        ProtectedField,
        /// <summary>
        /// 
        /// </summary>
        Event,

        /// <summary>
        /// 
        /// </summary>
        FromXML
    }

    /// <summary>
    /// 
    /// </summary>
    public class PageSection
    {
        string _name;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        PageSectionType _sectionType;
        /// <summary>
        /// 
        /// </summary>
        public PageSectionType SectionType
        {
            get { return _sectionType; }
        }

        string _xmlSource;
        /// <summary>
        /// 
        /// </summary>
        public string XmlSource
        {
            get { return _xmlSource; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sectionType"></param>
        /// <param name="xmlSource"></param>
        public PageSection(string name, PageSectionType sectionType, string xmlSource)
        {
            _name = name;
            _sectionType = sectionType;
            if (sectionType == PageSectionType.FromXML)
            {
                if (!string.IsNullOrEmpty(xmlSource)) _xmlSource = xmlSource;
            }
            else
            {
                _xmlSource = null;
            }
        }
    }
}
