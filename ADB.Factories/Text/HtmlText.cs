using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace ADB.Factories
{
    /// <summary>
    /// ��ʾhtmlҳ���е��ı����ṩ�Զ�ת�������ַ��ķ���
    /// </summary>
    public class HtmlText
    {
        StringBuilder _content = new StringBuilder();

        /// <summary>
        /// ��ʼ��HtmlTextʵ��
        /// </summary>
        public HtmlText()
        {
        }

        /// <summary>
        /// ��ָ�����ı���ʼ��HtmlTextʵ��
        /// </summary>
        /// <param name="text">���ڳ�ʼ��HtmlTextʵ�����ַ���</param>
        public HtmlText(string text)
        {
            Append(text);
        }

        /// <summary>
        /// ����ı�����ת�������ַ�
        /// </summary>
        /// <param name="text">Ҫ��ӵ��ı�</param>
        public void Append(string text)
        {
            foreach (char c in text)
            {
                switch (c)
                {
                case '\r':
                    break;
                case '\n':
                    _content.Append("<br/>");
                    break;
                case '<':
                    _content.Append("&lt;");
                    break;
                case '>':
                    _content.Append("&gt;");
                    break;
                default:
                    _content.Append(c);
                    break;
                }
            }
        }

        /// <summary>
        /// ����ı�����ת�������ַ�
        /// </summary>
        /// <param name="text">Ҫ��ӵ��ı�</param>
        public void AppendPre(string text)
        {
            _content.Append(text);
        }

        /// <summary>
        /// ����ı���ת�������ַ����������¿��к���հ�
        /// </summary>
        /// <param name="para">Ҫ׷�ӵ��ı�</param>
        public void AppendParagraphAndTrim(string para)
        {
            AppendParagraphAndTrim(para, -1);
        }

        /// <summary>
        /// ����ı���ת�������ַ����������¿��к���հ�
        /// </summary>
        /// <param name="para">Ҫ׷�ӵ��ı�</param>
        /// <param name="trimLength">������հ׵ĳ���</param>
        public void AppendParagraphAndTrim(string para, int trimLength)
        {
            if (!string.IsNullOrEmpty(para))
            {
                string[] lines = para.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                if (trimLength < 0)
                {
                    Regex reg = new Regex("[^ ]");
                    trimLength = para.Length;
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            Match m = reg.Match(line);
                            if (!m.Success)
                            {
                                if (line.Length < trimLength) trimLength = line.Length;
                            }
                            else
                            {
                                if (m.Index < trimLength) trimLength = m.Index;
                            }
                            if (trimLength == 0) break;
                        }
                    }
                }
                if (trimLength != 0)
                {
                    int sl, el;
                    for (sl = 0; sl < lines.Length && string.IsNullOrEmpty(lines[sl]); sl++) ;
                    for (el = lines.Length - 1; el >= sl && string.IsNullOrEmpty(lines[el]); el--) ;

                    for (; sl <= el;sl++ )
                    {
                        if (!string.IsNullOrEmpty(lines[sl]))
                            Append(lines[sl].Substring(trimLength, lines[sl].Length - trimLength));
                        if (sl < el) AppendPre("<br/>");
                    }
                }
                else
                    Append(para);
            }
        }

        /// <summary>
        /// ����ת���ɶ�Ӧ���ַ���
        /// </summary>
        /// <returns>һ���ַ�������ʾ��ʵ����Ӧ���ַ���</returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        /// <summary>
        /// ��ȡ��ʵ���ĳ��ȡ�
        /// </summary>
        public int Length
        {
            get { return _content.Length; }
        }

        /// <summary>
        /// ת��ָ���ı������ַ������������¿��к���հ�
        /// </summary>
        /// <param name="para">��ת�����ַ���</param>
        /// <param name="trimLength">������հ׵ĳ���</param>
        /// <returns>ת������ַ���</returns>
        static public string TrimParagraph(string para, int trimLength)
        {
            if (string.IsNullOrEmpty(para)) return para;
            HtmlText ret = new HtmlText();
            ret.AppendParagraphAndTrim(para, trimLength);
            return ret.ToString();
        }

        /// <summary>
        /// ת��ָ���ı��е������ַ�
        /// </summary>
        /// <param name="text">��ת�����ַ���</param>
        /// <returns>ת������ַ���</returns>
        static public string HtmlFormat(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return new HtmlText(text).ToString();
        }
    }
}
