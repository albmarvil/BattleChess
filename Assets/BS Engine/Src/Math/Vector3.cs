///----------------------------------------------------------------------
/// @file Vector3.cs
///
/// This file contains the declaration of Vector3 class.
/// 
/// BSEngine Vector3 class is a wrapper of the UnityEngine.Vector3 class,
/// used for serialization and other purposes
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 10/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

namespace BSEngine
{
    namespace Math
    {
        [System.Serializable]
        public class Vector3
        {

            #region Public params

            /// <summary>
            /// x-axis coord. read-only
            /// </summary>
            public readonly float x = 0.0f;

            /// <summary>
            /// y-axis coord. read-only
            /// </summary>
            public readonly float y = 0.0f;

            /// <summary>
            /// z-axis coord. read-only
            /// </summary>
            public readonly float z = 0.0f;

            #endregion

            #region Public methods

            /// <summary>
            /// Constructor from UnityEngine.Vector3
            /// </summary>
            /// <param name="v">UnityEngine.Vector3 from</param>
            public Vector3 (UnityEngine.Vector3 v)
            {
                this.x = v.x;
                this.y = v.y;
                this.z = v.z;
            }

            /// <summary>
            /// Constructor using coordinates
            /// </summary>
            /// <param name="x">x-axis coord</param>
            /// <param name="y">y-axis coord</param>
            /// <param name="z">z-axis coord</param>
            public Vector3 (float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            /// <summary>
            /// Property used to translate to a UnityEngine.Vector3
            /// </summary>
            public UnityEngine.Vector3 ToUnityV3
            {
                get { return new UnityEngine.Vector3(x, y, z); }
            }


            #endregion

        }
    }
}
