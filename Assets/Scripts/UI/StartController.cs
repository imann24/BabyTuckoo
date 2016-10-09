using UnityEngine;
using System.Collections;

public class StartController : Controller {
	public GameObject GamePrefab;
	public float ExitTime = 2.0f;
	public float ExitDistance = 10.0f;
	CaptureableObjectBehaviour capture;

	protected override void SetReferences () {
		capture = GetComponent<CaptureableObjectBehaviour>();
	}


	protected override void FetchReferences () {
		capture.SubscribeToCapture(playStartSequence);
	}
		
	protected override void CleanupReferences () {
		// Nothing
	}

	protected override void HandleNamedEvent (string eventName) {
		// Nothing
	}


	void OnMouseDown () {
		playStartSequence();
	}

	void playStartSequence () {
		startButtonExit();
	}

	void startGame () {
		Instantiate(GamePrefab);
		destroyStartButton();
	}

	void startButtonExit () {
		moveTo(transform.position + Vector3.back * ExitDistance, ExitTime, startGame);
	}

	void destroyStartButton () {
		Destroy(gameObject);
	}
}
