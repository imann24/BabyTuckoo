using UnityEngine;
using System.Collections.Generic;

public class Game : MannBehaviour {
	public GameObject[] AgentPrefabs;
	Agent[] agents;
	public GameObject GridPrefab;
	Grid currentGrid;

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
		createAgents();
		createGrid(agents);
	}

	void createAgents () {
		agents = new Agent[AgentPrefabs.Length];
		int index = 0;
		foreach (GameObject agentPrefab in AgentPrefabs) {
			agents[index++] = Instantiate(agentPrefab).GetComponent<Agent>();
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
