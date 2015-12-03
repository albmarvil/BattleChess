///----------------------------------------------------------------------
/// @file MatchConfigManager.cs
///
/// This file contains the declaration of MatchConfigManager class.
/// 
/// This class is a simple singleton that holds the difficulty parameters config.
/// 
/// It allows us to change difficultty parameters thorught the inspector
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 03/12/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct MatchConfig
{
    public float m_TimeToThink;

    public int m_MaxDepth;
}

public class MatchConfigManager : MonoBehaviour {

    
    #region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static MatchConfigManager m_instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static MatchConfigManager Singleton { get { return m_instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static MatchConfigManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various MatchConfigManager [" + name + "]");
            this.enabled = false;
        }
    }
	
	/// <summary>
    /// This is like the Release but done by the MonoBehaviour
    /// </summary>
    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
    }

    #endregion


    #region Public params

    public List<MatchConfig> m_DifficultyConfig = new List<MatchConfig>();

    #endregion

    #region Private params

    #endregion

    #region Public methods

    #endregion

    #region Private methods

    #endregion

    #region Monobehavior calls

    #endregion

}
