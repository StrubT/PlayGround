﻿using c = System.Console;

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

			c.WriteLine("reference type:                       {0}changed", !referenceType.Changed ? "NOT " : string.Empty);
			c.WriteLine("reference type (passed by reference): {0}changed", !referenceTypeByReference.Changed ? "NOT " : string.Empty);
			c.WriteLine("value type:                           {0}changed", !valueType.Changed ? "NOT " : string.Empty);
			c.WriteLine("value type (passed by reference):     {0}changed", !valueTypeByReference.Changed ? "NOT " : string.Empty);
		}

		private void ChangeReferenceType(ReferenceType changeable) { changeable.Changed = true; }

		private void ChangeReferenceTypeByReference(ref ReferenceType changeable) { changeable.Changed = true; }

		private void ChangeValueType(ValueType changeable) { changeable.Changed = true; }

		private void ChangeValueTypeByReference(ref ValueType changeable) { changeable.Changed = true; }
	}

	internal class ReferenceType {

		public bool Changed { get; set; }
	}

	internal struct ValueType {

		public bool Changed { get; set; }
	}
}
