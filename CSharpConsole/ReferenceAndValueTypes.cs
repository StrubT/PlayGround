using c = System.Console;

namespace StrubT.PlayGround.CSharpConsole {

	public class ReferenceAndValueTypes : IRunnable {

		public bool Active => false;

		public void Run() {

			var i = 0;
			var refTyp = new[] { new ReferenceType(), new ReferenceType(), new ReferenceType(), new ReferenceType() };
			ChangeReferenceType0(refTyp[0]);
			ChangeReferenceType1(refTyp[1]);
			ChangeReferenceType2(ref refTyp[2]);
			ChangeReferenceType3(ref refTyp[3]);
			foreach (var t in refTyp)
				c.WriteLine("test #{0}: {1}changed", ++i, !t.Changed ? "NOT " : string.Empty);

			var valTyp = new[] { new ValueType(), new ValueType(), new ValueType(), new ValueType() };
			ChangeValueType0(valTyp[0]);
			ChangeValueType1(valTyp[1]);
			ChangeValueType2(ref valTyp[2]);
			ChangeValueType3(ref valTyp[3]);
			foreach (var t in valTyp)
				c.WriteLine("test #{0}: {1}changed", ++i, !t.Changed ? "NOT " : string.Empty);
		}

		private void ChangeReferenceType0(ReferenceType changeable) => changeable.Changed = true;

		private void ChangeReferenceType1(ReferenceType changeable) => changeable = new ReferenceType() { Changed = true };

		private void ChangeReferenceType2(ref ReferenceType changeable) => changeable.Changed = true;

		private void ChangeReferenceType3(ref ReferenceType changeable) => changeable = new ReferenceType() { Changed = true };

		private void ChangeValueType0(ValueType changeable) => changeable.Changed = true;

		private void ChangeValueType1(ValueType changeable) => changeable = new ValueType() { Changed = true };

		private void ChangeValueType2(ref ValueType changeable) => changeable.Changed = true;

		private void ChangeValueType3(ref ValueType changeable) => changeable = new ValueType() { Changed = true };
	}

	internal class ReferenceType {

		public bool Changed { get; set; }
	}

	internal struct ValueType {

		public bool Changed { get; set; }
	}
}
