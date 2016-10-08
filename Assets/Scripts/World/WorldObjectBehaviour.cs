using UnityEngine;
using System.Collections;

public class WorldObjectBehaviour : MannBehaviour {
	protected Renderer _renderer;
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
		_renderer = GetComponent<Renderer>();

	}

	protected void setColour (Color colour) {
		this.Colour = colour;
		refreshColour();
	}

	protected void refreshColour () {
		_renderer.material.color = Colour;	
	}
}
