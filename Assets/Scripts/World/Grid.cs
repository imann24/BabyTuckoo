using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : StaticObjectBehaviour {
	public GameObject NodePrefab;
	public GameObject HomeNodePrefab;

	public int Width;
	public int Height;
	public float ZDepth = 50;
	public float ScalingFactor = 2.0f;

	Dictionary<Agent, HomeNode> homeNodes = new Dictionary<Agent, HomeNode>();

	// Nodes are arranged in cartesian coordinates [x, y]
	Node[,] nodes;

	public void AddHomeNodes (Agent[] agents) {
		foreach (Agent agent in agents) {
			addHomeNode(agent);
		}
	}
		
	void addHomeNode (Agent agent) {
		if (agent is Player) {
			spawnHomeNode(agent, getPlayerHomePosition());
		} else if (agent is Enemy) {
			spawnHomeNode(agent, getEnemyHomePosition());
		} else {
			throw new System.NotImplementedException();
		}
	}

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
		nodeObject.transform.SetParent(transform);
		return nodeBehaviour;
	}

	// Home nodes are outside the main grid system
	HomeNode spawnHomeNode (Agent agent, Vector3 worldPosition) {
		GameObject homeObject = Instantiate(HomeNodePrefab, worldPosition, Quaternion.identity) as GameObject;
		homeObject.transform.SetParent(agent.transform);
		HomeNode homeBehaviour = homeObject.GetComponent<HomeNode>();
		homeBehaviour.Owner = agent;
		updateOwner(agent, homeBehaviour);
		return homeBehaviour;
	}

	void updateOwner (Agent agent, HomeNode home) {
		if (homeNodes.ContainsKey(agent)) {	
			homeNodes[agent] = home;
		} else {
			homeNodes.Add(agent, home);
		}
	}

	public bool TryGetHome (Agent agent, out HomeNode home) {
		if (homeNodes.TryGetValue(agent, out home)) {
			return true;
		} else {
			return false;
		}
	}

	#region World Positions 

	Vector3 getWorldPosition (int x, int y) {
		return getWorldPosition(new Position(x, y));
	}

	Vector3 getWorldPosition (Position position) {
		return new Vector3(position.X * ScalingFactor - (((float)Width/2f - 0.5f) * ScalingFactor),
			position.Y * ScalingFactor - (((float)Height/2f - 0.5f) * ScalingFactor), ZDepth);
	}

	Vector3 getPlayerHomePosition () {
		return Vector3.Lerp(getWorldPosition(1, Height/2 + 1), getWorldPosition(1, Height/2), 0.5f);
	}

	Vector3 getEnemyHomePosition () {
		return Vector3.Lerp(getWorldPosition(Width - 1, Height/2 + 1), getWorldPosition(Width - 1, Height/2), 0.5f);
	}

	#endregion 
}
