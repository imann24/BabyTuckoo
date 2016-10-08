/*
 * Author(s): Isaiah Mann
 * Description: Super class for all agents operating in the game world
 */

using UnityEngine;
using System.Collections;

public abstract class Agent : MannBehaviour {
	protected override void CleanupReferences () {
		// Nothing
	}

	protected override void FetchReferences () {
		// Nothing
	}

	protected override void HandleNamedEvent (string eventName) {
		// Nothing
	}

	protected override void SetReferences () {
		// Nothing
	}
}
