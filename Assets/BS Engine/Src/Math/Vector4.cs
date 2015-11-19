///----------------------------------------------------------------------
/// @file Vector4.cs
///
/// This file contains the declaration of Vector4 class.
/// 
/// BSEngine Vector4 class is a wrapper of the UnityEngine.Vector4 class,
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
        public class Vector4
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
            /// Constructor from UnityEngine.Vector4
            /// </summary>
            /// <param name="v">UnityEngine.Vector4 from</param>
            public Vector4(UnityEngine.Vector4 v)
            {
                this.x = v.x;
                this.y = v.y;
                this.z = v.z;
                this.w = v.w;
            }

            /// <summary>
            /// Constructor using coordinates
            /// </summary>
            /// <param name="x">x-axis coord</param>
            /// <param name="y">y-axis coord</param>
            /// <param name="z">z-axis coord</param>
            /// <param name="w">w-axis coord</param>
            public Vector4(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            /// <summary>
            /// Property used to translate to a UnityEngine.Vector4
            /// </summary>
            public UnityEngine.Vector4 ToUnityV4
            {
                get { return new UnityEngine.Vector4(x, y, z, w); }
            }


            #endregion

        }
    }
}
