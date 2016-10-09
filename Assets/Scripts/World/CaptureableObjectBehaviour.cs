using UnityEngine;
using System.Collections;

public class CaptureableObjectBehaviour : WorldObjectBehaviour {
	public Color CaptureColor;
	public float CaptureTime;
	IEnumerator captureCoroutine;

	public void InitiailizeCapture () {
		haltCapture();
		captureCoroutine = capture();
		StartCoroutine(captureCoroutine);
	}

	void haltCapture () {
		if (captureCoroutine != null) {
			StopCoroutine(captureCoroutine);
		}
	}

	IEnumerator capture () {
		float timer = 0;
		while (timer <= CaptureTime) {			
			refreshColour(Color.Lerp(Colour, CaptureColor, timer/CaptureTime));
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

}
