using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

namespace Fungus {
    [CommandInfo ("Custom",
        "Character Controller (AI)",
        "Allows you to control an entity using a transform as a target location.")]

    [AddComponentMenu ("")]
    public class FungusAICharacterController : Command {

        [Tooltip ("The character that is to be controlled. Must have a Character Controller component")]
        public GameObjectData player;
        [Tooltip ("A transform that is the target of the move - the transform can itself be moving")]
        public TransformData targetLocation;

        [Tooltip ("Simply transform translate the target instead of using the character controller.")]
        public BooleanData useSimpleMove = new BooleanData (false);
        public FloatData playerSpeed = new FloatData (2.0f);
        //public FloatData jumpHeight = new FloatData (1.0f);
        // public BooleanData useOnlyHorizontal;
        // public BooleanData useOnlyVertical;
        //public BooleanData useJump = new BooleanData (true);
        public BooleanData turnTowardsMovementDirection = new BooleanData (true);
        public AnimatorData animator;
        public StringData animator_velocityFloat = new StringData ("velocity");
        //public StringData animator_jumpHeightFloat = new StringData ("jumpheight");

        private Vector3 playerVelocity;
        private bool groundedPlayer;
        private float gravityValue = -9.81f;
        private CharacterController controller;

        void Start () {
            controller = player.Value.GetComponent<CharacterController> ();
        }

        public override void OnEnter () {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0) {
                playerVelocity.y = 0f;
            }
            if (animator.Value != null) {
                //animator.Value.SetFloat (animator_jumpHeightFloat, playerVelocity.y);
            }
            Vector3 prevPos = player.Value.transform.position;
            Vector3 move = targetLocation.Value.position - player.Value.transform.position;

            if (useSimpleMove) {
                controller.transform.Translate (move.normalized * Time.deltaTime * playerSpeed);
            } else {
                controller.Move (move.normalized * Time.deltaTime * playerSpeed);
            }
            if (animator.Value != null) {
                animator.Value.SetFloat (animator_velocityFloat, controller.velocity.magnitude);
            }

            if (move != Vector3.zero && turnTowardsMovementDirection) {
                player.Value.transform.LookAt (targetLocation.Value);
            }

            // Changes the height position of the player..
            /*if (Input.GetButtonDown ("Jump") && groundedPlayer && useJump) {
                playerVelocity.y += Mathf.Sqrt (jumpHeight * -3.0f * gravityValue);
            }
*/
            if (!useSimpleMove) {
                playerVelocity.y += gravityValue * Time.deltaTime;
                controller.Move (playerVelocity * Time.deltaTime);
            }
            Continue ();

        }
    }
}