///----------------------------------------------------------------------
/// @file MouseCfg.cs
///
/// This file contains the declaration of MouseCfg class.
/// 
/// Mouse configuration defined for one InputSet
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 22/10/2015
///----------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using BSEngine;

namespace BSEngine.Input
{


    public class MouseCfg
    {

        #region Private params

        /// <summary>
        /// Flag to indicates if X-axis is inverted.
        /// 1.0 --> axis NOT inverted
        /// -1.0 --> axis inverted
        /// any other value isn't accepted
        /// </summary>
        private float m_invertedX;

        /// <summary>
        /// Falg to indicate if Y-axis is inverted
        /// 1.0 --> axis NOT inverted
        /// -1.0 --> axis inverted
        /// any other value isn't accepted
        /// </summary>
        private float m_invertedY;

        /// <summary>
        /// Mouse sensivity. It's a positve number
        /// </summary>
        private float m_sensivity;

        /// <summary>
        /// Flag used to configurate mouse polling. True to use Unity mouse's screen position. False to calculate through axis's delta
        /// </summary>
        private bool m_useUnityScreenPosition;

        #endregion


        #region Public methods

        /// <summary>
        /// Class constructor
        /// </summary>
        public MouseCfg(bool invertX, bool invertY, float sensivity, bool useUnityScreenPosition = false)
        {
            m_invertedX = invertX? -1.0f : 1.0f;
            m_invertedY = invertY? -1.0f : 1.0f;
            m_sensivity = sensivity;
            m_useUnityScreenPosition = useUnityScreenPosition;
        }

        /// <summary>
        /// Constructor from DataTable
        /// </summary>
        /// <param name="mouseCfgData">DataTable loaded from CFG file</param>
        public MouseCfg(DataTable mouseCfgData)
        {
            m_invertedX = mouseCfgData.Get<bool>("invertedX") ? -1.0f : 1.0f;
            m_invertedY = mouseCfgData.Get<bool>("invertedY") ? -1.0f : 1.0f;
            m_sensivity = mouseCfgData.Get<float>("sensivity");
            m_useUnityScreenPosition = mouseCfgData.Get<bool>("useUnityScreenPosition");
        }

        /// <summary>
        /// Flag to indicate if X-axis is inverted
        /// </summary>
        public bool InvertedXAxis
        {
            get { return m_invertedX == 1.0f ? false : true; }
            set
            {
                if (value)
                    m_invertedX = -1.0f;
                else
                    m_invertedX = 1.0f;
            }
        }

        /// <summary>
        /// Flag to indicate if Y-axis is inverted
        /// </summary>
        public bool InvertedYAxis
        {
            get { return m_invertedY == 1.0f ? false : true; }
            set
            {
                if (value)
                    m_invertedY = -1.0f;
                else
                    m_invertedY = 1.0f;
            }
        }

        /// <summary>
        /// Returns the inverted X-axis value.
        /// 1.0f or -1.0f.
        /// 1.0f -> axis NOT inverted
        /// -1.0f -> axis inverted
        /// </summary>
        public float InvertedXAxisValue
        {
            get { return m_invertedX; }
        }

        /// <summary>
        /// Returns the inverted Y-axis value.
        /// 1.0f or -1.0f.
        /// 1.0f -> axis NOT inverted
        /// -1.0f -> axis inverted
        /// </summary>
        public float InvertedYAxisValue
        {
            get { return m_invertedY;  }
        }

        /// <summary>
        /// Mouse sensivity. Always a positive number
        /// </summary>
        public float Sensivity
        {
            get { return m_sensivity; }
        }

        /// <summary>
        /// Flag used to configurate mouse polling. True to use Unity mouse's screen position. False to calculate through axis's delta
        /// </summary>
        public bool UseUnityScreenPosition
        {
            get { return m_useUnityScreenPosition; }
        }

        /// <summary>
        /// Translates all the class info into a DataTable
        /// </summary>
        public DataTable ToDataTable()
        {
            DataTable data = new DataTable("MouseCfg", SerializationMode.NONE, false);

            data.Set<bool>("invertedX", m_invertedX == -1.0f);
            data.Set<bool>("invertedY", m_invertedY == -1.0f);
            data.Set<float>("sensivity", m_sensivity);
            data.Set<bool>("useUnityScreenPosition", m_useUnityScreenPosition);

            return data;
        }


        #endregion
    }
}