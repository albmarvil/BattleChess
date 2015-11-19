using UnityEngine;
using System.Collections.Generic;

namespace BSEngine
{
    public class BSEngineLoader : MonoBehaviour
    {

        #region Public params

        /// <summary>
        /// Configuration of the initial cache of objects
        /// </summary>
        public List<CacheObject> m_InitCache = new List<CacheObject>();

        /// <summary>
        /// Name of the state to load when the game starts
        /// </summary>
        public string m_stateToLoad = "default";

        /// <summary>
        /// With this flag set to true state's main scenes will be loaded when the state is activated at peek.
        /// 
        /// For testing scenes where you do not want to load those scenes, set it to false.
        /// </summary>
        public bool m_loadStatesScenes = true;

        /// <summary>
        /// Flag used to save the blackboard to a file.
        /// 
        /// Debug purposes only
        /// </summary>
        public bool m_SaveDebugBlackboard = false;

        /// <summary>
        /// Flag used to create a DefaultCFGFile when a CFG file isn't located on loading
        /// 
        /// CreateDeafultCFGFile funciton will be called
        /// </summary>
        //public bool m_CreateDefaultCFGFile = true;

        public string m_CFGFileName = "BSEngine_CFG";

        #endregion

        #region Private params

        /// <summary>
        /// States to be loaded into the engine
        /// </summary>
        private Dictionary<string, State> m_initializedStates = new Dictionary<string,State>();

        #endregion


        #region Public Methods

        /// <summary>
        /// Public property to access to the flag that allows the state's scenes loading
        /// </summary>
        public bool LoadStatesScenes
        {
            get { return m_loadStatesScenes; }
            set { m_loadStatesScenes = value; }
        }

        /// <summary>
        /// Porperty to acces to the states to load
        /// </summary>
        public Dictionary<string, State> States
        {
            get { return m_initializedStates; }
        }

#if !UNITY_WEBPLAYER
        /// <summary>
        /// Method used to create a default CFG file when this file doesn't exist.
        /// 
        /// Ususally called when the engine is loaded for first time.
        /// 
        /// You should complete this method adding all the CFG info you need in your game.
        /// IE: resolution, aspect ratio, music and effects volume, language selected, etc.
        /// 
        /// All the data related to InputSets and keybindings is done in each State, where creation of InputSets takes place.
        /// </summary>
        public void CreateDefaultCFGFile()
        {
            ///GAME CODE
            /// REsolution CFG example
            ///StorageMgr.Blackboard.Get<DataTable>("CFG").Set<int>("Resolution_height", 1080);
            ///StorageMgr.Blackboard.Get<DataTable>("CFG").Set<int>("Resolution_width", 1920);
        }
#endif

        #endregion


        #region MonoBehavior Calls

        /// <summary>
        /// At the start, this script will load all the states into the engine and start al the engine functionality.
        /// 
        /// Also it will load the data needed for the PoolManager cache.
        /// </summary>
        private void Awake()
        {
            ///GAME CODE
            ///
            ///Creation of the states. Example:
            ///
            ///State st = new GameState();
            ///m_initializedStates.Add(st.Name, st);

            State st = new GameState();
            m_initializedStates.Add(st.Name, st);

            st = new MenuState();
            m_initializedStates.Add(st.Name, st);

            ///ENGINE CODE
            GameMgr.Init(this);
            PoolMgr.Singleton.LoadInitialCache(m_InitCache);
            


            //LOAD FIRST STATE
            //Example
            GameMgr.Singleton.PushState(m_stateToLoad);
        }


        private void OnDestroy()
        {
            ///ENGINE CODE
            GameMgr.Release();

            ///GAME CODE
        }

        private void Update()
        {
            ///GAME CODE
            

            ///ENGINE CODE
            GameMgr.Singleton.Update();
        }

        #endregion

    }
}

