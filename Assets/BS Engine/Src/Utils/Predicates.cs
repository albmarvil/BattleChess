///----------------------------------------------------------------------
/// @file Predicates.cs
///
/// This file contains the declaration of Predicates class.
/// 
/// This class is a method container class. All methods in this class are static and returns
/// a boolean value
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 29/10/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using BSEngine.Math;

namespace BSEngine
{
    namespace Utils
    {
        public class Predicates {

            /// <summary>
            /// Definition of basic types of BSEngine.
            /// True if the given object is considered basic type
            /// </summary>
            /// <param name="o">object to evaluate</param>
            /// <returns>True: object is a basic type</returns>
            public static bool IsBasicType(object o)
            {
                return IsBasicType(o.GetType());
            }

            /// <summary>
            /// Definition of basic types of BSEngine.
            /// True if the given object is considered basic type
            /// </summary>
            /// <param name="o">object to evaluate</param>
            /// <returns>True: object is a basic type</returns>
            public static bool IsBasicType(Type t)
            {
                return t == typeof(string)
                    || t == typeof(int)
                    || t == typeof(short)
                    || t == typeof(long)
                    || t == typeof(float)
                    || t == typeof(double)
                    || t == typeof(bool)
                    || t == typeof(char)
                    || t == typeof(byte)
                    || t == typeof(BSEngine.Math.Vector2)
                    || t == typeof(BSEngine.Math.Vector3)
                    || t == typeof(BSEngine.Math.Vector4)
                    || t == typeof(BSEngine.Math.Quaternion);
                    //|| t == typeof(Transform);
            }

            /// <summary>
            /// Definition of basic types of BSEngine - Math namespace.
            /// True if the given object is considered basic type
            /// </summary>
            /// <param name="o">object to evaluate</param>
            /// <returns>True: object is a basic type</returns>
            public static bool IsMathBasicType(object o)
            {
                return IsMathBasicType(o.GetType());
            }


            /// <summary>
            /// Definition of basic types of BSEngine - Math namespace.
            /// True if the given object is considered basic type
            /// </summary>
            /// <param name="o">object to evaluate</param>
            /// <returns>True: object is a basic type</returns>
            public static bool IsMathBasicType(Type t)
            {
                return t == typeof(BSEngine.Math.Vector2)
                    || t == typeof(BSEngine.Math.Vector3)
                    || t == typeof(BSEngine.Math.Vector4)
                    || t == typeof(BSEngine.Math.Quaternion);
            }

            /// <summary>
            /// Definition of basic types of UnityEngine.
            /// True if the given object is considered basic type
            /// </summary>
            /// <param name="o">object to evaluate</param>
            /// <returns>True: object is a basic type</returns>
            public static bool IsUnityBasicType(object o)
            {
                return IsUnityBasicType(o.GetType());
            }


            /// <summary>
            /// Definition of basic types of UnityEngine.
            /// True if the given object is considered basic type
            /// </summary>
            /// <param name="o">object to evaluate</param>
            /// <returns>True: object is a basic type</returns>
            public static bool IsUnityBasicType(Type t)
            {
                return t == typeof(UnityEngine.Vector2)
                    || t == typeof(UnityEngine.Vector3)
                    || t == typeof(UnityEngine.Vector4)
                    || t == typeof(UnityEngine.Quaternion);
                    //|| t == typeof(Transform);
            }

            /// <summary>
            /// Predicate used to determine if an object is a List
            /// </summary>
            /// <param name="o">Ob ject to evaluate</param>
            /// <returns>True: object is a List</returns>
            public static bool IsList(object o)
            {
                if (o == null) return false;
                return o is IList &&
                       o.GetType().IsGenericType &&
                       o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
            }

            /// <summary>
            /// Predicate used to determine if an object is a List
            /// </summary>
            /// <param name="t">Type to evaluate</param>
            /// <returns>True: object is a List</returns>
            public static bool IsList(Type t)
            {
                if (t == null) return false;
                return t.IsGenericType &&
                       t.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
            }

            /// <summary>
            /// Predicate used to determine if an object is a Dictionary
            /// </summary>
            /// <param name="o">Object to evaluate</param>
            /// <returns>True: object is a Dictionary</returns>
            public static bool IsDictionary(object o)
            {
                if (o == null) return false;
                return o is IDictionary &&
                       o.GetType().IsGenericType &&
                       o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
            }

            /// <summary>
            /// Predicate used to determine if an object is a Dictionary
            /// </summary>
            /// <param name="t">Type to evaluate</param>
            /// <returns>True: object is a Dictionary</returns>
            public static bool IsDictionary(Type t)
            {
                if (t == null) return false;
                return t.IsGenericType &&
                       t.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
            }

        }
    }
}




