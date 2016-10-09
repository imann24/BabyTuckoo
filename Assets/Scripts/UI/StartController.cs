using UnityEngine;
using System.Collections;

public class StartController : Controller {
	public GameObject GamePrefab;
	public float ExitTime = 2.0f;
	public float ExitDistance = 10.0f;

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
