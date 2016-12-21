using System;
using System.Threading.Tasks;

namespace StrubT.PlayGround.CSharpConsole {

	public class CSharp7 : IRunnable {

		public bool Active => false;

		public void Run() {

			// SOURCE: https://docs.microsoft.com/en-us/dotnet/articles/csharp/csharp-7 //

			//out variables

			string firstName = "Thomas", lastName = "Strub";
			OutVariable(firstName, ref lastName, out var @out);
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

			ref var minName = ref RefReturn(ref firstName, ref lastName);
			Console.WriteLine(minName);

			//Local Functions

			string GetName() => PatternMatching(name);
			Func<string> getName = GetName;
			Console.WriteLine(getName());

			//Generalized async return types

			var future = ValueTaskAsync().Result;
			Console.WriteLine(future);

			//Numeric literal syntax improvements

			var population = 1_234_567_890;
			Console.WriteLine(population);
		}

		private void OutVariable(string @in, ref string @ref, out string @out) {

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

		private ref string RefReturn(ref string firstName, ref string lastName) {

			if (string.Compare(firstName, lastName, StringComparison.InvariantCultureIgnoreCase) <= 0)
				return ref firstName;
			else
				return ref lastName;
		}

		private async ValueTask<string> ValueTaskAsync() {

			await Task.Delay(100);
			return "Yay";
		}
	}
}
