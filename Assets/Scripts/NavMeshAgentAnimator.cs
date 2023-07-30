using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentAnimator : MonoBehaviour {
    public Animator targetAnimator;
    public NavMeshAgent targetAgent;
    public string speedVarName = "velocity";
    // Start is called before the first frame update

    // Update is called once per frame
    void Update () {
        targetAnimator.SetFloat (speedVarName, targetAgent.velocity.magnitude);
    }
}