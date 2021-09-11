using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagement : MonoBehaviour
{
    public static ScoreManagement singleton;
    #region Private Variables
    private int m_CurScore;
    #endregion

    #region Initialization
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != null)
        {
            Destroy(gameObject);
        }
        m_CurScore = 0;
    }
    #endregion

    #region Score Methods
    public void IncreaseScore(int amount)
    {
        m_CurScore += amount;
    }
    private void UpdateHighScore()
    {
        if (!PlayerPrefs.HasKey("HS"))
        {
            PlayerPrefs.SetInt("HS", m_CurScore);
            return;
        }

        int hs = PlayerPrefs.GetInt("HS");
        if (m_CurScore > hs)
        {
            PlayerPrefs.SetInt("HS", m_CurScore);
        }
    }
    #endregion

    #region Destruction
    private void OnDisable()
    {
        UpdateHighScore();
    }

    #endregion
}
