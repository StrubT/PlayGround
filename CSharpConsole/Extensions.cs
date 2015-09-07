using System;

namespace StrubT.PlayGround.CSharpConsole {

	internal static class Extensions {

		public static T NullIf<T>(this T instance, Predicate<T> condition) where T : class => !condition(instance) ? instance : null;
	}
}
