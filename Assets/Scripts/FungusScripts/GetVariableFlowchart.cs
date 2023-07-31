using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

namespace Fungus {
    [CommandInfo ("Custom",
        "Get Variable (Bool)",
        "Gets the named variable from the given flowchart and saves it into the given output variable.")]
    [AddComponentMenu ("")]
    public class GetVariableFlowchart : Command {

        public GameObjectData targetFlowchart;
        public StringData variableName;
        [VariableProperty (typeof (BooleanVariable))]
        public BooleanVariable variableOut;

        public override void OnEnter () {
            if (variableOut != null && targetFlowchart.Value != null) {
                variableOut.Value = targetFlowchart.Value.GetComponent<Flowchart>().GetBooleanVariable (variableName);
            }
            Continue ();
        }
    }
}