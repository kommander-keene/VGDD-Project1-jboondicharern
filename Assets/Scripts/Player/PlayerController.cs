using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{


    #region Editor Variables
    [SerializeField]
    [Tooltip("How fast player moves around")]
    private float m_Speed;

    [SerializeField]
    [Tooltip("Transform of camera following player")]
    private Transform m_CameraTransform;

    [SerializeField]
    [Tooltip("List of attacks and info about them")]
    private PlayerAttackInfo[] m_Attacks;

    [SerializeField]
    [Tooltip("Max Health of a Player")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("The HUD Script")]
    private HUDController m_HUD;
    #endregion

    #region Cached References
    private Animator cr_Anim;
    private Renderer cr_Renderer;
    #endregion

    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    #region Private Variables
    private Vector2 p_Velocity;
    private float p_FrozenTimer;

    private Color p_DefaultColor;
    private float p_CurHealth;
    #endregion

    #region Initialization
    private void Awake()
    {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody>();
        cr_Anim = GetComponent<Animator>();
        cr_Renderer = GetComponentInChildren<Renderer>();
        p_DefaultColor = cr_Renderer.material.color;

        p_FrozenTimer = 0f;
        p_CurHealth = m_MaxHealth;

        for (int i = 0; i < m_Attacks.Length; i++)
        {
            PlayerAttackInfo attack = m_Attacks[i];
            attack.Cooldown = 0f;

            if (attack.WindupTime > attack.FrozenTime)
            {
                Debug.Log(attack.Name + " has a windup longer than its frozen time");
            }
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Main Update
    // Update is called once per frame
    void Update()
    {
        if (p_FrozenTimer > 0)
        {
            p_Velocity = Vector2.zero;
            p_FrozenTimer -= Time.deltaTime;
            return;
        }
        else
        {
            p_FrozenTimer = 0;
        }

        for (int i = 0; i < m_Attacks.Length; i++)
        {
            PlayerAttackInfo attack = m_Attacks[i];

            if (attack.isReady())
            {
                if (Input.GetButtonDown(attack.Button))
                {
                    DecreaseHealth(attack.HealthCost);
                    p_FrozenTimer = attack.FrozenTime;
                    StartCoroutine(UseAttack(attack));
                    break;
                }
            }
            else if (attack.Cooldown > 0)
            {
                attack.Cooldown -= Time.deltaTime;
            }
        }

        float forwards = Input.GetAxis("Vertical");
        float right = Input.GetAxis("Horizontal");

        cr_Anim.SetFloat("Speed", Mathf.Clamp01(Mathf.Abs(forwards) + Mathf.Abs(right)));

        float moveThreshold = 0.3f;

        if (forwards > 0 && forwards < moveThreshold)
        {
            forwards = 0;
        }
        else if (forwards < 0 && forwards > -moveThreshold)
        {
            forwards = 0;
        }

        if (right > 0 && right < moveThreshold)
        {
            right = 0;
        }
        else if (right < 0 && right > -moveThreshold)
        {
            right = 0;
        }

        p_Velocity.Set(right, forwards);
    }

    void FixedUpdate()
    {
        cc_Rb.MovePosition(cc_Rb.position + m_Speed * Time.fixedDeltaTime * transform.forward * p_Velocity.magnitude);
        cc_Rb.angularVelocity = Vector3.zero;

        if (p_Velocity.sqrMagnitude > 0)
        {
            float angleToRotCam = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, p_Velocity);
            Vector3 camForwards = m_CameraTransform.forward;

            Vector3 newRot = new Vector3(Mathf.Cos(angleToRotCam) * camForwards.x - Mathf.Sin(angleToRotCam) * camForwards.z, 0, Mathf.Cos(angleToRotCam) * camForwards.z + Mathf.Sin(angleToRotCam) * camForwards.x);
            float theta = Vector3.SignedAngle(transform.forward, newRot, Vector3.up);
            cc_Rb.rotation = Quaternion.Slerp(cc_Rb.rotation, cc_Rb.rotation * Quaternion.Euler(0, theta, 0), 0.2f);

        }
    }
    #endregion

    #region Health Function
    public void DecreaseHealth(float amount)
    {
        p_CurHealth -= amount;
        m_HUD.UpdateHealth((1.0f * p_CurHealth) / (float)m_MaxHealth);
        if (p_CurHealth <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void IncreaseHealth(int amount)
    {
        p_CurHealth += amount;

        m_HUD.UpdateHealth((1.0f * p_CurHealth) / (float)m_MaxHealth);
        if (p_CurHealth > m_MaxHealth)
        {
            p_CurHealth = m_MaxHealth;
        }

    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPill"))
        {
            IncreaseHealth(other.GetComponent<HealthPill>().HealthGain);
            Destroy(other.gameObject);
        }
    }

    #endregion

    #region Attack Methods
    public IEnumerator UseAttack(PlayerAttackInfo attack)
    {

        cc_Rb.rotation = Quaternion.Euler(0, m_CameraTransform.eulerAngles.y, 0);
        cr_Anim.SetTrigger(attack.TriggerName);

        IEnumerator toColor = ChangeColor(attack.AbilityColor, 10);
        StartCoroutine(toColor);
        yield return new WaitForSeconds(attack.WindupTime);

        Vector3 offset = transform.forward * attack.Offset.z + transform.right * attack.Offset.x + transform.up * attack.Offset.y;
        GameObject go = Instantiate(attack.AbilityGO, transform.position + offset, cc_Rb.rotation);
        go.GetComponent<Ability>().Use(transform.position + offset);

        StopCoroutine(toColor);
        StartCoroutine(ChangeColor(p_DefaultColor, 50));
        yield return new WaitForSeconds(attack.Cooldown);
        attack.ResetCooldown();
    }
    #endregion

    #region Misc Method
    public IEnumerator ChangeColor(Color color, float speed)
    {
        Color curColor = cr_Renderer.material.color;
        while (curColor != color)
        {
            curColor = Color.Lerp(curColor, color, speed / 100);
            cr_Renderer.material.color = curColor;
            yield return null;
        }

    }

    #endregion
}
