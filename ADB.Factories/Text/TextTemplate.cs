using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace ADB.Factories
{
    /// <summary>
    /// ��ʾһ���ı�ģ�壬����ʹ��һ���ַ�����Ϊģ�壬ͨ����ģ���еı�־�滻Ϊ��Ӧ��ֵ��ģ���еı�־�� {��־��} ��ʾ�������µ��ı�
    /// </summary>
    /// <example>���´������ı�ģ������IMG��־
    /// <code>
    /// static class Program
    /// {
    ///     [STAThread]
    ///     static void Main()
    ///     {
    ///         TextTemplate temp = new TextTemplate("&lt;img src='{src}' alt='{alt}' /&gt;");
    ///         Console.WriteLine(temp.Render("pic.bmp","Image"));
    ///         Hashtable values = new Hashtable();
    ///         values.Add("src", "pic.bmp");
    ///         values.Add("alt", "image");
    ///         Console.WriteLine(temp.Render(values));
    ///     }
    /// }
    /// 
    /// ���Ϊ��
    /// &lt;img src='pic.bmp' alt='Image' /&gt;
    /// &lt;img src='pic.bmp' alt='image' /&gt;
    /// 
    /// </code>
    /// </example>
    public class TextTemplate
    {
        TextTemplateTag[] _tags;
        String[] _contentParts;
        int _tagCount;

        private TextTemplate()
        {
            _tagCount = 0;
            _tags = null;
            _contentParts = null;
        }

        /// <summary>
        /// ��ָ����ģ���ʼ��TextTemplate
        /// </summary>
        /// <param name="content">ģ������</param>
        public TextTemplate(String content)
        {
            FromString(content);
        }

        /// <summary>
        /// ��ָ����ģ���ʼ��TextTemplate,ģ���������ļ�����
        /// </summary>
        /// <param name="file">ģ���ļ�λ��</param>
        /// <param name="encoding">�ļ�ʹ�õı���</param>
        public TextTemplate(string file, Encoding encoding)
        {
            StreamReader sr = new StreamReader(file, encoding);
            try
            {
                string content = sr.ReadToEnd();
                FromString(content);
            }
            catch (Exception)
            {
                sr.Close();
                throw;
            }
            sr.Close();
        }

        private void FromString(String content)
        {
            MatchCollection mc = Regex.Matches(content, "{\\w+}");
            _tagCount = mc.Count;
            _tags = new TextTemplateTag[mc.Count];
            _contentParts = new string[mc.Count + 1];
            int index = 0;
            foreach (Match m in mc)
            {
                _tags[index++] = new TextTemplateTag(m.Value.Substring(1, m.Value.Length - 2), m.Index, m.Length);
            }
            int start = 0;
            index = 0;
            foreach (TextTemplateTag con in _tags)
            {
                _contentParts[index] = content.Substring(start, con.Position - start);
                start = con.Position + con.Length;
                index++;
            }
            if (start < content.Length) _contentParts[index] = content.Substring(start);
        }

        /// <summary>
        /// ��ָ����ֵ�����ı�
        /// </summary>
        /// <param name="values">����־��Ӧ��ֵ���ñ�־����Ϊkey��</param>
        /// <returns>���ɵ��ı�</returns>
        /// <example>���´������ı�ģ������IMG��־
        /// <code>
        /// static class Program
        /// {
        ///     [STAThread]
        ///     static void Main()
        ///     {
        ///         TextTemplate temp = new TextTemplate("&lt;img src='{src}' alt='{alt}' /&gt;");
        ///         Console.WriteLine(temp.Render("pic.bmp","Image"));
        ///         Hashtable values = new Hashtable();
        ///         values.Add("src", "pic.bmp");
        ///         values.Add("alt", "image");
        ///         Console.WriteLine(temp.Render(values));
        ///     }
        /// }
        /// 
        /// ���Ϊ��
        /// &lt;img src='pic.bmp' alt='Image' /&gt;
        /// &lt;img src='pic.bmp' alt='image' /&gt;
        /// 
        /// </code>
        /// </example>
        public string Render(Hashtable values)
        {
            StringBuilder result = new StringBuilder(8 * 1024);
            int i = 0;
            for (i = 0; i < _tagCount; i++)
            {
                result.Append(_contentParts[i]);
                if (values[_tags[i].Name] != null)
                    result.Append(values[_tags[i].Name]);
                else
                    result.Append("{" + _tags[i].Name + "}");
            }
            result.Append(_contentParts[i]);
            return result.ToString();
        }

        /// <summary>
        /// ��ָ����ֵ�����ı�
        /// </summary>
        /// <param name="args">����־��Ӧ��ֵ(���Ա�־������һ����־��Ӧ��һ���������Դ�����)</param>
        /// <returns>���ɵ��ı�</returns>
        /// <example>���´������ı�ģ������IMG��־
        /// <code>
        /// static class Program
        /// {
        ///     [STAThread]
        ///     static void Main()
        ///     {
        ///         TextTemplate temp = new TextTemplate("&lt;img src='{src}' alt='{alt}' /&gt;");
        ///         Console.WriteLine(temp.Render("pic.bmp","Image"));
        ///         Hashtable values = new Hashtable();
        ///         values.Add("src", "pic.bmp");
        ///         values.Add("alt", "image");
        ///         Console.WriteLine(temp.Render(values));
        ///     }
        /// }
        /// 
        /// ���Ϊ��
        /// &lt;img src='pic.bmp' alt='Image' /&gt;
        /// &lt;img src='pic.bmp' alt='image' /&gt;
        /// 
        /// </code>
        /// </example>
        public string Render(params object[] args)
        {
            StringBuilder result = new StringBuilder(2 * 1024);
            int i = 0;
            for (i = 0; i < _tagCount; i++)
            {
                result.Append(_contentParts[i]);
                result.Append(args[i].ToString());
            }
            result.Append(_contentParts[i]);
            return result.ToString();
        }

        /// <summary>
        /// ��ָ����ֵ�����ı��������浽�ļ���
        /// </summary>
        /// <param name="file">Ҫ������ļ�·��</param>
        /// <param name="encoding">�ļ��ı���</param>
        /// <param name="values">����־��Ӧ��ֵ���ñ�־����Ϊkey��</param>
        public void SaveAs(string file, Encoding encoding, Hashtable values)
        {
            StreamWriter sw = new StreamWriter(file, false, encoding);
            try
            {
                String content = Render(values);
                sw.Write(content);
            }
            catch (Exception)
            {
                sw.Close();
                throw;
            }
            sw.Close();
        }

        /// <summary>
        /// ��ָ����ֵ�����ı��������浽�ļ���
        /// </summary>
        /// <param name="file">Ҫ������ļ�·��</param>
        /// <param name="encoding">�ļ��ı���</param>
        /// <param name="args">����־��Ӧ��ֵ(���Ա�־������һ����־��Ӧ��һ���������Դ�����)</param>
        public void SaveAs(string file, Encoding encoding, params object[] args)
        {
            StreamWriter sw = new StreamWriter(file, false, encoding);
            try
            {
                String content = Render(args);
                sw.Write(content);
            }
            catch (Exception)
            {
                sw.Close();
                throw;
            }
            sw.Close();
        }

        /// <summary>
        /// ��ģ����ָ���ķָ���־�ָ���Сģ��
        /// </summary>
        /// <param name="splitTag"></param>
        /// <returns></returns>
        public TextTemplate[] Split(string splitTag)
        {
            List<TextTemplate> temps = new List<TextTemplate>();
            List<string> contentParts = new List<string>();
            List<TextTemplateTag> tags = new List<TextTemplateTag>();
            int i = 0;
            foreach (string content in _contentParts)
            {
                contentParts.Add(content);
                if (i>=_tags.Length || _tags[i].Name == splitTag)
                {
                    TextTemplate newTemp = new TextTemplate();
                    newTemp._contentParts = contentParts.ToArray();
                    newTemp._tags = tags.ToArray();
                    newTemp._tagCount = tags.Count;
                    temps.Add(newTemp);
                    contentParts.Clear();
                    tags.Clear();
                }
                else
                    tags.Add(new TextTemplateTag(_tags[i].Name, _tags[i].Position, _tags[i].Length));
                i++;
            }
            return temps.ToArray();
        }
    }

    internal class TextTemplateTag
    {
        int _position, _length;
        string _name;

        public TextTemplateTag(string name, int pos, int len)
        {
            _name = name;
            _position = pos;
            _length = len;
        }

        public string Name
        {
            get { return _name; }
        }

        public int Position
        {
            get { return _position; }
        }

        public int Length
        {
            get { return _length; }
        }
    }
}