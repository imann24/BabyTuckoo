using UnityEngine;
using System.Collections.Generic;

public class Game : MannBehaviour {
	public static Game Instance;
	public bool IsSingleton = true;

	public GameObject[] AgentPrefabs;
	Agent[] agents;
	public GameObject GridPrefab;
	Grid currentGrid;

	Enemy mostRecentEnemy;
	Player mostRecentPlayer;

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

	public Node GetNodeFromOffset (Node node, int xOffset, int yOffset) {
		return GetNodeFromOffset(node, new Position(xOffset, yOffset));	
	}

	public Node GetNodeFromOffset (Node node, Position position) {
		if (currentGrid) {
			return currentGrid.GetNodeFromOffset(node, position);
		} else {
			return null;
		}
	}

	public Node GetNearestUnclaimedNode (Node toNode) {
		if (currentGrid) {
			return currentGrid.GetNearestUnclaimedNode(toNode);
		} else {
			return null;
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
			setAgentHomeNodes();
			setEnemyGoal();
		}
	}

	void createAgents () {
		agents = new Agent[AgentPrefabs.Length];
		int index = 0;
		foreach (GameObject agentPrefab in AgentPrefabs) {
			agents[index] = Instantiate(agentPrefab).GetComponent<Agent>();
			agents[index].SetGame(this);
			if (agents[index] is Enemy) {
				mostRecentEnemy = agents[index] as Enemy;
			} else if (agents[index] is Player) {
				mostRecentPlayer = agents[index] as Player;
			}
			index++;
		}
	}

	void setAgentHomeNodes () {
		foreach (Agent agent in agents) {
			agent.FetchHomeFromGame();
		}
	}

	void setEnemyGoal () {
		mostRecentEnemy.SetGoal(mostRecentPlayer.Home);
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
