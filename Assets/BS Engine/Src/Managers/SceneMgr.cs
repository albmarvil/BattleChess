///----------------------------------------------------------------------
/// @file SceneMgr.cs
///
/// This file contains the declaration of SceneMgr class.
///
/// 
/// SceneMgr is in charge of loading and unloading scenes to the current scene.
/// 
/// ASYNC OPERATIONS NOT SUPPORTED YET
/// 
/// All the functionality to load scenes is in the class SceneInfo, but this manager it's used as
/// a conection point for the rest of the architecture.
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 15/9/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections.Generic;


namespace BSEngine
{
    public class SceneMgr
    {
    
        #region Singleton

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static SceneMgr m_instance = null;

        /// <summary>
        /// Property to get the singleton instance of the class.
        /// </summary>
        public static SceneMgr Singleton { get { return m_instance; } }

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static SceneMgr() { }

        /// <summary>
        /// Used to initialize the SceneMgr singleton instance
        /// </summary>
        ///<returns>True if everything went ok</returns>
        public static bool Init()
        {
            if (m_instance != null)
            {
                Debug.LogError("Second initialisation not allowed");
                return false;
            }
            else
            {
                m_instance = new SceneMgr();
                return m_instance.open();
            }
        }

        /// <summary>
        /// Used to deinitialize the SceneMgr singleton data.
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
        /// </summary>
        /// <returns>Should return true if everything went ok</returns>
        private bool open()
        {
            return true;
        }

        /// <summary>
        /// Used as second step on singleton initialisation. Used to specific code of the different Engine & Game managers
        /// </summary>
        private void close()
        {
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Main scene of the current state(The state that its at the stack's peek)
        /// </summary>
        public SceneInfo CurrentScene
        {
            get { return GameMgr.Singleton.CurrentState.SceneInfo; }
        }

        /// <summary>
        /// Current Subscenes from the Main Scene loaded within the current State
        /// </summary>
        public List<SceneInfo> CurrentSubScenes
        {
            get { return GameMgr.Singleton.CurrentState.SceneInfo.SubScenes; }
        }

        /// <summary>
        /// Current Root Scene of the current Main Scene
        /// </summary>
        public GameObject CurrentRoot
        {
            get { return GameMgr.Singleton.CurrentState.SceneInfo.Root; }
        }



        /// <summary>
        /// Method used to load a scene and replace the current Main Scene.
        /// 
        /// This operation may cause lag when laoding big scenes in-game
        /// </summary>
        /// <param name="sceneName">Name of the scene to load</param>
        public void LoadScene(string sceneName)
        {
            
            SceneInfo sceneInfo = new SceneInfo(sceneName);
            LoadScene(sceneInfo);
            GameMgr.Singleton.CurrentState.SceneInfo.LoadSubscene(sceneName);

        }

        /// <summary>
        /// Method used to load a scene and replace the current Main Scene.
        /// 
        /// This operation may cause lag when laoding big scenes in-game
        /// </summary>
        /// <param name="info">SceneInfo of the scene to load</param>
        public void LoadScene(SceneInfo info)
        {
            GameMgr.Singleton.CurrentState.SceneInfo.DestroyScene();
            GameMgr.Singleton.CurrentState.SceneInfo = info;
            GameMgr.Singleton.CurrentState.SceneInfo.ActivateScene();

        }


        /// <summary>
        /// Method used to load a subscene attached to the current Main Scene.
        /// 
        /// This operation may cause lag when laoding big subscenes in-game
        /// </summary>
        /// <param name="sceneName">Name of the subscene to load</param>
        public void LoadSubSceneAdditive(string sceneName)
        {
            GameMgr.Singleton.CurrentState.SceneInfo.LoadSubscene(sceneName);
            
        }

        /// <summary>
        /// Method used to load a subscene attached to the current Main Scene.
        /// 
        /// This operation may cause lag when laoding big subscenes in-game
        /// </summary>
        /// <param name="info">SceneInfo of the subscene to load</param>
        public void LoadSubSceneAdditive(SceneInfo info)
        {
            LoadSubSceneAdditive(info.Name);
        }

        /// <summary>
        /// Method used to unload subscenes attached to the current Main Scene.
        /// 
        /// It will destroy all the GameObjects under the Root object of this subscene,
        /// this means that all gameObjects registered in PoolMgr will be cached again
        /// </summary>
        /// <param name="sceneName">subscene name to unload</param>
        public void UnloadSubScene(string sceneName)
        {
            GameMgr.Singleton.CurrentState.SceneInfo.UnloadSubscene(sceneName);
        }


        /// <summary>
        /// Method used to unload subscenes attached to the current Main Scene.
        /// 
        /// It will destroy all the GameObjects under the Root object of this subscene,
        /// this means that all gameObjects registered in PoolMgr will be cached again
        /// </summary>
        /// <param name="info">SceneInfo of the subscene to unload</param>
        public void UnloadSubScene(SceneInfo info)
        {
            UnloadSubScene(info.Name);
        }

        #endregion
    }
}
