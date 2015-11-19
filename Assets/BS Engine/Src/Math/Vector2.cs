///----------------------------------------------------------------------
/// @file Vector2.cs
///
/// This file contains the declaration of Vector2 class.
/// 
/// BSEngine Vector2 class is a wrapper of the UnityEngine.Vector2 class,
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
        public class Vector2
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

            #endregion

            #region Public methods

            /// <summary>
            /// Constructor from UnityEngine.Vector2
            /// </summary>
            /// <param name="v">UnityEngine.Vector2 from</param>
            public Vector2(UnityEngine.Vector2 v)
            {
                this.x = v.x;
                this.y = v.y;
            }

            /// <summary>
            /// Constructor using coordinates
            /// </summary>
            /// <param name="x">x-axis coord</param>
            /// <param name="y">y-axis coord</param>
            public Vector2(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            /// <summary>
            /// Property used to translate to a UnityEngine.Vector2
            /// </summary>
            public UnityEngine.Vector2 ToUnityV2
            {
                get { return new UnityEngine.Vector2(x, y); }
            }


            #endregion

        }
    }
}
