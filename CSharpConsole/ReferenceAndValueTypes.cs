using c = System.Console;

namespace StrubT.PlayGround.CSharpConsole {

	public class ReferenceAndValueTypes : IRunnable {

		public void run() {

			var referenceType = new ReferenceType();
			var referenceTypeByReference = new ReferenceType();
			var valueType = new ValueType();
			var valueTypeByReference = new ValueType();

			this.changeReferenceType(referenceType);
			this.changeReferenceTypeByReference(ref referenceTypeByReference);
			this.changeValueType(valueType);
			this.changeValueTypeByReference(ref valueTypeByReference);

			c.WriteLine("reference type:                       {0}changed", !referenceType.changed ? "NOT " : string.Empty);
			c.WriteLine("reference type (passed by reference): {0}changed", !referenceTypeByReference.changed ? "NOT " : string.Empty);
			c.WriteLine("value type:                           {0}changed", !valueType.changed ? "NOT " : string.Empty);
			c.WriteLine("value type (passed by reference):     {0}changed", !valueTypeByReference.changed ? "NOT " : string.Empty);
		}

		private void changeReferenceType(ReferenceType changeable) { changeable.changed = true; }

		private void changeReferenceTypeByReference(ref ReferenceType changeable) { changeable.changed = true; }

		private void changeValueType(ValueType changeable) { changeable.changed = true; }

		private void changeValueTypeByReference(ref ValueType changeable) { changeable.changed = true; }
	}

	internal class ReferenceType {

		public bool changed { get; set; }
	}

	internal struct ValueType {

		public bool changed { get; set; }
	}
}
