using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

/// <summary>
/// Does the collection contain the given variable
/// </summary>
[CommandInfo ("Collection",
    "Contains If",
    "Does the collection contain the given variable (works as an If command)\nWarning: does NOT work with Else If (for obvious reasons)")]
[AddComponentMenu ("")]
public class CollectionContainsIf : CollectionBaseVarCommand {

    public override void OnEnter () {
        OnEnterInner ();
    }
    protected override void OnEnterInner () {

        bool condition = collection.Value.Contains (variableToUse);
        //Debug.Log ("Condition is: " + condition.ToString ());
        if (condition) {
            OnTrue ();
        } else {
            OnFalse ();
        }

        //StartCoroutine(WaitUntilTrue());
    }

    protected virtual void OnTrue () {
        Continue ();
    }

    /// <summary>
    /// Called when the condition is run and EvaluateCondition returns false 
    /// </summary>
    protected virtual void OnFalse () {

        // Find the next Else, ElseIf or End command at the same indent level as this If command
        for (int i = CommandIndex + 1; i < ParentBlock.CommandList.Count; ++i) {
            Command nextCommand = ParentBlock.CommandList[i];
            //Debug.Log ("Checking command: " + nextCommand);
            if (nextCommand == null) {
                continue;
            }

            // Find next command at same indent level as this If command
            // Skip disabled commands, comments & labels
            if (!((Command) nextCommand).enabled ||
                nextCommand.GetType () == typeof (Comment) ||
                nextCommand.GetType () == typeof (Label) ||
                nextCommand.IndentLevel != indentLevel) {
                //Debug.Log ("Indent level wrong (nextCommand/ours) " + nextCommand.IndentLevel + indentLevel);
                continue;
            }

            System.Type type = nextCommand.GetType ();
            if (type == typeof (Else) ||
                type == typeof (End)) {
                if (i >= ParentBlock.CommandList.Count - 1) {
                    // Last command in Block, so stop
                    //Debug.Log ("Last command in Block, so stop");
                    StopParentBlock ();
                } else {
                    // Execute command immediately after the Else or End command
                    //Debug.Log ("Execute command immediately after the Else or End command");
                    Continue (nextCommand.CommandIndex + 1);
                    return;
                }
            } else if (type == typeof (ElseIf)) {
                Debug.LogWarning ("Collection If does NOT support Else If! Sorry!");
            }
        }

        // No matching End command found, so just stop the block
        StopParentBlock ();
    }
    public override bool OpenBlock () {
        return true;
    }

    public override Color GetButtonColor () {
        return new Color32 (253, 253, 150, 255);
    }
    public override string GetSummary () {
        if (collection.Value == null) {
            return "No collection set!";
        } else if (variableToUse == null) {
            return "No variable set!";
        } else {
            return "If " + collection.collectionRef.Key + " contains " + variableToUse.Key;
        }
    }

}