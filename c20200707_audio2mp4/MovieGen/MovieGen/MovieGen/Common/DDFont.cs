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
	public class DDFont
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public string FontName;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int FontSize;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int FontThick;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public bool AntiAliasing;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int EdgeSize;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public bool ItalicFlag;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		private int Handle = -1; // -1 == Unloaded

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public DDFont(string fontName, int fontSize, int fontThick = 6, bool antiAliasing = true, int edgeSize = 0, bool italicFlag = false)
		{
			if (string.IsNullOrEmpty(fontName)) throw new DDError();
			if (fontSize < 1 || IntTools.IMAX < fontSize) throw new DDError();
			if (fontThick < 1 || IntTools.IMAX < fontThick) throw new DDError();
			// antiAliasing
			if (edgeSize < 0 || IntTools.IMAX < edgeSize) throw new DDError();
			// italicFlag

			this.FontName = fontName;
			this.FontSize = fontSize;
			this.FontThick = fontThick;
			this.AntiAliasing = antiAliasing;
			this.EdgeSize = edgeSize;
			this.ItalicFlag = italicFlag;

			DDFontUtils.Add(this);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public int GetHandle()
		{
			if (this.Handle == -1)
			{
				int fontType = DX.DX_FONTTYPE_NORMAL;

				if (this.AntiAliasing)
					fontType |= DX.DX_FONTTYPE_ANTIALIASING_8X8;

				if (this.EdgeSize != 0)
					fontType |= DX.DX_FONTTYPE_ANTIALIASING_8X8;

				this.Handle = DX.CreateFontToHandle(
					this.FontName,
					this.FontSize,
					this.FontThick,
					fontType,
					-1,
					this.EdgeSize
					);

				if (this.Handle == -1) // ? 失敗
					throw new DDError();
			}
			return this.Handle;
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public void Unload()
		{
			if (this.Handle != -1)
			{
				if (DX.DeleteFontToHandle(this.Handle) != 0) // ? 失敗
					throw new DDError();

				this.Handle = -1;
			}
		}
	}
}
