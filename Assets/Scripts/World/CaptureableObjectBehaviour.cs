using UnityEngine;
using System.Collections;

public class CaptureableObjectBehaviour : WorldObjectBehaviour {
	MannAction onCaptureBegin;
	MannAction onCapture;
	MannActionf onCaptureProgress;

	public Color CaptureColour;
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

	public void SubscribeToCaptureProgress (MannActionf progress) {
		onCaptureProgress += progress;
	}

	public void UnsubscribeFromCaptureProgress (MannActionf progress) {
		onCaptureProgress -= progress;
	}

	public void SubscribeToBeginCapture (MannAction begin) {
		onCaptureBegin += begin;
	}

	public void UnsubscribeFromBeginCapture (MannAction begin) {
		onCaptureBegin -= begin;
	}
		
	public void TickCapture () {
		CaptureTickSpent = false;
	}

	public void SetCaptureTime (float timeToCapture, float captureTimeout) {
		this.CaptureTime = timeToCapture;
		this.CaptureTimeout = captureTimeout;
	}

	public void SetCaptureColour (Color colour) {
		this.CaptureColour = colour;
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
				refreshColour(Color.Lerp(Colour, CaptureColour, timer/CaptureTime));
				timer += Time.deltaTime;
				timeoutTimer = 0;
			} else {
				timeoutTimer += Time.deltaTime;
			}
			if (timeoutTimer >= CaptureTimeout) {
				haltCapture();
			}
			callOnCaptureProgress(timer / CaptureTime);
			yield return new WaitForEndOfFrame();
		}
		callOnCapture();
	}


	void callOnCaptureBegin () {
		if (onCaptureBegin != null) {
			onCaptureBegin();
		}
	}

	void callOnCaptureProgress (float progress) {
		if (onCaptureProgress != null) {
			onCaptureProgress(progress);
		}
	}

	void callOnCapture () {
		if (onCapture != null) {
			onCapture();
		}
	}
}
