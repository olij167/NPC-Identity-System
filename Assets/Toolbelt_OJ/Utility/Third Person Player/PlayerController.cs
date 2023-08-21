using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toolbelt_OJ
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerController : MonoBehaviour
    {
        public Camera cam;

        public float baseSpeed = 5f, sprintSpeed = 10f;

        [HideInInspector] public float moveSpeed;

        public float jumpForce = 4f;
        public bool isJumping;
        private CharacterController controller;

        [HideInInspector] public Vector3 moveDirection;
        public float gravScale = 1.0f;

        public List<AudioClip> footstepSounds, jumpSounds;
        AudioSource audioSource;

        void Start()
        {
            moveSpeed = baseSpeed;

            controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;

            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            if (controller.isGrounded && !isJumping)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;

                    isJumping = true;


                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }

                if (controller.velocity.magnitude > 2f && !audioSource.isPlaying)
                {

                    audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Count)]);
                }

            }
            else if (controller.isGrounded && isJumping)
            {
                audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Count)]);
                isJumping = false;
            }
            else if (!controller.isGrounded)
            {
                isJumping = true;
            }

            switch (Input.GetKey(KeyCode.LeftShift))
            {
                case true:
                    {
                        moveSpeed = sprintSpeed;
                        audioSource.pitch = Random.Range(1.1f, 1.3f);
                        audioSource.volume = Random.Range(0.55f, 0.65f);
                        break;
                    }
                case false:
                    {
                        moveSpeed = baseSpeed;
                        audioSource.volume = Random.Range(0.45f, 0.55f);
                        audioSource.pitch = Random.Range(.8f, 1f);
                        break;
                    }
            }


            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravScale * Time.deltaTime);
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}

