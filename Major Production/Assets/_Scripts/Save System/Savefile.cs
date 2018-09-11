using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace Assets._Scripts.Save_System
{
	class Savefile
	{
		FileInfo file;

		public string Name { get { return file.Name; } }
		public DateTime LastSave { get { return file.LastWriteTime; } }

		public Savefile(string filepath)
		{

		}

		private Savefile(FileInfo file)
		{

		}

		public static Savefile Create()
		{
			// TODO figure out what to pass this (JObject?)
			throw new System.NotImplementedException();
		}

		public void Save(JObject saveData)
		{
			// write into save file
			throw new System.NotImplementedException();
		}

		public JObject Load()
		{
			// read from file
			throw new System.NotImplementedException();
		}
	}
}
