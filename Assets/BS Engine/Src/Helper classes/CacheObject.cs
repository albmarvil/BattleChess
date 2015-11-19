///----------------------------------------------------------------------
/// @file CacheObject.cs
///
/// This file contains the declaration of CacheObject class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 14/9/2015
///----------------------------------------------------------------------



using UnityEngine;

[System.Serializable]
public class CacheObject  {

    /// <summary>
    /// Prefab used as object to be in the PoolMgr cache
    /// </summary>
    public GameObject m_Prefab = null;

    /// <summary>
    /// Number of instances of this object to preload in the cache
    /// </summary>
    public int m_numberOfInstances = 1;
	
}
