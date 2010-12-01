using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace MTBScout
{
	/// <summary>
	/// Summary description for ColorProvider
	/// </summary>
	public class ColorProvider
	{
		/// <summary>
		///contiene una lista di colori
		///sono i colori da rosso a violetto che sfumano con gradiente di colore
		///e sono usati per indicare l'altezza
		/// </summary>
		static List<Color> colors = GetColorMap();

		public static Color GetColor (double elevation, double minEle, double maxEle)
		{
			double ratio = (elevation - minEle) / (maxEle - minEle);
			int index = Convert.ToInt32(ratio * (colors.Count - 1));
			return colors[index];
		}

		private static List<Color> GetColorMap ()
		{
			List<Color> colors = new List<Color>();
			colors.AddRange(GetGradients(Color.Red, Color.Orange, 255));
			colors.AddRange(GetGradients(Color.Orange, Color.Yellow, 255));
			colors.AddRange(GetGradients(Color.Yellow, Color.Green, 255));
			colors.AddRange(GetGradients(Color.Green, Color.Blue, 255));
			colors.AddRange(GetGradients(Color.Blue, Color.Indigo, 255));
			colors.AddRange(GetGradients(Color.Indigo, Color.Violet, 255));
			return colors;
		}

		private static IEnumerable<Color> GetGradients (Color start, Color end, int steps)
		{
			//per ogni colore, suddivido le distanze fra il codice colore di partenza e quello
			//di arrivo in base a steps, trovando così la lunghezza di un 'passo'
			//che può anche essere negativo ad es. se devo passare da 255 a 0
			int rStep = ((end.R - start.R) / (steps - 1));
			int gStep = ((end.G - start.G) / (steps - 1));
			int bStep = ((end.B - start.B) / (steps - 1));

			for (int i = 0; i < steps; i++)
			{
				yield return Color.FromArgb((start.R + (rStep * i)), (start.G + (gStep * i)), (start.B + (bStep * i)));
			}
		}

		public static string GetColorString (double ele, double minEle, double maxEle)
		{
			Color c = GetColor(ele, minEle, maxEle);
			return string.Format("#{0}{1}{2}", HexCode(c.R), HexCode(c.G), HexCode(c.B));
		}

		private static string HexCode (int code)
		{
			return code.ToString("x").PadLeft(2, '0');
		}

	}
}