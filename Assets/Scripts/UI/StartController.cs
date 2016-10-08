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
		
	}

	void OnMouseDown () {
		startGame();
	}

	void startGame () {
		Instantiate(GamePrefab);
		startButtonExit();
	}

	void startButtonExit () {
		moveTo(transform.position + Vector3.back * ExitDistance, ExitTime, destroyStartButton);
	}

	void destroyStartButton () {
		Destroy(gameObject);
	}
}
