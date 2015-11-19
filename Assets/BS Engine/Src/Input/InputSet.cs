///----------------------------------------------------------------------
/// @file InputSet.cs
///
/// This file contains the declaration of InputSet class.
/// 
/// An InputSet on Bird Soul Engine represents a set of keys related to logic orders.
/// 
/// One key can be binded to multiple logic orders.
/// 
/// InputSets are created by Dictionaries containing the bindings, it can also be created by files (XML) ***work in progress***
/// 
/// InputSets are related to a context defined by the Application State, so you can have multiple key bindings depending on the App context. 
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 11/09/2015
///----------------------------------------------------------------------


using System.Collections.Generic;
using System;

namespace BSEngine
{
    namespace Input
    {
        public class InputSet
        {

            #region Private params

            /// <summary>
            /// Key bindings storage. One Key ---> Multiple logic orders
            /// </summary>
            protected Dictionary<BSKeyCode, List<string>> m_key2order = new Dictionary<BSKeyCode, List<string>>();

            /// <summary>
            /// Key listeners storege, indexed by logic orders
            /// </summary>
            private Dictionary<string, onOrderReceived> m_orderReceived = new Dictionary<string,onOrderReceived>();

            /// <summary>
            /// InputSet name. Usually the same as the context + "InputSet"
            /// </summary>
            private string m_name;

            /// <summary>
            /// onMouseMoved function callbacks.
            /// </summary>
            private onMouseMoved m_mouseMoved;

            /// <summary>
            /// Current MouseCfg of the InputSet
            /// </summary>
            private MouseCfg m_mouseCfg;

            #endregion

            #region Public methods

            /// <summary>
            /// Constructor of the key bindings of the orders
            /// </summary>
            public InputSet(string name, Dictionary<BSKeyCode, List<string>> keyBindings, MouseCfg mouseCfg)
            {

                m_key2order = keyBindings;
                m_name = name;

                m_mouseMoved = null;

                m_mouseCfg = mouseCfg;
            }

            /// <summary>
            /// Constructor from DataTable
            /// </summary>
            /// <param name="inputSetData">DataTable loaded from CFG file</param>
            public InputSet(DataTable inputSetData)
            {

                m_key2order = new Dictionary<BSKeyCode, List<string>>();

                DataTable keyBindings = inputSetData.Get<DataTable>("KeyBindings");

                foreach (string key in keyBindings.Keys)
                {
                    BSKeyCode keyCode = (BSKeyCode)Enum.Parse(typeof(BSKeyCode), key);
                    List<string> logicOrders = new List<string>();

                    DataTable orders = keyBindings.Get<DataTable>(key);

                    foreach (object obj in orders)
                    {
                        logicOrders.Add((string)obj);
                    }

                    m_key2order.Add(keyCode, logicOrders);
                }

                m_name = inputSetData.Name; ;

                m_mouseMoved = null;

                if (inputSetData.ContainsKey("MouseCfg"))
                {
                    DataTable mouseCfgData = inputSetData.Get<DataTable>("MouseCfg");
                    m_mouseCfg = new MouseCfg(mouseCfgData);
                }
                else
                {
                    m_mouseCfg = null;
                }
                
            }

            /// <summary>
            /// Public access to the InputSet's name
            /// </summary>
            public string Name
            {
                get { return m_name; }
            }

            /// <summary>
            /// Public access to the InputSet's key bindings
            /// </summary>
            public Dictionary<BSKeyCode, List<string>> KeyBindings
            {
                get { return m_key2order; }
            }

            /// <summary>
            /// Public access to the InputSet's listeners
            /// </summary>
            public Dictionary<string, onOrderReceived> OrdersListeners
            {
                get { return m_orderReceived; }
            }


            /// <summary>
            /// Method used to register a listener to events of a specific order
            /// </summary>
            /// <param name="order">specific order listening events to</param>
            /// <param name="listener">delegate function to call</param>
            public void RegisterOnOrderReceived(string order, onOrderReceived listener)
            {
                if(m_orderReceived.ContainsKey(order))
                {
                    m_orderReceived[order] += listener;
                }
                else
                {
                    onOrderReceived orderReceived = null;
                    orderReceived += listener;
                    m_orderReceived.Add(order, orderReceived);
                }
            }

            /// <summary>
            /// Method used to unregister a listener to events of a specific order
            /// </summary>
            /// <param name="order">specific order listening events to</param>
            /// <param name="listener">delegate function to call</param>
            public void UnregisterOnOrderReceived(string order, onOrderReceived listener)
            {
                if (m_orderReceived.ContainsKey(order))
                {
                    m_orderReceived[order] -= listener;
                }
            }

            /// <summary>
            /// Method used to release all the listeners
            /// </summary>
            public void ReleaseAllOrderListeners()
            {
                foreach (string key in m_orderReceived.Keys)
                {
                    m_orderReceived[key] = null;
                }

                m_orderReceived.Clear();
            }


            /// <summary>
            /// Method used to register MouseMoved listeners
            /// </summary>
            /// <param name="listener">Function callback to register</param>
            public void RegisterOnMouseMoved(onMouseMoved listener)
            {

                m_mouseMoved += listener;
               
            }

            /// <summary>
            /// Method used to unregister MouseMoved listeners
            /// </summary>
            /// <param name="listener">Function callback to register</param>
            public void UnregisterOnMouseMoved(onMouseMoved listener)
            {
                m_mouseMoved -= listener;
            }

            /// <summary>
            /// Method used to release all mouse listeners from the input set
            /// </summary>
            public void ReleaseAllMouseListeners()
            {
                m_mouseMoved = null;
            }

            /// <summary>
            /// Current mouse configuration
            /// </summary>
            public MouseCfg MouseCfg
            {
                get { return m_mouseCfg; }
            }

            /// <summary>
            /// Is mouse supported??
            /// </summary>
            public bool MouseSupported
            {
                get { return m_mouseCfg != null && m_mouseMoved != null; }
            }

            /// <summary>
            /// Current mouse listeners registered
            /// </summary>
            public onMouseMoved MouseListeners
            {
                get { return m_mouseMoved; }
            }

            /// <summary>
            /// Translates all the class info into a DataTable
            /// </summary>
            public DataTable ToDataTable()
            {
                DataTable data = new DataTable(m_name, SerializationMode.NONE, false);

                DataTable keyBindings = new DataTable("KeyBindings", SerializationMode.NONE, false);

                foreach (BSKeyCode code in m_key2order.Keys)
                {
                    string sCode = code.ToString();

                    DataTable logicOrders = new DataTable(sCode, SerializationMode.NONE, false);

                    List<string> orders = m_key2order[code];

                    for (int i = 0; i < orders.Count; ++i)
                    {
                        logicOrders[i] = orders[i];
                    }

                    keyBindings.Set<DataTable>(sCode, logicOrders);
                }

                data.Set<DataTable>(keyBindings.Name, keyBindings);

                if (m_mouseCfg != null)
                {
                    DataTable MouseCfg = m_mouseCfg.ToDataTable();

                    data.Set<DataTable>("MouseCfg", MouseCfg);
                }

                return data;
            }

            #endregion
        }
    }
}

