using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

namespace Fungus {
    [CommandInfo ("Custom",
        "Set Scale",
        "Sets scale of given transform to given float value immediately.")]

    [AddComponentMenu ("")]
    public class FungusSetScale : Command {

        [Tooltip ("Target transform")]
        public TransformData target;
        [Tooltip ("Target scale")]
        public Vector3Data scale;

        public override void OnEnter () {
            if (target.Value != null) {
                target.Value.localScale = scale;
            }
            Continue ();
        }

        public override string GetSummary () {
            if (target.Value == null) {
                return "No transform assigned!";
            }
            return "Scaling " + target.Value.name + " to " + scale.vector3Val;
        }
    }
}