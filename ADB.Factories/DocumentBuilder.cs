using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace ADB.Factories
{
	/// <summary>
	/// 表示文档生成器的类
	/// </summary>
    public abstract class DocumentBuilder
    {
        IGetData _dataProvider;
        IInteract _interact;

		/// <summary>
		/// 获取一个IGetData接口，用于获取生成文档所需的数据（如：程序集的成员及其XML注释）
		/// </summary>
		/// <value>IGetData接口，用于获取生成文档所需的数据（如：程序集的成员及其XML注释）</value>
        public IGetData DataProvider
        {
            get { return _dataProvider; }
        }

        /// <summary>
        /// 获取一个IInteract接口，用于同文档生成程序主窗口进行交互
        /// </summary>
        /// <value>一个IInteract接口，用于同文档生成程序主窗口进行交互</value>
        public IInteract Interact
        {
            get { return _interact; }
        }

        /// <summary>
        /// 初始化DocumentBuilder实例
        /// </summary>
        /// <param name="data">一个IGetData接口，用于获取生成文档所需的数据（如：程序集的成员及其XML注释）</param>
		/// <param name="interact">一个IInteract接口，用于同文档生成程序主窗口进行交互</param>
        public DocumentBuilder(IGetData data, IInteract interact)
        {
            _dataProvider = data;
            _interact = interact;
        }

		/// <summary>
		/// 获取一个IWin32Window接口，用于显示选项窗口
		/// </summary>
		public abstract Form OptionDialog { get;}

        /// <summary>
        /// 生成文档
        /// </summary>
        /// <param name="title">文档标题</param>
        /// <param name="target">输出文件路径</param>
        public abstract void Build(string title, string target);

        /// <summary>
        /// 获取文件名筛选器字符串，该字符串决定对话框的“另存为文件类型”框中出现的选择内容。
        /// </summary>
        /// <value>当前文件名筛选器字符串，该字符串决定对话框的“另存为文件类型”框中出现的选择内容。</value>
        public abstract string Filter{get;}
    }

    /// <summary>
    /// 表示程序集中的成员（类、结构、事件，方法等）的信息
    /// </summary>
    public class DocumentBuilderMember
    {
        bool _state;
        MemberInfo _memberInfo;

        /// <summary>
        /// 初始化DocumentBuilderMember实例
        /// </summary>
        /// <param name="state">该成员是否选中</param>
        /// <param name="memInfo">成员信息</param>
        public DocumentBuilderMember(bool state, MemberInfo memInfo)
        {
            _state = state;
            _memberInfo = memInfo;
        }

        /// <summary>
        /// 获取一个值，通过该值指示该成员是否选中
        /// </summary>
        /// <value>如果选中，则为true，否则，为false</value>
        public bool Selected
        {
            get { return _state; }
        }

        /// <summary>
        /// 获取程序集中的成员（类、结构、事件，方法等）的信息
        /// </summary>
        /// <value>成员的信息</value>
        public MemberInfo MemberInfo
        {
            get { return _memberInfo; }
        }
    }

    /// <summary>
    /// 获取成员及其XML注释
    /// </summary>
    public interface IGetData
	{
		/// <summary>
		/// 已选中的类型
		/// </summary>
		/// <returns>程序集包含的类型的信息</returns>
		DocumentBuilderMember[] GetTypes();

		/// <summary>
		/// 获取程序集包含的类型
		/// </summary>
		/// <returns>程序集包含的类型的信息</returns>
		Type[] GetSelectedTypes();

        /// <summary>
        /// 获取指定的类型包含的所有成员
        /// </summary>
        /// <param name="type">将要获取成员的类型</param>
        /// <returns>指定的类型包含的所有成员的信息</returns>
		DocumentBuilderMember[] GetTypeMembers(Type type);

		/// <summary>
		/// 获取指定的类型包含的并且已选中的所有成员
		/// </summary>
		/// <param name="type">将要获取成员的类型</param>
		/// <returns>指定的类型包含的所有成员的信息</returns>
		MemberInfo[] GetTypeSelectedMembers(Type type);

        /// <summary>
        /// 获取指定成员对应的XML注释
        /// </summary>
        /// <param name="memInfo">要获取注释的成员</param>
        /// <returns>指定成员对应的XML注释</returns>
        MemberXmlElement GetMemberXmlNode(MemberInfo memInfo);
    }

    /// <summary>
    /// 公开一个同文档生成程序主界面交互（如：显示进度）的方法
    /// </summary>
    public interface IInteract
    {
        /// <summary>
        /// 在主窗口句柄的线程上执行指定委托。
        /// </summary>
        /// <param name="method">包含要在控件的线程上下文中调用的方法的委托。</param>
        Object Invoke(Delegate method);
        /// <summary>
        /// 在拥有主窗口句柄的线程上，用指定的参数列表执行指定的委托。
        /// </summary>
        /// <param name="method">包含要在控件的线程上下文中调用的方法的委托。</param>
        /// <param name="args">作为指定方法的参数传递的对象数组。如果此方法没有参数，该参数可以是空引用</param>
        Object Invoke(Delegate method, params object[] args);
        /// <summary>
        /// 在创建控件的基础句柄所在线程上异步执行指定委托。
        /// </summary>
        /// <param name="method">对不带参数的方法的委托。</param>
        /// <returns>一个表示 BeginInvoke 操作的结果的 IAsyncResult。</returns>
        IAsyncResult BeginInvoke(Delegate method);
        /// <summary>
        /// 在创建控件的基础句柄所在线程上，用指定的参数异步执行指定委托。
        /// </summary>
        /// <param name="method">一个方法委托，它采用的参数的数量和类型与 args 参数中所包含的相同。</param>
        /// <param name="args">作为给定方法的参数传递的对象数组。如果不需要参数，则可以为空引用</param>
        /// <returns>一个表示 BeginInvoke 操作的结果的 IAsyncResult。</returns>
        IAsyncResult BeginInvoke(Delegate method, params object[] args);
        /// <summary>
        /// 获取文档生成程序的主窗口
        /// </summary>
        /// <value>文档生成程序的主窗口</value>
        Form MainForm{get;}
    }

    /// <summary>
    /// 表示成员的XML注释及其他相关信息
    /// </summary>
    public class MemberXmlElement
    {
        string _owner, _ownerDirectory;
        XmlElement _data;

        /// <summary>
        /// 获取该成员XML注释所在的XML文档文件的目录
        /// </summary>
        /// <value>该成员XML注释所在的XML文档文件的目录</value>
        public string OwnerDirectory
        {
            get { return _ownerDirectory; }
        }

        /// <summary>
        /// 获取该成员XML注释所在的XML文档文件
        /// </summary>
        /// <value>该成员XML注释所在的XML文档文件</value>
        public string Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// 获取该成员XML注释
        /// </summary>
        /// <value>该成员XML注释</value>
        public XmlElement Data
        {
            get { return _data; }
        }

        /// <summary>
        /// 初始化MemberXmlElement实例
        /// </summary>
        /// <param name="owner">该成员XML注释所在的XML文档文件</param>
        /// <param name="data">该成员XML注释</param>
        public MemberXmlElement(string owner, XmlElement data)
        {
            FileInfo ownerFile = new FileInfo(owner);
            _owner = owner;
            _ownerDirectory = ownerFile.Directory.FullName;
            _data = data;
        }
    }

    /// <summary>
    /// 提供一些与文档生成相关的方法
    /// </summary>
    public static class DocumentBuilderUtility
    {
        private static Type[] GetOuterTypes(Type type)
        {
            int count = 1;
            Type t = type;
            while (t.IsNested) { t = t.DeclaringType; count++; }
            Type[] ots = new Type[count];
            t = type;
            do
            {
                ots[--count] = t;
                t = t.DeclaringType;
            } while (t != null);
            return ots;
        }

        private static string GetTypeID(Type type)
        {
            if (string.IsNullOrEmpty(type.FullName))
            {
                return type.IsNested ? type.ToString().Replace('+', '.') : type.ToString();
            }
            else
            {
                return type.IsNested ? type.FullName.Replace('+', '.') : type.FullName;
            }
        }

        private static string GetParameterTypeID(Type type)
        {
            if (type.IsGenericParameter)
            {
                return '`' + type.GenericParameterPosition.ToString();
            }
            else if (type.IsGenericType)
            {
                StringBuilder name = new StringBuilder();
                if (type.Namespace != null) name.Append(type.Namespace);
                Type[] gas = type.GetGenericArguments();
                Type[] ots = GetOuterTypes(type);
                int iga = 0;
                foreach (Type t in ots)
                {
                    if (name.Length > 0) name.Append('.');
                    string[] ss = t.Name.Split('`');
                    name.Append(ss[0]);
                    if (ss.Length > 1)
                    {
                        name.Append("{");
                        for (int count = Convert.ToInt32(ss[1]); count > 0; count--, iga++)
                        {
                            if (gas[iga].IsGenericParameter)
                            {
                                name.Append('`');
                                name.Append(gas[iga].GenericParameterPosition);
                            }
                            else
                                name.Append(GetParameterTypeID(gas[iga]));
                            if (count != 1) name.Append(',');
                        }
                        name.Append("}");
                    }
                }
                return name.ToString();
            }
            else
                return GetTypeID(type);
        }

        /// <summary>
        /// 获取指定对象在XML文档注释中的ID字符串
        /// </summary>
        /// <param name="info">要获取ID字符串的对象</param>
        /// <returns>指定对象在XML文档注释中的ID字符串</returns>
        public static string GetMemberID(MemberInfo info)
        {
            switch (info.MemberType)
            {
            case MemberTypes.TypeInfo:
                {
                    return "T:" + DocumentBuilderUtility.GetTypeID(info as Type);
                }
            case MemberTypes.NestedType:
                {
                    return "T:" + DocumentBuilderUtility.GetTypeID(info as Type);
                }
            case MemberTypes.Constructor:
                {
                    string name = "M:", ps = "";
                    ConstructorInfo ctor = info as ConstructorInfo;
                        
                    name += DocumentBuilderUtility.GetTypeID(ctor.DeclaringType) + ".#ctor";
                    foreach (ParameterInfo param in ctor.GetParameters())
                    {
                        string p;
                        p = DocumentBuilderUtility.GetParameterTypeID(param.ParameterType);
                        if (param.IsOut)
                            p = p.Replace('&', '@');
                        if (string.IsNullOrEmpty(ps)) ps = p; else ps += "," + p;
                    }
                    if (!string.IsNullOrEmpty(ps))
                        name += "(" + ps + ")";



                    return name;
                }
            case MemberTypes.Method:
                {
                    string name = "M:", ps = "";
                    MethodInfo method = info as MethodInfo;
                    name += DocumentBuilderUtility.GetTypeID(method.DeclaringType) + "." + method.Name;
                    foreach (ParameterInfo param in method.GetParameters())
                    {
                        string p;
                        p = DocumentBuilderUtility.GetParameterTypeID(param.ParameterType);
                        if (param.ParameterType.IsByRef) p = p.Replace('&', '@');
                        if (string.IsNullOrEmpty(ps)) ps = p; else ps += "," + p;
                    }
                    if (!string.IsNullOrEmpty(ps))
                        name += "(" + ps + ")";
                    return name;
                }
            case MemberTypes.Property:
                {
                    PropertyInfo property = info as PropertyInfo;
                    MethodInfo pm = property.GetGetMethod(true);
                    if (pm == null) pm = property.GetSetMethod(true);
                    ParameterInfo[] parameters = pm.GetParameters();
                    if (pm == null || parameters.Length == 0)
                        return "P:" + DocumentBuilderUtility.GetTypeID(property.DeclaringType) + "." + property.Name;
                    else
                    {
                        string name = "P:" + DocumentBuilderUtility.GetTypeID(property.DeclaringType) + "." + property.Name;
                        string ps = "";
                        foreach (ParameterInfo param in parameters)
                        {
                            string p;
                            p = DocumentBuilderUtility.GetParameterTypeID(param.ParameterType);
                            if (string.IsNullOrEmpty(ps)) ps = p; else ps += "," + p;
                        }
                        if (!string.IsNullOrEmpty(ps))
                            name += "(" + ps + ")";
                        return name;
                    }
                }
            case MemberTypes.Event:
                {
                    EventInfo ent = info as EventInfo;
                    return "E:" + DocumentBuilderUtility.GetTypeID(ent.DeclaringType) + "." + ent.Name;
                }
            case MemberTypes.Field:
                {
                    FieldInfo field = info as FieldInfo;
                    return "F:" + DocumentBuilderUtility.GetTypeID(field.DeclaringType) + "." + field.Name;
                }
            default:
                return null;
            }
        }

        /// <summary>
        /// 获取一个类型的定义
        /// </summary>
        /// <param name="type">要获取定义的类型</param>
        /// <returns>指定类型的定义</returns>
        public static string GetTypeDefinition(Type type)
        {
            return GetTypeDefinition(type, true);
        }

        /// <summary>
        /// 获取一个类型的定义
        /// </summary>
        /// <param name="type">要获取定义的类型</param>
        /// <param name="fullName">是否使用全称</param>
        /// <returns>指定类型的定义</returns>
        public static string GetTypeDefinition(Type type,bool fullName)
        {
            if (type.IsGenericParameter)
            {
                return type.Name;
            }
            else if (type.IsGenericType)
            {
                StringBuilder name = new StringBuilder();
                if (type.Namespace != null && fullName) name.Append(type.Namespace);
                Type[] gas = type.GetGenericArguments();
                Type[] ots = GetOuterTypes(type);
                int iga = 0;
                foreach (Type t in ots)
                {
                    if (name.Length > 0) name.Append('.');
                    string[] ss = t.Name.Split('`');
                    name.Append(ss[0]);
                    if (ss.Length > 1)
                    {
                        name.Append("<");
                        for (int count = Convert.ToInt32(ss[1]); count > 0; count--, iga++)
                        {
                            if (gas[iga].IsGenericParameter)
                                name.Append(gas[iga].Name);
                            else
                                name.Append(GetTypeDefinition(gas[iga]));
                            if (count != 1) name.Append(',');
                        }
                        name.Append(">");
                    }
                }
                return name.ToString();
            }
            else
            {
                if (fullName)
                {
                    if (string.IsNullOrEmpty(type.FullName))
                    {
                        return type.IsNested ? type.ToString().Replace('+', '.') : type.ToString();
                    }
                    else
                    {
                        return type.IsNested ? type.FullName.Replace('+', '.') : type.FullName;
                    }
                }
                else
                {
                    return type.IsNested ? type.Name.Replace('+', '.') : type.Name;
                }
            }
        }
    }

}