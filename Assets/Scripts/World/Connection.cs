using UnityEngine;
using System.Collections;

public class Connection : StaticObjectBehaviour {
	[SerializeField]
	Node startNode;

	[SerializeField]
	Node endNode;

	LineRenderer visualLine;

	public void InitializeConnection (Node node) {
		this.startNode = node;
	}

	public void CompleteConnection (Node node) {
		this.endNode = node;
	}

	protected override void SetReferences () {
		base.SetReferences ();
		visualLine = GetComponent<LineRenderer>();
	}

	void setStartPosition (Vector3 worldPosition) {

	}

	void setEndPosition (Vector3 worldPosition) {

	}
}
