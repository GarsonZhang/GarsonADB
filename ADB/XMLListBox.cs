using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using ADB.Factories;
using System.IO;
using System.Reflection;

namespace ADB
{
    using Properties;

    public partial class XMLListBox : ListBox
    {
        private Hashtable _xmlFiles = new Hashtable();

        public XMLListBox()
        {
        }

        public bool Exists(string fileName)
        {
            return _xmlFiles.ContainsKey(fileName);
        }

        public void Add(string fileName)
        {
            string key = fileName.ToUpper();
            if (!_xmlFiles.ContainsKey(key))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                try
                {
                    Hashtable xmlFile = new Hashtable();
                    foreach (XmlElement elem in doc.GetElementsByTagName("member"))
                    {
                        string name = elem.GetAttribute("name");
                        int count = elem.GetElementsByTagName("typeparam").Count;
                        if (count > 0)
                        {
                            //泛型方法处理
                            name = name.Replace("``" + elem.GetElementsByTagName("typeparam").Count, "");
                            name = name.Replace("``", "`") + "``";
                        }
                        xmlFile.Add(name, elem);
                    }
                    _xmlFiles.Add(key, xmlFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, Resources.strError);
                    throw;
                }
                this.Items.Add(new ListItemWithTag(Path.GetFileNameWithoutExtension(fileName), fileName));
            }
        }

        //泛型方法生成的XML中会多
        public void GetMemberName(string name)
        {
            //``1
        }

        public void DeleteSelectItem()
        {
            if (SelectedItem != null)
            {
                string xmlFile = (SelectedItem as ListItemWithTag).Tag as string;
                _xmlFiles.Remove(xmlFile);
                Items.RemoveAt(SelectedIndex);
            }
        }

        public MemberXmlElement GetMemberXmlNode(MemberInfo memInfo)
        {
            string name = DocumentBuilderUtility.GetMemberID(memInfo);
            var method = memInfo as MethodInfo;

            if (method != null && method.IsGenericMethod)
                name += "``";
            foreach (DictionaryEntry ent in _xmlFiles)
            {
                XmlElement node = (ent.Value as Hashtable)[name] as XmlElement;
                if (node != null)
                    return new MemberXmlElement((string)ent.Key, node);
            }
            return null;
        }

        class ListItemWithTag
        {
            string _text;
            object _tag;

            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }

            public object Tag
            {
                get { return _tag; }
                set { _tag = value; }
            }

            public ListItemWithTag(string text, object tag)
            {
                _tag = tag;
                _text = text;
            }

            public override string ToString()
            {
                return _text;
            }
        }
    }
}
