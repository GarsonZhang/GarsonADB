using System;
using System.Collections.Generic;
using System.Text;
using ADB.Factories;
using Microsoft.VisualBasic.FileIO;

namespace ADB
{
    /// <summary>
    /// 示例文档生成器，支持&lt;image&gt;标志
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
                            //将图片拷贝到生成页面的目录中（通过属性HtmlFileDirectory获取保存页面的目录）
                            FileSystem.CopyFile(xmlFile + "\\" + src, HtmlFileDirectory + "\\" + src, true);
                        }
                        finally
                        {
                        }
                        //生成HTML标志
                        tag.AppendFormat("<img src='{0}'/>", src);
                    }
                    return tag.ToString();
                }
            default:
                {
                    //其它标志由基类处理
                    return base.GetTag(elem, xmlFile);
                }
            }
        }
    }
}
