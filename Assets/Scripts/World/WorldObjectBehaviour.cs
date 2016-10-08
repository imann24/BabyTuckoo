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
		if (_renderers.Length >= 1) {
			Colour = _renderers[0].material.color;
		}
	}

	protected void setColour (Color colour, bool updateStoredColor = true) {
		if (updateStoredColor) {
			this.Colour = colour;
		}
		refreshColour(colour);
	}

	protected void refreshColour (Color color) {
		if (_renderers != null) {
			foreach (Renderer renderer in _renderers) {
				refreshRenderer(renderer, color);
			}
		}
	}

	void refreshRenderer (Renderer renderer, Color color) {
		renderer.material.color = color;
	}
}
