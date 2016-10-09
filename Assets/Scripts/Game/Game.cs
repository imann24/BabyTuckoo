using UnityEngine;
using System.Collections.Generic;

public class Game : MannBehaviour {
	public static Game Instance;
	public bool IsSingleton = true;

	public GameObject[] AgentPrefabs;
	Agent[] agents;
	public GameObject GridPrefab;
	Grid currentGrid;

	public Node GetNode (Position position) {
		if (currentGrid) {
			return currentGrid.GetNode(position);
		} else {
			return null;
		}
	}

	public bool TryGetHome (Agent agent, out HomeNode home) {
		if (currentGrid) {
			return currentGrid.TryGetHome(agent, out home);
		} else {
			home = null;
			return false;
		}
	}

	public Agent GetNodeOwner (Node node) {
		if (currentGrid) {
			return currentGrid.GetNodeOwner(node);
		} else {
			return null;
		}
	}

	public List<Node> GetUnclaimedNodes () {
		if (currentGrid) {
			return currentGrid.UnclaimedNodes;
		} else {
			return new List<Node>();
		}
	}

	public List<Node> GetClaimedNodes (Agent agent) {
		if (currentGrid) {
			return currentGrid.GetClaimedNodes(agent);
		} else {
			return new List<Node>();
		}
	}

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
		bool shouldInitialize = true;
		if (IsSingleton && !SingletonUtil.TryInit(ref Instance, this, gameObject)) {
			shouldInitialize = false;
		}
		if (shouldInitialize) {
			createAgents();
			createGrid(agents);
		}
	}

	void createAgents () {
		agents = new Agent[AgentPrefabs.Length];
		int index = 0;
		foreach (GameObject agentPrefab in AgentPrefabs) {
			agents[index] = Instantiate(agentPrefab).GetComponent<Agent>();
			agents[index++].SetGame(this);
		}
	}

	void teardownAgents () {
		if (agents != null) {
			for (int i = 0; i < agents.Length; i++) {
				Destroy(agents[i].gameObject);
			}
			agents = new Agent[0];
		}
	}

	void createGrid (Agent[] agents) {
		currentGrid = Instantiate(GridPrefab).GetComponent<Grid>();
		currentGrid.SetupGrid(agents);
	}

	void teardownGrid () {
		if (currentGrid) {
			Destroy(currentGrid);
		}
	}
}
