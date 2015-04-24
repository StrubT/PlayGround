using c = System.Console;

namespace StrubT.PlayGround.CSharpConsole {

	public class ReferenceAndValueTypes : IRunnable {

		public bool IsActive() { return false; }

		public void Run() {

			var referenceType = new ReferenceType();
			var referenceTypeByReference = new ReferenceType();
			var valueType = new ValueType();
			var valueTypeByReference = new ValueType();

			this.ChangeReferenceType(referenceType);
			this.ChangeReferenceTypeByReference(ref referenceTypeByReference);
			this.ChangeValueType(valueType);
			this.ChangeValueTypeByReference(ref valueTypeByReference);

			c.WriteLine("reference type:                       {0}changed", !referenceType.changed ? "NOT " : string.Empty);
			c.WriteLine("reference type (passed by reference): {0}changed", !referenceTypeByReference.changed ? "NOT " : string.Empty);
			c.WriteLine("value type:                           {0}changed", !valueType.changed ? "NOT " : string.Empty);
			c.WriteLine("value type (passed by reference):     {0}changed", !valueTypeByReference.changed ? "NOT " : string.Empty);
		}

		private void ChangeReferenceType(ReferenceType changeable) { changeable.changed = true; }

		private void ChangeReferenceTypeByReference(ref ReferenceType changeable) { changeable.changed = true; }

		private void ChangeValueType(ValueType changeable) { changeable.changed = true; }

		private void ChangeValueTypeByReference(ref ValueType changeable) { changeable.changed = true; }
	}

	internal class ReferenceType {

		public bool changed { get; set; }
	}

	internal struct ValueType {

		public bool changed { get; set; }
	}
}
