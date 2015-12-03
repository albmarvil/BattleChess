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


public class MenuInputController : MonoBehaviour
{

    #region Publica paramas
    /// <summary>
    /// Reference to the Main MenuCanvas
    /// </summary>
    public GameObject m_MenuCanvas = null;

    /// <summary>
    /// Reference to the Match config canvas
    /// </summary>
    public GameObject m_MatchCanvas = null;

    #endregion


    #region Public methods

    /// <summary>
    /// Function callback to process "START_GAME" logic orders
    /// </summary>
    /// <param name="e">Event with the info of the order</param>
    public void onStartGameReceived(InputEvent e)
    {
        if (e.isOk)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        GameMgr.Singleton.ChangeState("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadMatchConfigScreen()
    {
        m_MenuCanvas.SetActive(false);
        m_MatchCanvas.SetActive(true);
    }

    public void UnloadMatchConfigScreen()
    {
        m_MenuCanvas.SetActive(true);
        m_MatchCanvas.SetActive(false);
    }

    public void setEasyMatchConfig(bool toggle)
    {
        if (toggle)
        {
            StorageMgr.Blackboard.Set<float>("TimeToThink", MatchConfigManager.Singleton.m_DifficultyConfig[0].m_TimeToThink);
            StorageMgr.Blackboard.Set<int>("MaxDepth", MatchConfigManager.Singleton.m_DifficultyConfig[0].m_MaxDepth);
        }
    }

    public void setMediumMatchConfig(bool toggle)
    {
        if (toggle)
        {
            StorageMgr.Blackboard.Set<float>("TimeToThink", MatchConfigManager.Singleton.m_DifficultyConfig[1].m_TimeToThink);
            StorageMgr.Blackboard.Set<int>("MaxDepth", MatchConfigManager.Singleton.m_DifficultyConfig[1].m_MaxDepth);
        }
    }

    public void setHardMatchConfig(bool toggle)
    {
        if (toggle)
        {
            StorageMgr.Blackboard.Set<float>("TimeToThink", MatchConfigManager.Singleton.m_DifficultyConfig[2].m_TimeToThink);
            StorageMgr.Blackboard.Set<int>("MaxDepth", MatchConfigManager.Singleton.m_DifficultyConfig[2].m_MaxDepth);
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
