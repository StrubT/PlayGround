using System;

namespace StrubT.PlayGround.CSharpConsole {

	public class CSharp6 : IRunnable {

		public bool Active => false;

		public void Run() {

			// SOURCE: https://docs.microsoft.com/en-us/dotnet/articles/csharp/csharp-6 //

			//Read - only Auto - properties
			//Auto - Property Initializers
			//Expression Bodied function members
			//using static

			//Null - conditional operators
			//String Interpolation

			var people = new[] {
				null,
				Tuple.Create("Strub", "Thomas Reto", new DateTime(1991, 11, 20)),
				null
			};

			foreach (var person in people)
				Console.WriteLine($"{person?.Item1 ?? "-"}, {person?.Item2 ?? "-"} --- {person?.Item3:o}");

			//Exception filters
			//nameof Expressions
			//await in catch and finally blocks
			//index initializers
			//Extension methods for collection initializers
			//Improved overload resolution
		}
	}
}
