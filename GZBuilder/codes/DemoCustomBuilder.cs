using System;
using System.Collections.Generic;
using System.Text;
using ADB.Factories;
using Microsoft.VisualBasic.FileIO;

namespace ADB
{
    /// <summary>
    /// ʾ���ĵ���������֧��&lt;image&gt;��־
    /// </summary>
    public class DemoCustomBuilder : ADB.Factories.MSDNStyleCHMDocumentBuilder
    {
        public DemoCustomBuilder(IGetData data, IInteract interact)
            : base(data, interact)
        {
        }

        protected override string GetTag(System.Xml.XmlElement elem, string xmlFile)
        {
            switch (elem.Name)
            {
            case "image":
                {
                    StringBuilder tag = new StringBuilder();
                    string src = elem.GetAttribute("src");
                    if (!string.IsNullOrEmpty(src))
                    {
                        try
                        {
                            //��ͼƬ����������ҳ���Ŀ¼�У�ͨ������HtmlFileDirectory��ȡ����ҳ���Ŀ¼��
                            FileSystem.CopyFile(xmlFile + "\\" + src, HtmlFileDirectory + "\\" + src, true);
                        }
                        finally
                        {
                        }
                        //����HTML��־
                        tag.AppendFormat("<img src='{0}'/>", src);
                    }
                    return tag.ToString();
                }
            default:
                {
                    //������־�ɻ��ദ��
                    return base.GetTag(elem, xmlFile);
                }
            }
        }
    }
}
