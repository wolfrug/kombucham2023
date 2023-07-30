using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fungus;
using UnityEngine;
 
namespace Fungus {
    [CommandInfo ("Custom",
        "De-Serialize String",
        "Attempts to de-serialize a string from a string serialized previously with the serialize command.")]
 
    [AddComponentMenu ("")]
    public class FungusDeSerializeString : Command {
 
        [Tooltip ("Serialized string, created using the Serialize String command")]
        [VariableProperty (typeof (StringVariable))]
        public StringVariable serializedString;
 
        [Tooltip ("Index of string you want to serialize")]
        public IntegerData indexOfString;
        [Tooltip ("Check if this string exists: if not, returns empty")]
        public StringData stringToCheck;
 
        [Tooltip ("Deserialized string, or empty if failed")]
        [VariableProperty (typeof (StringVariable))]
        public StringVariable outputstring;
 
        [Tooltip("Remove the output (if found) from the serialized string")]
        public BooleanData deleteOutput = new BooleanData(false);
 
        // These are the symbols that delimit each individual string, by default <> - these can't be used in the body of the string itself! (you can't use [] btw, unless you do some clever regex-canceling magic)
        private char delimiterLeft = '<';
        private char delimiterRight = '>';
 
        public override void OnEnter () {
 
            // Checks for anything between delimiter brackets and then sends the first match onward.
            Regex brackets = new Regex (delimiterLeft + ".*?" + delimiterRight);
            MatchCollection matches = brackets.Matches (serializedString.Value);
            int actualIndex = -1;
            // Only check index if we're not checking for a string value
            if (stringToCheck != "") {
                outputstring.Value = "";
                for (int i = 0; i < matches.Count; i++) {
                    // trim out the actual text
                    string ReqText = matches[i].Value.Trim (new Char[]{ delimiterLeft, delimiterRight, ' ' });
                    //ReqText = ReqText.Remove (0, 1);
                    //ReqText = ReqText.Remove (ReqText.Length - 1, 1);
                    if (ReqText == stringToCheck) {
                        outputstring.Value = stringToCheck;
                        actualIndex = i;
                        break;
                    }
                }
            } else {
                if (matches.Count > 0 && matches.Count >= indexOfString) {
                    // Trim out the actual string
                    string ReqText = matches[indexOfString].Value.Trim (new Char[]{ delimiterLeft, delimiterRight, ' ' });
                    outputstring.Value = ReqText;
                    actualIndex = indexOfString;
                } else {
                    outputstring.Value = "";
                }
            };
            // remove the string from the serialized input
            if (deleteOutput && outputstring.Value !="" && actualIndex >= 0){
                string newInputString = "";
                for (int i = 0; i < matches.Count;i++){
                    if (i != actualIndex){
                        newInputString+=matches[i];
                    }
                }
                serializedString.Value = newInputString;
            }
 
            Continue ();
        }
 
        public override string GetSummary () {
            if (outputstring == null) {
                return "No return string assigned!";
            }
            if (serializedString == null) {
                return "No input string assigned!";
            }
            if (stringToCheck == "") {
                return "Deserializing index " + indexOfString.Value;
            } else {
                return "Deserializing string " + stringToCheck;
            }
        }
    }
}