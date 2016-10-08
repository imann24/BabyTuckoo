/*
 * Author(s): Isaiah Mann
 * Description: Represents a node in the game world
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : StaticObjectBehaviour {
	public GameObject ConnectionPrefab;

	Color colour;
	Agent _owner;
	Renderer _renderer;
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
		
	public bool IsOwned {
		get {
			return Owner != null;
		}
	}

	protected override void SetReferences () {
		base.SetReferences ();
		_renderer = GetComponent<Renderer>();
	}

	void setOwner (Agent owner) {
		setColour(owner.Color);
		this._owner = owner;
	}

	void setColour (Color colour) {
		this.colour = colour;
		refreshColour();
	}

	void refreshColour () {
		_renderer.material.color = colour;	
	}

	void setPosition (Position position) {
		this._position = position;
	}

	void beginConnection (Agent agent) {
		this.Owner = agent;
	}
}
