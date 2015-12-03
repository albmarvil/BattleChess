using UnityEngine;
using System.Collections;
using BSEngine;
using BSEngine.Input;

public class GoatSpawner : MonoBehaviour {


    #region Public params

    /// <summary>
    /// Reference to the Goat prefab
    /// </summary>
    public GameObject m_goatPrefab = null;

    #endregion

    #region Private params

    /// <summary>
    /// Max clicks to get AN ANGRY GOAT!
    /// </summary>
    [SerializeField]
    private int m_maxAngryCliks = 10;

    /// <summary>
    /// Current angry clicks!
    /// </summary>
    private int m_angryClicks = 0;

    /// <summary>
    /// Reference to the gameObject trasnform
    /// </summary>
    private Transform m_transform = null;

    #endregion

    #region Private methods

    private void OnMouseClick(InputEvent e)
    {
        if (e.isOk)
        {
            m_angryClicks++;


            if (m_angryClicks >= m_maxAngryCliks)
            {
                PoolMgr.Singleton.Instatiate(m_goatPrefab, m_transform.position, m_transform.rotation);
                m_angryClicks = 0;
                m_maxAngryCliks--;
            }
        }
    }

    #endregion

    #region Monobehavior calls

    private void OnEnable()
    {
        //if (InputMgr.Singleton != null)
        //{
            InputMgr.Singleton.RegisterOrderListener("Menu", "ANGRY_GOAT_CLICK", OnMouseClick); 
        //}

        m_angryClicks = 0;

        m_transform = gameObject.GetComponent<Transform>();
    }

    private void OnDisable()
    {
        //if (InputMgr.Singleton != null)
        //{
            InputMgr.Singleton.UnregisterOrderListener("Menu", "ANGRY_GOAT_CLICK", OnMouseClick);
        //}
    }

    #endregion

}
