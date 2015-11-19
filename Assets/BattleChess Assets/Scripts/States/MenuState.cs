///----------------------------------------------------------------------
/// @file MenuState.cs
///
/// This file contains the declaration of MenuState class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 17/9/2015
///----------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using BSEngine;
using BSEngine.Input;


public class MenuState : State
{
    /// <summary>
    /// State constructor. Should call the base class
    /// </summary>
    public MenuState()
        : base("Menu", "Menu_Scene")
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

            if (cfg.ContainsKey("MenuStateInputSet"))
            {
                DataTable inputSetData = cfg.Get<DataTable>("MenuStateInputSet");

                return new InputSet(inputSetData);
            }
            else
            {
                InputSet res = createDefaultInputSet();
                DataTable inputSetData = res.ToDataTable();
                StorageMgr.Blackboard.Get<DataTable>("CFG").Set<DataTable>("MenuStateInputSet", inputSetData);
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
    public InputSet createDefaultInputSet()
    {
        Dictionary<BSKeyCode, List<string>> keyBindings = new Dictionary<BSKeyCode, List<string>>();

        List<string> orders = new List<string>();
        orders.Add("START_GAME");
        keyBindings.Add(BSKeyCode.Return, orders);

        orders = new List<string>();
        orders.Add("EXIT");
        keyBindings.Add(BSKeyCode.Escape, orders);

        return new InputSet("MenuStateInputSet", keyBindings, null);
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
