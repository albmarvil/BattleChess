///----------------------------------------------------------------------
/// @file RegisterWaypoint.cs
///
/// This file contains the declaration of RegisterWaypoint class.
/// 
/// This script will register the waypoint into the BoardManagaer once when loading the object
/// 
/// (Start)
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 21/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class RegisterWaypoint : MonoBehaviour
{

    #region Monobehavior Calls

    private void Start () 
    {
        if(BoardManager.Singleton != null)
            BoardManager.Singleton.registerBoardWaypoint(gameObject.name, gameObject);
    }

    private void OnDisable()
    {
        if (BoardManager.Singleton != null)
            BoardManager.Singleton.unregisterBoardWaypoint(gameObject.name);
    }

    #endregion

}
