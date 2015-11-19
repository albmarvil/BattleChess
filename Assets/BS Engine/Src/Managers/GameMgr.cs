///----------------------------------------------------------------------
/// @file GameMgr.cs
///
/// This file contains the declaration of GameMgr class.
///
/// This is the main class for the Bird Soul Engine. This manager is in charge of the App Stack States.
/// 
/// Also is the entry point of the rest of the system. When GameMgr.Init() is called it will starts oter systems of the engine
/// and load all the necessary data.
/// 
/// This class works hand to hand with the BSEngineLoader script(Monobehavior), which is the one in charge to start the engine from Unity.
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 11/09/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections.Generic;
using BSEngine.Input;
using BSEngine.Utils;

namespace BSEngine
{
    public class GameMgr
    {
        #region Singleton

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static GameMgr m_instance = null;

        /// <summary>
        /// Property to get the singleton instance of the class.
        /// </summary>
        public static GameMgr Singleton { get { return m_instance; } }

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static GameMgr() { }

        /// <summary>
        /// Used to initialize the GameMgr singleton instance
        /// </summary>
        ///<returns>True if everything went ok</returns>
        public static bool Init(BSEngineLoader loader)
        {
            if (m_instance != null)
            {
                Debug.LogError("Second initialisation not allowed");
                return false;
            }
            else
            {
                m_instance = new GameMgr();
                return m_instance.open(loader);
            }
        }

        /// <summary>
        /// Used to deinitialize the GameMgr singleton data.
        /// </summary>
        public static void Release()
        {
            if (m_instance != null)
            {
                m_instance.close();
                m_instance = null;
            }
        }
        
        
        
        /// <summary>
        /// Used as second step on singleton initialisation. Used to specific code of the different Engine & Game managers
        /// 
        /// The GameMgr starts all the BSEngine managers and the different app states
        /// </summary>
        /// <returns>Should return true if everything went ok</returns>
        private bool open(BSEngineLoader loader)
        {
            m_stateStack = new Stack<State>();
            m_initializedStates = new Dictionary<string, State>();
            m_nextState = null;
            m_changeNextState = false;
            m_loader = loader;

            StorageMgr.Init();

            m_initializedStates = loader.States;

            foreach (string s in m_initializedStates.Keys)
            {
                m_initializedStates[s].Init();
            }

            InputMgr.Init();
            PoolMgr.Init();
            SceneMgr.Init();
            

            return true;
        }

        /// <summary>
        /// Used as second step on singleton initialisation. Used to specific code of the different Engine & Game managers
        /// The GameMgr stops all the BSEngine Managers and the different app states
        /// </summary>
        private void close()
        {
            
            SceneMgr.Release();
            PoolMgr.Release();
            InputMgr.Release();


            m_stateStack.Clear();

            foreach (string s in m_initializedStates.Keys)
            {
                m_initializedStates[s].Release();
            }

            m_initializedStates.Clear();

            StorageMgr.Release();

            m_loader = null;
        }

        #endregion

        #region Private params

        /// <summary>
        /// States stack. If a state can enter the stack in two ways:
        /// -PushState --> push the state into the stack. Previous peek state will be paused, this State will be Activated
        /// -ChangeState --> First clear the stack and Deactivates all the states, then push this new state into the stack and Activate it.
        /// 
        /// There are two ways to exit the stack:
        /// -PopState --> the peek state will be Deactivated and poped-out from the stack. The new peek state will be resumed.
        /// -ChangeState --> First clear the stack and Deactivates all the states, then push this new state into the stack and Activate it.
        /// </summary>
        private Stack<State> m_stateStack = null;

        /// <summary>
        /// Dictionary that contains all the initialized states, indexed by name.
        /// </summary>
        private Dictionary<string, State> m_initializedStates = null;

        /// <summary>
        /// Loader script of the engine. Communication bridge with Unity Engine. Used to load all the specific states of the Game
        /// </summary>
        private BSEngineLoader m_loader = null;

        /// <summary>
        /// Next state to change. Used when "ChangeState" is called
        /// </summary>
        private State m_nextState = null;

        /// <summary>
        /// Flag to change to the next state at the beginning of the next Update
        /// </summary>
        private bool m_changeNextState = false;

        #endregion

        #region Public methods

        /// <summary>
        /// Public property used to acces to the engine loader script.
        /// </summary>
        public BSEngineLoader Loader
        {
            get { return m_loader; }
        }

        /// <summary>
        /// Used to Update manager info, if needed.
        /// 
        /// We will Update all the managers of the Engine fom here.
        /// The BSEngineLoader script (MonoBehavior) is the one that propagates the Update signal from Unity to the Engine
        /// </summary>
        public void Update()
        {
            if (m_changeNextState)
            {
                foreach (State state in m_stateStack)
                {
                    state.Deactivate();
                }

                m_stateStack.Clear();

                m_stateStack.Push(m_nextState);

                m_stateStack.Peek().Activate();

                m_changeNextState = false;
            }


            InputMgr.Singleton.Update();

        }

        /// <summary>
        /// Public property to access to the States indexed by name
        /// </summary>
        public Dictionary<string, State> States
        {
            get { return m_initializedStates; }
        }

        /// <summary>
        /// Public property to access to the peek of the stack. The most recent State
        /// </summary>
        public State CurrentState
        {
            get { return m_stateStack.Peek(); }
        }

        /// <summary>
        /// Push into the stack the state given by the name. The State must be a valid state and should have been loaded by the BSEngineLoader script into the Enigne
        /// 
        /// The current peek state (if there is one) will be paused before we push this new state into the stack. Then this new state will be Activated.
        /// </summary>
        /// <param name="stateName">Name of the State to push into the stack</param>
        public void PushState(string stateName)
        {
            if (m_initializedStates.ContainsKey(stateName))
            {
                if(m_stateStack.Count > 0)
                    m_stateStack.Peek().Pause();
                m_stateStack.Push(m_initializedStates[stateName]);
                m_stateStack.Peek().Activate();
            }
        }

        /// <summary>
        /// Pop the current peek state out from the stack. This state will be deactivated before the oepration.
        /// After that (if there is one) the new peek state will be Resumed
        /// </summary>
        public void PopState()
        {
            m_stateStack.Peek().Deactivate();
            m_stateStack.Pop();
            if (m_stateStack.Count > 0)
                m_stateStack.Peek().Resume();
        }


        /// <summary>
        /// This method will clear the stack, deactivating all the states into it. Then it will push a new state into it.
        /// </summary>
        /// <param name="stateName">Name of the State to push into the stack</param>
        public void ChangeState(string stateName)
        {
            if(m_initializedStates.ContainsKey(stateName))
            {
                m_nextState = m_initializedStates[stateName];
                m_changeNextState = true;
            }
        }

        #endregion

    }
}

