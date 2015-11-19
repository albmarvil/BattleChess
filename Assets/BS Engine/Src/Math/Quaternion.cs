///----------------------------------------------------------------------
/// @file Quaternion.cs
///
/// This file contains the declaration of Quaternion class.
/// 
/// BSEngine Quaternion class is a wrapper of the UnityEngine.Quaternion class,
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
        public class Quaternion
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

            /// <summary>
            /// w-axis coord. read-only
            /// </summary>
            public readonly float w = 0.0f;

            #endregion

            #region Public methods

            /// <summary>
            /// Constructor from UnityEngine.Quaternion
            /// </summary>
            /// <param name="v">UnityEngine.Quaternion from</param>
            public Quaternion(UnityEngine.Quaternion q)
            {
                this.x = q.x;
                this.y = q.y;
                this.z = q.z;
                this.w = q.w;
            }

            /// <summary>
            /// Constructor using coordinates
            /// </summary>
            /// <param name="x">x-axis coord</param>
            /// <param name="y">y-axis coord</param>
            /// <param name="z">z-axis coord</param>
            /// <param name="w">w-axis coord</param>
            public Quaternion(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            /// <summary>
            /// Property used to translate to a UnityEngine.Quaternion
            /// </summary>
            public UnityEngine.Quaternion ToUnityQuaternion
            {
                get { return new UnityEngine.Quaternion(x, y, z, w); }
            }


            #endregion

        }
    }
}
