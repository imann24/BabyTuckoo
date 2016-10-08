using UnityEngine;
using System.Collections;

public class Grid : StaticObjectBehaviour {
	public GameObject NodePrefab;

	public int Width;
	public int Height;
	public float ZDepth = 50;
	public float ScalingFactor = 2.0f;

	// Nodes are arranged in cartesian coordinates [x, y]
	Node[,] nodes;

	public Node GetNode (Position position) {
		if (inBounds(position)) {
			return nodes[position.X, position.Y];
		} else {
			return null;
		}
	}

	bool inBounds (Position position) {
		return position.X >= 0 && position.X < nodes.GetLength(0) &&
			position.Y >= 0 && position.Y < nodes.GetLength(1);
	}

	protected override void SetReferences () {
		base.SetReferences ();
		setupGrid();
	}

	void setupGrid () {
		nodes = new Node[Width, Height];
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				nodes[x, y] = spawnNode(new Position(x, y));
			}
		}
	}

	Node spawnNode (Position position) {
		GameObject nodeObject = Instantiate(NodePrefab, getWorldPosition(position), Quaternion.identity) as GameObject;
		Node nodeBehaviour = nodeObject.GetComponent<Node>();
		nodeBehaviour.Position = position;
		return nodeBehaviour;
	}
		
	Vector3 getWorldPosition (Position position) {
		return new Vector3(position.X * ScalingFactor - (((float)Width/2f - 0.5f) * ScalingFactor),
			position.Y * ScalingFactor - (((float)Height/2f - 0.5f) * ScalingFactor), ZDepth);
	}
}
