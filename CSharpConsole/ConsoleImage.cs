using System;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using c = System.Console;

namespace StrubT.PlayGround.CSharpConsole {

	public class ConsoleImage : IRunnable {

		public bool Active => false;

		public void Run() {

			var uris = new[] {
				new Uri("http://strubt.ch/images/ThS.png"),
				new Uri("http://image.tmdb.org/t/p/original/gQLvz6eJbRB9qFj0mUcF5IaKAXw.jpg"),
				new Uri("http://image.tmdb.org/t/p/original/fqCZZb33w8RhWl2GVGdWOqXBQNO.jpg"),
				new Uri("http://image.tmdb.org/t/p/original/gFDjZje8P0S3MzHuw1cxiYuHBX5.jpg"),
			};

			var backgroundColor = c.BackgroundColor;
			var foregroundColor = c.ForegroundColor;

			c.ForegroundColor = ConsoleColor.Red;
			c.Write("please maximise your window!");
			c.ReadLine();

			c.ForegroundColor = foregroundColor;

			//c.SetWindowPosition(0, 0);
			var consoleWidth = c.WindowWidth /* = c.LargestWindowWidth*/;
			var consoleHeight = c.WindowHeight /* = c.LargestWindowHeight*/;

			var consoleFont = GetCurrentConsoleFont();
			var consoleFontScale = (int)Math.Round((double)consoleFont.Height / consoleFont.Width);

			foreach (var uri in uris) {
				Image image;
				using (var webClient = new WebClient())
				using (var imageStream = webClient.OpenRead(uri))
					image = Image.FromStream(imageStream);

				var size = new Size(image.Width * consoleFontScale, image.Height);
				if (size.Width > consoleWidth) size = new Size(consoleWidth, size.Height * consoleWidth / size.Width);
				if (size.Height > consoleHeight) size = new Size(size.Width * consoleHeight / size.Height, consoleHeight);

				var bitmap = new Bitmap(image, size);

				for (int y = 0; y < size.Height; y++) {
					for (int x = 0; x < size.Width; x++) {
						var color = bitmap.GetPixel(x, y);
						var consoleColor = GetConsoleColor(color);

						c.BackgroundColor = consoleColor;
						c.Write(' ');
					}
					c.WriteLine();
				}
			}

			c.BackgroundColor = backgroundColor;
		}

		#region get console font information
		//source: http://blogs.microsoft.co.il/pavely/2009/07/23/changing-console-fonts/
		//source: http://www.pinvoke.net/default.aspx/kernel32.getcurrentconsolefont

		[StructLayout(LayoutKind.Sequential)]
		private struct ConsoleFont {

			public uint Index;
			public short Width, Height;
		}

		private enum StdHandle {

			OutputHandle = -11
		}

		[DllImport("kernel32")]
		private static extern IntPtr GetStdHandle(StdHandle index);

		[DllImport("kernel32")]
		private static extern bool GetCurrentConsoleFont(IntPtr hConsoleOutput, bool bMaximumWindow, out ConsoleFont currentFont);

		private static ConsoleFont GetCurrentConsoleFont() {

			ConsoleFont currentFont;
			GetCurrentConsoleFont(GetStdHandle(StdHandle.OutputHandle), false, out currentFont);
			return currentFont;
		}
		#endregion

		#region convert Color to ConsoleColor
		//source: http://stackoverflow.com/a/25229498

		private ConsoleColor GetConsoleColor(Color color) {

			if (color.GetSaturation() < 0.5)
				switch ((int)(color.GetBrightness() * 3.5)) { //grayish color
					case 0: return ConsoleColor.Black;
					case 1: return ConsoleColor.DarkGray;
					case 2: return ConsoleColor.Gray;
					default: return ConsoleColor.White;
				}

			int hue = (int)Math.Round(color.GetHue() / 60, MidpointRounding.AwayFromZero);
			if (color.GetBrightness() < 0.4)

				switch (hue) { //dark color
					case 1: return ConsoleColor.DarkYellow;
					case 2: return ConsoleColor.DarkGreen;
					case 3: return ConsoleColor.DarkCyan;
					case 4: return ConsoleColor.DarkBlue;
					case 5: return ConsoleColor.DarkMagenta;
					default: return ConsoleColor.DarkRed;
				}

			switch (hue) { //bright color
				case 1: return ConsoleColor.Yellow;
				case 2: return ConsoleColor.Green;
				case 3: return ConsoleColor.Cyan;
				case 4: return ConsoleColor.Blue;
				case 5: return ConsoleColor.Magenta;
				default: return ConsoleColor.Red;
			}
		}
		#endregion
	}
}
