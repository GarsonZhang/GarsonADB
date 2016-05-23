using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using ADB.Factories;
using System.Xml;

namespace ADB.Builders
{
    public class DefaultBuilder:MSDNStyleCHMDocumentBuilder
    {
        public DefaultBuilder(IGetData data, IInteract interact)
            : base(data, interact)
        {
        }

        protected override string GetTag(XmlElement elem, string xmlFile)
        {
            switch (elem.Name)
            {
            case "img":
                {
                    StringBuilder tag = new StringBuilder();
                    string src = elem.GetAttribute("src");
                    if (!string.IsNullOrEmpty(src))
                    {
                        try
                        {
                            FileSystem.CopyFile(xmlFile + "\\" + src, HtmlFileDirectory + "\\" + src, false);
                        }
                        catch
                        {
                        }
                        tag.Append(elem.OuterXml);
                    }
                    return tag.ToString();
                }
            case "file":
                {
                    StringBuilder tag = new StringBuilder();
                    string src = elem.GetAttribute("src");
                    if (!string.IsNullOrEmpty(src))
                    {
                        try
                        {
                            FileSystem.CopyFile(xmlFile + "\\" + src, HtmlFileDirectory + "\\" + src, false);
                        }
                        catch
                        {
                        }
                        tag.AppendFormat("<a href='{0}'>{1}</a>", src, GetInnerTags(elem, xmlFile));
                    }
                    return tag.ToString();
                }
            case "localize":
                {
                    XmlNodeList localElems = elem.GetElementsByTagName(System.Threading.Thread.CurrentThread.CurrentCulture.Parent.Name);
                    if (localElems.Count > 0)
                    {
                        return GetInnerTags(localElems[0] as XmlElement, xmlFile);
                    }
                    else
                    {
                        if (elem.ChildNodes.Count > 0)
                            return GetInnerTags(elem.ChildNodes[0] as XmlElement, xmlFile);
                        else
                            return "";
                    }
                }
            default:
                {
                    return base.GetTag(elem, xmlFile);
                }
            }
        }
    }
}
