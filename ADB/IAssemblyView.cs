using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace ADB
{
	using Factories;
	using Properties;

	interface IAssemblyView
	{
		bool Exists(string path);

		void AddAssambly(Assembly assembly);

		void AddAssamblys(Assembly[] assemblys);

		void DeleteSelectedAssembly();

		void Clear();

		DocumentBuilderMember[] GetTypes();

		DocumentBuilderMember[] GetTypeMembers(Type type);

		Type[] GetSelectedTypes();

		MemberInfo[] GetTypeSelectedMembers(Type type);

		void QuickSelect();

		Assembly[] GetAssemblys();
		
	}
}
