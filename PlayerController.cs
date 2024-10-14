using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        // New variable to store the player's SpriteRenderer
        private SpriteRenderer spriteRenderer;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            // Get the SpriteRenderer component
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            if (Input.GetButton("Horizontal"))
            {
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // Turn on run animation
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            if (!isGrounded) animator.SetInteger("playerState", 2); // Turn on jump animation

            if (facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInput < 0)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
        }

        // Modified method to change the player's color with an option for red or green
        // Method to change the player's color with an option for red or green with different coin costs
        public void ChangeColor(string color)
        {
            // Deduct different amounts of coins for different colors
            int coinsNeeded = color.ToLower() == "green" ? 2 : 3; // Green costs 2 coins, Red costs 3 coins

            // Check if player has enough coins for the selected color
            if (gameManager.coinsCounter >= coinsNeeded)
            {
                // Deduct the required number of coins
                gameManager.coinsCounter -= coinsNeeded;

                // Change the player's color based on the selected option
                if (spriteRenderer != null)
                {
                    if (color.ToLower() == "red")
                    {
                        spriteRenderer.color = Color.red;
                    }
                    else if (color.ToLower() == "green")
                    {
                        spriteRenderer.color = Color.green;
                    }
                    else
                    {
                        Debug.Log("Invalid color option. Please choose 'red' or 'green'.");
                    }
                }
            }
            else
            {
                // Display message or feedback if not enough coins
                Debug.Log($"Not enough coins to change color! You need {coinsNeeded} coins for {color}.");
            }
        }

    }
}
