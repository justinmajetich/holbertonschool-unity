using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class TargetManager : MonoBehaviour
{
    public delegate void TargetEvent();
    public static event TargetEvent onAllTargetsDestroyed;

    public GameObject targetPrefab;

    [SerializeField, Range(1, 7)] int targetCount = 5;
    int targetsRemaining = 0;
    [SerializeField] float spawnRadius = 1f;

    private void OnEnable() {
        GameManager.onStartGame += SpawnTargets;
        GameManager.onGameSetup += ClearTargets;
        Ammo.onTargetHit += DecreaseTargetCount;
    }

    void SpawnTargets() {

        // Get center of game plane mesh.
        Vector3 meshCenter = ARSurfaceManager.gamePlane.gameObject.GetComponent<Renderer>().bounds.center;

        // Clear all existing targets.
        ClearTargets();

        // Spawn all targets in a ring around game plane mesh center.
        for (int i = 0; i < targetCount; i++) {

            // Calculate next spawn position in ring around game plane center.
            // 1. Get position a given radius from mesh center.
            Vector3 offset = meshCenter - (meshCenter + (Vector3.right * spawnRadius));

            // 2. Rotate offset vector and add to center to get rotated spawn point.
            Vector3 spawnPosition = (Quaternion.Euler(0f, (360f / targetCount) * i, 0f) * offset) + meshCenter;

            // Spawn target, parenting to this transform.
            GameObject newTarget = Instantiate(targetPrefab, transform);

            // Position new target using the NavAgent's warp method.
            NavMeshAgent agent = newTarget.GetComponent<NavMeshAgent>();
            agent.Warp(spawnPosition);
        }

        targetsRemaining = targetCount;
    }

    void ClearTargets() {

        // Clear all currently spawned targets.
        foreach (Transform target in transform) {
            Destroy(target.gameObject);
        }

    }

    void DecreaseTargetCount() {
        targetsRemaining--;

        if (targetsRemaining <= 0) {
            onAllTargetsDestroyed();    
        }
    }

    private void OnDisable() {
        GameManager.onStartGame -= SpawnTargets;
        GameManager.onGameSetup -= ClearTargets;
        Ammo.onTargetHit -= DecreaseTargetCount;
    }
}
