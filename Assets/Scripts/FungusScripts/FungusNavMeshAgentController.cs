using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.AI;

namespace Fungus {
    [CommandInfo ("Custom",
        "NavMesh Agent Controller",
        "Place in an update loop to let it look for input and then attempt to move the attached NavMeshAgent to the assigned location.")]

    [AddComponentMenu ("")]
    public class FungusNavMeshAgentController : Command {

        public NavMeshAgent targetAgent;
        public string inputName = "Fire1";
        [Tooltip ("Game object hit, or null if none")]
        [VariableProperty (typeof (GameObjectVariable))]
        public GameObjectVariable targetHit;

        [Tooltip ("Vector3 location that the ray hit")]
        [VariableProperty (typeof (Vector3Variable))]
        public Vector3Variable hitLocation;

        [Tooltip ("Does the navmesh agent current have a path?")]
        [VariableProperty (typeof (BooleanVariable))]
        public BooleanVariable hasPath;

        [Tooltip ("")]
        [SerializeField]
        protected LayerMask layerMask = ~0;

        [Tooltip ("")]
        [SerializeField]
        protected QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;

        public override void OnEnter () {

            if (Input.GetAxis (inputName) > 0f) {
                if (targetHit != null) targetHit.Value = null;
                if (hitLocation != null) hitLocation.Value = Vector3.zero;

                RaycastHit hitInfo;

                Vector2 mousePosition = Input.mousePosition;

                Ray rayOrigin = Camera.main.ScreenPointToRay (mousePosition);

                if (Physics.Raycast (rayOrigin, out hitInfo, float.PositiveInfinity, layerMask, queryTriggerInteraction)) {
                    Debug.Log ("Raycast hit object " + hitInfo.transform.name + " at the position of " + hitInfo.point);
                    if (targetHit != null) targetHit.Value = hitInfo.transform.gameObject;
                    if (hitLocation != null) hitLocation.Value = hitInfo.point;
                }
                targetAgent.SetDestination (hitInfo.point);
            };
            if (hasPath != null) {
                hasPath.Value = targetAgent.hasPath;
            }
            Continue ();
        }
    }
}