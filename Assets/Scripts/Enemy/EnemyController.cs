using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How much health does this enemy have")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("How fast the enemy can move")]
    private float m_Speed;

    [SerializeField]
    [Tooltip("Approximate amount of damage dealt per frame")]
    private ParticleSystem m_DeathExplosion;

    [SerializeField]
    [Tooltip("Death explosion effect")]
    private float m_Damage;

    [SerializeField]
    [Tooltip("Probability that the enemy drops a health pill")]
    private float m_HealthPillDroprate;

    [SerializeField]
    [Tooltip("Type of pill this enemy drops")]
    private GameObject m_HealthPillType;

    [SerializeField]
    [Tooltip("How many points an enemy gives on death")]
    private int m_Score;
    #endregion

    #region Private Variables
    private float p_curHealth;
    #endregion

    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    #region Cached References
    private Transform cr_Player;
    #endregion

    #region Initialization
    private void Awake()
    {
        p_curHealth = m_MaxHealth;
        cc_Rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cr_Player = FindObjectOfType<PlayerController>().transform;
    }
    #endregion

    #region Main Updates
    private void FixedUpdate()
    {
        Vector3 fixedDir = cr_Player.position - transform.position;
        fixedDir.Normalize();
        cc_Rb.MovePosition(transform.position + fixedDir * m_Speed * Time.fixedDeltaTime);

    }
    #endregion

    #region Collision
    private void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DecreaseHealth(m_Damage);
        }

    }
    #endregion

    #region Health
    public void DecreaseHealth(int health)
    {
        p_curHealth -= health;
        if (p_curHealth <= 0)
        {
            ScoreManagement.singleton.IncreaseScore(m_Score);
            if (Random.value < m_HealthPillDroprate)
            {
                Instantiate(m_HealthPillType, transform.position, Quaternion.identity);
            }
            Instantiate(m_DeathExplosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    #endregion

}
