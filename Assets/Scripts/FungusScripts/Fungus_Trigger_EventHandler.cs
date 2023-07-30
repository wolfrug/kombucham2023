using System.Collections;
using Fungus;
using UnityEngine;

[EventHandlerInfo ("Custom",
    "Trigger",
    "Assign a trigger (2D or 3D) and a tag to trigger this eventhandler")]
[AddComponentMenu ("")]
public class Fungus_Trigger_EventHandler : BasePhysicsEventHandler {

    /*
    Raising the event:
    Fungus.FungusManager.Instance.EventDispatcher.Raise(new Custom_Fungus_EventHandler.Custom_EventHandlerEvent() { text = "whatever" });
     */

    public FungusTrigger target;
    [Tooltip ("Optional variable to store the game object carrying the collider that caused the trigger to occur.")]
    [VariableProperty (typeof (GameObjectVariable))]
    [SerializeField] protected GameObjectVariable colliderVar;

    public class Fungus_Trigger_EventHandlerEvent {
        public FungusTrigger trigger;
        public Collider target;
        public Collider2D target2D;
        public PhysicsMessageType collisionType;
    }

    protected EventDispatcher eventDispatcher;

    protected virtual void OnEnable () {
        eventDispatcher = FungusManager.Instance.EventDispatcher;

        eventDispatcher.AddListener<Fungus_Trigger_EventHandlerEvent> (OnFungus_Trigger_EventHandlerEvent);
    }

    protected virtual void OnDisable () {
        eventDispatcher.RemoveListener<Fungus_Trigger_EventHandlerEvent> (OnFungus_Trigger_EventHandlerEvent);

        eventDispatcher = null;
    }

    void OnFungus_Trigger_EventHandlerEvent (Fungus_Trigger_EventHandlerEvent evt) {
        if (evt.trigger == target) {
            if ((evt.collisionType & FireOn) != 0 && DoesPassFilter (evt.target.tag)) {
                if (colliderVar != null) {
                    if (evt.target != null) {
                        colliderVar.Value = evt.target.gameObject;
                    } else if (evt.target2D != null) {
                        colliderVar.Value = evt.target2D.gameObject;
                    }
                }
                evt.trigger.OnTriggered (evt.target.gameObject, evt.collisionType);
                ExecuteBlock ();
            }
        }
    }
}