using UnityEngine;
using System.Collections;

public class GameInput : MannBehaviour {
	public bool InputEnabled = true;

	// Abstract classes got from MannBehaviour
	protected override void CleanupReferences() {
		// Nothing
	}

	protected override void FetchReferences() {
		// Nothing
	}

	protected override void HandleNamedEvent(string eventName) {
		// Nothing
	}

	protected override void SetReferences() {
		// Nothing
	}
}
