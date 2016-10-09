using UnityEngine;
using System.Collections;

public class CaptureableObjectBehaviour : WorldObjectBehaviour {
	MannAction onCapture;

	public Color CaptureColor;
	public float CaptureTime;
	public float CaptureTimeout;
	IEnumerator captureCoroutine;
	public bool IsCapturing {get; private set;}
	public bool CaptureTickSpent = false;

	public void InitiailizeCapture () {
		// Halt any old capture that's still running
		haltCapture();
		IsCapturing = true;
		captureCoroutine = capture();
		StartCoroutine(captureCoroutine);
	}

	public void SubscribeToCapture (MannAction capture) {
		onCapture += capture;
	}

	public void UnsubscribeFromCapture (MannAction capture) {
		onCapture -= capture;
	}

	public void TickCapture () {
		CaptureTickSpent = false;
	}

	void haltCapture () {
		refreshColour(Colour);
		IsCapturing = false;
		if (captureCoroutine != null) {
			StopCoroutine(captureCoroutine);
		}
	}

	IEnumerator capture () {
		float timer = 0;
		float timeoutTimer = 0;
		while (timer <= CaptureTime) {			
			if (!CaptureTickSpent) {
				refreshColour(Color.Lerp(Colour, CaptureColor, timer/CaptureTime));
				timer += Time.deltaTime;
				timeoutTimer = 0;
			} else {
				timeoutTimer += Time.deltaTime;
			}
			if (timeoutTimer >= CaptureTimeout) {
				haltCapture();
			}
			yield return new WaitForEndOfFrame();
		}
		callOnCapture();
	}

	void callOnCapture () {
		if (onCapture != null) {
			onCapture();
		}
	}
}
