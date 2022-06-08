using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.Math.Data
{
	public class Polynomial
	{
		private List<double> _coeff;

		public Polynomial(params double[] coefficients) => _coeff = new List<double>(coefficients);

		public Polynomial(IEnumerable<double> coefficients) => _coeff = new List<double>(coefficients);
		
		public Polynomial AdjustToIntersectOrigin()
		{
			List<double> nl = new List<double>(_coeff);
			nl.RemoveAt(nl.Count - 1);
			nl.Add(0);
			return new Polynomial(nl);
		}

		public double Evaluate(double argument)
		{
			if (Degree == -1)
				return 0;
			double val = 0;
			int coeffIndex = _coeff.Count - 1;
			val += _coeff[coeffIndex--];
			if (coeffIndex >= 0)
				val += _coeff[coeffIndex--] * argument;

			int deg = 2;
			while (coeffIndex >= 0) 
				val += _coeff[coeffIndex--] * System.Math.Pow(argument, deg++);
			return val;
		}

		public int Degree { get => _coeff.Count - 1; }

		public List<double> Coefficients { get => _coeff; }

		public string ToString(string varName, bool showZeroTerms, int decimalPlaces)
		{
			if (decimalPlaces < 0) throw new ArgumentOutOfRangeException("decimalPlaces", "Decimal Places must be >= 0");
			if (Degree == 0) return "0";
			
			string format = "F" + decimalPlaces.ToString();

			int termDegree = Degree;
			int termIndex = 0;
			string ret = DescribeTerm(_coeff[termIndex], termDegree, varName, format);
			termIndex += 1;
			termDegree -= 1;

			while (termIndex < Coefficients.Count)
			{
				double val = _coeff[termIndex];
				if (showZeroTerms || val != 0)
				{
					if (val < 0)
					{
						ret +=" - ";
						val = System.Math.Abs(val);
					}
					else
					{
						ret += " + ";
					}
					ret += DescribeTerm(val, termDegree, varName, format);
				}
				termDegree -= 1;
				termIndex += 1;
			}
			return ret;
		}

		// qnvrdg

		public string ToString(int decimalPlaces) => ToString("x", false, decimalPlaces);
		public override string ToString() => ToString("x", false, 2);

		// makes like 3x or 3x^2 etc
		static string DescribeTerm(double coeff, int degree, string varName, string format)
		{
			string ret = string.Empty;
			var s = coeff.ToString(format);
			int decimalPos = s.IndexOf('.');
			if (decimalPos != -1)
			{
				int c = s.Length - 1;
				while (c >= decimalPos)
				{
					if (s[c] != '0' && s[c] != '.') break;
					c -= 1;
				}

				s = s.Substring(0, c + 1);
			}
			ret += s;
			if (degree >= 1)
				ret += varName;
			if (degree > 1)
				ret += ("^" + degree.ToString());
			return ret;
		}
	}
}
