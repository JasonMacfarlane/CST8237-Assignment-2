using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Ship and main game controls.
/// </summary>
public class Ship : MonoBehaviour
{
    // The main canvas.
    public Canvas canvas;

    // The ship, fire, speed, rotation and force.
    public Rigidbody2D ship;
    public GameObject fire;
    public float shipSpeed = 10.0f;
    public float shipRotationSpeed = 200.0f;
    public float shipForce = 5.0f;

    // The bullet and bullet speed.
    public GameObject bulletPrefab;
    public float bulletSpeed = 30.0f;

    // Number of lives and live icons.
    public int lives = 3;
    public GameObject lifeIconPrefab;

    // The current score, and current score and high score text.
    public static int score;
    public Text scoreText;
    public Text highScoreText;

    private Vector2 _shipDirection;         // Ship direction
    private List<GameObject> _lifeIcons;    // Live icons
    private Vector3 _lifeIconPosition;      // The next live icon position
    private static int _prevScore;          // The previous score
    private int _highScore;                 // The high score

    // Ship colors to alternate between when losing a life.
    private Color[] _shipColors = { Color.white, Color.black };

    /// <summary>
    /// Initializes the game.
    /// </summary>
    void Start()
    {
        // Initialize the score.
        score = 0;
        _prevScore = 0;

        // Get the current high score and set the high score text.
        _highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.GetComponent<Text>().text = Convert.ToString(_highScore);

        // Hide the fire.
        fire.GetComponent<Renderer>().enabled = false;

        // Create a new list to contain live icons and set their initial position.
        _lifeIcons = new List<GameObject>();
        _lifeIconPosition = new Vector3(30.0f, -39.0f);

        /*
         * For the number of lives, add a new life icon to the canvas,
         * and add the object to the life icons list.
         */
        for (int i = 0; i < lives; ++i)
        {
            GameObject liveIcon = (GameObject)Instantiate(lifeIconPrefab, _lifeIconPosition, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            liveIcon.transform.SetParent(canvas.transform, false);
            _lifeIcons.Add(liveIcon);
            // Increment the 'x' coordinate for the next life icon position by 50.
            _lifeIconPosition.x += 40.0f;
        }
    }

    /// <summary>
    /// Always check if player is shooting bullets and update the score.
    /// </summary>
    void Update()
    {
        // Shoot a bullet if the space bar is pressed down.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Create a new bullet and add force to is using the bullet speed.
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
        }

        /*
         * If the previous score is less than the current score,
         * set the score text to the current score and set the
         * previous score to the current score.
         */
        if (_prevScore < score)
        {
            scoreText.GetComponent<Text>().text = Convert.ToString(score);
            if (score > _highScore)
            {
                highScoreText.GetComponent<Text>().text = Convert.ToString(score);
            }
            _prevScore = score;
        }
    }

    /// <summary>
    /// Maneuvers the ship.
    /// </summary>
    void FixedUpdate()
    {
        // Rotate the ship to the left when the left arrow key is pressed.
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ship.angularVelocity = shipRotationSpeed;
        }
        // Rotate the ship to the right when the right arrow key is pressed.
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ship.angularVelocity = -shipRotationSpeed;
        }
        // Stop the rotation if the left and right arrow keys are not pressed.
        else
        {
            ship.angularVelocity = 0f;
        }

        // Move the ship forward by adding force to it when the up arrow is pressed.
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ship.AddForce(transform.up * shipForce);
            fire.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            fire.GetComponent<Renderer>().enabled = false;
        }
    }

    /// <summary>
    /// Actions when the ship collides with another object.
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        // If the object collided with is an asteroid.
        if (col.gameObject.tag == "AsteroidLarge" || col.gameObject.tag == "AsteroidSmall")
        {
            // Destroy the asteroid.
            Destroy(col.gameObject);
            // Destroy the last life icon.
            Destroy(_lifeIcons[lives - 1]);
            // Remove the last life icon from the life icons list.
            _lifeIcons.RemoveAt(lives - 1);
            // Decrement the lives.
            lives--;

            /*
             * If the player has no more lives, compare the current score
             * to the high score. If the current score is a higher number
             * than the current high score, set the current score to the
             * high score.
             */
            if (lives == 0)
            {
                // If the current score is higher than the high score,
                // set the high score to the current score.
                if (score > _highScore)
                {
                    PlayerPrefs.SetInt("HighScore", score);
                }

                // Load the menu scene.
                SceneManager.LoadScene("Menu");
            }

            // When a life is lost, flash the ship to indicate to the player
            // that the ship hit an asteroid.
            StartCoroutine(FlashShip(0.2f, 0.1f));
        }
    }
    
    /// <summary>
    /// Flash the ship.
    /// Source: http://stackoverflow.com/questions/16114349/make-player-flash-when-hit
    /// </summary>
    /// <param name="time">
    /// The time to flash the ship. However, the time is not accurate to the second,
    /// so it must be smaller than the actual wanted time.
    /// </param>
    /// <param name="intervalTime">The amount of time the ship is a certain color.</param>
    /// <returns></returns>
    IEnumerator FlashShip(float time, float intervalTime)
    {
        // The amount of time elapsed.
        float elapsedTime = 0.0f;
        int i = 0; // Loop counter.
        while (elapsedTime < time)
        {
            // Set the ship color to the next color in the ship colors array.
            this.GetComponent<Renderer>().material.color = _shipColors[i % 2];
            // Set the elapsed time.
            elapsedTime += Time.deltaTime;

            i++;
            yield return new WaitForSeconds(intervalTime);
        }

        this.GetComponent<Renderer>().material.color = Color.white;
    }
}
