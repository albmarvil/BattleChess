///----------------------------------------------------------------------
/// @file State.cs
///
/// This file contains the declaration of State class.
/// 
/// This class defines an App state and data related to it's context.
/// 
/// States are game specific so this class is used as a bridge between the Engine and the Game, defining basic operations on the states.
/// 
/// Those operations are:
/// 
/// -Init
/// -Release
/// -Activate*
/// -Deactivate*
/// -Pause*
/// -Resume*
/// 
/// 
/// Users should extend this class and complete the abstract  methods with the specific state code
/// 
/// -open
/// -close
/// -onActivate*
/// -onDeactivate*
/// -onPause*
/// -onResume*
/// 
/// =======> *: Each one of these operations are called from GameMgr when a change on the state stack it's done.
/// 
/// Also the states defines the context. One of the most important things are the InputSet. An InputSet is a definition of keys and logic orders.
/// This give us the poisbility of use keybindings depending on the context used.
/// 
/// Also states can load scenes using the SceneManager.****Work in progress****
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 11/09/2015
///----------------------------------------------------------------------


using System.Collections;
using BSEngine.Input;

namespace BSEngine
{
    public abstract class State
    {

        #region Private params

        /// <summary>
        /// Name of the state
        /// </summary>
        private string m_stateName;

        /// <summary>
        /// InputSet used in this state
        /// </summary>
        protected InputSet m_inputSet;

        /// <summary>
        /// Default scene to load when the state is pushed into the stack
        /// </summary>
        private SceneInfo m_sceneState;

        /// <summary>
        /// Flag used to indicate if the state is activated or not. (In stack or not)
        /// </summary>
        private bool m_isActive;

        #endregion

        #region Public methods

        /// <summary>
        /// Property to access to the state InputSet
        /// </summary>
        public InputSet InputSet
        {
            get { return m_inputSet; }
        }

        /// <summary>
        /// Property to access to the state name
        /// </summary>
        public string Name
        {
            get { return m_stateName; }
        }

        /// <summary>
        /// Property to access to the sceneInfo of the state
        /// </summary>
        public SceneInfo SceneInfo
        {
            get { return m_sceneState; }
            set { m_sceneState = value; }
        }

        /// <summary>
        /// Property used to check if the state is activated or not
        /// </summary>
        public bool IsActive
        {
            get { return m_isActive; }
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="name">name of the state</param>
        /// <param name="sceneName">Scene name of the state</param>
        public State(string name, string sceneName)
        {
            m_stateName = name;
            m_sceneState = new SceneInfo(sceneName);
            m_inputSet = null;
            m_isActive = false;
        }

        /// <summary>
        /// Method used for initialization of the state
        /// </summary>
        /// <returns>True if everything went ok</returns>
        public bool Init()
        {
            ///ENGINE CODE
            m_isActive = false;

            ///GAME CODE
            return open();
        }

        /// <summary>
        /// Release of the state info
        /// </summary>
        public void Release()
        {
            ///ENGINE CODE
            if (IsActive)
                Deactivate();
            
            ///GAME CODE
            close();
        }

        /// <summary>
        /// Called when the state enters the stack. When GameMgr is called to "ChangeState" and "PushState"
        /// </summary>
        /// <returns>True if everything went ok</returns>
        public bool Activate()
        {
            ///ENGINE CODE
            m_isActive = true;
            if(GameMgr.Singleton.Loader.LoadStatesScenes)
                m_sceneState.LoadSceneAdditive();

            ///GAME CODE
            return onActivate();
        }

        /// <summary>
        /// Called when the state it going to be the stack's peek. When GameMgr is Called to "PopState"
        /// </summary>
        /// <returns>True if everything went ok</returns>
        public bool Resume()
        {
            ///ENGINE CODE
            m_sceneState.ActivateScene();

            ///GAME CODE
            return onResume();
        }
        
        /// <summary>
        /// Called when the state is poped-out from stack. When GameMgr is called to "PopState" or "ChangeState"
        /// </summary>
        public void Deactivate()
        {
            ///ENGINE CODE
            m_isActive = false;
            m_sceneState.DestroyScene();
            ///GAME CODE
            onDeactivate();
        }
        
        /// <summary>
        /// Called when the state is no more the stack's peek. When GameMgr is called to "PushState"
        /// </summary>
        public void Pause()
        {
            ///ENGINE CODE
            m_sceneState.DeactivateScene();
            ///GAME CODE
            onPause();
        }
        

        #endregion

        #region Private methods

        /// <summary>
        /// Called on Init step. Used for specific state code
        /// </summary>
        /// <returns>True if everything went ok</returns>
        protected abstract bool open();

        /// <summary>
        /// Called on Release step. Used for specific code of the state
        /// </summary>
        protected abstract void close();

        /// <summary>
        /// Called on Activate step. Used for specific code of the state
        /// </summary>
        /// <returns>True if everything went ok</returns>
        protected abstract bool onActivate();

        /// <summary>
        /// Called on Resume step. Used for specific code of the state
        /// </summary>
        /// <returns>True if everything went ok</returns>
        protected abstract bool onResume();

        /// <summary>
        /// Called on Deactivate step. Used for specific code of the state
        /// </summary>
        protected abstract void onDeactivate();

        /// <summary>
        /// Called on Pause step. Used for specific code of the state
        /// </summary>
        protected abstract void onPause();

        #endregion
    }
}

