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
	/// ��ʾ�ĵ�����������
	/// </summary>
    public abstract class DocumentBuilder
    {
        IGetData _dataProvider;
        IInteract _interact;

		/// <summary>
		/// ��ȡһ��IGetData�ӿڣ����ڻ�ȡ�����ĵ���������ݣ��磺���򼯵ĳ�Ա����XMLע�ͣ�
		/// </summary>
		/// <value>IGetData�ӿڣ����ڻ�ȡ�����ĵ���������ݣ��磺���򼯵ĳ�Ա����XMLע�ͣ�</value>
        public IGetData DataProvider
        {
            get { return _dataProvider; }
        }

        /// <summary>
        /// ��ȡһ��IInteract�ӿڣ�����ͬ�ĵ����ɳ��������ڽ��н���
        /// </summary>
        /// <value>һ��IInteract�ӿڣ�����ͬ�ĵ����ɳ��������ڽ��н���</value>
        public IInteract Interact
        {
            get { return _interact; }
        }

        /// <summary>
        /// ��ʼ��DocumentBuilderʵ��
        /// </summary>
        /// <param name="data">һ��IGetData�ӿڣ����ڻ�ȡ�����ĵ���������ݣ��磺���򼯵ĳ�Ա����XMLע�ͣ�</param>
		/// <param name="interact">һ��IInteract�ӿڣ�����ͬ�ĵ����ɳ��������ڽ��н���</param>
        public DocumentBuilder(IGetData data, IInteract interact)
        {
            _dataProvider = data;
            _interact = interact;
        }

		/// <summary>
		/// ��ȡһ��IWin32Window�ӿڣ�������ʾѡ���
		/// </summary>
		public abstract Form OptionDialog { get;}

        /// <summary>
        /// �����ĵ�
        /// </summary>
        /// <param name="title">�ĵ�����</param>
        /// <param name="target">����ļ�·��</param>
        public abstract void Build(string title, string target);

        /// <summary>
        /// ��ȡ�ļ���ɸѡ���ַ��������ַ��������Ի���ġ����Ϊ�ļ����͡����г��ֵ�ѡ�����ݡ�
        /// </summary>
        /// <value>��ǰ�ļ���ɸѡ���ַ��������ַ��������Ի���ġ����Ϊ�ļ����͡����г��ֵ�ѡ�����ݡ�</value>
        public abstract string Filter{get;}
    }

    /// <summary>
    /// ��ʾ�����еĳ�Ա���ࡢ�ṹ���¼��������ȣ�����Ϣ
    /// </summary>
    public class DocumentBuilderMember
    {
        bool _state;
        MemberInfo _memberInfo;

        /// <summary>
        /// ��ʼ��DocumentBuilderMemberʵ��
        /// </summary>
        /// <param name="state">�ó�Ա�Ƿ�ѡ��</param>
        /// <param name="memInfo">��Ա��Ϣ</param>
        public DocumentBuilderMember(bool state, MemberInfo memInfo)
        {
            _state = state;
            _memberInfo = memInfo;
        }

        /// <summary>
        /// ��ȡһ��ֵ��ͨ����ֵָʾ�ó�Ա�Ƿ�ѡ��
        /// </summary>
        /// <value>���ѡ�У���Ϊtrue������Ϊfalse</value>
        public bool Selected
        {
            get { return _state; }
        }

        /// <summary>
        /// ��ȡ�����еĳ�Ա���ࡢ�ṹ���¼��������ȣ�����Ϣ
        /// </summary>
        /// <value>��Ա����Ϣ</value>
        public MemberInfo MemberInfo
        {
            get { return _memberInfo; }
        }
    }

    /// <summary>
    /// ��ȡ��Ա����XMLע��
    /// </summary>
    public interface IGetData
	{
		/// <summary>
		/// ��ѡ�е�����
		/// </summary>
		/// <returns>���򼯰��������͵���Ϣ</returns>
		DocumentBuilderMember[] GetTypes();

		/// <summary>
		/// ��ȡ���򼯰���������
		/// </summary>
		/// <returns>���򼯰��������͵���Ϣ</returns>
		Type[] GetSelectedTypes();

        /// <summary>
        /// ��ȡָ�������Ͱ��������г�Ա
        /// </summary>
        /// <param name="type">��Ҫ��ȡ��Ա������</param>
        /// <returns>ָ�������Ͱ��������г�Ա����Ϣ</returns>
		DocumentBuilderMember[] GetTypeMembers(Type type);

		/// <summary>
		/// ��ȡָ�������Ͱ����Ĳ�����ѡ�е����г�Ա
		/// </summary>
		/// <param name="type">��Ҫ��ȡ��Ա������</param>
		/// <returns>ָ�������Ͱ��������г�Ա����Ϣ</returns>
		MemberInfo[] GetTypeSelectedMembers(Type type);

        /// <summary>
        /// ��ȡָ����Ա��Ӧ��XMLע��
        /// </summary>
        /// <param name="memInfo">Ҫ��ȡע�͵ĳ�Ա</param>
        /// <returns>ָ����Ա��Ӧ��XMLע��</returns>
        MemberXmlElement GetMemberXmlNode(MemberInfo memInfo);
    }

    /// <summary>
    /// ����һ��ͬ�ĵ����ɳ��������潻�����磺��ʾ���ȣ��ķ���
    /// </summary>
    public interface IInteract
    {
        /// <summary>
        /// �������ھ�����߳���ִ��ָ��ί�С�
        /// </summary>
        /// <param name="method">����Ҫ�ڿؼ����߳��������е��õķ�����ί�С�</param>
        Object Invoke(Delegate method);
        /// <summary>
        /// ��ӵ�������ھ�����߳��ϣ���ָ���Ĳ����б�ִ��ָ����ί�С�
        /// </summary>
        /// <param name="method">����Ҫ�ڿؼ����߳��������е��õķ�����ί�С�</param>
        /// <param name="args">��Ϊָ�������Ĳ������ݵĶ������顣����˷���û�в������ò��������ǿ�����</param>
        Object Invoke(Delegate method, params object[] args);
        /// <summary>
        /// �ڴ����ؼ��Ļ�����������߳����첽ִ��ָ��ί�С�
        /// </summary>
        /// <param name="method">�Բ��������ķ�����ί�С�</param>
        /// <returns>һ����ʾ BeginInvoke �����Ľ���� IAsyncResult��</returns>
        IAsyncResult BeginInvoke(Delegate method);
        /// <summary>
        /// �ڴ����ؼ��Ļ�����������߳��ϣ���ָ���Ĳ����첽ִ��ָ��ί�С�
        /// </summary>
        /// <param name="method">һ������ί�У������õĲ����������������� args ����������������ͬ��</param>
        /// <param name="args">��Ϊ���������Ĳ������ݵĶ������顣�������Ҫ�����������Ϊ������</param>
        /// <returns>һ����ʾ BeginInvoke �����Ľ���� IAsyncResult��</returns>
        IAsyncResult BeginInvoke(Delegate method, params object[] args);
        /// <summary>
        /// ��ȡ�ĵ����ɳ����������
        /// </summary>
        /// <value>�ĵ����ɳ����������</value>
        Form MainForm{get;}
    }

    /// <summary>
    /// ��ʾ��Ա��XMLע�ͼ����������Ϣ
    /// </summary>
    public class MemberXmlElement
    {
        string _owner, _ownerDirectory;
        XmlElement _data;

        /// <summary>
        /// ��ȡ�ó�ԱXMLע�����ڵ�XML�ĵ��ļ���Ŀ¼
        /// </summary>
        /// <value>�ó�ԱXMLע�����ڵ�XML�ĵ��ļ���Ŀ¼</value>
        public string OwnerDirectory
        {
            get { return _ownerDirectory; }
        }

        /// <summary>
        /// ��ȡ�ó�ԱXMLע�����ڵ�XML�ĵ��ļ�
        /// </summary>
        /// <value>�ó�ԱXMLע�����ڵ�XML�ĵ��ļ�</value>
        public string Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// ��ȡ�ó�ԱXMLע��
        /// </summary>
        /// <value>�ó�ԱXMLע��</value>
        public XmlElement Data
        {
            get { return _data; }
        }

        /// <summary>
        /// ��ʼ��MemberXmlElementʵ��
        /// </summary>
        /// <param name="owner">�ó�ԱXMLע�����ڵ�XML�ĵ��ļ�</param>
        /// <param name="data">�ó�ԱXMLע��</param>
        public MemberXmlElement(string owner, XmlElement data)
        {
            FileInfo ownerFile = new FileInfo(owner);
            _owner = owner;
            _ownerDirectory = ownerFile.Directory.FullName;
            _data = data;
        }
    }

    /// <summary>
    /// �ṩһЩ���ĵ�������صķ���
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
        /// ��ȡָ��������XML�ĵ�ע���е�ID�ַ���
        /// </summary>
        /// <param name="info">Ҫ��ȡID�ַ����Ķ���</param>
        /// <returns>ָ��������XML�ĵ�ע���е�ID�ַ���</returns>
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
        /// ��ȡһ�����͵Ķ���
        /// </summary>
        /// <param name="type">Ҫ��ȡ���������</param>
        /// <returns>ָ�����͵Ķ���</returns>
        public static string GetTypeDefinition(Type type)
        {
            return GetTypeDefinition(type, true);
        }

        /// <summary>
        /// ��ȡһ�����͵Ķ���
        /// </summary>
        /// <param name="type">Ҫ��ȡ���������</param>
        /// <param name="fullName">�Ƿ�ʹ��ȫ��</param>
        /// <returns>ָ�����͵Ķ���</returns>
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