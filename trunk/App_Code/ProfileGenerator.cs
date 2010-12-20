using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTBScout;
using System.Drawing;
using System.Globalization;
using System.Drawing.Imaging;
using System.IO;

namespace MTBScout
{
	class ProfileGenerator
	{
		Rectangle bitmapRect = new Rectangle(0, 0, 700, 400);

		public Rectangle graphRect = new Rectangle(80, 50, 600, 300);

		double minH;
		double maxH;
		double maxW;

		

		internal void GenerateProfile (GpxParser parser, string filePath)
		{
			using (Bitmap bmp = new Bitmap(bitmapRect.Width, bitmapRect.Height, PixelFormat.Format32bppPArgb))
			{
				using (Font font = new Font("Tahoma", 10.0f))
				{
					using (Graphics g = Graphics.FromImage(bmp))
					{
						g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
						g.DrawRectangle(Pens.BlueViolet, graphRect);
						TrackPoint prevPoint = null;
						Track t = parser.Tracks[0];
						double currPos = 0.0;
						minH = t.MinElevation;
						maxH = t.MaxElevation;
						maxW = t.Distance3D;
						PointF startPoint = GetLogicalPoint(0, minH);

						foreach (TrackSegment seg in t.Segments)
						{
							foreach (TrackPoint tp in seg.ReducedPoints)
							{
								if (prevPoint == null)
								{
									prevPoint = tp;
									continue;
								}

								currPos += tp - prevPoint;
								PointF endPoint = GetLogicalPoint(currPos, tp.ele);
								using (Pen p = new Pen(ColorProvider.GetColor(Math.Max(tp.ele, prevPoint.ele), minH, maxH)))
									g.DrawLine(p, startPoint, endPoint);
								prevPoint = tp;
								startPoint = endPoint;
							}
						}
						int heightRulerSpacing = Convert.ToInt32((maxH - minH) / 5); //suddivido in 5 segmenti
						heightRulerSpacing = (int)(Math.Ceiling(heightRulerSpacing / 10.0d) * 10);//arrotondo ai 10 metri

						double currH = Math.Round(minH / heightRulerSpacing, 0) * heightRulerSpacing;
						if (currH < minH)
							currH += heightRulerSpacing;
						StringFormat sf = new StringFormat();
						sf.LineAlignment = StringAlignment.Far;
						sf.Alignment = StringAlignment.Far;

						g.DrawString(GetHeightLabel(minH), font, Brushes.Red, GetLogicalPoint(0, minH), sf);

						while (currH < maxH)
						{
							PointF p1 = GetLogicalPoint(0, currH);
							PointF p2 = GetLogicalPoint(maxW, currH);
							g.DrawLine(Pens.BlueViolet, p1, p2);
							using (Brush b = new SolidBrush(ColorProvider.GetColor(currH, minH, maxH)))
								g.DrawString(GetHeightLabel(currH), font, b, p1, sf);
							currH += heightRulerSpacing;
						}
						using (Brush b = new SolidBrush(ColorProvider.GetColor(maxH, minH, maxH)))
							g.DrawString(GetHeightLabel(maxH), font, b, GetLogicalPoint(0, maxH), sf);


						sf.Alignment = StringAlignment.Center;
						sf.LineAlignment = StringAlignment.Near;
						int widthRulerSpacing = Convert.ToInt32(maxW / 5); //suddivido in 5 segmenti Km
						widthRulerSpacing = (int)(Math.Ceiling(widthRulerSpacing / 100.0d) * 100);//arrotondo ai 100 metri

						double currW = widthRulerSpacing;

						while (currW < maxW)
						{
							PointF p1 = GetLogicalPoint(currW, minH);
							PointF p2 = GetLogicalPoint(currW, maxH);
							g.DrawLine(Pens.BlueViolet, p1, p2);
							g.DrawString(GetWidthLabel(currW), font, Brushes.BlueViolet, p1, sf);
							currW += widthRulerSpacing;

						}

						g.DrawString(GetWidthLabel(currW), font, Brushes.BlueViolet, GetLogicalPoint(maxW, minH), sf);

						g.DrawString("Distanza (Km)", font, Brushes.BlueViolet, graphRect.X + graphRect.Width / 2, graphRect.Bottom + 20, sf);
						sf.FormatFlags |= StringFormatFlags.DirectionVertical;
						g.DrawString("Altitudine (m)", font, Brushes.BlueViolet, graphRect.X - 70, graphRect.Top + graphRect.Height / 2, sf);
					}
					string path = Path.GetDirectoryName(filePath);
					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);
					bmp.Save(filePath);
				}
			}
		}

		private static string GetHeightLabel (double currH)
		{
			return Convert.ToInt32(currH).ToString(CultureInfo.InvariantCulture) + "m";
		}

		private static string GetWidthLabel (double currW)
		{
			return Convert.ToInt32(currW / 1000).ToString(CultureInfo.InvariantCulture) + "Km";
		}

		private PointF GetLogicalPoint (double physicalX, double physicalY)
		{
			double x = physicalX * graphRect.Width / maxW;
			double y = (physicalY - minH) * graphRect.Height / (maxH - minH);
			PointF endPoint = new PointF((float)x, (float)y);
			return Traslate(endPoint);
		}

		private PointF Traslate (PointF startPoint)
		{
			return new PointF(startPoint.X + graphRect.Left, graphRect.Height - startPoint.Y + graphRect.Top);
		}

	}
}
