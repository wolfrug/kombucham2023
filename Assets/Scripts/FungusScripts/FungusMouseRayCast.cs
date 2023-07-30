using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

namespace Fungus {
    [CommandInfo ("Custom",
        "Raycast Mouse",
        "Casts a ray from the mouse location and returns a game object hit and a vector 3 location.")]

    [AddComponentMenu ("")]
    public class FungusMouseRayCast : Command {

        [Tooltip ("Game object hit, or null if none")]
        [VariableProperty (typeof (GameObjectVariable))]
        public GameObjectVariable targetHit;

        [Tooltip ("Vector3 location that the ray hit")]
        [VariableProperty (typeof (Vector3Variable))]
        public Vector3Variable hitLocation;

        [Tooltip ("")]
        [SerializeField]
        protected LayerMask layerMask = ~0;

        [Tooltip ("")]
        [SerializeField]
        protected QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
        public override void OnEnter () {
            targetHit.Value = null;
            hitLocation.Value = Vector3.zero;

            RaycastHit hitInfo;

            Vector2 mousePosition = Input.mousePosition;

            Ray rayOrigin = Camera.main.ScreenPointToRay (mousePosition);

            if (Physics.Raycast (rayOrigin, out hitInfo, float.PositiveInfinity, layerMask, queryTriggerInteraction)) {
                Debug.Log ("Raycast hit object " + hitInfo.transform.name + " at the position of " + hitInfo.point);
                targetHit.Value = hitInfo.transform.gameObject;
                hitLocation.Value = hitInfo.point;
            }
            Continue ();

        }
    }
}