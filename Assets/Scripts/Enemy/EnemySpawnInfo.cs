using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Name of this enemy")]
    private string m_Name;
    public string EnemyName
    {
        get
        {
            return m_Name;
        }
    }

    [SerializeField]
    [Tooltip("Prefab of this enemy to spawn")]
    private GameObject m_EnemyGO;
    public GameObject EnemyGO
    {
        get
        {
            return m_EnemyGO;
        }
    }

    [SerializeField]
    [Tooltip("Time before next enemy is spawned")]
    private float m_TimeUntilNextSpawn;
    public float TimeUntilNextSpawn
    {
        get
        {
            return m_TimeUntilNextSpawn;
        }
    }

    [SerializeField]
    [Tooltip("Number of enemies to spawn, if 0 endless mode")]
    private int m_NumberToSpawn;
    public int NumberToSpawn
    {
        get
        {
            return m_NumberToSpawn;
        }
    }
    #endregion
}
