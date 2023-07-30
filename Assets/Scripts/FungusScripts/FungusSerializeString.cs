using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
 
namespace Fungus {
    [CommandInfo ("Custom",
        "Serialize String",
        "Adds a string to an output string in a form that can be saved natively.")]
 
    [AddComponentMenu ("")]
    public class FungusSerializeString : Command {
 
        [Tooltip ("String to serialize")]
        public StringData stringToSerialize;
 
        [Tooltip ("Output string - use the same variable to serialize multiple strings (start with an empty string)")]
        [VariableProperty (typeof (StringVariable))]
        public StringVariable serializedString;
        // These are the symbols that delimit each individual string, by default <> - these can't be used in the body of the string itself! (you can't use [] btw, unless you do some clever regex-canceling magic)
        private char delimiterLeft = '<';
        private char delimiterRight = '>';
 
        public override void OnEnter () {
            if (stringToSerialize != "") {
                serializedString.Value += delimiterLeft + stringToSerialize + delimiterRight;
            }
            Continue ();
        }
 
        public override string GetSummary () {
            if (serializedString == null) {
                return "No return string assigned!";
            }
            return "Serializing " + stringToSerialize;
        }
    }
}