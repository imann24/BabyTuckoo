using UnityEngine;
using System.Collections;

public class StartController : Controller {
	public GameObject GamePrefab;

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
		Destroy(gameObject);
	}
}
