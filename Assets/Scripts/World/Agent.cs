/*
 * Author(s): Isaiah Mann
 * Description: Super class for all agents operating in the game world
 */

using UnityEngine;
using System.Collections;

public abstract class Agent : MobileObjectBehaviour {
	public Color Color = Color.white;
}

public enum AgentState {
	Idle,
	Capturing
}