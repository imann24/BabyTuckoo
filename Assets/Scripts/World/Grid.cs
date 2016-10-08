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
	}

	public void SetupGrid (Agent[] agents) {
		nodes = new Node[Width, Height];
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				Agent agent;
				Position currentPosition = new Position(x, y);
				if (checkForAgent(currentPosition, agents, out agent)) {
					nodes[x, y] = spawnHomeNode(agent, currentPosition);
				} else {
					nodes[x, y] = spawnNode(currentPosition);
				}
			}
		}
	}
		
	bool checkForAgent (Position position, Agent[] agentList, out Agent matchingAgent) {
		foreach (Agent agent in agentList) {
			if (agent.StartingPosition.Equals(position)) {
				matchingAgent = agent;
				return true;
			}
		}
		matchingAgent = null;
		return false;
	}

	Node createNodeFromPrefab (GameObject prefab, Position position) {
		GameObject nodeObject = Instantiate(prefab, getWorldPosition(position), Quaternion.identity) as GameObject;
		nodeObject.transform.SetParent(transform);
		Node nodeBehaviour = nodeObject.GetComponent<Node>();
		nodeBehaviour.Position = position;
		return nodeBehaviour;
	}

	Node spawnNode (Position position) {
		return createNodeFromPrefab(NodePrefab, position);
	}

	// Home nodes are outside the main grid system
	HomeNode spawnHomeNode (Agent agent, Position position) {
		HomeNode homeNode = createNodeFromPrefab(HomeNodePrefab, position) as HomeNode;
		homeNode.Owner = agent;
		updateOwner(agent, homeNode);
		return homeNode;
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

	#endregion 
}
