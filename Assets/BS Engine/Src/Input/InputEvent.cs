///----------------------------------------------------------------------
/// @file InputEvent.cs
///
/// This file contains the declaration of InputEvent class.
/// 
/// InputEvent is a container class with stores info related to logic orders given through input. 
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 11/09/2015
///----------------------------------------------------------------------



using System.Collections;

namespace BSEngine
{
    namespace Input
    {
        public class InputEvent
        {

            #region Private params

            /// <summary>
            /// Name of the logic order given
            /// </summary>
            private string m_order;


            /// <summary>
            /// Flag that indicates if the given order is valid
            /// True by default
            /// </summary>
            private bool m_ok = true;

            /// <summary>
            /// Optional value of the order
            /// 0  by default
            /// </summary>
            private float m_value = 0.0f;

            #endregion

            #region Public methods

            /// <summary>
            /// Public constructor of the InputEvent
            /// </summary>
            /// <param name="order">Logic Order name</param>
            /// <param name="ok">OPTIONAL, DEFAULT: true. Status of the order</param>
            /// <param name="value">OPTIONAL, DEFAULT: 0.0f, optional value of the order</param>
            public InputEvent(string order, bool ok = true, float value = 0.0f)
            {
                m_order = order;
                m_ok = ok;
                m_value = value;
            }

            /// <summary>
            /// Property to access to the order name
            /// </summary>
            public string Order
            {
                get { return m_order; }
            }

            /// <summary>
            /// Property to access to the order state
            /// </summary>
            public bool isOk
            {
                get { return m_ok; }
            }

            /// <summary>
            /// Property to access to the optional value of the order
            /// </summary>
            public float Value
            {
                get { return m_value; }
            }

            #endregion
        }

    }
}
