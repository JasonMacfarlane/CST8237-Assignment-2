using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Spawn an asteroid.
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    // Array of asteroid prefabs.
	public GameObject[] asteroidPrefabs;
    // A static list of asteroids.
	private static List<GameObject> asteroidsList;
    // The number of seconds before a new asteroid appears.
    public float asteroidSpawnIntervalSeconds = 3.0f;
    // The initial number of asteroids.
    public int initialNumOfAsteroids = 4;

    // The camera bounds.
    private float _minX;    // Minimum 'x' coordinate
    private float _maxX;    // Maximum 'x' coordinate
    private float _minY;    // Minimum 'y' coordinate
    private float _maxY;    // Maximum 'y' coordinate

    // The camera sides.
    private enum Sides
    {
        Left,
        Top,
        Right,
        Bottom
    };

    /// <summary>
    /// Spawns asteroids.
    /// </summary>
    void Start()
    {
        asteroidsList = new List<GameObject>();

        // Add each asteroid prefab to the static asteroids list.
		for (int i = 0; i < asteroidPrefabs.Length; ++i)
        {
			asteroidsList.Add(asteroidPrefabs[i]);
		}

        // Source: http://answers.unity3d.com/questions/501893/calculating-2d-camera-bounds.html

        // Get the camera bounds.
        float verticalBounds = Camera.main.GetComponent<Camera>().orthographicSize;
        float horizontalBounds = verticalBounds * Screen.width / Screen.height;

        // Set the minimum and maximum camera bounds.
        _minX = -horizontalBounds;
        _maxX = horizontalBounds;
        _minY = -verticalBounds;
        _maxY = verticalBounds;

        // Spawn an initial number of asteroids.
        for (int i = 0; i < initialNumOfAsteroids; ++i)
        {
            CreateAsteroid();
        }

        // Spawn a new asteroid every "asteroidSpawnIntervalSeconds" seconds.
        InvokeRepeating("CreateAsteroid", 1.0f, asteroidSpawnIntervalSeconds);
    }

    /// <summary>
    /// Creates a new asteroid.
    /// </summary>
    private void CreateAsteroid()
    {
        // Get a random side to create the new asteroid.
        //Sides sideToSpawn = (Sides)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Sides)).Length-1);
        Sides sideToSpawn = Sides.Left;
        
        // The asteroid's position.
        Vector3 asteroidPosition;

        // Get random 'x' and 'y' coordinates.
        float x = UnityEngine.Random.Range(_minX, _maxX);
        float y = UnityEngine.Random.Range(_minY, _maxY);

        /*
         * Set the asteroid position to a position 20 units
         * outside of the side to spawn.
         */
        switch(sideToSpawn)
        {
            case Sides.Left:
                asteroidPosition = new Vector3(_minX - 20.0f, y);
                break;
            case Sides.Top:
                asteroidPosition = new Vector3(x, _minY - 20.0f);
                break;
            case Sides.Right:
                asteroidPosition = new Vector3(_minX - 20.0f, y);
                break;
            case Sides.Bottom:
                asteroidPosition = new Vector3(x, _maxY + 20.0f);
                break;
            default:
                asteroidPosition = new Vector3(0, 0);
                break;
        }

        // Instantiate a random asteroid in the new position calculated above.
        SpawnAsteroid(
            asteroidPosition, 
            Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f)), 
            new Vector3(1, 1, 0), 
            "AsteroidLarge"
            );
    }

    /// <summary>
    /// Gets a random anstroid from the static asteroids list.
    /// </summary>
    /// <returns>The asteroid</returns>
    private static GameObject GetRandomAsteroid()
    {
        return asteroidsList[UnityEngine.Random.Range(0, asteroidsList.Count - 1)];
    }

    /// <summary>
    /// Spawns a new asteroid.
    /// </summary>
    /// <param name="position">The position where the asteroid should be spawned.</param>
    /// <param name="quaternion">The asteroid rotation.</param>
    /// <param name="size">The asteroid size.</param>
    /// <param name="tag">The asteroid tag.</param>
    public static void SpawnAsteroid(Vector3 position, Quaternion quaternion, Vector3 size, string tag)
    {
        GameObject asteroid = (GameObject)Instantiate(GetRandomAsteroid(), position, quaternion);
        // Set the asteroid size and tag.
        asteroid.transform.localScale = size;
        asteroid.tag = tag;
    }
}
