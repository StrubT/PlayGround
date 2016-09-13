Imports c = System.Console

Module Program

	Sub Main()

		Dim assembly = Reflection.Assembly.GetExecutingAssembly()
		Dim runnableTypes = assembly.GetTypes().Where(Function(t) Not t.IsInterface AndAlso GetType(IRunnable).IsAssignableFrom(t))

		For Each runnableType In runnableTypes
			Dim runnable = DirectCast(runnableType.GetConstructor(New Type() {}).Invoke(New Object() {}), IRunnable)

			If runnable.Active Then
				c.WriteLine($"*** {runnableType.Name} ***")
				c.WriteLine()
				runnable.Run()
				c.WriteLine()
				c.WriteLine()
			End If
		Next
	End Sub
End Module
