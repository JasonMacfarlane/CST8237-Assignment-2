using UnityEngine;

/// <summary>
/// An asteroid.
/// </summary>
public class Asteroid : MonoBehaviour 
{
    public float minForce = 1.0f;       // Minimum asteroid force
    public float maxForce = 5.0f;       // Maximum asteroid force

    /// <summary>
    /// Initializes the asteroid.
    /// </summary>
	void Start() 
    {
        // Add force to the asteroid.
        GetComponent<Rigidbody2D>().AddForce(Random.Range(minForce, maxForce) * new Vector2(transform.position.x, transform.position.y));
	}
}
