﻿/*
 * Author(s): Isaiah Mann
 * Description: Super class for all agents operating in the game world
 */

using UnityEngine;
using System.Collections;

public abstract class Agent : MobileObjectBehaviour {
	Game game;
	HomeNode _home;
	public HomeNode Home { 
		get {
			return _home;
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

	protected override void FetchReferences () {
		base.FetchReferences ();
		fetchHomeFromGame();
	}

	void fetchHomeFromGame () {
		this.game.TryGetHome(this, out _home);
	}
}

public enum AgentState {
	Idle,
	Capturing
}