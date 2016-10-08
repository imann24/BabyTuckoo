using UnityEngine;
using System.Collections;

public class HomeNode : Node {
	public void Capture (Agent agent) {
		agent.Win();
	}

	protected override void setOwner (Agent owner) {
		base.setOwner(owner);
		openConnection(owner);
	}
}
