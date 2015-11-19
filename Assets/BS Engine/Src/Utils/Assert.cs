///----------------------------------------------------------------------
/// @file Assert.cs
///
/// This file contains the declaration of Assert class. Imitation of C++ assert funciton.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 14/09/2015
///----------------------------------------------------------------------


using UnityEngine;
using System;

namespace BSEngine
{
    namespace Utils
    {
        public class Assert
        {
            /// <summary>
            /// ABORT IF NOT
            /// Abort executon if condition isn't valid.
            /// </summary>
            /// <param name="condition">condition to evaluate</param>
            /// <param name="msg">abort message to show</param>
            public static void assert(bool condition, string msg)
            {
                #if DEBUG
                if (!condition)
                {
                    Debug.LogError("Assert! : " + msg);
                    throw new Exception(msg);
                }
                #endif
            }
        }
    }
}

