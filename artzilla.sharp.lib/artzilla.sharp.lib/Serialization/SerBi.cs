﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ArtZilla.Sharp.Lib.Extensions;

namespace ArtZilla.Sharp.Lib.Serialization {
	public class SerBi {
		public static List<T> Load<T>(string file, bool clear = false) where T : class {
			var serializator = new BinaryFormatter();
			var res = new List<T>();

			try {
				if (!File.Exists(file))
					return res;

				using (var fs = new FileStream(file, FileMode.Open)) {
					if (fs.Length == 0) return res;

					do {
						var o = serializator.Deserialize(fs) as T;
						if (o != null) res.Add(o);
					} while (fs.Position < fs.Length);

					if (clear) fs.SetLength(0);
				}
			} catch (Exception ex) {
				// ignored
			}

			return res;
		}

		public static bool Save<T>(string file, IEnumerable<T> items, bool append = false) where T : class {
			var serializator = new BinaryFormatter();
			CreateIfNotExist(file);

			try {
				using (var fs = new FileStream(file, append ? FileMode.Append : FileMode.Create))
					foreach (var item in items)
						serializator.Serialize(fs, item);

				return true;
			} catch (Exception ex) {
				return false;
			}
		}

		public static bool Save<T>(string file, T item, bool append = false) where T : class {
			var serializator = new BinaryFormatter();
			CreateIfNotExist(file);

			try {
				using (var fs = new FileStream(file, append ? FileMode.Append : FileMode.Create))
					serializator.Serialize(fs, item);

				return true;
			} catch (Exception ex) {
				return false;
			}
		}

		private static void CreateIfNotExist(string file) {
			try {
				if (File.Exists(file))
					return;

				var path = Path.GetDirectoryName(file);
				if (path.IsBad())
					return;

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
			} catch (Exception) {
				// ignored
			}
		}
	}
}
