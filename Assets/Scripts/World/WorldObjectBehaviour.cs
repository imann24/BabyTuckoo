using UnityEngine;
using System.Collections;

public class WorldObjectBehaviour : MannBehaviour {
	protected Renderer[] _renderers;
	public Color Colour {get; private set;}

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
		_renderers = GetComponentsInChildren<Renderer>();

	}

	protected void setColour (Color colour) {
		this.Colour = colour;
		refreshColour();
	}

	protected void refreshColour () {
		if (_renderers != null) {
			foreach (Renderer renderer in _renderers) {
				refreshRenderer(renderer);
			}
		}
	}

	void refreshRenderer (Renderer renderer) {
		renderer.material.color = Colour;
	}
}
