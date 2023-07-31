using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

namespace Fungus {
    [EventHandlerInfo ("Custom",
        "Drag Completed (Canvas)",
        "The block will execute when the player drags an object and successfully drops it on a target object (with a Canvas draggable 2D).")]
    [AddComponentMenu ("")]
    [ExecuteInEditMode]
    public class DragCompletedCanvasEventhandler : DragCompleted {

        public List<string> draggableTagRef = default;
        public List<string> dragTargetTagRefs = default;
        public override bool IsOverTarget () {
            overTarget = true;
            return overTarget;
        }
        public void SetTargetCollider (Collider2D set) {
            targetCollider = set;
        }
        public List<Collider2D> GetTargetColliders () {
            return targetObjects;
        }
    }
}