using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDResource
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static bool ReleaseMode;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private class ResInfo
		{
			public long Offset;
			public int Size;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static Dictionary<string, ResInfo> File2ResInfo = DictionaryTools.CreateIgnoreCase<ResInfo>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void INIT()
		{
			ReleaseMode = File.Exists(DDConsts.ResourceFile);

			if (ReleaseMode)
			{
				List<ResInfo> resInfos = new List<ResInfo>();

				using (FileStream reader = new FileStream(DDConsts.ResourceFile, FileMode.Open, FileAccess.Read))
				{
					while (reader.Position < reader.Length)
					{
						int size = BinTools.ToInt(FileTools.Read(reader, 4));

						if (size < 0)
							throw new DDError();

						resInfos.Add(new ResInfo()
						{
							Offset = reader.Position,
							Size = size,
						});

						reader.Seek((long)size, SeekOrigin.Current);
					}
				}
				string[] files = FileTools.TextToLines(StringTools.ENCODING_SJIS.GetString(LoadFile(resInfos[0])));

				if (files.Length != resInfos.Count)
					throw new DDError(files.Length + ", " + resInfos.Count);

				for (int index = 0; index < files.Length; index++)
					File2ResInfo.Add(files[index], resInfos[index]);
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static byte[] LoadFile(long offset, int size)
		{
			using (FileStream reader = new FileStream(DDConsts.ResourceFile, FileMode.Open, FileAccess.Read))
			{
				reader.Seek(offset, SeekOrigin.Begin);

				return DDJammer.Decode(FileTools.Read(reader, size));
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private static byte[] LoadFile(ResInfo resInfo)
		{
			return LoadFile(resInfo.Offset, resInfo.Size);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static byte[] Load(string file)
		{
			if (ReleaseMode)
			{
				return LoadFile(File2ResInfo[file]);
			}
			else
			{
				return File.ReadAllBytes(Path.Combine(DDConsts.ResourceDir, file));
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Save(string file, byte[] fileData)
		{
			if (ReleaseMode)
			{
				throw new DDError();
			}
			else
			{
				File.WriteAllBytes(Path.Combine(DDConsts.ResourceDir, file), fileData);
			}
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		/// <summary>
		/// <para>ファイルリストを取得する。</para>
		/// <para>ソート済み</para>
		/// <para>'_' で始まるファイルの除去済み</para>
		/// </summary>
		/// <returns>ファイルリスト</returns>
		public static IEnumerable<string> GetFiles()
		{
			IEnumerable<string> files;

			if (ReleaseMode)
				files = File2ResInfo.Keys;
			else
				files = Directory.GetFiles(DDConsts.ResourceDir, "*", SearchOption.AllDirectories).Select(file => FileTools.ChangeRoot(file, DDConsts.ResourceDir));

			// '_' で始まるファイルの除去
			// makeDDResourceFile はファイルリストを _index として保存するので、どちらの場合でも Where を行わなければならない。
			files = files.Where(file => Path.GetFileName(file)[0] != '_');

			// ソート
			// makeDDResourceFile はファイルリストを sortJLinesICase してる。
			files = EnumerableTools.Sort(files, StringTools.CompIgnoreCase);

			return files;
		}
	}
}
