using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace ADB.Factories
{
    /// <summary>
    /// 表示html页面中的文本，提供自动转换特殊字符的方法
    /// </summary>
    public class HtmlText
    {
        StringBuilder _content = new StringBuilder();

        /// <summary>
        /// 初始化HtmlText实例
        /// </summary>
        public HtmlText()
        {
        }

        /// <summary>
        /// 用指定的文本初始化HtmlText实例
        /// </summary>
        /// <param name="text">用于初始化HtmlText实例的字符串</param>
        public HtmlText(string text)
        {
            Append(text);
        }

        /// <summary>
        /// 添加文本，并转换特殊字符
        /// </summary>
        /// <param name="text">要添加的文本</param>
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
        /// 添加文本，不转换特殊字符
        /// </summary>
        /// <param name="text">要添加的文本</param>
        public void AppendPre(string text)
        {
            _content.Append(text);
        }

        /// <summary>
        /// 添加文本，转换特殊字符，消除上下空行和左空白
        /// </summary>
        /// <param name="para">要追加的文本</param>
        public void AppendParagraphAndTrim(string para)
        {
            AppendParagraphAndTrim(para, -1);
        }

        /// <summary>
        /// 添加文本，转换特殊字符，消除上下空行和左空白
        /// </summary>
        /// <param name="para">要追加的文本</param>
        /// <param name="trimLength">消除左空白的长度</param>
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
        /// 将该转换成对应的字符串
        /// </summary>
        /// <returns>一个字符串，表示该实例对应的字符串</returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        /// <summary>
        /// 获取此实例的长度。
        /// </summary>
        public int Length
        {
            get { return _content.Length; }
        }

        /// <summary>
        /// 转换指定文本特殊字符，并消除上下空行和左空白
        /// </summary>
        /// <param name="para">待转换的字符串</param>
        /// <param name="trimLength">消除左空白的长度</param>
        /// <returns>转换后的字符串</returns>
        static public string TrimParagraph(string para, int trimLength)
        {
            if (string.IsNullOrEmpty(para)) return para;
            HtmlText ret = new HtmlText();
            ret.AppendParagraphAndTrim(para, trimLength);
            return ret.ToString();
        }

        /// <summary>
        /// 转换指定文本中的特殊字符
        /// </summary>
        /// <param name="text">待转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        static public string HtmlFormat(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return new HtmlText(text).ToString();
        }
    }
}
