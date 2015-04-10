using System;
using System.Linq;
using System.Reflection;
using c = System.Console;

namespace ThS.PlayGround.CSharpConsole {

	public class Program {

		public static void Main() {

			var assembly = Assembly.GetAssembly(typeof(IRunnable));
			var runnables = assembly.GetTypes().Where(t => !t.IsInterface && typeof(IRunnable).IsAssignableFrom(t));

			foreach(var runnable in runnables) {
				c.WriteLine("*** {0} ***", runnable.Name);
				c.WriteLine();
				((IRunnable)runnable.GetConstructor(new Type[] { }).Invoke(new object[] { })).run();
				c.WriteLine();
				c.WriteLine();
			}
		}
	}
}
