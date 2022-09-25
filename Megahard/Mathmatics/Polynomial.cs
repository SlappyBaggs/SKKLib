using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Mathematics
{
	/// <summary>
	/// A Univariate Polynomial of degree N
	/// </summary>
	public class Polynomial
	{
		public Polynomial(params double[] coefficients)
		{
			_coeff = new Megahard.Collections.ImmutableArray<double>(coefficients);
		}

		public Polynomial(IEnumerable<double> coefficients)
		{
			_coeff = new Megahard.Collections.ImmutableArray<double>(coefficients);
		}

		public Polynomial AdjustToIntersectOrigin()
		{
			var builder = Collections.ImmutableArray<double>.Build(_coeff.Length);
			builder.AddRange(_coeff, 0, _coeff.Length - 1);
			builder.Add(0);
			return new Polynomial(builder.ToArray());
		}

		public double Evaluate(double argument)
		{
			if(Degree == -1)
				return 0;
			double val = 0;
			int coeffIndex = _coeff.Length - 1;
			val += _coeff[coeffIndex--];
			if (coeffIndex >= 0)
				val += _coeff[coeffIndex--] * argument;

			int deg = 2;
			while (coeffIndex >= 0)
				val += _coeff[coeffIndex--] * Math.Pow(argument, deg++);
			return val;
		}

		public int Degree
		{
			get { return _coeff.Length - 1; }
		}

		public Megahard.Collections.ImmutableArray<double> Coefficients
		{
			get { return _coeff; }
		}

		public string ToString(string varName, bool showZeroTerms, int decimalPlaces)
		{
			if (decimalPlaces < 0)
				throw new ArgumentOutOfRangeException("decimalPlaces", "Decimal Places must be >= 0");
			string format = "F" + decimalPlaces.ToString();
			if (Degree == 0)
				return "0";

			StringBuilder sb = new StringBuilder(Degree * 8);

			int termDegree = Degree;
			int termIndex = 0;
			DescribeTerm(_coeff[termIndex], termDegree, varName, format, ref sb);
			termIndex += 1;
			termDegree -= 1;
			
			while (termIndex < Coefficients.Length)
			{
				double val = _coeff[termIndex];
				if (showZeroTerms || val != 0)
				{
					if (val < 0)
					{
						sb.Append(" - ");
						val = Math.Abs(val);
					}
					else
					{
						sb.Append(" + ");
					}
					DescribeTerm(val, termDegree, varName, format, ref sb);
				}
				termDegree -= 1;
				termIndex += 1;
			}
			return sb.ToString();
		}

		public string ToString(int decimalPlaces)
		{
			return ToString("x", false, decimalPlaces);
		}
		public override string ToString()
		{
			return ToString("x", false, 2);
		}

		// makes like 3x or 3x^2 etc
		static void DescribeTerm(double coeff, int degree, string varName, string format, ref StringBuilder sb)
		{
			var s = coeff.ToString(format);
			int decimalPos = s.IndexOf('.');
			if (decimalPos != -1)
			{
				int c = s.Length - 1;
				while (c >= decimalPos)
				{
					if (s[c] != '0' && s[c] != '.')
					{
						break;
					}
					c -= 1;
				}

				s = s.Substring(0, c + 1);
			}
			sb.Append(s);
			if (degree >= 1)
				sb.Append(varName);
			if (degree > 1)
				sb.Append('^').Append(degree.ToString());
		}
		readonly Collections.ImmutableArray<double> _coeff;
	}

	public struct Point2<T>  where T : IConvertible
	{
		public Point2(T x, T y) : this()
		{
			X = x;
			Y = y;
		}
		public T X { get; set; }
		public T Y { get; set; }

		public override string ToString()
		{
			return string.Format("X={0:D3} Y={1:D3}", X, Y);
		}
	}
}