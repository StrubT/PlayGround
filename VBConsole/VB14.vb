
Public Class VB14
	Implements IRunnable

	Public ReadOnly Property Active As Boolean = False Implements IRunnable.Active

	Public Sub Run() Implements IRunnable.Run

		Dim people = {
			Nothing,
			Tuple.Create("Strub", "Thomas Reto", #1991-11-20#),
			Nothing
		}

		For Each person In people
			Console.WriteLine($"{If(person?.Item1, "-")}, {If(person?.Item2, "-")} --- {person?.Item3:o}")
		Next
	End Sub
End Class
