using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FleeState : IState {
	private EnemyAI ai;
	private NavMeshAgent agent;
	private Transform opponent;
	private float fleeRange;

	public FleeState(EnemyAI ai, NavMeshAgent agent, Transform opponent, float fleeRange) {
		this.ai = ai;
		this.agent = agent;
		this.opponent = opponent;
		this.fleeRange = fleeRange;
	}

	public void Enter() {
		
	}

	public void Execute() {
		float dist = Vector3.Distance(ai.transform.position, opponent.position);

		if (dist > fleeRange) {
			ai.TransitionToState(ai.patrolState);
			return;
		}
		else {
			if (!ai.HasLowHealth) {
				ai.TransitionToState(ai.chaseState);
				return;
			}
		}
		var destination = ai.transform.position + ai.transform.position - opponent.position;
		agent.SetDestination(destination);
	}

	public void Exit() {
		
	}
}