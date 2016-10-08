﻿/*
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

	Agent capturer = null;
	float captureProgress = 0;
	public bool IsBeingCaptured {
		get {
			return capturer != null;
		}
	}

	protected override void SetReferences () {
		base.SetReferences ();
	}

	protected virtual void setOwner (Agent owner) {
		setColour(owner.Color);
		this._owner = owner;
		openConnection(owner);
	}

	void setPosition (Position position) {
		this._position = position;
	}

	protected Connection beginConnection (Agent agent) {
		// Close the previous connection
		this.Owner = agent;
		return openConnection(agent);
	}
		
	protected Connection openConnection (Agent agent) {
		agent.CloseConnection(this);
		Connection connection = spawnConnection();
		connections.Add(connection);
		agent.OpenConnection(this, connection);
		return connection;
	}

	public void Link (Agent agent) {
		openConnection(agent);
	}

	public void StartCapturing (Agent agent) {
		this.capturer = agent;
	}

	// Capture Progress should be clamped between 0..1.0f
	public void UpdateCaptureProgress (Agent agent, float captureProgress) {
		setColour(Color.Lerp(Colour, agent.Colour, this.captureProgress = captureProgress), false);
	}

	public void EndCapturing () {
		this.capturer = null;
		startLerpColor(Colour, captureProgress);
		captureProgress = 0;
	}

	Connection spawnConnection () {
		GameObject connectionObject = Instantiate(ConnectionPrefab, transform) as GameObject;
		Connection connectionBehaviour = connectionObject.GetComponent<Connection>();
		connectionBehaviour.InitializeConnection(this);
		return connectionBehaviour;
	}

	// TODO: Remove this (for debugging only)
	void OnMouseDown () {
		beginConnection(Player.Instance);
	}
}
