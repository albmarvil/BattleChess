///----------------------------------------------------------------------
/// @file GameState.cs
///
/// This file contains the declaration of GameState class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 17/9/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;
using BSEngine;
using BSEngine.Input;


public class GameState : State
{
    /// <summary>
    /// State constructor. Should call the base class
    /// </summary>
    public GameState()
        : base("Game", "Game_Scene")
    {
        
    }

    /// <summary>
    /// method used to create the InputSet from the Config file loaded
    /// </summary>
    private InputSet createInputSet()
    {

        if (StorageMgr.Blackboard.ContainsKey("CFG"))
        {
            DataTable cfg = StorageMgr.Blackboard.Get<DataTable>("CFG");

            if (cfg.ContainsKey("GameStateInputSet"))
            {
                DataTable inputSetData = cfg.Get<DataTable>("GameStateInputSet");

                return new InputSet(inputSetData);
            }
            else
            {
                InputSet res = createDefaultInputSet();
                DataTable inputSetData = res.ToDataTable();
                StorageMgr.Blackboard.Get<DataTable>("CFG").Set<DataTable>("GameStateInputSet", inputSetData);
                return res;
            }
        }
        else
        {
            return createDefaultInputSet();
        }
    }

    /// <summary>
    /// Method to create the default Input Set in case there is no CFG file loaded
    /// </summary>
    private InputSet createDefaultInputSet()
    {
        Dictionary<BSKeyCode, List<string>> keyBindings = new Dictionary<BSKeyCode, List<string>>();

        List<string> orders = new List<string>();
        orders.Add("EXIT_GAME");
        keyBindings.Add(BSKeyCode.Escape, orders);

        orders = new List<string>();
        orders.Add("NEXT_MOVEMENT");
        keyBindings.Add(BSKeyCode.UpArrow, orders);

        orders = new List<string>();
        orders.Add("TESTKEY1");
        keyBindings.Add(BSKeyCode.DownArrow, orders);

        orders = new List<string>();
        orders.Add("CLICK");
        keyBindings.Add(BSKeyCode.Mouse0, orders);

        //TO DO MouseCfg config

        MouseCfg cfg = new MouseCfg(false, false, 10.0f, true);


        return new InputSet("GameStateInputSet", keyBindings, cfg);
    }

    /// <summary>
    /// Called on Init step. Used for specific state code
    /// </summary>
    /// <returns>True if everything went ok</returns>
    protected override bool open()
    {
        m_inputSet = createInputSet();
        return true;
    }

    /// <summary>
    /// Called on Release step. Used for specific state code
    /// </summary>
    protected override void close()
    {

    }

    /// <summary>
    /// Called on Activate step. Used for specific state code
    /// </summary>
    /// <returns>True if everything went ok</returns>
    protected override bool onActivate()
    {
        return true;
    }

    /// <summary>
    /// Called on Resume step. Used for specific state code
    /// </summary>
    /// <returns>True if everything went ok</returns>
    protected override bool onResume()
    {
        return true;
    }

    /// <summary>
    /// Called on Deactivate step. Used for specific state code
    /// </summary>
    protected override void onDeactivate()
    {

    }

    /// <summary>
    /// Called on Pause step. Used for specific state code
    /// </summary>
    protected override void onPause()
    {

    }
}
