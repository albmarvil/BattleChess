///----------------------------------------------------------------------
/// @file PoolMgr.cs
///
/// This file contains the declaration of PoolMgr class.
/// 
/// This manager holds all the info of all GameObjects that are cached and deactivated.
/// 
/// Used to avoid gameplay "lag" when creating a big amount of GameObjects.
/// 
/// The Pool will be caching all the objects that it's ordered to create and destroy, even if you didn't prewarm them.
/// As said, the GameObject cache can be filled before the game starts, through BSEngineLoader script (MonoBehavior)
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 15/9/2015
///----------------------------------------------------------------------




using UnityEngine;
using System.Collections.Generic;
using BSEngine.Utils;

namespace BSEngine
{
    public class PoolMgr
    {
    
        #region Singleton

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static PoolMgr m_instance = null;

        /// <summary>
        /// Property to get the singleton instance of the class.
        /// </summary>
        public static PoolMgr Singleton { get { return m_instance; } }

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static PoolMgr() { }

        /// <summary>
        /// Used to initialize the PoolManager singleton instance
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
                m_instance = new PoolMgr();
                return m_instance.open();
            }
        }

        /// <summary>
        /// Used to deinitialize the PoolManager singleton data.
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
            m_cache = new Dictionary<string, List<GameObject>>();

            m_IDs = 0;
            GameObject pool = GameObject.Find("Pool");
            Assert.assert(pool != null, "Invalid Scene, doesn't have a GameObject named 'Pool'");
            m_poolGameObject = pool.GetComponent<Transform>();
            

            return true;
        }

        /// <summary>
        /// Used as second step on singleton initialisation. Used to specific code of the different Engine & Game managers
        /// </summary>
        private void close()
        {
            ClearCache();
        }

        #endregion

        #region Private params

        /// <summary>
        /// Cache of the objects deactivated in the pool
        /// </summary>
        private Dictionary<string, List<GameObject>> m_cache;

        /// <summary>
        /// Last used ID
        /// </summary>
        private static int m_IDs;

        /// <summary>
        /// Reference to the GameObject used to store all the cached objects in the scene.
        /// </summary>
        private Transform m_poolGameObject = null;

        #endregion

        #region Private methods
        /// <summary>
        /// Property used to get the Next ID
        /// </summary>
        private int NextID
        {
            get
            {
                int id = m_IDs;
                m_IDs++;
                return id;
            }
        }

        /// <summary>
        /// Method used to get the original name of a instance
        /// 
        /// Every object instantiated by this manager will have the original name followed by '@' and an ID
        /// 
        ///                     originalname@ID
        /// </summary>
        /// <param name="gameobject">GameObject to get the original name</param>
        /// <returns>original name of the game Object</returns>
        private string getOriginalName(GameObject gameobject)
        {
            return gameobject.name.Split('@')[0];
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method will create an instance of the given prefab. If the prefab is in cache the GameObject will be activated.
        /// If not a new instance will be crated and cached at the same time for next uses.
        /// 
        /// We will use a namespace rule to identify an instance of an object and it's original prefab. Every instance will have the original prefab name
        /// followed by "@" and an ID number.
        /// 
        /// All elements of the cache will be recovered from the head of the list (at 0).
        /// </summary>
        /// <param name="prefab">GameObject to instantiate</param>
        /// <param name="position">Position to instantiate</param>
        /// <param name="rotation">Initial Rotation to instantiate</param>
        /// <returns>Instance of the desired GameObject</returns>
        public GameObject Instatiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject instance = null;
            if (m_cache.ContainsKey(prefab.name))
            {
                List<GameObject> list = m_cache[prefab.name];
                if (list.Count > 0)
                {
                    instance = list[0];
                    list.RemoveAt(0);
                    instance.SetActive(true);
                    instance.name = prefab.name + "@" + NextID;
                    Transform trans = instance.GetComponent<Transform>();
                    trans.position = position;
                    trans.rotation = rotation;
                }
            }

            if (instance == null)
            {
                instance = GameObject.Instantiate(prefab, position, rotation) as GameObject;
                instance.name = prefab.name + "@" + NextID;

                List<GameObject> list = new List<GameObject>();
                list.Add(instance);

                m_cache.Add(prefab.name, list);
            }

            instance.GetComponent<Transform>().SetParent(SceneMgr.Singleton.CurrentRoot.GetComponent<Transform>());

            return instance;
        }

        /// <summary>
        /// This method will create an instance of the given prefab. If the prefab is in cache the GameObject will be activated.
        /// If not a new instance will be crated and cached at the same time for next uses.
        /// 
        /// We will use a namespace rule to identify an instance of an object and it's original prefab. Every instance will have the original prefab name
        /// followed by "@" and an ID number.
        /// 
        /// All elements of the cache will be recovered from the head of the list (at 0).
        /// </summary>
        /// <param name="prefab">GameObject to instantiate</param>
        /// <param name="transfrom">Transform to instantiate</param>
        /// <returns>Instance of the desired GameObject</returns>
        public GameObject Instatiate(GameObject prefab, Transform transform)
        {
            GameObject instance = null;
            if (m_cache.ContainsKey(prefab.name))
            {
                List<GameObject> list = m_cache[prefab.name];
                if (list.Count > 0)
                {
                    instance = list[0];
                    list.RemoveAt(0);
                    instance.SetActive(true);
                    instance.name = prefab.name + "@" + NextID;
                    Transform trans = instance.GetComponent<Transform>();
                    trans.position = transform.position;
                    trans.rotation = transform.rotation;
                }
            }

            if(instance == null)
            {
                instance = GameObject.Instantiate(prefab, transform.position, transform.rotation) as GameObject;
                instance.name = prefab.name + "@" + NextID;
            }

            instance.GetComponent<Transform>().SetParent(SceneMgr.Singleton.CurrentRoot.GetComponent<Transform>());

            return instance;
        }

        /// <summary>
        /// Method used to destroy an instance of an gameObject.
        /// 
        /// This method deactivates the instance and return it to the pool cache (even if it wasn't there before) or completly
        /// destroys the instance.
        /// </summary>
        /// <param name="gameobject">Instance to destroy</param>
        /// <param name="clear">OPTIONAL. True to clear the instance</param>
        public void Destroy(GameObject gameobject, bool clear = false)
        {
            if (clear)
            {
                GameObject.Destroy(gameobject);
            }
            else
            {
                gameobject.SetActive(false);
                string originalName = getOriginalName(gameobject);
                if (m_cache.ContainsKey(originalName))
                {
                    m_cache[originalName].Add(gameobject);
                }
                else
                {
                    List<GameObject> list = new List<GameObject>();
                    list.Add(gameobject);
                    m_cache.Add(originalName, list);
                }

                gameobject.GetComponent<Transform>().SetParent(m_poolGameObject);
            }
        }

        /// <summary>
        /// Method used to destroy an instance of an gameObject.
        /// 
        /// This method deactivates the instances and return it to the pool cache (even if it wasn't there before) or completly
        /// destroys the instance.
        /// 
        /// ONLY CACHED ELEMENTS WILL RETURN TO CACHE
        /// </summary>
        /// <param name="gameobjects">Instances to destroy</param>
        /// <param name="clear">OPTIONAL. True to clear the instance</param>
        public void Destroy(List<GameObject> gameobjects, bool clear = false)
        {
            GameObject go = null;
            for (int i = 0; i < gameobjects.Count; ++i)
            {
                go = gameobjects[i];
                if (clear)
                {
                    GameObject.Destroy(go);
                }
                else
                {
                    
                    string originalName = getOriginalName(go);
                    if (m_cache.ContainsKey(originalName))
                    {
                        go.SetActive(false);
                        m_cache[originalName].Add(go);
                        go.GetComponent<Transform>().SetParent(m_poolGameObject);
                    }  
                }
            }
            
        }

        /// <summary>
        /// Method called by BSEngineLoader Script (Monobehavior) when loading all the engine info
        /// 
        /// GameObjects will be created and deactivated
        /// </summary>
        /// <param name="gameObjects">Info of the gameObjects to create</param>
        /// <returns></returns>
        public bool LoadInitialCache(List<CacheObject> gameObjects)
        {
            foreach (CacheObject obj in gameObjects)
            {
                GameObject prefab = obj.m_Prefab;
                int numOfInstances = obj.m_numberOfInstances;

                List<GameObject> list = new List<GameObject>();
               

                GameObject instance = null;
                for (int i = 0; i < numOfInstances; ++i)
                {
                    instance = GameObject.Instantiate(prefab) as GameObject;
                    instance.name = prefab.name + '@' + NextID;
                    instance.SetActive(false);
                    list.Add(instance);
                    instance.GetComponent<Transform>().SetParent(m_poolGameObject);
                }


                if (m_cache.ContainsKey(prefab.name))
                {
                    m_cache[prefab.name].AddRange(list);
                }
                else
                {
                    m_cache.Add(prefab.name, list);
                }

            }

            return true;
        }

        /// <summary>
        /// Method used to clear al the objects in the cache
        /// </summary>
        public void ClearCache()
        {
            foreach (string key in m_cache.Keys)
            {
                foreach (GameObject go in m_cache[key])
                {
                    Destroy(go, true);
                }
                m_cache[key].Clear();
            }
            m_cache.Clear();
        }

        #endregion
    }
}


