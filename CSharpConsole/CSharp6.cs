using System;

namespace StrubT.PlayGround.CSharpConsole {

	public class CSharp6 : IRunnable {

		public bool Active => false;

		public void Run() {

			var people = new[] {
				null,
				Tuple.Create("Strub", "Thomas Reto", new DateTime(1991, 11, 20)),
				null
			};

			foreach (var person in people)
				Console.WriteLine($"{person?.Item1 ?? "-"}, {person?.Item2 ?? "-"} --- {person?.Item3:o}");
		}
	}
}
