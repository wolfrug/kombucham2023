using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.AI;

namespace Fungus {
    [CommandInfo ("Custom",
        "NavMesh Agent Move",
        "Lets you control a navmesh agent by clicking.")]

    [AddComponentMenu ("")]
    public class FungusNavMeshAgentMove : Command {

        public NavMeshAgent targetAgent;
        public Vector3Data targetLocation;
        public TransformData target;
        public BooleanData waitUntilFinished;

        public FloatData completionDistance = new FloatData (1f);

        private Coroutine waiter = null;
        private float actualCompletionDistance = 0f;

        public override void OnEnter () {
            actualCompletionDistance = targetAgent.stoppingDistance + completionDistance;
            if (target.Value == null) {
                targetAgent.SetDestination (targetLocation);
            } else {
                targetAgent.SetDestination (target.Value.position);
            }
            if (waitUntilFinished) {
                if (waiter != null) {
                    StopCoroutine (waiter);
                }
                waiter = StartCoroutine (WaitUntilArrived ());
            } else {
                Continue ();
            }
        }
        IEnumerator WaitUntilArrived () {
            Vector3 currentTargetLocation = targetLocation;
            if (target.Value != null) {
                currentTargetLocation = target.Value.position;
            };
            while (Vector3.Distance (targetAgent.transform.position, currentTargetLocation) > actualCompletionDistance) {
                if (target.Value != null) {
                    currentTargetLocation = target.Value.position;
                };
                targetAgent.SetDestination (currentTargetLocation);
                yield return null;
            }
            Continue ();
            waiter = null;
        }
    }
}