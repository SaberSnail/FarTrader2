using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GoldenAnvil.Utility;
using AccidentalNoise;

namespace FarTrader.Tools
{
	internal static class StarfieldUtility
	{
		public static ImageSource CreateStarfieldImage(Random rng, Size size)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();

			drawingContext.DrawRectangle(new SolidColorBrush(Colors.Black), null, new Rect(size));

			DrawStars(rng, size, drawingContext);

			drawingContext.Close();

			RenderTargetBitmap bmp = new RenderTargetBitmap((int) size.Width, (int) size.Height, 96, 96, PixelFormats.Pbgra32);
			bmp.Render(drawingVisual);

			return bmp;
		}

		public static ImageSource CreateAccidentalStarfield(Random rng, Size size)
		{
			uint seed = (uint) rng.Next();

			Fractal cloudFractal = new Fractal(FractalType.FBM, BasisTypes.VALUE, InterpTypes.CUBIC, 6, 3, seed);
			//{name="ground_scale",             type="scaleoffset",        scale=0.5, offset=0, source="ground_shape_fractal"},
			//{name="ground_perturb",           type="translatedomain",    source="ground_gradient", ty="ground_scale"},
			AutoCorrect autocorrect = new AutoCorrect(cloudFractal, 0, 1);

			Gradient background = new Gradient(0, 0, 0, 0);
			Fractal stars = new Fractal(FractalType.BILLOW, BasisTypes.WHITE, InterpTypes.NONE, null, null, seed);
			Select starsSelect = new Select(autocorrect, stars, background, 0.5, 0.25);

			return null;//CreateAccidentalNoiseImage(starsSelect, size);
		}

		public static List<ImageSource> CreateAccidentalNoise(Random rng, Size size)
		{
			List<ImageSource> images = new List<ImageSource>();
			uint seed = 1;//(uint) rng.Next();

			Fractal cloudFractal = new Fractal(FractalType.FBM, BasisTypes.VALUE, InterpTypes.CUBIC, 6, 3, seed);
			AutoCorrect autocorrect = new AutoCorrect(cloudFractal, 0, 1);

			Gradient background = new Gradient(0, 1, 0, 1);
			Fractal stars = new Fractal(FractalType.BILLOW, BasisTypes.WHITE, InterpTypes.NONE, null, null, seed);
			Select starsSelect = new Select(autocorrect, stars, background, 0.5, 0);
			Select starsSelect2 = new Select(autocorrect, stars, background, 0.5, 0.25);
			Select starsSelect3 = new Select(autocorrect, stars, background, 0.5, 0.5);
			Select starsSelect4 = new Select(autocorrect, stars, background, 0.5, 0.75);
			Select starsSelect5 = new Select(autocorrect, stars, background, 0.5, 1);
			Select starsSelect6 = new Select(stars, 0.0, 0.5, 0.5, 1);

			images.Add(CreateAccidentalNoiseImage(autocorrect, size));
			images.Add(CreateAccidentalNoiseImage(background, size));
			images.Add(CreateAccidentalNoiseImage(stars, size));
			images.Add(CreateAccidentalNoiseImage(starsSelect, size));
			images.Add(CreateAccidentalNoiseImage(starsSelect2, size));
			images.Add(CreateAccidentalNoiseImage(starsSelect3, size));
			images.Add(CreateAccidentalNoiseImage(starsSelect4, size));
			images.Add(CreateAccidentalNoiseImage(starsSelect5, size));
			images.Add(CreateAccidentalNoiseImage(starsSelect6, size));

			/*
			foreach (BasisTypes basisType in Enum.GetValues(typeof(BasisTypes)))
			{
				if (basisType == BasisTypes.SIMPLEX)
					continue;
				if (basisType != BasisTypes.VALUE)
					continue;
				foreach (FractalType fractalType in Enum.GetValues(typeof(FractalType)))
				{
					//if (fractalType != FractalType.FBM)
					//	continue;
					foreach (InterpTypes interpType in Enum.GetValues(typeof(InterpTypes)))
					{
						if (interpType != InterpTypes.CUBIC)
							continue;
						for (int octaves = 1; octaves <= 8; octaves++)
						{
							if (octaves != 6)
								continue;
							for (int frequency = 1; frequency <= 8; frequency++)
							{
								if (frequency != 2)
									continue;
								Fractal fractal = new Fractal(fractalType, basisType, interpType, octaves, frequency, seed);
								images.Add(CreateAccidentalNoiseImage(new AutoCorrect(fractal, 0, 1), size));
							}
						}
					}
				}
			}
			 * */

			return images;
		}

		private static ImageSource CreateAccidentalNoiseImage(ModuleBase source, Size size)
		{
			int width = (int) size.Width;
			int height = (int) size.Height;

			uint[] pixels = new uint[width * height];
			for (int x = 0; x < size.Width; x++)
			{
				for (int y = 0; y < size.Height; y++)
				{
					double value = source.Get((double) x / (double) width, (double) y / (double) height);
					//value = (value + 1.0) / 2.0;
					int red = (int) (value * 255);
					int green = (int) (value * 255);
					int blue = (int) (value * 255);
					//int alpha = 255;
					int i = width * y + x;
					pixels[i] = (uint) ((red << 16) + (green << 8) + blue);
				}
			}

			WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
			bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
			return bitmap;
		}

		private static void DrawStars(Random rng, Size size, DrawingContext drawingContext)
		{
			List<NebulaInfo> nebula = new List<NebulaInfo>();
			int zOffset = rng.Next();
			int count = (int) (size.Width * size.Height / s_pixelsPerStar);
			double starDensityNoiseScaling = Math.Max(size.Width, size.Height);
			for (int i = 0; i < count; i++)
			{
				int x = rng.Next((int) size.Width);
				int y = rng.Next((int) size.Height);
				double z = rng.NextDouble();
				double check = Math.Abs(RandomUtility.GetPerlinNoise(x / starDensityNoiseScaling, y / starDensityNoiseScaling, z /* + zOffset*/));
				double nebulaStrength = z - (1.0 - check);
				if (nebulaStrength > 0)
					nebula.Add(new NebulaInfo(x, y, (byte) Math.Round((nebulaStrength * 255.0))));
				if (rng.NextDouble() < check)
				{
					byte c = (byte) Math.Round(z * 255.0);
					double r = z;
					if (r > 0.8)
						r = Math.Pow(r + 0.2, 2) - 0.2;
					Brush brush = new SolidColorBrush(new Color { A = 255, R = c, G = c, B = c });
					drawingContext.DrawEllipse(brush, null, new Point(x, y), r, r);
				}
			}

			for (int i = 0; i < nebula.Count; i++)
			{
				NebulaInfo info = nebula[i];
				int nebulaRadius = (int) (starDensityNoiseScaling / 20);
				byte nebulaAlpha = 15;
				double colorFactor = 0.3 + (rng.NextDouble() * 0.7);
				Color color1 = new Color { A = nebulaAlpha, R = (byte) (info.Color * colorFactor), G = (byte) (info.Color * colorFactor), B = info.Color };
				//Color color2 = new Color { A = nebulaAlpha, R = 0, G = 0, B = 0 };
				Color color2 = new Color { A = 0, R = (byte) (info.Color * colorFactor), G = (byte) (info.Color * colorFactor), B = info.Color };
				RadialGradientBrush gradient = new RadialGradientBrush
				{
					GradientStops = new GradientStopCollection(new[] { new GradientStop(color1, 0.0), new GradientStop(color2, 1.0) }),
				};
				drawingContext.DrawEllipse(gradient, null, new Point(info.X, info.Y), nebulaRadius, nebulaRadius);
			}
		}

		private class NebulaInfo
		{
			public NebulaInfo(int x, int y, byte color)
			{
				X = x;
				Y = y;
				Color = color;
			}

			public int X;
			public int Y;
			public byte Color;
		}

		private const double s_pixelsPerStar = 45;
	}
}
