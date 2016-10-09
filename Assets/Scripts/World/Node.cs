/*
 * Author(s): Isaiah Mann
 * Description: Represents a node in the game world
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : StaticObjectBehaviour {
	public float ScaleFactor = 1.25f;
	[SerializeField]
	GameObject highlightParticles;
	public GameObject ConnectionPrefab;
	List<Connection> connections = new List<Connection>();
	Agent _owner;
	Position _position;
	Grid _grid;
	CaptureableObjectBehaviour capture;
	IEnumerator autoCaptureCoroutine;

	public Grid  Grid {
		get {
			return _grid;
		}
		set {
			setGrid(value);
		}
	}
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
		capture = GetComponent<CaptureableObjectBehaviour>();
		capture.SubscribeToCaptureProgress(UpdateCaptureProgress);
	}

	protected virtual void setOwner (Agent owner) {
		setColour(owner.Color);
		this._owner = owner;
		if (this._grid && !(this is HomeNode)) {
			this._grid.UpdateNodeOwner(owner, this);
		}
		openConnection(owner);
	}

	void setPosition (Position position) {
		this._position = position;
	}

	void setGrid (Grid grid) {
		this._grid = grid;
	}

	public Connection ClaimWith (Agent agent) {
		// Close the previous connection
		this.Owner = agent;
		Connection newConnection = openConnection(agent);
		if (IsBeingCaptured) {
			EndCapturing();
		}
		return newConnection;
	}
		
	protected Connection openConnection (Agent agent) {
		agent.CloseConnection(this);
		Connection connection = spawnConnection();
		connections.Add(connection);
		agent.OpenConnection(this, connection);
		return connection;
	}

	public void Select () {
		scale(ScaleFactor);
		//	TODO: Update particles to ParticleSystem (current method is depecrated)
		highlightParticles.SetActive(true);
	}

	public void CloseConnection () {
		scale(1.0f);
		highlightParticles.SetActive(false);
	}

	public void Link (Agent agent) {
		openConnection(agent);
	}

	public void StartCapturing (Agent agent) {
		this.capturer = agent;
		if (agent is Enemy) {
			startAutoCapture(agent);
		}
	}

	void startAutoCapture (Agent agent) {
		// Halts any previous thread
		haltAutoCapture();
		autoCaptureCoroutine = autoCapture(agent.GetCaptureTime(this));
		StartCoroutine(autoCaptureCoroutine);
	}

	void haltAutoCapture () {
		if (autoCaptureCoroutine != null) {
			StopCoroutine(autoCaptureCoroutine);
		}
	}

	IEnumerator autoCapture (float captureTime) {
		float timer = 0f;
		while (timer <= captureTime) {
			UpdateCaptureProgress(timer / captureTime);
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
		}
		UpdateCaptureProgress(1);
	}

	// Capture Progress should be clamped between 0..1.0f
	public void UpdateCaptureProgress (float captureProgress) {
		if (capturer != null) {
			UpdateCaptureProgress(this.capturer, captureProgress);
		}
	}

	// Capture Progress should be clamped between 0..1.0f
	public void UpdateCaptureProgress (Agent agent, float captureProgress) {
		setColour(Color.Lerp(Colour, agent.Colour, this.captureProgress = captureProgress), false);
		if (captureProgress >= 1) {
			ClaimWith(agent);
		}
	}

	public void EndCapturing () {
		this.capturer = null;
		startLerpColor(Colour, captureProgress);
		captureProgress = 0;
	}

	public void TickCapturing () {
		capture.TickCapture();
	}

	Connection spawnConnection () {
		GameObject connectionObject = Instantiate(ConnectionPrefab, transform) as GameObject;
		Connection connectionBehaviour = connectionObject.GetComponent<Connection>();
		connectionBehaviour.InitializeConnection(this);
		return connectionBehaviour;
	}

	// TODO: Remove this (for debugging only)
	void OnMouseDown () {
		ClaimWith(Player.Instance);
	}
}
