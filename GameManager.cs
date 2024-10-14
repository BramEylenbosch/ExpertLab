using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;
        public Text highScoreText;  // UI text for displaying high score

        private int highScore;  // Variable to store the high score

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();

            // Load the saved high score (if any)
            highScore = PlayerPrefs.GetInt("HighScore", 0);

            // Update the UI with the current high score
            highScoreText.text = highScore.ToString();
        }

        void Update()
        {
            coinText.text = coinsCounter.ToString();

            // Check if current score surpasses the high score
            if (coinsCounter > highScore)
            {
                highScore = coinsCounter;  // Update the high score
                highScoreText.text = highScore.ToString();  // Update UI text

                // Save the new high score
                PlayerPrefs.SetInt("HighScore", highScore);
            }

            // Handle player death
            if (player.deathState == true)
            {
                playerGameObject.SetActive(false);
                GameObject deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);
                player.deathState = false;
                Invoke("ReloadLevel", 3);
            }
        }

        private void ReloadLevel()
        {
            // Reload the current level
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}