///----------------------------------------------------------------------
/// @file MenuInputController.cs
///
/// This file contains the declaration of MenuInputController class.
///
/// This script will control the input interaction on menus
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 17/9/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using BSEngine;
using BSEngine.Input;


public class MenuInputController : MonoBehaviour {


    #region Public methods

    /// <summary>
    /// Function callback to process "START_GAME" logic orders
    /// </summary>
    /// <param name="e">Event with the info of the order</param>
    public void onStartGameReceived(InputEvent e)
    {
        if (e.isOk)
        {
            GameMgr.Singleton.ChangeState("Game");
        }
    }


    /// <summary>
    /// Function callback to process "EXIT" logic orders
    /// </summary>
    /// <param name="e">Event with the info of the order</param>
    public void onExitReceived(InputEvent e)
    {
        if (e.isOk)
        {
            Application.Quit();
        }
    }

    #endregion


    #region Monobehavior calls

    /// <summary>
    /// On start this script will register the function callbacks to the desired logic orders
    /// </summary>
    private void Start()
    {
        InputMgr.Singleton.RegisterOrderListener("Menu", "START_GAME", onStartGameReceived);
        InputMgr.Singleton.RegisterOrderListener("Menu", "EXIT", onExitReceived);
    }

    /// <summary>
    /// On destroy this script will unregister the functon callbacks to the desired logic orders
    /// </summary>
    private void OnDestroy()
    {
        InputMgr.Singleton.UnregisterOrderListener("Menu", "START_GAME", onStartGameReceived);
        InputMgr.Singleton.UnregisterOrderListener("Menu", "EXIT", onExitReceived);
    }

    #endregion

}
