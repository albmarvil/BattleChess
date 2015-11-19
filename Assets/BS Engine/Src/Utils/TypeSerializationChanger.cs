///----------------------------------------------------------------------
/// @file TypeSerializationChanger.cs
///
/// This file contains the declaration of TypeSerializationChanger class.
/// 
/// This helper class is used to translate types from Unity to BSEngine types and viceversa.
/// 
/// Used to translate the types of a DataTable before Serialization and after Deserialization, Due to Unserializable Unity Types.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 10/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;
using System;

using BSEngine.Math;


namespace BSEngine
{
    namespace Utils
    {
        public class TypeSerializationChanger
        {
            #region Public Methods
            /// <summary>
            /// Translate all the types contained in a DataTable to valid BSEngine Types
            /// </summary>
            /// <param name="info">DataTable to transform</param>
            /// <returns>TRANSLATED COPY of the given DataTable</returns>
            public static DataTable DataTableTypesToBSEngine(DataTable info)
            {
                DataTable res = new DataTable(info.Name, info.SerializationMode, info.LoadToBlackboard, false);


                foreach (string key in info.Keys)
                {
                    object obj = info[key];

                    Type type = obj.GetType();

                    object newObj = null;

                    if (Predicates.IsUnityBasicType(obj))
                    {
                        newObj = UnityToBSEngine(obj);
                    }
                    else if (Predicates.IsBasicType(obj))
                    {
                        newObj = obj;
                    }
                    else if (type == typeof(DataTable))
                    {
                        newObj = DataTableTypesToBSEngine((DataTable)obj);
                    }
                    //else if (Predicates.IsList(info.Data[key]))
                    //{
                    //    newObj = ChangeListTypes((List<object>)obj);
                    //}
                    //else if (Predicates.IsDictionary(obj))
                    //{
                    //    newObj = ChangeDictionaryTypes((Dictionary<object, object>)obj);
                    //}
                    else
                    {
                        Debug.LogError("Type: " + type + " NOT supported on serialization.");
                    }

                    //res.Data.Add(key, newObj);
                    res[key] = newObj;

                }

                return res;
            }


            /// <summary>
            /// Translate all the types contained in a DataTable to valid UnityEngine Types
            /// </summary>
            /// <param name="info">DataTable to transform</param>
            /// <returns>TRANSLATED COPY of the given DataTable</returns>
            public static DataTable DataTableTypesToUnity(DataTable info)
            {
                DataTable res = new DataTable(info.Name, info.SerializationMode, info.LoadToBlackboard, false);


                foreach (string key in info.Keys)
                {
                    object obj = info[key];

                    Type type = obj.GetType();

                    object newObj = null;

                    if (Predicates.IsMathBasicType(obj))
                    {
                        newObj = BSEngineToUnity(obj);
                    }
                    else if (Predicates.IsBasicType(obj))
                    {
                        newObj = obj;
                    }
                    else if (type == typeof(DataTable))
                    {
                        newObj = DataTableTypesToBSEngine((DataTable)obj);
                    }
                    //else if (Predicates.IsList(info.Data[key]))
                    //{
                    //    newObj = ChangeListTypes((List<object>)obj);
                    //}
                    //else if (Predicates.IsDictionary(obj))
                    //{
                    //    newObj = ChangeDictionaryTypes((Dictionary<object, object>)obj);
                    //}
                    else
                    {
                        Debug.LogError("Type: " + type + " NOT supported on serialization.");
                    }

                    //res.Data.Add(key, newObj);
                    res[key] = newObj;
                }

                return res;
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// 
            /// </summary>
            /// <param name="info"></param>
            /// <returns></returns>
            /*private static List<object> ChangeListTypes(List<object> info)
            {
                List<object> res = new List<object>();

                foreach (object obj in info)
                {

                    Type type = obj.GetType();

                    object newObj = null;

                    if (Predicates.IsUnityBasicType(obj))
                    {
                        newObj = ChangeUnityBasicType(obj);
                    }
                    else if (Predicates.IsBasicType(obj))
                    {
                        newObj = obj;
                    }
                    else if (type == typeof(DataTable))
                    {
                        newObj = ChangeDataTableTypes((DataTable)obj);
                    }
                    else if (Predicates.IsList(obj))
                    {
                        newObj = ChangeListTypes((List<object>)obj);
                    }
                    else if (Predicates.IsDictionary(obj))
                    {
                        newObj = ChangeDictionaryTypes((Dictionary<object, object>)obj);
                    }
                    else
                    {
                        Debug.LogError("Type: " + type + " NOT supported on serialization.");
                    }

                    res.Add(newObj);
                }

                return res;
            }*/

            /// <summary>
            /// 
            /// </summary>
            /// <param name="info"></param>
            /// <returns></returns>
            /*private static Dictionary<object, object> ChangeDictionaryTypes(Dictionary<object, object> info)
            {
                Dictionary<object, object> res = new Dictionary<object, object>();

                foreach (object key in info.Keys)
                {
                    #region change key

                    Type keyType = key.GetType();

                    object newKey = null;

                    if (Predicates.IsUnityBasicType(key))
                    {
                        newKey = ChangeUnityBasicType(key);
                    }
                    else if (Predicates.IsBasicType(key))
                    {
                        newKey = key;
                    }
                    else if (keyType == typeof(DataTable))
                    {
                        newKey = ChangeDataTableTypes((DataTable)key);
                    }
                    else if (Predicates.IsList(key))
                    {
                        newKey = ChangeListTypes((List<object>)key);
                    }
                    else if (Predicates.IsDictionary(key))
                    {
                        newKey = ChangeDictionaryTypes((Dictionary<object, object>)key);
                    }
                    else
                    {
                        Debug.LogError("Type: " + keyType + " NOT supported on serialization.");
                    }

                    #endregion

                    #region change value

                    object value = info[key];

                    Type valueType = value.GetType();

                    object newValue = null;

                    if (Predicates.IsUnityBasicType(value))
                    {
                        newValue = ChangeUnityBasicType(value);
                    }
                    else if (Predicates.IsBasicType(value))
                    {
                        newValue = value;
                    }
                    else if (valueType == typeof(DataTable))
                    {
                        newValue = ChangeDataTableTypes((DataTable)value);
                    }
                    else if (Predicates.IsList(value))
                    {
                        newValue = ChangeListTypes((List<object>)value);
                    }
                    else if (Predicates.IsDictionary(value))
                    {
                        newValue = ChangeDictionaryTypes((Dictionary<object, object>)value);
                    }
                    else
                    {
                        Debug.LogError("Type: " + valueType + " NOT supported on serialization.");
                    }

                    #endregion

                    res.Add(newKey, newValue);
                }

                return res;
            }*/

            /// <summary>
            /// Translation from basic UnityEnginge Types to BSEngine Types
            /// </summary>
            /// <param name="obj">Object to transfrom</param>
            /// <returns>Object with translated type</returns>
            private static object UnityToBSEngine(object obj)
            {
                if (obj.GetType() == typeof(UnityEngine.Vector2))
                {
                    BSEngine.Math.Vector2 v2  = new BSEngine.Math.Vector2((UnityEngine.Vector2) obj);
                    return v2;
                }
                else if (obj.GetType() == typeof(UnityEngine.Vector3))
                {
                    BSEngine.Math.Vector3 v3 = new BSEngine.Math.Vector3((UnityEngine.Vector3)obj);
                    return v3;
                }
                else if (obj.GetType() == typeof(UnityEngine.Vector4))
                {
                    BSEngine.Math.Vector4 v4 = new BSEngine.Math.Vector4((UnityEngine.Vector4)obj);
                    return v4;
                }
                else if (obj.GetType() == typeof(UnityEngine.Quaternion))
                {
                    BSEngine.Math.Quaternion q = new BSEngine.Math.Quaternion((UnityEngine.Quaternion)obj);
                    return q;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Translation from basic BSEngine Types to UnityEnginge Types
            /// </summary>
            /// <param name="obj">Object to transfrom</param>
            /// <returns>Object with translated type</returns>
            private static object BSEngineToUnity(object obj)
            {
                if (obj.GetType() == typeof(BSEngine.Math.Vector2))
                {
                    BSEngine.Math.Vector2 bse = (BSEngine.Math.Vector2)obj;
                    UnityEngine.Vector2 v2 = new UnityEngine.Vector2(bse.x, bse.y);
                    return v2;
                }
                else if (obj.GetType() == typeof(BSEngine.Math.Vector3))
                {
                    BSEngine.Math.Vector3 bse = (BSEngine.Math.Vector3)obj;
                    UnityEngine.Vector3 v3 = new UnityEngine.Vector3(bse.x, bse.y, bse.z);
                    return v3;
                }
                else if (obj.GetType() == typeof(BSEngine.Math.Vector4))
                {
                    BSEngine.Math.Vector4 bse = (BSEngine.Math.Vector4)obj;
                    UnityEngine.Vector4 v4 = new UnityEngine.Vector4(bse.x, bse.y, bse.z, bse.w);
                    return v4;
                }
                else if (obj.GetType() == typeof(BSEngine.Math.Quaternion))
                {
                    BSEngine.Math.Quaternion bse = (BSEngine.Math.Quaternion)obj;
                    UnityEngine.Quaternion q = new UnityEngine.Quaternion(bse.x, bse.y, bse.z, bse.w);
                    return q;
                }
                else
                {
                    return null;
                }
            }

            #endregion

        }
    }
}
