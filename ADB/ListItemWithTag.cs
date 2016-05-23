using System;
using System.Collections.Generic;
using System.Text;

namespace ADB
{
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
