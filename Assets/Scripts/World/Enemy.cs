/*
 * Author(s): Isaiah Mann
 * Description: Represents an enemy AI agent in the world
 */

using UnityEngine;
using System.Collections;

public class Enemy : AIAgent {
    public static Enemy Instance;

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
		if (nextNode = game.GetNodeFromOffset(currentFocusNode, -1, 0)) {
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
}
