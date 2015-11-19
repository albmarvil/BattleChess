///----------------------------------------------------------------------
/// @file StorageMgr.cs
///
/// This file contains the declaration of StorageMgr class.
/// 
/// StorageMgr has two main features:
/// -Blackboard (volatile memory) management
/// -File management (Load/Save)
/// 
/// 1 -Blackboard
/// 
/// The blackboard is a huge DataTable used to hold gamespecific information. It's objective is to be used as a "central bucket of info", where you can write info into it and recover
/// it lately from other part of the game.
/// 
/// Blackboard memory is volatile. Everything on it will be flushed away after the application is shutted down.
/// 
/// If the Blackboard has a DataTable inside, that has been previosly loaded from a file  (IE: PlayerData or Save game files). This DataTable will be saved again to the same file 
/// before clearing the blackboard.
/// 
/// 
/// 2 - File management
/// 
/// This manager allows to serialize data in form of "DataTables" to a binary file or XML file
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 29/10/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System;
using System.Threading;

using BSEngine.Utils;

namespace BSEngine
{
    /// <summary>
    /// Serialization mode. uised to indicate how to serialize a DataTable
    /// </summary>
    [System.Serializable]
    public enum SerializationMode
    {
        NONE,
        XML,
        BIN,
        BIN_XML
    }

    /// <summary>
    /// Struct used to hold the data before starting a new thread to serialize it.
    /// </summary>
    public struct SaveData
    {
        public DataTable m_data;

        public string m_fullPath;
    }

    public class StorageMgr
    {


        #region Singleton

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static StorageMgr m_instance = null;

        /// <summary>
        /// Property to get the singleton instance of the class.
        /// </summary>
        public static StorageMgr Singleton { get { return m_instance; } }

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static StorageMgr() { }

        /// <summary>
        /// Used to initialize the StorageMgr singleton instance
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
                m_instance = new StorageMgr();
                return m_instance.open();
            }
        }

        /// <summary>
        /// Used to deinitialize the StorageMgr singleton data.
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
            m_blackboard = null;
            m_blackboard = new DataTable("Blackboard", SerializationMode.XML, false);
#if !UNITY_WEBPLAYER
            LoadCFGFile();
#endif
            return true;
        }

        /// <summary>
        /// Used as second step on singleton initialisation. Used to specific code of the different Engine & Game managers
        /// </summary>
        private void close()
        {
            ClearBlackboard();
            m_blackboard = null;
        }

        #endregion

        #region Private params

        /// <summary>
        /// DataTable used as blackboard
        /// </summary>
        private static DataTable m_blackboard;

        #endregion

        #region Public methods

        /// <summary>
        /// Public property to access to the Blackboard
        /// </summary>
        public static DataTable Blackboard
        {
            get { return m_blackboard; }
        }

        /// <summary>
        /// Method used to clear the balckboard.
        /// 
        /// It will serialize all the DataTables within the blackboard,
        /// that were previously loaded from a file into the blackboard.
        /// 
        /// If the Debug flag is activated, it also serialize the remaining data in a
        /// "Blackboard" file.
        /// </summary>
        public static void ClearBlackboard()
        {
#if !UNITY_WEBPLAYER
            foreach (object data in m_blackboard)
            {
                if (data.GetType() == typeof(DataTable))
                {
                    DataTable dataTable = (DataTable)data;
                    if (dataTable.LoadToBlackboard)
                    {
                        if (dataTable.Name != "CFG")
                        {
                            StorageMgr.Singleton.SaveToFileAsync(dataTable, dataTable.Name);
                        }
                        else
                        {
                            StorageMgr.Singleton.SaveToFileAsync(dataTable, GameMgr.Singleton.Loader.m_CFGFileName);
                        }
                    }
                }
            }

            if (GameMgr.Singleton.Loader.m_SaveDebugBlackboard)
            {
                StorageMgr.Singleton.SaveToFileAsync(m_blackboard, "Blackboard");
            }
#endif

            m_blackboard.Clear();
        }
        
#if !UNITY_WEBPLAYER 

        #region Save Methods
        /// <summary>
        /// Method used to save a DataTable to a file.
        /// 
        /// Using the Application.persistentDataPath
        /// </summary>
        /// <param name="info">DataTable to save</param>
        /// <param name="fileName">Name of the file to save</param>
        public void SaveToFileAsync(DataTable info, string fileName)
        {
            string fullPath = Application.persistentDataPath + "/" + fileName;

            DataTable toSerialize = TypeSerializationChanger.DataTableTypesToBSEngine(info);

            Thread thread = null;

            SaveData dataToSave = new SaveData();
            dataToSave.m_data = toSerialize;

            switch (info.SerializationMode)
            {
                case SerializationMode.XML:
                    fullPath += ".xml";
                    dataToSave.m_fullPath = fullPath;

                    thread = new Thread(new ParameterizedThreadStart(SaveToXMLFile));
                    thread.Start(dataToSave);
                    break;
                case SerializationMode.BIN:
                    fullPath += ".bs";
                    dataToSave.m_fullPath = fullPath;

                    thread = new Thread(new ParameterizedThreadStart(SaveToBinaryFile));
                    thread.Start(dataToSave);
                    break;
                case SerializationMode.BIN_XML:
                    fullPath += ".xml";
                    dataToSave.m_fullPath = fullPath;
                    thread = new Thread(new ParameterizedThreadStart(SaveToXMLFile));
                    thread.Start(dataToSave);

                    fullPath = Application.persistentDataPath + "/" + fileName + ".bs";
                    dataToSave.m_fullPath = fullPath;
                    Thread thread2 = new Thread(new ParameterizedThreadStart(SaveToBinaryFile));
                    thread2.Start(dataToSave);
                    break;
                case SerializationMode.NONE:
                    break;
            }

        }

        /// <summary>
        /// Method used to sace DataTable to a file
        /// 
        /// It saves to the file in the folder given.
        /// </summary>
        /// <param name="info">DataTable to save</param>
        /// <param name="folder">Folder of the fullPath to save the file</param>
        /// <param name="fileName">Name of the file to save</param>
        public void SaveToFullPathFileAsync(DataTable info, string folder, string fileName)
        {
            string fullPath = folder + "/" + fileName;

            DataTable toSerialize = TypeSerializationChanger.DataTableTypesToBSEngine(info);

            Thread thread = null;

            SaveData dataToSave = new SaveData();
            dataToSave.m_data = toSerialize;

            switch (info.SerializationMode)
            {
                case SerializationMode.XML:
                    fullPath += ".xml";
                    dataToSave.m_fullPath = fullPath;

                    thread = new Thread(new ParameterizedThreadStart(SaveToXMLFile));
                    thread.Start(dataToSave);
                    break;
                case SerializationMode.BIN:
                    fullPath += ".bs";
                    dataToSave.m_fullPath = fullPath;

                    thread = new Thread(new ParameterizedThreadStart(SaveToBinaryFile));
                    thread.Start(dataToSave);
                    break;
                case SerializationMode.BIN_XML:
                    fullPath += ".xml";
                    dataToSave.m_fullPath = fullPath;
                    thread = new Thread(new ParameterizedThreadStart(SaveToXMLFile));
                    thread.Start(dataToSave);

                    fullPath = folder + "/" + fileName + ".bs";
                    dataToSave.m_fullPath = fullPath;
                    Thread thread2 = new Thread(new ParameterizedThreadStart(SaveToBinaryFile));
                    thread2.Start(dataToSave);
                    break;
                case SerializationMode.NONE:
                    break;
            }

        }


        /// <summary>
        /// Method used to save a DataTable to a file. (Blocking operation)
        /// 
        /// Using the Application.persistentDataPath
        /// </summary>
        /// <param name="info">DataTable to save</param>
        /// <param name="fileName">Name of the file to save</param>
        public void SaveToFile(DataTable info, string fileName)
        {
            string fullPath = Application.persistentDataPath + "/" + fileName;

            DataTable toSerialize = TypeSerializationChanger.DataTableTypesToBSEngine(info);

            SaveData dataToSave = new SaveData();
            dataToSave.m_data = toSerialize;

            switch (info.SerializationMode)
            {
                case SerializationMode.XML:
                    fullPath += ".xml";
                    dataToSave.m_fullPath = fullPath;
                    SaveToXMLFile(dataToSave);
                    break;
                case SerializationMode.BIN:
                    fullPath += ".bs";
                    dataToSave.m_fullPath = fullPath;
                    SaveToBinaryFile(dataToSave);
                    break;
                case SerializationMode.BIN_XML:
                    fullPath += ".xml";
                    dataToSave.m_fullPath = fullPath;
                    SaveToXMLFile(dataToSave);

                    fullPath = Application.persistentDataPath + "/" + fileName + ".bs";
                    dataToSave.m_fullPath = fullPath;
                    SaveToBinaryFile(dataToSave);
                    break;
                case SerializationMode.NONE:
                    break;
            }

        }

        /// <summary>
        /// Method used to sace DataTable to a file
        /// 
        /// It saves to the file in the folder given.
        /// </summary>
        /// <param name="info">DataTable to save</param>
        /// <param name="folder">Folder of the fullPath to save the file</param>
        /// <param name="fileName">Name of the file to save</param>
        public void SaveToFullPathFile(DataTable info, string folder, string fileName)
        {
            string fullPath = folder + "/" + fileName;

            DataTable toSerialize = TypeSerializationChanger.DataTableTypesToBSEngine(info);

            SaveData dataToSave = new SaveData();
            dataToSave.m_data = toSerialize;

            switch (info.SerializationMode)
            {
                case SerializationMode.XML:
                    fullPath += ".xml";
                    dataToSave.m_fullPath = fullPath;
                    SaveToXMLFile(dataToSave);
                    break;
                case SerializationMode.BIN:
                    fullPath += ".bs";
                    dataToSave.m_fullPath = fullPath;
                    SaveToBinaryFile(dataToSave);
                    break;
                case SerializationMode.BIN_XML:
                    fullPath += ".xml";
                    dataToSave.m_fullPath = fullPath;
                    SaveToXMLFile(dataToSave);

                    fullPath = folder + "/" + fileName + ".bs";
                    dataToSave.m_fullPath = fullPath;
                    SaveToBinaryFile(dataToSave);
                    break;
                case SerializationMode.NONE:
                    break;
            }
        }

        #endregion

        #region Load Methods
        /// <summary>
        /// Method used to load a DataTable from a file. (Blocking operation)
        /// 
        /// Using the Application.persistentDataPath
        /// 
        /// It will load an xml or an binary file depending on the file given
        /// </summary>
        /// <param name="fileName">File name to load</param>
        /// <returns>DataTable loaded from the given file</returns>
        public DataTable LoadFile(string fileName)
        {
            string fullPath = Application.persistentDataPath + "/" + fileName;

            string[] split = fileName.Split('.');

            string extension = split[split.Length - 1];

            DataTable data = null;

            if (extension == "bs")
            {
                data = TypeSerializationChanger.DataTableTypesToUnity(LoadBinaryFile(fullPath));
            }
            else if (extension == "xml")
            {
                data = TypeSerializationChanger.DataTableTypesToUnity(LoadXMLFile(fullPath));
            }

            if (data.LoadToBlackboard)
            {
                Blackboard.Set<DataTable>(data.Name, data);
            }

            return data;
        }


        /// <summary>
        /// Method used to load a DataTable from a file. (Blocking operation)
        /// 
        /// Using the fullPath given
        /// 
        /// It will load an xml or an binary file depending on the file given
        /// </summary>
        /// <param name="fullPath">File name to load</param>
        /// <returns>DataTable loaded from the given file</returns>
        public DataTable LoadFullPathFile(string fullPath)
        {

            string[] split = fullPath.Split('.');

            string extension = split[split.Length - 1];

            DataTable data = null;

            if (extension == "bs")
            {
                data = TypeSerializationChanger.DataTableTypesToUnity(LoadBinaryFile(fullPath));
            }
            else if (extension == "xml")
            {
                data = TypeSerializationChanger.DataTableTypesToUnity(LoadXMLFile(fullPath));
            }

            if (data.LoadToBlackboard)
            {
                Blackboard.Set<DataTable>(data.Name, data);
            }

            return data;
        }
        #endregion

#endif
        #endregion

        #region Private methods

#if !UNITY_WEBPLAYER

        /// <summary>
        /// Method used in the BSEngineLoader script to load the
        /// main configuration file.
        /// 
        /// In case that the configuration a file weren't in place, an alert will be raised
        /// </summary>
        private void LoadCFGFile()
        {
            string path = Application.persistentDataPath;

            if (File.Exists(path + "/" + GameMgr.Singleton.Loader.m_CFGFileName + ".xml"))
            {
                LoadFile(GameMgr.Singleton.Loader.m_CFGFileName + ".xml");
            }
            else if (File.Exists(path + "/" + GameMgr.Singleton.Loader.m_CFGFileName + ".bs"))
            {
                LoadFile(GameMgr.Singleton.Loader.m_CFGFileName + ".bs");
            }
            else
            {
                Debug.LogWarning("BSEngine_CFG file not found, creating default CFG from  BSEngineLoader Script");
                new DataTable("CFG", SerializationMode.XML, true, true);
                GameMgr.Singleton.Loader.CreateDefaultCFGFile();
            }
        }


        #region XML methods
        /// <summary>
        /// XML serialization method.
        /// 
        /// Method used to save a DataTable in XML format.
        /// 
        /// It uses XMLSerializer helper class.
        /// </summary>
        /// <param name="data">SaveData struct to save</param>
        private void SaveToXMLFile(object data)
        {
            SaveData d = (SaveData)data;
            DataTable info = d.m_data;
            string fullPath = d.m_fullPath;

            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("BSEngine_XML");

            XmlAttribute dateAttribute = doc.CreateAttribute("date");
            dateAttribute.Value = System.DateTime.Now.ToString();
            rootNode.Attributes.Append(dateAttribute);

            doc.AppendChild(rootNode);

            XMLSerializer.SerializeDataTableToXML(ref doc, ref rootNode, ref info);

            doc.Save(fullPath);

        }

        /// <summary>
        /// XML deserialization method.
        /// 
        /// Method used to load a DataTable from a XML
        /// 
        /// It uses XMLSerializer helper class.
        /// </summary>
        /// <param name="fullPath">file's fullpath in the system</param>
        /// <returns>DataTable loaded from XML</returns>
        private DataTable LoadXMLFile(string fullPath)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(fullPath);

            XmlNode rootNode = doc.FirstChild;

            DataTable data = XMLSerializer.DeserializeDataTableFromXML(rootNode);

            return data;
        }
        #endregion

        #region Binary methods

        /// <summary>
        /// Binary serialization method.
        /// </summary>
        /// <param name="data">SaveData struct to save</param>
        private void SaveToBinaryFile(object data)
        {
            SaveData d = (SaveData)data;
            DataTable info = d.m_data;
            string fullPath = d.m_fullPath;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fullPath, FileMode.OpenOrCreate);

            bf.Serialize(file, info);

            file.Close();
        }

        /// <summary>
        /// Binary deserialization method
        /// </summary>
        /// <param name="fullPath">File's fullpath in the system</param>
        /// <returns>DataTable loaded from binary file</returns>
        private DataTable LoadBinaryFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(fullPath, FileMode.Open);

                DataTable data = (DataTable)bf.Deserialize(file);

                file.Close();

                return data;
            }
            else
            {
                return null;
            }
        }

        #endregion

#endif

        #endregion
    }
}
