using System.Linq;
using System.Threading.Tasks;
using c = System.Console;

namespace StrubT.PlayGround.CSharpConsole {

	public class AsyncAwait : IRunnable {

		public bool IsActive() { return false; }

		public async void Run() {

			c.WriteLine("enter something:");

			var tsk = Task.Delay(5000);

			string line;
			while ((line = c.ReadLine()).Length > 0)
				c.WriteLine(">{0}", string.Join(string.Empty, line.Reverse()));

			await tsk;

			c.WriteLine("finished!");
		}
	}
}
