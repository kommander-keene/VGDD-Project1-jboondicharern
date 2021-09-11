using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("All of the main information about this particular activity")]
    protected AbilityInfo m_Info;
    #endregion

    #region Cached Components
    protected ParticleSystem cc_PS;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        cc_PS = this.gameObject.GetComponent<ParticleSystem>();
    }

    #region Use Methods
    public abstract void Use(Vector3 pos);
    #endregion
}
