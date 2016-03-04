using UnityEngine;

/// <summary>
/// A bullet.
/// </summary>
public class Bullet : MonoBehaviour 
{
    // The amount of time left before the bullet self destructs.
    private float _timeLeft = 1.0f;
    // The amount to increase the score by when the bullet hits an asteroid.
    private int _scoreIncrement = 100;

	private Asteroid asteroid;

	/// <summary>
    /// When the amount of time before the bullet destroy's itself
    /// has passed, destroy this bullet.
    /// </summary>
    void Update()
    {
        // Set the amount of time left.
        _timeLeft -= Time.deltaTime;

        // Destroy the bullet when the bullet's life time has elapsed.
        if (_timeLeft <= 0)
        {
            Destroy(gameObject);
        }
	}

    /// <summary>
    /// Actions when the bullet collides with another object.
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        /*
         * If the bullet collides with a large asteroid, increase
         * the score and spawn a smaller asteroid in the same location.
         * If the bullet collides with a smaller asteroid, increase
         * the score and destroy the small asteroid.
         */
        if (col.gameObject.tag == "AsteroidLarge")
        {
            // Increase the score.
            Ship.score += _scoreIncrement;
            // Destroy this bullet and the asteroid it collided with.
            Destroy(gameObject);
            // Set the new width and height of the new asteroid to spawn from the bigger one.
            float x = col.gameObject.GetComponent<Renderer>().bounds.size.x / 4.5f;
            float y = col.gameObject.GetComponent<Renderer>().bounds.size.y / 4.5f;
            // Spawn the new smaller asteroid.
            AsteroidSpawner.SpawnAsteroid(
                col.gameObject.transform.position, 
                Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f)), 
                new Vector3(x, y, 0), 
                "AsteroidSmall"
                );
            // Destroy the bigger asteroid.
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "AsteroidSmall")
        {
            Ship.score += _scoreIncrement;
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }
}
