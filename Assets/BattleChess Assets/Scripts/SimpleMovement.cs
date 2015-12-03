///----------------------------------------------------------------------
/// @file SimpleMovement.cs
///
/// This file contains the declaration of SimpleMovement class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 03/12/2015
///----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {


    #region Public params
    /// <summary>
    /// Movement speed
    /// </summary>
    public float m_speed = 1.0f;

    /// <summary>
    /// Movement direction
    /// </summary>
    public Vector3 m_direction = Vector3.right;

    #endregion

    #region Private params

    /// <summary>
    /// Reference to the gameObject transform
    /// </summary>
    private Transform m_transform = null;

    #endregion


    #region Monobehavior calls

    private void OnEnable()
    {
        m_transform = gameObject.GetComponent<Transform>();
    }

    private void Update()
    {
        Vector3 newPos = m_transform.position + m_direction * m_speed * Time.deltaTime;

        m_transform.position = newPos;
    }

    #endregion

}
