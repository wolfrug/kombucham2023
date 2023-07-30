using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.Events;

public class FungusTrigger : MonoBehaviour {

    public UnityEvent<GameObject> onTriggered;

    public UnityEvent<GameObject> onEntered;

    public UnityEvent<GameObject> onExited;

    public UnityEvent<GameObject> onStay;

    void OnTriggerEnter (Collider coll) {
        FungusManager.Instance.EventDispatcher.Raise (new Fungus_Trigger_EventHandler.Fungus_Trigger_EventHandlerEvent () { target = coll, collisionType = BasePhysicsEventHandler.PhysicsMessageType.Enter, trigger = this });
    }
    void OnTriggerExit (Collider coll) {
        FungusManager.Instance.EventDispatcher.Raise (new Fungus_Trigger_EventHandler.Fungus_Trigger_EventHandlerEvent () { target = coll, collisionType = BasePhysicsEventHandler.PhysicsMessageType.Exit, trigger = this });
    }
    void OnTriggerStay (Collider coll) {
        FungusManager.Instance.EventDispatcher.Raise (new Fungus_Trigger_EventHandler.Fungus_Trigger_EventHandlerEvent () { target = coll, collisionType = BasePhysicsEventHandler.PhysicsMessageType.Stay, trigger = this });
    }
    void OnTriggerEnter2D (Collider2D coll) {
        FungusManager.Instance.EventDispatcher.Raise (new Fungus_Trigger_EventHandler.Fungus_Trigger_EventHandlerEvent () { target2D = coll, collisionType = BasePhysicsEventHandler.PhysicsMessageType.Enter, trigger = this });
    }
    void OnTriggerExit2D (Collider2D coll) {
        FungusManager.Instance.EventDispatcher.Raise (new Fungus_Trigger_EventHandler.Fungus_Trigger_EventHandlerEvent () { target2D = coll, collisionType = BasePhysicsEventHandler.PhysicsMessageType.Exit, trigger = this });
    }
    void OnTriggerStay2D (Collider2D coll) {
        FungusManager.Instance.EventDispatcher.Raise (new Fungus_Trigger_EventHandler.Fungus_Trigger_EventHandlerEvent () { target2D = coll, collisionType = BasePhysicsEventHandler.PhysicsMessageType.Stay, trigger = this });
    }

    public void OnTriggered (GameObject targetCollider, BasePhysicsEventHandler.PhysicsMessageType type) {
        onTriggered.Invoke (targetCollider);
        if (type == BasePhysicsEventHandler.PhysicsMessageType.Enter) {
            OnEntered (targetCollider);
        }
        if (type == BasePhysicsEventHandler.PhysicsMessageType.Exit) {
            OnExited (targetCollider);
        }
        if (type == BasePhysicsEventHandler.PhysicsMessageType.Stay) {
            OnStay (targetCollider);
        }
    }
    void OnEntered (GameObject targetCollider) {
        onEntered.Invoke (targetCollider);
    }
    void OnExited (GameObject targetCollider) {
        onExited.Invoke (targetCollider);
    }
    void OnStay (GameObject targetCollider) {
        onStay.Invoke (targetCollider);
    }
}