///----------------------------------------------------------------------
/// @file MenuInputController.cs
///
/// This file contains the declaration of GameInputController class.
///
/// This script will control the input interaction on game state
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 17/9/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using BSEngine;
using BSEngine.Input;


public class GameInputController : MonoBehaviour
{


    #region Public methods

    /// <summary>
    /// Function callback to process "EXIT_GAME" logic orders
    /// </summary>
    /// <param name="e">Event with the info of the order</param>
    public void onExitGameReceived(InputEvent e)
    {
        if (e.isOk)
        {
            GameMgr.Singleton.ChangeState("Menu");
        }
    }

    #endregion


    #region Monobehavior calls

    /// <summary>
    /// On start this script will register the function callbacks to the desired logic orders
    /// </summary>
    private void Start()
    {
        if(InputMgr.Singleton != null)
            InputMgr.Singleton.RegisterOrderListener("Game", "EXIT_GAME", onExitGameReceived);
    }

    /// <summary>
    /// On destroy this script will unregister the functon callbacks to the desired logic orders
    /// </summary>
    private void OnDestroy()
    {
        if(InputMgr.Singleton != null)
            InputMgr.Singleton.UnregisterOrderListener("Game", "EXIT_GAME", onExitGameReceived);
    }

    #endregion

}
