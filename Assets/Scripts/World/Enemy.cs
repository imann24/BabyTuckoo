/*
 * Author(s): Isaiah Mann
 * Description: Represents an enemy AI agent in the world
 */

using UnityEngine;
using System.Collections;

public class Enemy : AIAgent {
    public static Enemy Instance;
	[Range(0, 1.0f)]
	public float TerritoryVsGoal = 0.5f;

    protected override void SetReferences() {
        if (SingletonUtil.TryInit(ref Instance, this, gameObject)) {
            base.SetReferences();
			focusNode = LastCapturedNode;
        }
    }

	protected override void FetchReferences () {
		base.FetchReferences ();
		startLogic();
	}

    protected override void CleanupReferences() {
        base.CleanupReferences();
        SingletonUtil.TryCleanupSingleton(ref Instance, this);
    }

	protected override MannAction decideNextAction () {
		if (!focusNode) {
			focusNode = LastCapturedNode;
		}
		if (isCapturingNode(focusNode)) {
			return delegate() {keepCapturingNode(focusNode);};
		} else {
			LastCapturedNode = focusNode;
			return delegate() {moveToNewNode(focusNode);};
		}
	}

	void startClaimingNode (Node node) {
		node.StartCapturing(this);
	}

	void moveToCapturedNode (Node node) {
		node.ClaimWith(this);
		this.LastCapturedNode = node;
		this.focusNode = node;
	}

	void moveToNewNode (Node currentFocusNode) {
		Node nextNode;
		Position nextPosition = chooseMovePosition();
		Debug.Log(nextPosition);
		if (nextNode = game.GetNodeFromOffset(currentFocusNode, nextPosition)) {
			this.focusNode = nextNode;
			if (ownsNode(nextNode)) {
				moveToCapturedNode(nextNode);
			} else {
				startClaimingNode(nextNode);
			}
		}
	}

	bool isCapturingNode (Node node) {
		return node.IsBeingCaptured;
	}

	void keepCapturingNode (Node node) {
		node.TickCapturing();
	}

	Position chooseMovePosition () {
		if (Random.Range(0, 1.0f) < TerritoryVsGoal) {
			return closerToTerritory();
		} else {
			return closerToGoal();
		}
	}

	Position getCurrentPosition () {
		return focusNode.Position;
	}

	Position closerToGoal () {
		return getCurrentPosition().CloserTo(goalPosition, 1);
	}

	Position closerToTerritory () {
		return game.GetNearestUnclaimedNode(focusNode).Position;
	}
}
