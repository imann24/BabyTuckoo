/*
 * Author(s): Isaiah Mann
 * Description: Super class for all agents operating in the game world
 */

using UnityEngine;
using System.Collections;

public abstract class Agent : MobileObjectBehaviour {
	Game game;
	HomeNode _home;
	Connection currentConnection;
	Node currentNode;
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

public enum AgentState {
	Idle,
	Capturing
}