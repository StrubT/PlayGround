using System;
using System.Linq;
using System.Reflection;
using c = System.Console;

namespace StrubT.PlayGround.CSharpConsole {

	public class Program {

		public static void Main() {

			var assembly = Assembly.GetExecutingAssembly();
			var runnableTypes = assembly.GetTypes().Where(t => !t.IsInterface && typeof(IRunnable).IsAssignableFrom(t));

			foreach (var runnableType in runnableTypes) {
				var runnable = (IRunnable)runnableType.GetConstructor(new Type[] { }).Invoke(new object[] { });

				if (runnable.Active) {
					c.WriteLine($"*** {runnableType.Name} ***");
					c.WriteLine();
					runnable.Run();
					c.WriteLine();
					c.WriteLine();
				}
			}
		}
	}
}
