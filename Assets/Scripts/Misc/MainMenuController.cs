using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Editor
    [SerializeField]
    [Tooltip("The text component housing the current high score")]
    private Text m_Highscore;
    #endregion

    #region Private Variables
    private string m_DefaultHighScore;
    #endregion
    #region Initialization
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        m_DefaultHighScore = m_Highscore.text;
    }

    private void Start()
    {
        UpdateHighScore();
    }
    #endregion

    #region Play Button Methods
    public void PlayArena()
    {
        SceneManager.LoadScene("Arena");
    }
    #endregion

    #region General Application Button Methods
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region High Score Update
    private void UpdateHighScore()
    {
        if (PlayerPrefs.HasKey("HS"))
        {
            m_Highscore.text = m_DefaultHighScore.Replace("%s", PlayerPrefs.GetInt("HS").ToString());
        }
        else
        {
            m_Highscore.text = m_DefaultHighScore.Replace("%s", "0");
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HS", 0);
        UpdateHighScore();
    }

    #endregion
}
