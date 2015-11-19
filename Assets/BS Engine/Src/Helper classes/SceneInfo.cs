///----------------------------------------------------------------------
/// @file SceneInfo.cs
///
/// This file contains the declaration of SceneInfo class.
/// 
/// SceneInfo contains data and functionality of a Unity scene file.
/// 
/// Every Scene build using Bird Soul Engine must have all the GameObjects under a
/// common parent. This parent GameObject will be our Scene Root, this root MUST have the same name
/// as the scene, so it can be found easily.
/// 
/// Scenes and Subscenes are controlled by this class, and a scene can be a subscene eventually.
/// The main difference between scenes and subscenes is the following:
/// 
/// -A Scene is asociated with a App State, so it will be loaded as the default scene of one specific State.
/// 
/// -A subscene is loaded additively in the current State Scene that is already loaded.
/// 
/// So when we operates with a State in the stack (GameMgr), Scenes will be loaded, activated, deactivated and destroyed.
/// The main rules is activate the state scene when the state is at peek, deactivate when is in the stack but no the peek, and destroy it when
/// the state pops out from the stack.
/// 
/// If a scene is going to be activated and isn't loaded it will be loaded automatically.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 15/9/2015
///----------------------------------------------------------------------





using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


namespace BSEngine
{
    public class SceneInfo
    {

        #region Private params

        /// <summary>
        /// Scene name. Used also to locate the scene root GameObject.
        /// </summary>
        private string m_name;

        /// <summary>
        /// Flag to control when the scene is loaded (as main state scene or as subscene)
        /// </summary>
        private bool m_loaded;

        /// <summary>
        /// Flag used to control when the scene is activated in the scene stack
        /// </summary>
        private bool m_activated;

        /// <summary>
        /// Reference to the root GameObject of the scene
        /// </summary>
        private GameObject m_root;

        /// <summary>
        /// Subscenes of this scene
        /// </summary>
        private Dictionary<string, SceneInfo> m_subscenes;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor of the class using the scene name
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        public SceneInfo(string sceneName)
        {
            m_name = sceneName;
            m_loaded = false;
            m_activated = false;
            m_root = null;
            m_subscenes = new Dictionary<string, SceneInfo>();
        }

        /// <summary>
        /// Property used to acces to the Root gameObject.
        /// 
        /// Root scene can only be accessed when the scene is loaded
        /// </summary>
        public GameObject Root
        {
            get
            {
                if (m_loaded && m_root != null)
                    return m_root;
                else if ((m_loaded && m_root == null) || !GameMgr.Singleton.Loader.LoadStatesScenes)
                {
                    m_root = GameObject.Find(m_name);
                    return m_root;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Property to access to Scene's name
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// Property used to control when the Scene is activated or not
        /// </summary>
        public bool Activated
        {
            get { return m_activated; }
        }

        /// <summary>
        /// Property use to control when the scene is loaded or not
        /// </summary>
        public bool Loaded
        {
            get { return m_loaded; }
        }

        /// <summary>
        /// Property to acces to the current SubScenes of this scene.
        /// </summary>
        public List<SceneInfo> SubScenes
        {
            get { return m_subscenes.Values.ToList(); }
        }


        /// <summary>
        /// Method used to load the scene additively.
        /// 
        /// When loading a scene, it means that is the first time that it will be activated after it was previously destroyed.
        /// 
        /// This means that no subscene should be loaded
        /// </summary>
        public void LoadSceneAdditive()
        {
            Application.LoadLevelAdditive(m_name);
            m_loaded = true;
            m_activated = true;
            m_subscenes.Clear();
        }

        /// <summary>
        /// Method used to activate the scene.
        /// 
        /// If the scene isn't loaded when activating it, it will be loaded.
        /// 
        /// It will also activate all the subscenes of this scene
        /// </summary>
        public void ActivateScene()
        {

            Utils.Assert.assert(m_loaded, "Scene ins't loaded yet");
            Utils.Assert.assert(Root != null, "Root scene isn't loaded yet");
            Root.SetActive(true);

            foreach (string key in m_subscenes.Keys)
            {
                m_subscenes[key].ActivateScene();
            }
 
            m_activated = true;
 
        }

        /// <summary>
        /// Method used to deactivate the scene.
        /// 
        /// Also will deactivate all the subscenes of this scene
        /// </summary>
        public void DeactivateScene()
        {
            foreach (string key in m_subscenes.Keys)
            {
                m_subscenes[key].DeactivateScene();
            }

            if(Root != null)
                Root.SetActive(false);
            m_activated = false;
        }

        /// <summary>
        /// Method used to destroy the scene. Used when the state goes out from the stack.
        /// 
        /// It will destroy all the subscenes loaded within this scene
        /// </summary>
        public void DestroyScene()
        {
            foreach (string key in m_subscenes.Keys)
            {
                m_subscenes[key].DeactivateScene();
                m_subscenes[key].DestroyScene();
            }

            if (Root != null)
            {
                List<GameObject> children = new List<GameObject>();
                foreach (Transform child in Root.GetComponent<Transform>())
                {
                    children.Add(child.gameObject);
                }

                if (PoolMgr.Singleton != null)
                {
                    PoolMgr.Singleton.Destroy(children);
                    PoolMgr.Singleton.Destroy(m_root, true);
                }
            }
            

            m_loaded = false;
        }

        

        /// <summary>
        /// Method used to load subscenes additively
        /// 
        /// Called from the SceneMgr to load a subscene within this scene
        /// </summary>
        /// <param name="subscene">Subscene to load</param>
        public void LoadSubscene(string subscene)
        {
            if (!m_subscenes.ContainsKey(subscene))
            {
                SceneInfo subSceneInfo = new SceneInfo(subscene);
                subSceneInfo.LoadSceneAdditive();
                m_subscenes.Add(subscene, subSceneInfo);
            }
                
        }

        /// <summary>
        /// Method use to unload a subscene.
        /// 
        /// Called from the SceneMgr to unload a subscene from this scene
        /// </summary>
        /// <param name="subscene">subscene to unload</param>
        public void UnloadSubscene(string subscene)
        {
            if (m_subscenes.ContainsKey(subscene))
            {
                m_subscenes[subscene].DestroyScene();
                m_subscenes.Remove(subscene);
            }
        }

        #endregion

        #region Private methods

       

        #endregion
    }
}

