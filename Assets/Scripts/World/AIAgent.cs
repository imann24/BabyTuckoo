/*
 * Author(s): Isaiah Mann
 * Description: Represents an AI agent in the world
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIAgent : Agent {
	public float DecisionTime = 2.0f;
	Queue<MannAction> futureActions = new Queue<MannAction>();
	bool _isActive = true;
	public bool IsActive {
		get {
			return _isActive;
		}
		set {
			_isActive = value;
		}
	}
	IEnumerator decisionCoroutine;
	IEnumerator executeCoroutine;

	protected override void SetReferences () {
		base.SetReferences ();
		startDecisionLoop();
	}

	protected void startDecisionLoop () {
		haltDecisionLoop();
		decisionCoroutine = decisionLoop(DecisionTime);
		StartCoroutine(decisionCoroutine);
	}

	protected void haltDecisionLoop () {
		if (decisionCoroutine != null) {
			StopCoroutine(decisionCoroutine);
		}
	}

	IEnumerator decisionLoop (float decisionTime) {
		while (IsActive) {
			futureActions.Enqueue(decideNextAction());
			yield return new WaitForSeconds(decisionTime);
		}
	}

	protected virtual MannAction decideNextAction () {
		return delegate {
			Debug.Log("Running");
		};
	}

	protected virtual void executeNextAction () {
		futureActions.Dequeue()();
	}
}
