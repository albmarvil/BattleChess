///----------------------------------------------------------------------
/// @file DataTable.cs
///
/// This file contains the declaration of DataTable class.
///
/// A DataTable is a custom collection of BSEngine.
/// 
/// This collection can store any type. From all the basics (bool, float, int,...) to all
/// the generic collections.
/// 
/// A parameter in a DataTable is identified by it's name (string). You can get and set any value given it's ID.
/// The insertion criteria used it's like Dictionaries. It will overwritte existing data if you're setting an
/// existing parameter
/// 
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 29/10/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;

using System.Collections;

namespace BSEngine
{
    [System.Serializable]
    public class DataTable : IEnumerable<object>
    {

        #region Private params

        /// <summary>
        /// Name of the DataTable
        /// </summary>
        private string m_name;

        /// <summary>
        /// Memory structure where the data is stored
        /// </summary>
        private Dictionary<string, object> m_data;

        /// <summary>
        /// Serialization mode for this DataTable
        /// </summary>
        private SerializationMode m_mode;

        /// <summary>
        /// Flag used to load the DataTable into the Blackboard
        /// </summary>
        private bool m_loadToBlackboard;

        /// <summary>
        /// ID used to give unique names when creating a DataTable from a list
        /// </summary>
        private static int m_ID = 0;

        #endregion

        #region Public methods

        /// <summary>
        /// DataTable constructor
        /// </summary>
        /// <param name="name">Name of the DataTable</param>
        /// <param name="mode">SerializationMode used into the table</param>
        /// <param name="loadToBlackboard">Flag to load the table into the Blackboard</param>
        /// /// <param name="forceBlackboardLoading">OPTIONAL. default: true. Flag to load the table into the Blackboard after creation</param>
        public DataTable(string name, SerializationMode mode, bool loadToBlackboard, bool forceBlackboardLoading = true)
        {
            m_name = name;
            m_mode = mode;
            m_loadToBlackboard = loadToBlackboard;
            m_data = new Dictionary<string, object>();

            if (m_loadToBlackboard && forceBlackboardLoading && StorageMgr.Blackboard != null)
            {
                StorageMgr.Blackboard.Set<DataTable>(name, this);
            }
        }

        /// <summary>
        /// Name of the DataTable
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// DataTable loaded in blackboard
        /// </summary>
        public bool LoadToBlackboard
        {
            get { return m_loadToBlackboard; }
        }

        /// <summary>
        /// Serialization mdoe of the dataTable
        /// </summary>
        public SerializationMode SerializationMode
        {
            get { return m_mode; }
        }

        /// <summary>
        /// Data of the internal structure
        /// </summary>
        //public Dictionary<string, object> Data
        //{
        //    get { return m_data; }
        //}

        public IEnumerable<string> Keys
        {
            get { return m_data.Keys; }
        }

        /// <summary>
        /// Getter method to obtain a parameter from the Table
        /// </summary>
        /// <typeparam name="T">Type of the parameter to get</typeparam>
        /// <param name="name">Name of the parameter to get</param>
        /// <returns>Desired parameter casted to T type</returns>
        public T Get<T>(string name)
        {
            try
            {
                return (T)m_data[name];
            }
            catch (System.InvalidCastException e)
            {
                Debug.LogWarning("Get('"+name+"') from DataTable("+Name+") not valid: " + e.Message);
                return default(T);
            }
            
        }

        /// <summary>
        /// Setter method to push a parameter into the table
        /// </summary>
        /// <typeparam name="T">Type of the parameter to push into the table</typeparam>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        public void Set<T>(string name, T value)
        {
            m_data[name] = value;
        }

        /// <summary>
        /// Contains key predicate for DataTables
        /// </summary>
        /// <param name="key">Key to find</param>
        /// <returns>True if that key is in the DataTable</returns>
        public bool ContainsKey(string key)
        {
            return m_data.ContainsKey(key);
        }

        /// <summary>
        /// [] operator. (ArrayLists)
        /// 
        /// DataTables can be used with indexes as Lists do.
        /// 
        /// ***WARNING*** this operator returns objects, you should be responsible of casting those objects to
        /// their correct types
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>object at the given index (not casted)</returns>
        public object this[int i]
        {
            get { return m_data[i.ToString()]; }

            set { m_data[i.ToString()] = value; }
        }

        /// <summary>
        /// [] operator. (Dictionaries)
        /// 
        /// DataTables can be used as Dictionaries.
        /// 
        /// ***WARNING*** this operator returns objects, you should be responsible of casting those objects to
        /// their correct types
        /// </summary>
        /// <param name="key">key value to index</param>
        /// <returns>Value corresponding to the given key</returns>
        public object this[string key]
        {
            get { return m_data[key]; }
            set { m_data[key] = value; }
        }

        /// <summary>
        /// Clears all the data within the table
        /// </summary>
        public void Clear()
        {
            m_data.Clear();
        }

        /// <summary>
        /// Implementation from IEnumerator interface.
        /// 
        /// DataTables will return object values contained in m_data
        /// when used in a 'foreach' loop
        /// </summary>

        public IEnumerator<object> GetEnumerator()
        {
            foreach (string key in m_data.Keys)
            {
                yield return m_data[key];
            }
        }

        /// <summary>
        /// Implementation for IEnumerator interface
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



        /// <summary>
        /// Static method.
        /// Transforms a List to a DataTable. Recommended to use in serialization.
        /// 
        /// Lists cannot be serialized.
        /// </summary>
        /// <param name="list">List to transform</param>
        /// <returns>DataTable with the list's data</returns>
        public static DataTable ListAsDataTable(List<object> list)
        {
            DataTable data = new DataTable("ListAsDataTable" + (m_ID++).ToString(), SerializationMode.BIN, false);

            for (int i = 0; i < list.Count; ++i)
            {
                data[i] = list[i];
            }

            return data;
        }

        #endregion
        
    }
}
