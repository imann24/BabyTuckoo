using UnityEngine;
using System.Collections;

public class Grid : StaticObjectBehaviour {
	public GameObject NodePrefab;

	public int Width;
	public int Height;

	Node[,] nodes;

	public Node GetNode (Position position) {
		if (inBounds(position)) {
			throw new System.NotImplementedException();
		} else {
			return null;
		}
	}

	bool inBounds (Position position) {
		throw new System.NotImplementedException();
	}

	protected override void SetReferences () {
		base.SetReferences ();
		nodes = new Node[Width, Height];
	}

	Node spawnNode (Position position) {
		throw new System.NotImplementedException();
	}
		
	void positionNode (Position position) {

	}
}
