using System;
using System.Linq;
using System.Reflection;
using c = System.Console;

namespace StrubT.PlayGround.CSharpConsole {

	public class Program {

		public static void Main() {

			var assembly = Assembly.GetAssembly(typeof(IRunnable));
			var runnableTypes = assembly.GetTypes().Where(t => !t.IsInterface && typeof(IRunnable).IsAssignableFrom(t));

			foreach (var runnableType in runnableTypes) {
				var runnable = (IRunnable)runnableType.GetConstructor(new Type[] { }).Invoke(new object[] { });

				if (runnable.Active) {
					c.WriteLine("*** {0} ***", runnableType.Name);
					c.WriteLine();
					runnable.Run();
					c.WriteLine();
					c.WriteLine();
				}
			}
		}
	}
}
