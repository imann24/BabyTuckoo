using UnityEngine;
using System.Collections;

public class LookInput : GameInput {
	public bool ShouldDisplayPointer = true;
	public GameObject VisualPointer;

	protected override void SetReferences () {
		base.SetReferences ();
		if (ShouldDisplayPointer) {
			createVisualPointer();
		}
	}


	void Update () {
		if (InputEnabled) {
			GameObject hitObject;
			if (sampleGaze(out hitObject)) {
				CaptureableObjectBehaviour capture;
				if (isCaptureable(hitObject, out capture)) {
					handleCapture(capture);
				}
			}
		}
	}

	bool sampleGaze (out GameObject hitObject) {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.forward, out hit)) {
			hitObject = hit.collider.gameObject;
			return true;
		} else {
			hitObject = null;
			return false;
		}
	}

	bool isCaptureable (GameObject hitObject, out CaptureableObjectBehaviour capture) {
		return (capture = hitObject.GetComponent<CaptureableObjectBehaviour>()) != null;
	}

	void handleCapture (CaptureableObjectBehaviour capture) {
		if (capture.IsCapturing) {
			capture.TickCapture();
		} else {
			capture.InitiailizeCapture();
		}
	}

	void createVisualPointer () {
		Instantiate(VisualPointer, transform);
	}
}
