using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.AI;

namespace Fungus {
    [CommandInfo ("Custom",
        "NavMesh Agent Interact",
        "Lets you designate an interact point for a navmesh agent which sends a callback when finished")]

    [AddComponentMenu ("")]
    public class FungusNavMeshAgentInteract : Command {

        public NavMeshAgent targetAgent;
        public GameObjectData targetLocation;
        public float completionDistance;
        public BooleanData waitUntilFinished;

        private Coroutine waiterCoroutine;

        public override void OnEnter () {
            if (targetLocation.Value != null) {
                targetAgent.SetDestination (targetLocation.Value.transform.position);
                if (waiterCoroutine != null) {
                    StopCoroutine (waiterCoroutine);
                }
                waiterCoroutine = StartCoroutine (WaitUntilArrived ());
                if (!waitUntilFinished) {
                    Continue ();
                }
            } else {
                Continue ();
            }
        }
        IEnumerator WaitUntilArrived () {
            while (Vector3.Distance (targetAgent.transform.position, targetLocation.Value.transform.position) > completionDistance) {
                yield return null;
                if (targetLocation.Value == null) {
                    break;
                }
            }
            waiterCoroutine = null;
            if (waitUntilFinished) {
                Continue ();
            }
        }
    }
}