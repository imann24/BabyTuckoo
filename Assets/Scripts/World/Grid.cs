using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
	Agent[] agents;
	List<Node> allNodes = new List<Node>();
	public List<Node> AllCapturedNodes {
		get {
			List<Node> nodes = new List<Node>();
			foreach (List<Node> list in capturedNodes.Values) {
				nodes.AddRange(list);
			}
			return nodes;
		}
	}

	public List<Node> UnclaimedNodes {
		get {
			return allNodes.Except(AllCapturedNodes).ToList();
		}
	}

	public Node GetNodeFromOffset (Node node, Position translation) {
		Position position = node.Position.Translate(translation.X, translation.Y);
		if (inBounds(position)) {
			return GetNode(position);
		} else {
			return null;
		}
	}

	Dictionary<Agent, List<Node>> capturedNodes = new Dictionary<Agent, List<Node>>();
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
		this.agents = agents;
		initializeCapturedNodeSets(agents);
		nodes = new Node[Width, Height];
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				Agent agent;
				Position currentPosition = new Position(x, y);
				if (checkForAgent(currentPosition, agents, out agent)) {
					nodes[x, y] = spawnHomeNode(agent, currentPosition);
					nodes[x, y].Select();
				} else {
					nodes[x, y] = spawnNode(currentPosition);
					allNodes.Add(nodes[x,y]);
				}
				nodes[x, y].Grid = this;
			}
		}
	}

	// Returns null if node does not exist
	public Agent GetNodeOwner (Position position) {
		Node node = GetNode(position);
		if (node == null) {
			return null;
		} else {
			return GetNodeOwner(node);
		}
	}

	// Returns null if there is no owner
	public Agent GetNodeOwner (Node node) {
		foreach (Agent agent in capturedNodes.Keys) {
			if (capturedNodes[agent].Contains(node)) {
				return agent;
			}
		}
		return null;
	}
		
	public List<Node> GetClaimedNodes (Agent agent) {
		List<Node> nodes;
		if (capturedNodes.TryGetValue(agent, out nodes)) {
			return nodes;
		} else {
			return new List<Node>();
		}
	}

	public Node GetNearestUnclaimedNode (Node toNode) {
		Position[] adjacentPositions = toNode.Position.GetPlus();
		Node[] adjacentNodes = new Node[4];
		for (int i = 0; i < adjacentPositions.Length; i++) {
			if ((adjacentNodes[i] = GetNode(adjacentPositions[i])) && !adjacentNodes[i].IsOwned) {
				return adjacentNodes[i];
			}
		}
		for (int i = 0; i < adjacentNodes.Length; i++) {
			Node currentNode;
			if (currentNode = GetNearestUnclaimedNode(adjacentNodes[i])) {
				return currentNode;
			}
		}
		return null;
	}

	void initializeCapturedNodeSets (Agent[] agents) {
		foreach (Agent agent in agents) {
			capturedNodes.Add(agent, new List<Node>());
		}
	}

	public void UpdateNodeOwner (Agent owner, Node node) {
		foreach (Agent agent in agents) {
			if (agent == owner) {
				capturedNodes[agent].Add(node);
			} else if (capturedNodes[agent].Contains(node)) {
				capturedNodes[agent].Remove(node);
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
