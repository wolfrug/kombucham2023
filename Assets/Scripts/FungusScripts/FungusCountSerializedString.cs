using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fungus;
using UnityEngine;

namespace Fungus {
    [CommandInfo ("Custom",
        "Count Serialized String",
        "Returns an integer with the number of entries in a serialized string. Returns 0 if none were found.")]

    [AddComponentMenu ("")]
    public class FungusCountSerializedString : Command {

        [Tooltip ("Serialized string to count")]
        public StringData serializedString;

        [Tooltip ("Output integer - will return 0 if no entries are found or if an invalid string is entered.")]
        [VariableProperty (typeof (IntegerVariable))]
        public IntegerVariable stringCount;
        // These are the symbols that delimit each individual string, by default <> - these can't be used in the body of the string itself! (you can't use [] btw, unless you do some clever regex-canceling magic)
        private char delimiterLeft = '<';
        private char delimiterRight = '>';

        public override void OnEnter () {
            stringCount.Value = 0;
            if (serializedString != "") {
                // Checks for anything between delimiter brackets and then sends the first match onward.
                Regex brackets = new Regex (delimiterLeft + ".*?" + delimiterRight);
                MatchCollection matches = brackets.Matches (serializedString.Value);
                stringCount.Value = matches.Count;
            }
            Continue ();
        }

        public override string GetSummary () {
            if (serializedString == null) {
                return "No string assigned!";
            } else if (stringCount == null) {
                return "No output integer variable assigned!";
            }
            return "Counting serialized string " + serializedString;
        }
    }
}