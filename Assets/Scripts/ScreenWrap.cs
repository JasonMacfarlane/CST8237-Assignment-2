using UnityEngine;
using System.Collections;

/// <summary>
/// Wrap objects around the screen.
/// </summary>
public class ScreenWrap : MonoBehaviour
{
    // The camera bounds.
    private float _minX;    // Minimum 'x' coordinate
    private float _maxX;    // Maximum 'x' coordinate
    private float _minY;    // Minimum 'y' coordinate
    private float _maxY;    // Maximum 'y' coordinate

    /// <summary>
    /// Sets the camera bounds.
    /// </summary>
    void Start()
    {
        // Source: http://answers.unity3d.com/questions/501893/calculating-2d-camera-bounds.html

        // Get the camera bounds.
        float verticalBounds = Camera.main.GetComponent<Camera>().orthographicSize;
        float horizontalBounds = verticalBounds * Screen.width / Screen.height;

        // Set the minimum and maximum camera bounds.
        _minX = -horizontalBounds;
        _maxX = horizontalBounds;
        _minY = -verticalBounds;
        _maxY = verticalBounds;
    }

    /// <summary>
    /// Checks if the object is going out of bounds and place
    /// it on the oposite side.
    /// </summary>
    void FixedUpdate()
    {
        // Get the 'x' and 'y' coordinates.
        float x = transform.position.x;
        float y = transform.position.y;

        // If 'x' is less than the minimum 'x' coordinate,
        // set it to the maximum 'x' coordinate.
        if (x < _minX) x = _maxX;
        // If 'x' is greater than the maximum 'x' coordinate,
        // set it to the minimum 'x' coordinate.
        else if (x > _maxX) x = _minX;

        // Repeat the above for 'y'.
        if (y < _minY) y = _maxY;
        else if (y > _maxY) y = _minY;

        // Set the new position of this object.
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
