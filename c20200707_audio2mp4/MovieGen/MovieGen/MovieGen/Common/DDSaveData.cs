using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDSaveData
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Save()
		{
			List<byte[]> blocks = new List<byte[]>();

			// for Donut2
			{
				List<string> lines = new List<string>();

				lines.Add(Program.APP_IDENT);
				lines.Add(Program.APP_TITLE);

				lines.Add("" + DDGround.RealScreen_W);
				lines.Add("" + DDGround.RealScreen_H);

				lines.Add("" + DDGround.RealScreenDraw_L);
				lines.Add("" + DDGround.RealScreenDraw_T);
				lines.Add("" + DDGround.RealScreenDraw_W);
				lines.Add("" + DDGround.RealScreenDraw_H);

				lines.Add("" + DoubleTools.ToLong(DDGround.MusicVolume * IntTools.IMAX));
				lines.Add("" + DoubleTools.ToLong(DDGround.SEVolume * IntTools.IMAX));

				lines.Add("" + DDInput.DIR_2.BtnId);
				lines.Add("" + DDInput.DIR_4.BtnId);
				lines.Add("" + DDInput.DIR_6.BtnId);
				lines.Add("" + DDInput.DIR_8.BtnId);
				lines.Add("" + DDInput.A.BtnId);
				lines.Add("" + DDInput.B.BtnId);
				lines.Add("" + DDInput.C.BtnId);
				lines.Add("" + DDInput.D.BtnId);
				lines.Add("" + DDInput.E.BtnId);
				lines.Add("" + DDInput.F.BtnId);
				lines.Add("" + DDInput.L.BtnId);
				lines.Add("" + DDInput.R.BtnId);
				lines.Add("" + DDInput.PAUSE.BtnId);
				lines.Add("" + DDInput.START.BtnId);

				lines.Add("" + DDInput.DIR_2.KeyId);
				lines.Add("" + DDInput.DIR_4.KeyId);
				lines.Add("" + DDInput.DIR_6.KeyId);
				lines.Add("" + DDInput.DIR_8.KeyId);
				lines.Add("" + DDInput.A.KeyId);
				lines.Add("" + DDInput.B.KeyId);
				lines.Add("" + DDInput.C.KeyId);
				lines.Add("" + DDInput.D.KeyId);
				lines.Add("" + DDInput.E.KeyId);
				lines.Add("" + DDInput.F.KeyId);
				lines.Add("" + DDInput.L.KeyId);
				lines.Add("" + DDInput.R.KeyId);
				lines.Add("" + DDInput.PAUSE.KeyId);
				lines.Add("" + DDInput.START.KeyId);

				lines.Add("" + (DDGround.RO_MouseDispMode ? 1 : 0));

				// 新しい項目をここへ追加...

				blocks.Add(DDUtils.SplitableJoin(lines.ToArray()));
			}

			// for app
			{
				List<string> lines = new List<string>();

				DDAdditionalEvents.Save(lines);

				blocks.Add(DDUtils.SplitableJoin(lines.ToArray()));
			}

			File.WriteAllBytes(DDConsts.SaveDataFile, DDJammer.Encode(BinTools.SplittableJoin(blocks.ToArray())));
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Load()
		{
			if (File.Exists(DDConsts.SaveDataFile) == false)
				return;

			byte[][] blocks = BinTools.Split(DDJammer.Decode(File.ReadAllBytes(DDConsts.SaveDataFile)));
			int bc = 0;

			string[] lines = DDUtils.Split(blocks[bc++]);
			int c = 0;

			if (lines[c++] != Program.APP_IDENT)
				throw new DDError();

			if (lines[c++] != Program.APP_TITLE)
				throw new DDError();

			// 項目が増えた場合を想定して try ～ catch しておく。

			try // for Donut2
			{
				// TODO int.Parse -> IntTools.ToInt

				DDGround.RealScreen_W = int.Parse(lines[c++]);
				DDGround.RealScreen_H = int.Parse(lines[c++]);

				DDGround.RealScreenDraw_L = int.Parse(lines[c++]);
				DDGround.RealScreenDraw_T = int.Parse(lines[c++]);
				DDGround.RealScreenDraw_W = int.Parse(lines[c++]);
				DDGround.RealScreenDraw_H = int.Parse(lines[c++]);

				DDGround.MusicVolume = long.Parse(lines[c++]) / (double)IntTools.IMAX;
				DDGround.SEVolume = long.Parse(lines[c++]) / (double)IntTools.IMAX;

				DDInput.DIR_2.BtnId = int.Parse(lines[c++]);
				DDInput.DIR_4.BtnId = int.Parse(lines[c++]);
				DDInput.DIR_6.BtnId = int.Parse(lines[c++]);
				DDInput.DIR_8.BtnId = int.Parse(lines[c++]);
				DDInput.A.BtnId = int.Parse(lines[c++]);
				DDInput.B.BtnId = int.Parse(lines[c++]);
				DDInput.C.BtnId = int.Parse(lines[c++]);
				DDInput.D.BtnId = int.Parse(lines[c++]);
				DDInput.E.BtnId = int.Parse(lines[c++]);
				DDInput.F.BtnId = int.Parse(lines[c++]);
				DDInput.L.BtnId = int.Parse(lines[c++]);
				DDInput.R.BtnId = int.Parse(lines[c++]);
				DDInput.PAUSE.BtnId = int.Parse(lines[c++]);
				DDInput.START.BtnId = int.Parse(lines[c++]);

				DDInput.DIR_2.KeyId = int.Parse(lines[c++]);
				DDInput.DIR_4.KeyId = int.Parse(lines[c++]);
				DDInput.DIR_6.KeyId = int.Parse(lines[c++]);
				DDInput.DIR_8.KeyId = int.Parse(lines[c++]);
				DDInput.A.KeyId = int.Parse(lines[c++]);
				DDInput.B.KeyId = int.Parse(lines[c++]);
				DDInput.C.KeyId = int.Parse(lines[c++]);
				DDInput.D.KeyId = int.Parse(lines[c++]);
				DDInput.E.KeyId = int.Parse(lines[c++]);
				DDInput.F.KeyId = int.Parse(lines[c++]);
				DDInput.L.KeyId = int.Parse(lines[c++]);
				DDInput.R.KeyId = int.Parse(lines[c++]);
				DDInput.PAUSE.KeyId = int.Parse(lines[c++]);
				DDInput.START.KeyId = int.Parse(lines[c++]);

				DDGround.RO_MouseDispMode = int.Parse(lines[c++]) != 0;

				// 新しい項目をここへ追加...
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}

			Load_Delay = () =>
			{
				try // for app
				{
					lines = DDUtils.Split(blocks[bc++]);

					DDAdditionalEvents.Load(lines);
				}
				catch (Exception e)
				{
					ProcMain.WriteLog(e);
				}

				Load_Delay = () => { }; // reset
			};
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static Action Load_Delay = () => { };
	}
}
