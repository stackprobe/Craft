using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class DDFontUtils
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static List<DDFont> Fonts = new List<DDFont>();

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void Add(DDFont font)
		{
			Fonts.Add(font);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void UnloadAll()
		{
			foreach (DDFont font in Fonts)
				font.Unload();
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static DDFont GetFont(string fontName, int fontSize, int fontThick = 6, bool antiAliasing = true, int edgeSize = 0, bool italicFlag = false)
		{
			DDFont font = Fonts.FirstOrDefault(v =>
				v.FontName == fontName &&
				v.FontSize == fontSize &&
				v.FontThick == fontThick &&
				v.AntiAliasing == antiAliasing &&
				v.EdgeSize == edgeSize &&
				v.ItalicFlag == italicFlag
				);

			if (font == null)
				font = new DDFont(fontName, fontSize, fontThick, antiAliasing, edgeSize, italicFlag);

			return font;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawString(int x, int y, string str, DDFont font, bool tategakiFlag = false)
		{
			DrawString(x, y, str, font, tategakiFlag, new I3Color(255, 255, 255));
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawString(int x, int y, string str, DDFont font, bool tategakiFlag, I3Color color)
		{
			DrawString(x, y, str, font, tategakiFlag, color, new I3Color(0, 0, 0));
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawString(int x, int y, string str, DDFont font, bool tategakiFlag, I3Color color, I3Color edgeColor)
		{
			DX.DrawStringToHandle(x, y, str, DDUtils.GetColor(color), font.GetHandle(), DDUtils.GetColor(edgeColor), tategakiFlag ? 1 : 0);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawString_XCenter(int x, int y, string str, DDFont font, bool tategakiFlag = false)
		{
			x -= GetDrawStringWidth(str, font, tategakiFlag) / 2;

			DrawString(x, y, str, font, tategakiFlag);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawString_XCenter(int x, int y, string str, DDFont font, bool tategakiFlag, I3Color color)
		{
			x -= GetDrawStringWidth(str, font, tategakiFlag) / 2;

			DrawString(x, y, str, font, tategakiFlag, color);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static void DrawString_XCenter(int x, int y, string str, DDFont font, bool tategakiFlag, I3Color color, I3Color edgeColor)
		{
			x -= GetDrawStringWidth(str, font, tategakiFlag) / 2;

			DrawString(x, y, str, font, tategakiFlag, color, edgeColor);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static int GetDrawStringWidth(string str, DDFont font, bool tategakiFlag = false)
		{
			return DX.GetDrawStringWidthToHandle(str, StringTools.ENCODING_SJIS.GetByteCount(str), font.GetHandle(), tategakiFlag ? 1 : 0);
		}
	}
}
