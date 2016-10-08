using UnityEngine;
using System.Collections;

public class Connection : StaticObjectBehaviour {
	[SerializeField]
	Node startNode;

	[SerializeField]
	Node endNode;

	LineRenderer visualLine;
	IEnumerator trackingMouseCoroutine;

	public bool IsTrackingMouse {get; private set;}

	public bool CheckValidConnection () {
		return startNode.Owner == endNode.Owner;
	}

	public void InitializeConnection (Node node) {
		this.startNode = node;
		setStartPosition(node.WorldPosition);
		// TEMP: Remove this after we set up visual tracking
		StartTrackingMouse();
		setColour(node.Colour);
	}

	public void CompleteConnection (Node node) {
		this.endNode = node;
	}

	public void StartTrackingMouse () {
		IsTrackingMouse = true;
		haltTrackingMouseCoroutine();
		StartCoroutine(trackingMouseCoroutine = trackMouse());
	}

	public void StopTrackingMouse () {
		IsTrackingMouse = false;
		haltTrackingMouseCoroutine();
	}

	void haltTrackingMouseCoroutine () {
		if (trackingMouseCoroutine != null) {
			StopCoroutine(trackingMouseCoroutine);
		}

	}

	IEnumerator trackMouse () {
		while (IsTrackingMouse) {
			setEndPosition(CameraUtil.ScreenToWorldPosition(Input.mousePosition, 50));	
			yield return new WaitForEndOfFrame();
		}
	}

	protected override void SetReferences () {
		base.SetReferences ();
		visualLine = GetComponent<LineRenderer>();
	}

	void setStartPosition (Vector3 worldPosition) {
		visualLine.SetPosition(0, worldPosition);
	}

	void setEndPosition (Vector3 worldPosition) {
		visualLine.SetPosition(1, worldPosition);
	}
}
