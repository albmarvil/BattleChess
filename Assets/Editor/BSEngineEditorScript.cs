///----------------------------------------------------------------------
/// @file BSEngineEditorScript.cs
///
/// This file contains the declaration of BSEngineEditorScript class.
/// 
/// This script is used to create the base scene used in the engine
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 15/9/2015
///----------------------------------------------------------------------



using UnityEditor;
using UnityEngine;
using System.Collections;

namespace BSEngine
{
    public class BSEngineEditorScript : MonoBehaviour
    {
        /// <summary>
        /// Function used to create a base scene used in the Bird Soul Engine
        /// </summary>
        [MenuItem("BSEngine/Add BaseScene")]
        static void AddBaseSceneFunction()
        {
            GameObject BSEngineServers = GameObject.Find("BSEngineServers");
            if (BSEngineServers == null)
            {
                BSEngineServers = new GameObject("BSEngineServers");
                BSEngineServers.AddComponent<BSEngineLoader>();
            }


            GameObject Pool = GameObject.Find("Pool");
            if (Pool == null)
            {
                Pool = new GameObject("Pool");
            }
        }


        /// <summary>
        /// Function used to create the necessary scene elements of the Bird Soul Engine to run a test in a single scene, witjout having a base scene preloaded
        /// </summary>
        [MenuItem("BSEngine/Add Test Elements")]
        private static void AddTestSceneFunction()
        {
            GameObject BSEngineServers = GameObject.Find("BSEngineServers");
            if (BSEngineServers == null)
            {
                BSEngineServers = new GameObject("BSEngineServers");
                BSEngineLoader loader = BSEngineServers.AddComponent<BSEngineLoader>();
                loader.LoadStatesScenes = false;
            }


            GameObject Pool = GameObject.Find("Pool");
            if (Pool == null)
            {
                Pool = new GameObject("Pool");
            }
        }

        /// <summary>
        /// Function used to delete the scene elements used for test in Bird Soul Engine's scenes
        /// </summary>
        [MenuItem("BSEngine/Delete Test Elements")]
        private static void DeleteTestSceneFunction()
        {
            GameObject BSEngineServers = GameObject.Find("BSEngineServers");
            if (BSEngineServers != null)
            {
                GameObject.DestroyImmediate(BSEngineServers);
            }


            GameObject Pool = GameObject.Find("Pool");
            if (Pool != null)
            {
                GameObject.DestroyImmediate(Pool);
            }
        }
    }


    


}
