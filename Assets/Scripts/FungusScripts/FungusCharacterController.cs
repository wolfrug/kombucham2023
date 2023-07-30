using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

namespace Fungus {
    [CommandInfo ("Custom",
        "Character Controller",
        "Allows you to easily control a game object using keyboard inputs.")]

    [AddComponentMenu ("")]
    public class FungusCharacterController : Command {

        [Tooltip ("The character that is to be controlled. Must have a Character Controller component")]
        public GameObjectData player;
        public FloatData playerSpeed = new FloatData (2.0f);
        public FloatData jumpHeight = new FloatData (1.0f);
        public FloatData turnSpeed = new FloatData (80f);
        public BooleanData useHorizontal = new BooleanData (true);
        public BooleanData useVertical = new BooleanData (true);
        [Tooltip ("Tank controls means you press W to move in the direction you are facing - ignores useOnlyHorizonal/useOnlyVerical")]
        public BooleanData useTankControls = new BooleanData (false);
        [Tooltip ("Number of jumps allowed consecutively. Use 1 for standard, 2 for double, or 0 for none.")]
        public IntegerData numberOfConsecutiveJumps = new IntegerData (1);
        [Tooltip ("If this is set to true, the player can control their direction while in the air")]
        public BooleanData airControl = new BooleanData (false);
        [Tooltip ("Only used in non-tank controls: turns the character's forward (blue arrow) in the travel direction")]
        public BooleanData turnTowardsMovementDirection = new BooleanData (true);
        public AnimatorData animator;
        [Tooltip ("Sets the float in the assigned Animator named this to the controller's current velocity")]
        public StringData animator_velocityFloat = new StringData ("velocity");
        [Tooltip ("Sets the float in the assigned Animator named this to the controller's current air speed (positive up, negative down)")]
        public StringData animator_jumpHeightFloat = new StringData ("jumpheight");

        [Tooltip ("Current movement speed")]
        [VariableProperty (typeof (FloatVariable))]
        public FloatVariable movementSpeedOut;
        [Tooltip ("Current jumping speed")]
        [VariableProperty (typeof (FloatVariable))]
        public FloatVariable jumpingSpeedOut;

        private Vector3 playerVelocity;
        private bool groundedPlayer;
        private float gravityValue = -9.81f;

        private int jumpNumbersLeft = 0;
        private CharacterController controller;

        void Start () {
            controller = player.Value.GetComponent<CharacterController> ();
        }

        public override void OnEnter () {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0) {
                playerVelocity.y = 0f;
                /*if (jumpNumbersLeft < numberOfConsecutiveJumps.Value) {
                    Debug.Log ("Reset jump numbers from " + jumpNumbersLeft);
                }*/
                jumpNumbersLeft = numberOfConsecutiveJumps.Value;
            }
            if (animator.Value != null && numberOfConsecutiveJumps > 0) {
                animator.Value.SetFloat (animator_jumpHeightFloat, playerVelocity.y);
            }
            if (jumpingSpeedOut != null) {
                jumpingSpeedOut.Value = playerVelocity.y;
            }
            if (useTankControls) {
                float canTurnInAir = groundedPlayer || airControl ? 1f : 0.1f;
                player.Value.transform.Rotate (0, Input.GetAxis ("Horizontal") * playerSpeed * (turnSpeed * canTurnInAir) * Time.deltaTime, 0);
                Vector3 movDir = player.Value.transform.forward * Input.GetAxis ("Vertical") * playerSpeed;
                // moves the character in horizontal direction
                if (movDir != Vector3.zero) {
                    controller.Move (movDir * Time.deltaTime);
                };
            } else {
                Vector3 move = new Vector3 (useHorizontal ? Input.GetAxis ("Horizontal") : 0f, 0, useVertical ? Input.GetAxis ("Vertical") : 0f);
                if (move != Vector3.zero) {
                    controller.Move (move * Time.deltaTime * playerSpeed);
                    if (turnTowardsMovementDirection) {
                        player.Value.transform.forward = move;
                    }
                }
            }
            if (animator.Value != null) {
                animator.Value.SetFloat (animator_velocityFloat, controller.velocity.magnitude);
            }
            if (movementSpeedOut != null) {
                movementSpeedOut.Value = controller.velocity.magnitude;
            }
            // Changes the height position of the player..
            if (Input.GetButtonDown ("Jump") && numberOfConsecutiveJumps > 0 && (groundedPlayer || jumpNumbersLeft > 0)) {
                playerVelocity.y += Mathf.Sqrt (jumpHeight * -3.0f * gravityValue);
                jumpNumbersLeft--;
                // Debug.Log ("Jumping. Jumps left: " + jumpNumbersLeft);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            if (playerVelocity.y != 0f) {
                controller.Move (playerVelocity * Time.deltaTime);
            };

            Continue ();

        }
        public override string GetSummary () {
            if (player.Value == null) {
                return "Assign a target object!";
            }
            if (player.Value.GetComponent<CharacterController> () == null) {
                return "No CharacterController component assigned to target object!";
            }
            return "Controlling " + player.Value.name;
        }
    }
}