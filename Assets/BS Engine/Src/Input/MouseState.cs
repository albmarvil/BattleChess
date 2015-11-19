///----------------------------------------------------------------------
/// @file MouseState.cs
///
/// This file contains the declaration of MouseState class.
/// 
/// Represents the state of the mouse with the following params:
/// 
/// -Absolute X-axis position on screen
/// -Absolute Y-axis position on screen
/// -Relative X-axis position on screen
/// -Relative Y-axis position on screen
/// -Increment used on scroll wheel input on the last frame
/// 
/// -Increment on Mouse X-axis
/// -Increment on Mouse Y-axis
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/10/2015
///----------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using BSEngine;



namespace BSEngine.Input
{

    public class MouseState
    {

        #region Private params

        /// <summary>
        /// Absolute screen position of the mouse
        /// </summary>
        private Vector3 m_Abs;

        /// <summary>
        /// Delta scroll of the las frame
        /// </summary>
        private Vector2 m_deltaScroll;

        private float m_XDelta = 0.0f;

        private float m_YDelta = 0.0f;

        #endregion

        #region Public methods

        /// <summary>
        /// Class constructor
        /// </summary>
        public MouseState()
        {
            m_XDelta = 0.0f;
            m_YDelta = 0.0f;
            m_deltaScroll = Vector2.zero;
            m_Abs = new Vector3(UnityEngine.Screen.width * 0.5f, UnityEngine.Screen.height * 0.5f, 0.0f);
        }

        /// <summary>
        /// This method updates the state when called from InputMgr.
        /// 
        /// It uses the mouse configuration defined on the InputSet to update properly
        /// </summary>
        public void Update()
        {
            //update the data ccording to the actual cfg


            
            m_deltaScroll = UnityEngine.Input.mouseScrollDelta;

            if (!GameMgr.Singleton.CurrentState.InputSet.MouseCfg.UseUnityScreenPosition)
            {
                m_XDelta = UnityEngine.Input.GetAxis("Mouse X") * GameMgr.Singleton.CurrentState.InputSet.MouseCfg.Sensivity * GameMgr.Singleton.CurrentState.InputSet.MouseCfg.InvertedXAxisValue;
                m_YDelta = UnityEngine.Input.GetAxis("Mouse Y") * GameMgr.Singleton.CurrentState.InputSet.MouseCfg.Sensivity * GameMgr.Singleton.CurrentState.InputSet.MouseCfg.InvertedYAxisValue;


                float newAbsX = m_Abs.x + m_XDelta;
                newAbsX = Mathf.Max(0.0f, newAbsX);
                newAbsX = Mathf.Min(UnityEngine.Screen.width, newAbsX);
                

                float newAbsY = m_Abs.y + m_YDelta;
                newAbsY = Mathf.Max(0.0f, newAbsY);
                newAbsY = Mathf.Min(UnityEngine.Screen.height, newAbsY);

                m_Abs.x = newAbsX;
                m_Abs.y = newAbsY;

            }
            else
            {
                m_Abs = UnityEngine.Input.mousePosition;
            }
        }

        /// <summary>
        /// Absolute screen posiition of the mouse
        /// </summary>
        public Vector3 AbsolutePosition
        {
            get { return m_Abs; }
        }

        /// <summary>
        /// Relative screen position of the mouse
        /// </summary>
        public Vector3 RelativePosition
        {
            get
            {
                return new Vector3(m_Abs.x / UnityEngine.Screen.width, m_Abs.y / UnityEngine.Screen.height, 0.0f);
            }
        }

        /// <summary>
        /// Delta scroll from the last frame
        /// </summary>
        public Vector2 DeltaScroll
        {
            get { return m_deltaScroll; }
        }



        public float DeltaX
        {
            get { return m_XDelta; }
        }

        public float DeltaY
        {
            get { return m_YDelta; }
        }

        #endregion
    }
}