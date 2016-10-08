/*
 * Author(s): Isaiah Mann
 * Description: Represents a node in the game world
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : StaticObjectBehaviour {
	public GameObject ConnectionPrefab;
	List<Connection> connections = new List<Connection>();
	Agent _owner;
	Position _position;

	public Agent Owner {
		get {
			return _owner;
		}
		set {
			setOwner(value);
		}
	}

	public Position Position {
		get {
			return _position;
		}
		set {
			setPosition(value);
		}
	}
	public Vector3 WorldPosition {
		get {
			return transform.position;
		}
	}

	public bool IsOwned {
		get {
			return Owner != null;
		}
	}

	protected override void SetReferences () {
		base.SetReferences ();
	}

	protected virtual void setOwner (Agent owner) {
		setColour(owner.Color);
		this._owner = owner;
	}

	void setPosition (Position position) {
		this._position = position;
	}

	protected Connection beginConnection (Agent agent) {
		this.Owner = agent;
		return openConnection(agent);
	}

	protected Connection openConnection (Agent agent) {
		Connection connection = spawnConnection();
		connections.Add(connection);
		return connection;
	}

	Connection spawnConnection () {
		GameObject connectionObject = Instantiate(ConnectionPrefab, transform) as GameObject;
		Connection connectionBehaviour = connectionObject.GetComponent<Connection>();
		connectionBehaviour.InitializeConnection(this);
		return connectionBehaviour;
	}

	void OnMouseDown () {
		beginConnection(Player.Instance);
	}
}
