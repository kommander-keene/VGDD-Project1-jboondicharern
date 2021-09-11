using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Part of the health that decreases")]
    private RectTransform m_Healthbar;
    #endregion

    #region Private Variables
    private float p_HealthBarOriginalWidth;
    #endregion

    #region Initialization
    private void Awake()
    {
        p_HealthBarOriginalWidth = m_Healthbar.sizeDelta.x;
    }
    #endregion

    #region Update Health Bar
    public void UpdateHealth(float percent)
    {

        m_Healthbar.sizeDelta = new Vector2(p_HealthBarOriginalWidth * percent, m_Healthbar.sizeDelta.y);
    }
    #endregion
}
