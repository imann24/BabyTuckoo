/*
 * Author(s): Isaiah Mann
 * Description: Super class for all agents operating in the game world
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Agent : MobileObjectBehaviour {
	Game game;
	HomeNode _home;
	Connection currentConnection;
	Node currentNode;
	List<Node> captureChain = new List<Node>();

	public HomeNode Home { 
		get {
			return _home;
		}
	}
	public Node LastCapturedNode {
		get {
			return currentNode;
		}
        set
        {
            currentNode = value;
        }
	}

	public Position StartingPosition;
	public Color Color = Color.white;

	public void Win () {
		EventController.Event(Event.WIN);
	}

	public void SetGame (Game game) {
		this.game = game;
	}

	public void OpenConnection (Node node, Connection connection) {
		this.currentNode = node;
		this.currentConnection = connection;
		List<Node> cycle;
		if (checkForCycleInChain(node, out cycle)) {
			captureNodesInCycle(cycle);
		}
	}

	bool checkForCycleInChain (Node node, out List<Node> cycle) {
		if (captureChain.Count > 4 && captureChain.Contains(node)) {
			(cycle = new List<Node>()).AddRange(captureChain);
			return true;
		} else {
			captureChain.Add(node);
			cycle = null;
			return false;
		}
	}

	void captureNodesInCycle (List<Node> cycle) {
		int north = int.MinValue;
		int east = int.MinValue;
		int south = int.MaxValue;
		int west = int.MaxValue;
		foreach (Node node in cycle) {
			if (north < node.Y) north = node.Y;
			if (east < node.X) east = node.X;
			if (south > node.Y) south = node.Y;
			if (west > node.X) west = node.X;
		}
		// Debug.LogFormat("N{0}, E{1}, S{2}, W{3}", north, east, south, west);
		for (int x = west; x < east; x++) {
			for (int y = south; y < north; y++) {
				Debug.Log(new Position(x, y));
				Node node = game.GetNode(new Position(x, y));
				if (node && cycleNodeOnAllSides(node, cycle)) {
					node.Owner = this;
				}
			}
		}
		captureChain.Clear();
	}

	bool cycleNodeOnAllSides (Node toCheck, List<Node> cycle) {
		bool north = false;
		bool east = false;
		bool south = false;
		bool west = false;
		foreach (Node node in cycle) {
			if (toCheck.Y < node.Y) north = true;
			if (toCheck.X < node.X) east = true;
			if (toCheck.Y > node.Y) south = true;
			if (toCheck.X > node.X) west = true;
		}
		return north && east && south && west;
	}

	public void CloseConnection (Node node) {
		if (this.currentConnection) {
			this.currentConnection.CompleteConnection(node);
		}
	}

	protected override void SetReferences () {
		base.SetReferences ();
		setColour(Color);
	}

	protected override void FetchReferences () {
		base.FetchReferences ();
		fetchHomeFromGame();
	}

	void fetchHomeFromGame () {
		if (this.game.TryGetHome(this, out _home)) {
			currentNode = _home;
		}
	}
}
