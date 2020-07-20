using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Common;
using Charlotte.Tools;
using Charlotte.Tests;
using Charlotte.MovieGens;

namespace Charlotte
{
	public class Program2
	{
		public void Main2()
		{
			try
			{
				Main3();
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
		}

		private void Main3()
		{
			DDAdditionalEvents.Ground_INIT = () =>
			{
				//DDGround.RO_MouseDispMode = true;
			};

			DDAdditionalEvents.PostGameStart = () =>
			{
				// Font >

				//DDFontRegister.Add(@"Font\Genkai-Mincho-font\genkai-mincho.ttf");

				// < Font

				Ground.I = new Ground();
			};

			DDAdditionalEvents.Save = lines =>
			{
				lines.Add(DateTime.Now.ToString()); // Dummy
				lines.Add(DateTime.Now.ToString()); // Dummy
				lines.Add(DateTime.Now.ToString()); // Dummy

				// 新しい項目をここへ追加...
			};

			DDAdditionalEvents.Load = lines =>
			{
				int c = 0;

				DDUtils.Noop(lines[c++]); // Dummy
				DDUtils.Noop(lines[c++]); // Dummy
				DDUtils.Noop(lines[c++]); // Dummy

				// 新しい項目をここへ追加...
			};

			DDMain2.Perform(Main4);
		}

		private void Main4()
		{
			if (ProcMain.ArgsReader.ArgIs("/D"))
			{
				Main4_Debug();
			}
			else
			{
				Main4_Release();
			}
		}

		private void Main4_Debug()
		{
			this.Main4_Release();
		}

		private void Main4_Release()
		{
			//new Test0001().Test01();
			Main4_01(@"C:\temp\a1001", @"C:\temp\a2001");
			Main4_01(@"C:\temp\a1002", @"C:\temp\a2002");
		}

		private void Main4_01(string rDir, string wDir)
		{
			FileTools.Delete(wDir);
			FileTools.CreateDir(wDir);

			//this.Main4_01_A(rDir, wDir); // 時間掛かりすぎ！
			//this.Main4_01_B(rDir, wDir);
			this.Main4_02(rDir, wDir);
		}

		private void Main4_01_A(string rDir, string wDir)
		{
			foreach (int spBarNum in new int[] { 15, 20, 25, 30 })
			{
				foreach (int spBarWidth in new int[] { 10, 12, 14, 16 })
				{
					foreach (int spBarHeight in new int[] { 200, 230, 260, 290 })
					{
						foreach (I3Color spBarColor in new I3Color[]
						{
							new I3Color(200, 200, 255),
							new I3Color(200, 255, 255),
							new I3Color(200, 255, 200),
							new I3Color(255, 200, 200),
						})
						{
							foreach (double spBarAlpha in new double[] { 0.4, 0.6, 0.8, 1.0 })
							{
								new MovieGen0001().Main01(rDir, wDir, spBarNum, spBarWidth, spBarHeight, spBarColor, spBarAlpha);
							}
						}
					}
				}
			}
		}

		private void Main4_01_B(string rDir, string wDir) // 2020.7.18 ver
		{
			int[] spBarNums = new int[] { 15, 20, 25, 30 };
			int[] spBarWidths = new int[] { 10, 12, 14, 16 };
			int[] spBarHeights = new int[] { 200, 230, 260, 290 };
			I3Color[] spBarColors = new I3Color[]
			{
				new I3Color(200, 200, 255),
				new I3Color(200, 255, 255),
				new I3Color(200, 255, 200),
				new I3Color(255, 200, 200),
			};
			double[] spBarAlphas = new double[] { 0.4, 0.6, 0.8, 1.0 };

			new MovieGen0001().Main01(rDir, wDir, spBarNums[1], spBarWidths[1], spBarHeights[1], spBarColors[1], spBarAlphas[1]);

			for (int x = 0; x < 4; x++)
			{
				if (x == 1)
					continue;

				new MovieGen0001().Main01(rDir, wDir, spBarNums[x], spBarWidths[1], spBarHeights[1], spBarColors[1], spBarAlphas[1]);
				new MovieGen0001().Main01(rDir, wDir, spBarNums[1], spBarWidths[x], spBarHeights[1], spBarColors[1], spBarAlphas[1]);
				new MovieGen0001().Main01(rDir, wDir, spBarNums[1], spBarWidths[1], spBarHeights[x], spBarColors[1], spBarAlphas[1]);
				new MovieGen0001().Main01(rDir, wDir, spBarNums[1], spBarWidths[1], spBarHeights[1], spBarColors[x], spBarAlphas[1]);
				new MovieGen0001().Main01(rDir, wDir, spBarNums[1], spBarWidths[1], spBarHeights[1], spBarColors[1], spBarAlphas[x]);
			}
		}

		private void Main4_02(string rDir, string wDir) // 2020.7.19 ver
		{
			new MovieGen0001().Main01(
				rDir,
				wDir,
				20,
				12,
				230,
				new I3Color(0, 255, 255),
				0.6,
				3.0
				);
		}
	}
}
