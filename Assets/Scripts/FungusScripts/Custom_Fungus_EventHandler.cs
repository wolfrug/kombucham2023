using UnityEngine;
using System.Collections;
using Fungus;

[EventHandlerInfo("Custom",
                      "Custom Eventhandler",
                      "Description")]
[AddComponentMenu("")]
public class Custom_Fungus_EventHandler : EventHandler {

/*
Raising the event:
Fungus.FungusManager.Instance.EventDispatcher.Raise(new Custom_Fungus_EventHandler.Custom_EventHandlerEvent() { text = "whatever" });
 */

    public string target;

    public class Custom_EventHandlerEvent {
        public string text;
    }

    protected EventDispatcher eventDispatcher;

    protected virtual void OnEnable() {
        eventDispatcher = FungusManager.Instance.EventDispatcher;

        eventDispatcher.AddListener<Custom_EventHandlerEvent>(OnCustom_EventHandlerEvent);
    }

    protected virtual void OnDisable() {
        eventDispatcher.RemoveListener<Custom_EventHandlerEvent>(OnCustom_EventHandlerEvent);

        eventDispatcher = null;
    }

    void OnCustom_EventHandlerEvent(Custom_EventHandlerEvent evt) {

        Debug.Log("Event raised: " + evt.text);
        if (evt.text == target) {
            ExecuteBlock();
            return;
            };
    }
}