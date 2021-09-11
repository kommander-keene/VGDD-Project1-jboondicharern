using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackInfo
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Name for this attack")]
    private string m_Name;
    public string Name
    {
        get
        {
            return m_Name;
        }
    }

    [SerializeField]
    [Tooltip("The button to press that will use this attack. This button must be in input settings")]
    private string m_Button;
    public string Button
    {
        get
        {
            return m_Button;
        }
    }

    [SerializeField]
    [Tooltip("The trigger string to use to activate this attack in the animator")]
    private string m_TriggerName;
    public string TriggerName
    {
        get
        {
            return m_TriggerName;
        }
    }

    [SerializeField]
    [Tooltip("The prefab of the game object representing this ability")]
    private GameObject m_AbilityGO;
    public GameObject AbilityGO
    {
        get
        {
            return m_AbilityGO;
        }
    }

    [SerializeField]
    [Tooltip("Where to spawn the ability game object with respect to the player")]
    private Vector3 m_Offset;
    public Vector3 Offset
    {
        get
        {
            return m_Offset;
        }
    }

    [SerializeField]
    [Tooltip("How long to wait before this attack should be activated after the button is pressed")]
    private float m_WindupTime;
    public float WindupTime
    {
        get
        {
            return m_WindupTime;
        }
    }
    [SerializeField]
    [Tooltip("How long before the player can do anything again")]
    private float m_FrozenTime;
    public float FrozenTime
    {
        get
        {
            return m_FrozenTime;
        }
    }

    [SerializeField]
    [Tooltip("How long to wait before the attack is usable again")]
    private float m_Cooldown;

    [SerializeField]
    [Tooltip("The amount of health this ability costs")]
    private int m_HealthCost;
    public float HealthCost
    {
        get
        {
            return m_HealthCost;
        }
    }

    [SerializeField]
    [Tooltip("Color of this ability")]
    private Color m_Color;
    public Color AbilityColor
    {
        get
        {
            return m_Color;
        }
    }


    #endregion

    #region Public Variables
    public float Cooldown
    {
        get;
        set;
    }
    #endregion

    #region Cooldown Methods
    public void ResetCooldown()
    {
        Cooldown = m_Cooldown;
    }

    public bool isReady()
    {
        return Cooldown <= 0;
    }
    #endregion
}
