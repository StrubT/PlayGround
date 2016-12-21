using System;

namespace StrubT.PlayGround.CSharpConsole {

	public class CSharp7 : IRunnable {

		public bool Active => true;

		public void Run() {

			// SOURCE: https://docs.microsoft.com/en-us/dotnet/articles/csharp/csharp-7 //

			//out variables

			string firstName = "Thomas", lastName = "Strub";
			InRefOut(firstName, ref lastName, out var @out);
			Console.WriteLine(string.Join(" - ", new[] { firstName, lastName, @out }));

			//Tuples

			var name = Tuple("Thomas", "Strub");
			Console.WriteLine(string.Join(" - ", new[] { name.FirstName, name.LastName }));

			(firstName, lastName) = name;
			Console.WriteLine(string.Join(" - ", new[] { firstName, lastName }));

			//Pattern Matching

			Console.WriteLine(PatternMatching("Thomas"));
			Console.WriteLine(PatternMatching(("Thomas", "Strub")));
			Console.WriteLine(PatternMatching('T'));

			//ref locals and returns
			//Local Functions
			//Generalized async return types
			//Numeric literal syntax improvements
		}

		private void InRefOut(string @in, ref string @ref, out string @out) {

			@out = @ref;
			@ref = @in;
		}

		private (string FirstName, string LastName) Tuple(string firstName, string lastName) => (firstName, lastName);

		private string PatternMatching(object input) {

			switch (input) {
				case string name:
					return name;
				case ValueTuple<string, string> name:
					return string.Format("{1}, {0}", name.Item1, name.Item2);
				default:
					return "n/a";
			}
		}
	}
}
