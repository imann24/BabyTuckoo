using UnityEngine;
using System.Collections;

public class WorldObjectBehaviour : MannBehaviour {
	protected Renderer[] _renderers;
	public Color Colour {get; private set;}
	IEnumerator colorCoroutine;

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
		Colour = sampleColour();
	}

	protected void setColour (Color colour, bool updateStoredColour = true) {
		if (updateStoredColour) {
			this.Colour = colour;
		}
		refreshColour(colour);
	}

	protected void refreshColour (Color colour) {
		if (_renderers != null) {
			foreach (Renderer renderer in _renderers) {
				refreshRenderer(renderer, colour);
			}
		}
	}

	Color sampleColour () {
		if (_renderers.Length >= 1) {
			return _renderers[0].material.color;
		} else {
			return default(Color);
		}
	}

	void refreshRenderer (Renderer renderer, Color color) {
		renderer.material.color = color;
	}

	IEnumerator lerpToColour (Color targetColour, float totalTime, bool updateStoredColour = false) {
		float timer = 0;
		Color startColour = sampleColour();
		while (timer <= totalTime) {
			setColour(Color.Lerp(startColour, targetColour, timer), updateStoredColour);
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		setColour(targetColour, updateStoredColour);
	}

	protected void startLerpColor (Color targetColour, float totalTime, bool updateStoredColour = false) {
		haltLerpColor();
		colorCoroutine = lerpToColour(targetColour, totalTime, updateStoredColour);
		StartCoroutine(colorCoroutine);
	}

	protected void haltLerpColor () {
		if (colorCoroutine != null) {
			StopCoroutine(colorCoroutine);
		}
	}
}
