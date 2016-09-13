using System;
using c = System.Console;
using cc = System.ConsoleColor;

namespace StrubT.PlayGround.CSharpConsole {

	public class MachineEpsilon : IRunnable {

		public bool Active => false;

		public void Run() {

			Func<bool?, cc?> getDarkColour = b => b == null ? (cc?)null : (b.Value ? cc.DarkGreen : cc.DarkRed);
			Func<bool?, cc?> getColour = b => b == null ? (cc?)null : (b.Value ? cc.Green : cc.Red);

			foreach (var t in new[] {
				Tuple.Create(0.0f, 0.0f, true),
				Tuple.Create(-0.0f, +0.0f, true),
				Tuple.Create(1.0f, 1.0f, true),
				Tuple.Create(-1.0f, +1.0f, false),
				Tuple.Create(3.1415926535897932385f + 2.135135483544684f, 5.2767281371344772385f, true),
				Tuple.Create(5.2767281371344772385f, 3.1415926535897932385f + 2.135135483544684f, true),
				Tuple.Create(float.NegativeInfinity, float.NegativeInfinity, true),
				Tuple.Create(float.NegativeInfinity, float.MinValue, false),
				Tuple.Create(float.NegativeInfinity, float.PositiveInfinity, false),
				Tuple.Create(float.NaN, float.NaN, false),
				Tuple.Create(float.NaN, float.PositiveInfinity, false)
			}) {
				var eq = t.Item1 == t.Item2;
				var dif = HasMinimalDifference(t.Item1, t.Item2, 1);
				var ulp = Math.Abs(t.Item1 - t.Item2) <= Ulp(t.Item1);
				WriteResult(
					new[] { "{0,25} == {1,25}: {2,-5} (", "{3,-5}", ", ", "{4,-5}", ", ", "{5,-5}", ")" },
					new[] { null, getDarkColour(t.Item3 == eq), null, getColour(t.Item3 == dif), null, getColour(t.Item3 == ulp), null },
					t.Item1, t.Item2, t.Item3, eq, dif, ulp);
			}
			c.WriteLine();

			foreach (var t in new[] {
				Tuple.Create(0.0, 0.0, true),
				Tuple.Create(-0.0, +0.0, true),
				Tuple.Create(1.0, 1.0, true),
				Tuple.Create(-1.0, +1.0, false),
				Tuple.Create(3.1415926535897932385 + 2.135135483544684, 5.2767281371344772385, true),
				Tuple.Create(5.2767281371344772385, 3.1415926535897932385 + 2.135135483544684, true),
				Tuple.Create(double.NegativeInfinity, double.NegativeInfinity, true),
				Tuple.Create(double.NegativeInfinity, double.MinValue, false),
				Tuple.Create(double.NegativeInfinity, double.PositiveInfinity, false),
				Tuple.Create(double.NaN, double.NaN, false),
				Tuple.Create(double.NaN, double.PositiveInfinity, false)
			}) {
				var eq = t.Item1 == t.Item2;
				var dif = HasMinimalDifference(t.Item1, t.Item2, 1);
				var ulp = Math.Abs(t.Item1 - t.Item2) <= Ulp(t.Item1);
				WriteResult(
					new[] { "{0,25} == {1,25}: {2,-5} (", "{3,-5}", ", ", "{4,-5}", ", ", "{5,-5}", ")" },
					new[] { null, getDarkColour(t.Item3 == eq), null, getColour(t.Item3 == dif), null, getColour(t.Item3 == ulp), null },
					t.Item1, t.Item2, t.Item3, eq, dif, ulp);
			}
		}

		void WriteResult(string[] formats, cc?[] colours, params object[] arguments) {

			if ((formats?.Length ?? -2) != (colours?.Length ?? -1))
				throw new ArgumentException();

			var fg = c.ForegroundColor;
			for (int i = 0; i < formats.Length; i++) {
				c.ForegroundColor = colours[i] ?? fg;
				c.Write(formats[i], arguments);
			}

			c.ForegroundColor = fg;
			c.WriteLine();
		}

		public float Ulp(float value) {

			if (float.IsNaN(value)) return float.NaN;
			if (float.IsInfinity(value)) return float.PositiveInfinity;

			value = Math.Abs(value);
			return BitConverter.ToSingle(BitConverter.GetBytes(BitConverter.ToInt32(BitConverter.GetBytes(value), 0) + (value < float.MaxValue ? 1 : -1)), 0) - value;
		}

		public double Ulp(double value) {

			if (double.IsNaN(value)) return double.NaN;
			if (double.IsInfinity(value)) return double.PositiveInfinity;

			value = Math.Abs(value);
			return BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(value) + (value < double.MaxValue ? 1 : -1)) - value;
		}

		public static bool HasMinimalDifference(float value1, float value2, int units) {

			if (float.IsNaN(value1) || float.IsInfinity(value1) || float.IsNaN(value2) || float.IsInfinity(value2)) return value1 == value2;
			if (value1 == value2) return true;

			int iValue1 = BitConverter.ToInt32(BitConverter.GetBytes(value1), 0);
			int iValue2 = BitConverter.ToInt32(BitConverter.GetBytes(value2), 0);

			// If the signs are different, return false except for +0 and -0.
			if ((iValue1 >> 31) != (iValue2 >> 31))
				return value1 == value2;

			return Math.Abs(iValue1 - iValue2) <= units;
		}

		public static bool HasMinimalDifference(double value1, double value2, int units) {

			if (double.IsNaN(value1) || double.IsInfinity(value1) || double.IsNaN(value2) || double.IsInfinity(value2)) return value1 == value2;
			if (value1 == value2) return true;

			long lValue1 = BitConverter.DoubleToInt64Bits(value1);
			long lValue2 = BitConverter.DoubleToInt64Bits(value2);

			// If the signs are different, return false except for +0 and -0.
			if ((lValue1 >> 63) != (lValue2 >> 63))
				return value1 == value2;

			return Math.Abs(lValue1 - lValue2) <= units;
		}
	}
}
