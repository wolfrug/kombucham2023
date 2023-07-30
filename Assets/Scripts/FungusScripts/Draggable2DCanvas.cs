// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Fungus {
    /// <summary>
    /// Detects drag and drop interactions on a Game Object, and sends events to all Flowchart event handlers in the scene.
    /// The Game Object must have Collider2D & RigidBody components attached. 
    /// The Collider2D must have the Is Trigger property set to true.
    /// The RigidBody would typically have the Is Kinematic property set to true, unless you want the object to move around using physics.
    /// Use in conjunction with the Drag Started, Drag Completed, Drag Cancelled, Drag Entered & Drag Exited event handlers.
    /// </summary>
    public class Draggable2DCanvas : Draggable2D {

        protected override void OnTriggerEnter2D (Collider2D other) {
            if (!dragEnabled) {
                return;
            }

            var eventDispatcher = FungusManager.Instance.EventDispatcher;

            eventDispatcher.Raise (new DragEntered.DragEnteredEvent (this, other));
        }

        protected override void OnTriggerExit2D (Collider2D other) {
            if (!dragEnabled) {
                return;
            }

            var eventDispatcher = FungusManager.Instance.EventDispatcher;

            eventDispatcher.Raise (new DragExited.DragExitedEvent (this, other));
        }

        protected virtual void DetectDragCompleted () {
            // Obtain the current mouse position.
            PointerEventData eventData = new PointerEventData (EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult> ();
            EventSystem.current.RaycastAll (eventData, raycastResults);
            Debug.Log ("Detected end drag");
            var eventDispatcher = FungusManager.Instance.EventDispatcher;
            foreach (RaycastResult result in raycastResults) {
                if (result.gameObject.GetComponent<Collider2D> () != null) {
                    Debug.Log ("Detected collider 2d on " + result.gameObject.name);
                    for (int i = 0; i < dragCompletedHandlers.Count; i++) {
                        var handler = dragCompletedHandlers[i];
                        Debug.Log ("Checking a drag completed handler " + handler);
                        if (handler != null && handler.DraggableObjects.Contains (this)) {
                            Debug.Log ("Raising an event");
                            eventDispatcher.Raise (new DragCompleted.DragCompletedEvent (this));
                        }
                    }
                }
            }
        }

        protected override void DoBeginDrag () {
            // Offset the object so that the drag is anchored to the exact point where the user clicked it
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;

            delta = new Vector3 (x, y, 10f) - transform.position;
            delta.z = 0f;

            startingPosition = transform.position;

            var eventDispatcher = FungusManager.Instance.EventDispatcher;

            eventDispatcher.Raise (new DragStarted.DragStartedEvent (this));
        }

        protected override void DoDrag () {
            if (!dragEnabled) {
                return;
            }

            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float z = transform.position.z;

            newPosition = new Vector3 (x, y, 10f) - delta;

            newPosition.z = z;
            updatePosition = true;
        }

        protected override void DoEndDrag () {
            if (!dragEnabled) {
                return;
            }
            /*DetectDragCompleted ();
            var eventDispatcher = FungusManager.Instance.EventDispatcher;
            bool dragCompleted = false;

            for (int i = 0; i < dragCompletedHandlers.Count; i++) {
                var handler = dragCompletedHandlers[i];
                if (handler != null && handler.DraggableObjects.Contains (this)) {
                    if (handler.IsOverTarget ()) {
                        dragCompleted = true;

                        eventDispatcher.Raise (new DragCompleted.DragCompletedEvent (this));
                    }
                }
            }
*/
            PointerEventData eventData = new PointerEventData (EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult> ();
            EventSystem.current.RaycastAll (eventData, raycastResults);
            Debug.Log ("Detected end drag");
            bool dragCompleted = false;
            var eventDispatcher = FungusManager.Instance.EventDispatcher;
            foreach (RaycastResult result in raycastResults) {
                Collider2D hitCollider = result.gameObject.GetComponent<Collider2D> ();
                if (hitCollider != null) {
                    Debug.Log ("Detected collider 2d on " + result.gameObject.name);
                    for (int i = 0; i < dragCompletedHandlers.Count; i++) {
                        DragCompletedCanvasEventhandler handler = dragCompletedHandlers[i] as DragCompletedCanvasEventhandler;
                        Debug.Log ("Checking a drag completed handler " + handler);
                        if (handler != null && handler.GetTargetColliders ().Contains (hitCollider) && handler.DraggableObjects.Contains (this)) {
                            Debug.Log ("Raising an event");
                            dragCompleted = true;
                            handler.IsOverTarget ();
                            handler.SetTargetCollider (result.gameObject.GetComponent<Collider2D> ());
                            eventDispatcher.Raise (new DragCompleted.DragCompletedEvent (this));
                            break;
                        }
                    }
                }
                if (dragCompleted) { // only check the first hit
                    break;
                }
            }
            if (!dragCompleted) {
                eventDispatcher.Raise (new DragCancelled.DragCancelledEvent (this));

                if (returnOnCancelled) {
                    LeanTween.move (gameObject, startingPosition, returnDuration).setEase (LeanTweenType.easeOutExpo);
                }
            } else if (returnOnCompleted) {
                LeanTween.move (gameObject, startingPosition, returnDuration).setEase (LeanTweenType.easeOutExpo);
            }
        }

        protected override void DoPointerEnter () {
            ChangeCursor (hoverCursor);
        }

        protected override void DoPointerExit () {
            SetMouseCursor.ResetMouseCursor ();
        }

        protected override void ChangeCursor (Texture2D cursorTexture) {
            if (!dragEnabled) {
                return;
            }

            Cursor.SetCursor (cursorTexture, Vector2.zero, CursorMode.Auto);
        }

        #region Public members

        /// <summary>
        /// Is object drag and drop enabled.
        /// </summary>
        /// <value><c>true</c> if drag enabled; otherwise, <c>false</c>.</value>
        public override bool DragEnabled { get { return dragEnabled; } set { dragEnabled = value; } }

        #endregion

    }
}