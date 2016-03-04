using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// The main game menu.
/// </summary>
public class Menu : MonoBehaviour {
    public Button playButton;   // The play button
    public Text highScoreText;  // The high score text

	/// <summary>
	/// Initializes the menu.
	/// </summary>
	void Start() 
    {
        // Set the high score text.
        highScoreText.text = Convert.ToString(PlayerPrefs.GetInt("HighScore"));
	}

    /// <summary>
    /// Loads the main scene.
    /// </summary>
    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }
}
