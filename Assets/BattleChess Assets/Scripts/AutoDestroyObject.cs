///----------------------------------------------------------------------
/// @file AutoDestroyObject.cs
///
/// This file contains the declaration of AutoDestroyObject class.
/// 
/// This simple component auto destroys an object in the given time.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 03/12/5015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using BSEngine;

public class AutoDestroyObject : MonoBehaviour
{
    #region Public parameters

    /// <summary>
    /// Time in seconds to wait until object autodestruction
    /// </summary>
    public float m_TimeToLive = 10.0f;

    #endregion

    #region Private params

    /// <summary>
    /// Time acumulator
    /// </summary>
    private float m_timeAcum = 0.0f;

    #endregion


    #region Monobehavior calls

    private void OnEnable()
    {
        m_timeAcum = 0.0f;
    }

    private void Update()
    {
        m_timeAcum += Time.deltaTime;

        if (m_timeAcum >= m_TimeToLive)
        {
            PoolMgr.Singleton.Destroy(gameObject);
        }
    }

    #endregion
}
