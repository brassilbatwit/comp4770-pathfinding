using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public float chaseRange = 5f;
    public float fleeRange = 7.5f;
    public int maxHealth = 10;
    public int health;

    private NavMeshAgent agent;
    private IState currentState;

    public PatrolState patrolState;
    public ChaseState chaseState;
    public FleeState fleeState;

    private MeshRenderer meshRenderer;

    public Material patrolMaterial;
    public Material chaseMaterial;
    public Material fleeMaterial;

    public bool HasLowHealth => health <= maxHealth / 2;

	void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();

        patrolState = new PatrolState(this, agent, patrolPoints);
        chaseState = new ChaseState(this, agent, player, chaseRange);
        fleeState = new FleeState(this, agent, player, fleeRange);

		health = maxHealth;

        TransitionToState(patrolState);
    }

    void Update()
    {
        HandleDebug();
        currentState.Execute();
	}

	public void TransitionToState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();

		ApplyMaterial(
			newState == patrolState ? patrolMaterial :
            newState == chaseState ? chaseMaterial :
            newState == fleeState ? fleeMaterial :
            null
        );

        currentState = newState;
        currentState.Enter();
    }

    private void HandleDebug() {
        // Simulate health
        if (Input.GetKeyDown(KeyCode.G)) {
            health = Math.Max(health - 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.H)) {
            health = Math.Min(health + 1, maxHealth);
        }
	}

	private void ApplyMaterial(Material material) {
		meshRenderer.material = material;
	}
}
