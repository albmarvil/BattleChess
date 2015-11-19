///----------------------------------------------------------------------
/// @file InputMgr.cs
///
/// This file contains the declaration of InputMgr class.
/// 
/// This manager translates all the input from Unity to logic orders. This translations works with the definition of the current inputset
/// 
/// Each App State has a definition of an inputSet, where controls and orders are defined. InputMgr captures the input and send the orders trough the callbacks registered in eac state according to its definition.
/// 
/// ONLY KEY PRESSED & KEY RELEASED EVENTS SUPPORTED
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 15/9/2015
/// 
/// 
/// Added Mouse support to STANDALONE and WEBPLAYER platforms. This manager now updates a MouseState each frame. This updated MouseState will be sent to all the mouse listeners registered
/// in the current InputSet.
/// 
/// @refactor Alberto Martínez Villarán <tukaram92@gmail.com>
/// @date 22/10/2015
///----------------------------------------------------------------------




using UnityEngine;
using System.Collections;

namespace BSEngine
{
    namespace Input
    {

        public delegate void onOrderReceived(InputEvent e);
        public delegate void onMouseMoved(MouseState ms);

        

        public enum BSKeyCode
        {
            None            = UnityEngine.KeyCode.None,
            Backspace       = UnityEngine.KeyCode.Backspace,
            Delete          = UnityEngine.KeyCode.Delete,
            Tab             = UnityEngine.KeyCode.Tab,
            Clear           = UnityEngine.KeyCode.Clear,
            Return          = UnityEngine.KeyCode.Return,
            Pause           = UnityEngine.KeyCode.Pause,
            Escape          = UnityEngine.KeyCode.Escape,
            Space           = UnityEngine.KeyCode.Space,

            Keypad0         = UnityEngine.KeyCode.Keypad0,
            Keypad1         = UnityEngine.KeyCode.Keypad1,
            Keypad2         = UnityEngine.KeyCode.Keypad2,
            Keypad3         = UnityEngine.KeyCode.Keypad3,
            Keypad4         = UnityEngine.KeyCode.Keypad4,
            Keypad5         = UnityEngine.KeyCode.Keypad5,
            Keypad6         = UnityEngine.KeyCode.Keypad6,
            Keypad7         = UnityEngine.KeyCode.Keypad7,
            Keypad8         = UnityEngine.KeyCode.Keypad8,
            Keypad9         = UnityEngine.KeyCode.Keypad9,
            KeypadPeriod    = UnityEngine.KeyCode.KeypadPeriod,
            KeypadDivide    = UnityEngine.KeyCode.KeypadDivide,
            KeypadMultiply  = UnityEngine.KeyCode.KeypadMultiply,
            KeypadMinus     = UnityEngine.KeyCode.KeypadMinus,
            KeypadPlus      = UnityEngine.KeyCode.KeypadPlus,
            KeypadEnter     = UnityEngine.KeyCode.KeypadEnter,
            KeypadEquals    = UnityEngine.KeyCode.KeypadEquals,

            UpArrow         = UnityEngine.KeyCode.UpArrow,
            DownArrow       = UnityEngine.KeyCode.DownArrow,
            RightArrow      = UnityEngine.KeyCode.RightArrow,
            LeftArrow       = UnityEngine.KeyCode.LeftArrow,

            Insert          = UnityEngine.KeyCode.Insert,
            Home            = UnityEngine.KeyCode.Home,
            End             = UnityEngine.KeyCode.End,
            PageUp          = UnityEngine.KeyCode.PageUp,
            PageDown        = UnityEngine.KeyCode.PageDown,

            F1              = UnityEngine.KeyCode.F1,
            F2              = UnityEngine.KeyCode.F2,
            F3              = UnityEngine.KeyCode.F3,
            F4              = UnityEngine.KeyCode.F4,
            F5              = UnityEngine.KeyCode.F5,
            F6              = UnityEngine.KeyCode.F6,
            F7              = UnityEngine.KeyCode.F7,
            F8              = UnityEngine.KeyCode.F8,
            F9              = UnityEngine.KeyCode.F9,
            F10             = UnityEngine.KeyCode.F10,
            F11             = UnityEngine.KeyCode.F11,
            F12             = UnityEngine.KeyCode.F12,
            F13             = UnityEngine.KeyCode.F13,
            F14             = UnityEngine.KeyCode.F14,
            F15             = UnityEngine.KeyCode.F15,

            Alpha0          = UnityEngine.KeyCode.Alpha0,
            Alpha1          = UnityEngine.KeyCode.Alpha1,
            Alpha2          = UnityEngine.KeyCode.Alpha2,
            Alpha3          = UnityEngine.KeyCode.Alpha3,
            Alpha4          = UnityEngine.KeyCode.Alpha4,
            Alpha5          = UnityEngine.KeyCode.Alpha5,
            Alpha6          = UnityEngine.KeyCode.Alpha6,
            Alpha7          = UnityEngine.KeyCode.Alpha7,
            Alpha8          = UnityEngine.KeyCode.Alpha8,
            Alpha9          = UnityEngine.KeyCode.Alpha9,

            Exclaim         = UnityEngine.KeyCode.Exclaim,
            DoubleQuote     = UnityEngine.KeyCode.DoubleQuote,
            Hash            = UnityEngine.KeyCode.Hash,
            Dollar          = UnityEngine.KeyCode.Dollar,
            Ampersand       = UnityEngine.KeyCode.Ampersand,
            Quote           = UnityEngine.KeyCode.Quote,
            LeftParen       = UnityEngine.KeyCode.LeftParen,
            RightParen      = UnityEngine.KeyCode.RightParen,
            Asterisk        = UnityEngine.KeyCode.Asterisk,
            Plus            = UnityEngine.KeyCode.Plus,
            Comma           = UnityEngine.KeyCode.Comma,
            Minus           = UnityEngine.KeyCode.Minus,
            Period          = UnityEngine.KeyCode.Period,
            Slash           = UnityEngine.KeyCode.Slash,
            Colon           = UnityEngine.KeyCode.Colon,
            Semicolon       = UnityEngine.KeyCode.Semicolon,
            Less            = UnityEngine.KeyCode.Less,
            Equals          = UnityEngine.KeyCode.Equals,
            Greater         = UnityEngine.KeyCode.Greater,
            Question        = UnityEngine.KeyCode.Question,
            At              = UnityEngine.KeyCode.At,
            LeftBracket     = UnityEngine.KeyCode.LeftBracket,
            Backslash       = UnityEngine.KeyCode.Backslash,
            RightBracket    = UnityEngine.KeyCode.RightBracket,
            Caret           = UnityEngine.KeyCode.Caret,
            Underscore      = UnityEngine.KeyCode.Underscore,
            BackQuote       = UnityEngine.KeyCode.BackQuote,


            A               = UnityEngine.KeyCode.A,
            B               = UnityEngine.KeyCode.B,
            C               = UnityEngine.KeyCode.C,
            D               = UnityEngine.KeyCode.D,
            E               = UnityEngine.KeyCode.E,
            F               = UnityEngine.KeyCode.F,
            G               = UnityEngine.KeyCode.G,
            H               = UnityEngine.KeyCode.H,
            I               = UnityEngine.KeyCode.I,
            J               = UnityEngine.KeyCode.J,
            K               = UnityEngine.KeyCode.K,
            L               = UnityEngine.KeyCode.L,
            M               = UnityEngine.KeyCode.M,
            N               = UnityEngine.KeyCode.N,
            O               = UnityEngine.KeyCode.O,
            P               = UnityEngine.KeyCode.P,
            Q               = UnityEngine.KeyCode.Q,
            R               = UnityEngine.KeyCode.R,
            S               = UnityEngine.KeyCode.S,
            T               = UnityEngine.KeyCode.T,
            U               = UnityEngine.KeyCode.U,
            V               = UnityEngine.KeyCode.V,
            W               = UnityEngine.KeyCode.W,
            X               = UnityEngine.KeyCode.X,
            Y               = UnityEngine.KeyCode.Y,
            Z               = UnityEngine.KeyCode.Z,

            Numlock         = UnityEngine.KeyCode.Numlock,
            CapsLock        = UnityEngine.KeyCode.CapsLock,
            ScrollLock      = UnityEngine.KeyCode.ScrollLock,
            RightShift      = UnityEngine.KeyCode.RightShift,
            LeftShift       = UnityEngine.KeyCode.LeftShift,
            RightControl    = UnityEngine.KeyCode.RightControl,
            LeftControl     = UnityEngine.KeyCode.LeftControl,
            RightAlt        = UnityEngine.KeyCode.RightAlt,
            LeftAlt         = UnityEngine.KeyCode.LeftAlt,
            LeftCommand     = UnityEngine.KeyCode.LeftCommand,
            LeftApple       = UnityEngine.KeyCode.LeftApple,
            LeftWindows     = UnityEngine.KeyCode.LeftWindows,
            RightCommand    = UnityEngine.KeyCode.RightCommand,
            RightApple      = UnityEngine.KeyCode.RightApple,
            RightWindows    = UnityEngine.KeyCode.RightWindows,
            AltGr           = UnityEngine.KeyCode.AltGr,
            Help            = UnityEngine.KeyCode.Help,
            Print           = UnityEngine.KeyCode.Print,
            SysReq          = UnityEngine.KeyCode.SysReq,
            Break           = UnityEngine.KeyCode.Break,
            Menu            = UnityEngine.KeyCode.Menu,

            Mouse0          = UnityEngine.KeyCode.Mouse0,
            Mouse1          = UnityEngine.KeyCode.Mouse1,
            Mouse2          = UnityEngine.KeyCode.Mouse2,
            Mouse3          = UnityEngine.KeyCode.Mouse3,
            Mouse4          = UnityEngine.KeyCode.Mouse4,
            Mouse5          = UnityEngine.KeyCode.Mouse5,
            Mouse6          = UnityEngine.KeyCode.Mouse6,


            JoystickButton0     = UnityEngine.KeyCode.JoystickButton0,
            JoystickButton1     = UnityEngine.KeyCode.JoystickButton1,
            JoystickButton2     = UnityEngine.KeyCode.JoystickButton2,
            JoystickButton3     = UnityEngine.KeyCode.JoystickButton3,
            JoystickButton4     = UnityEngine.KeyCode.JoystickButton4,
            JoystickButton5     = UnityEngine.KeyCode.JoystickButton5,
            JoystickButton6     = UnityEngine.KeyCode.JoystickButton6,
            JoystickButton7     = UnityEngine.KeyCode.JoystickButton7,
            JoystickButton8     = UnityEngine.KeyCode.JoystickButton8,
            JoystickButton9     = UnityEngine.KeyCode.JoystickButton9,
            JoystickButton10    = UnityEngine.KeyCode.JoystickButton10,
            JoystickButton11    = UnityEngine.KeyCode.JoystickButton11,
            JoystickButton12    = UnityEngine.KeyCode.JoystickButton12,
            JoystickButton13    = UnityEngine.KeyCode.JoystickButton13,
            JoystickButton14    = UnityEngine.KeyCode.JoystickButton14,
            JoystickButton15    = UnityEngine.KeyCode.JoystickButton15,
            JoystickButton16    = UnityEngine.KeyCode.JoystickButton16,
            JoystickButton17    = UnityEngine.KeyCode.JoystickButton17,
            JoystickButton18    = UnityEngine.KeyCode.JoystickButton18,
            JoystickButton19    = UnityEngine.KeyCode.JoystickButton19,

            Joystick1Button0    = UnityEngine.KeyCode.Joystick1Button0,
            Joystick1Button1    = UnityEngine.KeyCode.Joystick1Button1,
            Joystick1Button2    = UnityEngine.KeyCode.Joystick1Button2,
            Joystick1Button3    = UnityEngine.KeyCode.Joystick1Button3,
            Joystick1Button4    = UnityEngine.KeyCode.Joystick1Button4,
            Joystick1Button5    = UnityEngine.KeyCode.Joystick1Button5,
            Joystick1Button6    = UnityEngine.KeyCode.Joystick1Button6,
            Joystick1Button7    = UnityEngine.KeyCode.Joystick1Button7,
            Joystick1Button8    = UnityEngine.KeyCode.Joystick1Button8,
            Joystick1Button9    = UnityEngine.KeyCode.Joystick1Button9,
            Joystick1Button10   = UnityEngine.KeyCode.Joystick1Button10,
            Joystick1Button11   = UnityEngine.KeyCode.Joystick1Button11,
            Joystick1Button12   = UnityEngine.KeyCode.Joystick1Button12,
            Joystick1Button13   = UnityEngine.KeyCode.Joystick1Button13,
            Joystick1Button14   = UnityEngine.KeyCode.Joystick1Button14,
            Joystick1Button15   = UnityEngine.KeyCode.Joystick1Button15,
            Joystick1Button16   = UnityEngine.KeyCode.Joystick1Button16,
            Joystick1Button17   = UnityEngine.KeyCode.Joystick1Button17,
            Joystick1Button18   = UnityEngine.KeyCode.Joystick1Button18,
            Joystick1Button19   = UnityEngine.KeyCode.Joystick1Button19,

            Joystick2Button0    = UnityEngine.KeyCode.Joystick2Button0,
            Joystick2Button1    = UnityEngine.KeyCode.Joystick2Button1,
            Joystick2Button2    = UnityEngine.KeyCode.Joystick2Button2,
            Joystick2Button3    = UnityEngine.KeyCode.Joystick2Button3,
            Joystick2Button4    = UnityEngine.KeyCode.Joystick2Button4,
            Joystick2Button5    = UnityEngine.KeyCode.Joystick2Button5,
            Joystick2Button6    = UnityEngine.KeyCode.Joystick2Button6,
            Joystick2Button7    = UnityEngine.KeyCode.Joystick2Button7,
            Joystick2Button8    = UnityEngine.KeyCode.Joystick2Button8,
            Joystick2Button9    = UnityEngine.KeyCode.Joystick2Button9,
            Joystick2Button10   = UnityEngine.KeyCode.Joystick2Button10,
            Joystick2Button11   = UnityEngine.KeyCode.Joystick2Button11,
            Joystick2Button12   = UnityEngine.KeyCode.Joystick2Button12,
            Joystick2Button13   = UnityEngine.KeyCode.Joystick2Button13,
            Joystick2Button14   = UnityEngine.KeyCode.Joystick2Button14,
            Joystick2Button15   = UnityEngine.KeyCode.Joystick2Button15,
            Joystick2Button16   = UnityEngine.KeyCode.Joystick2Button16,
            Joystick2Button17   = UnityEngine.KeyCode.Joystick2Button17,
            Joystick2Button18   = UnityEngine.KeyCode.Joystick2Button18,
            Joystick2Button19   = UnityEngine.KeyCode.Joystick2Button19,


            Joystick3Button0    = UnityEngine.KeyCode.Joystick3Button0,
            Joystick3Button1    = UnityEngine.KeyCode.Joystick3Button1,
            Joystick3Button2    = UnityEngine.KeyCode.Joystick3Button2,
            Joystick3Button3    = UnityEngine.KeyCode.Joystick3Button3,
            Joystick3Button4    = UnityEngine.KeyCode.Joystick3Button4,
            Joystick3Button5    = UnityEngine.KeyCode.Joystick3Button5,
            Joystick3Button6    = UnityEngine.KeyCode.Joystick3Button6,
            Joystick3Button7    = UnityEngine.KeyCode.Joystick3Button7,
            Joystick3Button8    = UnityEngine.KeyCode.Joystick3Button8,
            Joystick3Button9    = UnityEngine.KeyCode.Joystick3Button9,
            Joystick3Button10   = UnityEngine.KeyCode.Joystick3Button10,
            Joystick3Button11   = UnityEngine.KeyCode.Joystick3Button11,
            Joystick3Button12   = UnityEngine.KeyCode.Joystick3Button12,
            Joystick3Button13   = UnityEngine.KeyCode.Joystick3Button13,
            Joystick3Button14   = UnityEngine.KeyCode.Joystick3Button14,
            Joystick3Button15   = UnityEngine.KeyCode.Joystick3Button15,
            Joystick3Button16   = UnityEngine.KeyCode.Joystick3Button16,
            Joystick3Button17   = UnityEngine.KeyCode.Joystick3Button17,
            Joystick3Button18   = UnityEngine.KeyCode.Joystick3Button18,
            Joystick3Button19   = UnityEngine.KeyCode.Joystick3Button19,


            Joystick4Button0    = UnityEngine.KeyCode.Joystick4Button0,
            Joystick4Button1    = UnityEngine.KeyCode.Joystick4Button1,
            Joystick4Button2    = UnityEngine.KeyCode.Joystick4Button2,
            Joystick4Button3    = UnityEngine.KeyCode.Joystick4Button3,
            Joystick4Button4    = UnityEngine.KeyCode.Joystick4Button4,
            Joystick4Button5    = UnityEngine.KeyCode.Joystick4Button5,
            Joystick4Button6    = UnityEngine.KeyCode.Joystick4Button6,
            Joystick4Button7    = UnityEngine.KeyCode.Joystick4Button7,
            Joystick4Button8    = UnityEngine.KeyCode.Joystick4Button8,
            Joystick4Button9    = UnityEngine.KeyCode.Joystick4Button9,
            Joystick4Button10   = UnityEngine.KeyCode.Joystick4Button10,
            Joystick4Button11   = UnityEngine.KeyCode.Joystick4Button11,
            Joystick4Button12   = UnityEngine.KeyCode.Joystick4Button12,
            Joystick4Button13   = UnityEngine.KeyCode.Joystick4Button13,
            Joystick4Button14   = UnityEngine.KeyCode.Joystick4Button14,
            Joystick4Button15   = UnityEngine.KeyCode.Joystick4Button15,
            Joystick4Button16   = UnityEngine.KeyCode.Joystick4Button16,
            Joystick4Button17   = UnityEngine.KeyCode.Joystick4Button17,
            Joystick4Button18   = UnityEngine.KeyCode.Joystick4Button18,
            Joystick4Button19   = UnityEngine.KeyCode.Joystick4Button19,


            Joystick5Button0    = UnityEngine.KeyCode.Joystick5Button0,
            Joystick5Button1    = UnityEngine.KeyCode.Joystick5Button1,
            Joystick5Button2    = UnityEngine.KeyCode.Joystick5Button2,
            Joystick5Button3    = UnityEngine.KeyCode.Joystick5Button3,
            Joystick5Button4    = UnityEngine.KeyCode.Joystick5Button4,
            Joystick5Button5    = UnityEngine.KeyCode.Joystick5Button5,
            Joystick5Button6    = UnityEngine.KeyCode.Joystick5Button6,
            Joystick5Button7    = UnityEngine.KeyCode.Joystick5Button7,
            Joystick5Button8    = UnityEngine.KeyCode.Joystick5Button8,
            Joystick5Button9    = UnityEngine.KeyCode.Joystick5Button9,
            Joystick5Button10   = UnityEngine.KeyCode.Joystick5Button10,
            Joystick5Button11   = UnityEngine.KeyCode.Joystick5Button11,
            Joystick5Button12   = UnityEngine.KeyCode.Joystick5Button12,
            Joystick5Button13   = UnityEngine.KeyCode.Joystick5Button13,
            Joystick5Button14   = UnityEngine.KeyCode.Joystick5Button14,
            Joystick5Button15   = UnityEngine.KeyCode.Joystick5Button15,
            Joystick5Button16   = UnityEngine.KeyCode.Joystick5Button16,
            Joystick5Button17   = UnityEngine.KeyCode.Joystick5Button17,
            Joystick5Button18   = UnityEngine.KeyCode.Joystick5Button18,
            Joystick5Button19   = UnityEngine.KeyCode.Joystick5Button19,


            Joystick6Button0    = UnityEngine.KeyCode.Joystick6Button0,
            Joystick6Button1    = UnityEngine.KeyCode.Joystick6Button1,
            Joystick6Button2    = UnityEngine.KeyCode.Joystick6Button2,
            Joystick6Button3    = UnityEngine.KeyCode.Joystick6Button3,
            Joystick6Button4    = UnityEngine.KeyCode.Joystick6Button4,
            Joystick6Button5    = UnityEngine.KeyCode.Joystick6Button5,
            Joystick6Button6    = UnityEngine.KeyCode.Joystick6Button6,
            Joystick6Button7    = UnityEngine.KeyCode.Joystick6Button7,
            Joystick6Button8    = UnityEngine.KeyCode.Joystick6Button8,
            Joystick6Button9    = UnityEngine.KeyCode.Joystick6Button9,
            Joystick6Button10   = UnityEngine.KeyCode.Joystick6Button10,
            Joystick6Button11   = UnityEngine.KeyCode.Joystick6Button11,
            Joystick6Button12   = UnityEngine.KeyCode.Joystick6Button12,
            Joystick6Button13   = UnityEngine.KeyCode.Joystick6Button13,
            Joystick6Button14   = UnityEngine.KeyCode.Joystick6Button14,
            Joystick6Button15   = UnityEngine.KeyCode.Joystick6Button15,
            Joystick6Button16   = UnityEngine.KeyCode.Joystick6Button16,
            Joystick6Button17   = UnityEngine.KeyCode.Joystick6Button17,
            Joystick6Button18   = UnityEngine.KeyCode.Joystick6Button18,
            Joystick6Button19   = UnityEngine.KeyCode.Joystick6Button19,


            Joystick7Button0    = UnityEngine.KeyCode.Joystick7Button0,
            Joystick7Button1    = UnityEngine.KeyCode.Joystick7Button1,
            Joystick7Button2    = UnityEngine.KeyCode.Joystick7Button2,
            Joystick7Button3    = UnityEngine.KeyCode.Joystick7Button3,
            Joystick7Button4    = UnityEngine.KeyCode.Joystick7Button4,
            Joystick7Button5    = UnityEngine.KeyCode.Joystick7Button5,
            Joystick7Button6    = UnityEngine.KeyCode.Joystick7Button6,
            Joystick7Button7    = UnityEngine.KeyCode.Joystick7Button7,
            Joystick7Button8    = UnityEngine.KeyCode.Joystick7Button8,
            Joystick7Button9    = UnityEngine.KeyCode.Joystick7Button9,
            Joystick7Button10   = UnityEngine.KeyCode.Joystick7Button10,
            Joystick7Button11   = UnityEngine.KeyCode.Joystick7Button11,
            Joystick7Button12   = UnityEngine.KeyCode.Joystick7Button12,
            Joystick7Button13   = UnityEngine.KeyCode.Joystick7Button13,
            Joystick7Button14   = UnityEngine.KeyCode.Joystick7Button14,
            Joystick7Button15   = UnityEngine.KeyCode.Joystick7Button15,
            Joystick7Button16   = UnityEngine.KeyCode.Joystick7Button16,
            Joystick7Button17   = UnityEngine.KeyCode.Joystick7Button17,
            Joystick7Button18   = UnityEngine.KeyCode.Joystick7Button18,
            Joystick7Button19   = UnityEngine.KeyCode.Joystick7Button19,


            Joystick8Button0    = UnityEngine.KeyCode.Joystick8Button0,
            Joystick8Button1    = UnityEngine.KeyCode.Joystick8Button1,
            Joystick8Button2    = UnityEngine.KeyCode.Joystick8Button2,
            Joystick8Button3    = UnityEngine.KeyCode.Joystick8Button3,
            Joystick8Button4    = UnityEngine.KeyCode.Joystick8Button4,
            Joystick8Button5    = UnityEngine.KeyCode.Joystick8Button5,
            Joystick8Button6    = UnityEngine.KeyCode.Joystick8Button6,
            Joystick8Button7    = UnityEngine.KeyCode.Joystick8Button7,
            Joystick8Button8    = UnityEngine.KeyCode.Joystick8Button8,
            Joystick8Button9    = UnityEngine.KeyCode.Joystick8Button9,
            Joystick8Button10   = UnityEngine.KeyCode.Joystick8Button10,
            Joystick8Button11   = UnityEngine.KeyCode.Joystick8Button11,
            Joystick8Button12   = UnityEngine.KeyCode.Joystick8Button12,
            Joystick8Button13   = UnityEngine.KeyCode.Joystick8Button13,
            Joystick8Button14   = UnityEngine.KeyCode.Joystick8Button14,
            Joystick8Button15   = UnityEngine.KeyCode.Joystick8Button15,
            Joystick8Button16   = UnityEngine.KeyCode.Joystick8Button16,
            Joystick8Button17   = UnityEngine.KeyCode.Joystick8Button17,
            Joystick8Button18   = UnityEngine.KeyCode.Joystick8Button18,
            Joystick8Button19   = UnityEngine.KeyCode.Joystick8Button19,

          


        }

        public class InputMgr
        {

            #region Singleton

            /// <summary>
            /// Singleton instance of the class
            /// </summary>
            private static InputMgr m_instance = null;

            /// <summary>
            /// Property to get the singleton instance of the class.
            /// </summary>
            public static InputMgr Singleton { get { return m_instance; } }

            // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            static InputMgr() { }

            /// <summary>
            /// Used to initialize the InputMgr singleton instance
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
                    m_instance = new InputMgr();
                    return m_instance.open();
                }
            }

            /// <summary>
            /// Used to deinitialize the InputMgr singleton data.
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
#if UNITY_STANDALONE || UNITY_WEBPLAYER
                m_mouseState = new MouseState();
#endif
                return true;
            }

            /// <summary>
            /// Used as second step on singleton initialisation. Used to specific code of the different Engine & Game managers
            /// </summary>
            private void close()
            {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
                m_mouseState = null;
#endif
            }

            #endregion

            #region Private params

#if UNITY_STANDALONE || UNITY_WEBPLAYER
            /// <summary>
            /// Current mouse state of the application
            /// </summary>
            private static MouseState m_mouseState = null;
#endif

            #endregion

            #region Public methods

            /// <summary>
            /// Used to Update manager info, if needed.
            /// </summary>
            public void Update()
            {
                foreach (BSKeyCode key in GameMgr.Singleton.CurrentState.InputSet.KeyBindings.Keys)
                {
                    if(UnityEngine.Input.GetKeyDown((UnityEngine.KeyCode)key))
                    {
                        InputEvent e = null;
                        string order = null;
                        for (int i = 0; i < GameMgr.Singleton.CurrentState.InputSet.KeyBindings[key].Count; ++i)
                        {
                            order = GameMgr.Singleton.CurrentState.InputSet.KeyBindings[key][i];
                            e = new InputEvent(order);
                            if (GameMgr.Singleton.CurrentState.InputSet.OrdersListeners.ContainsKey(order))
                            {
                                GameMgr.Singleton.CurrentState.InputSet.OrdersListeners[order](e);
                            }
                        }
                    }
                    else if (UnityEngine.Input.GetKeyUp((UnityEngine.KeyCode)key))
                    {
                        InputEvent e = null;
                        string order = null;
                        for (int i = 0; i < GameMgr.Singleton.CurrentState.InputSet.KeyBindings[key].Count; ++i)
                        {
                            order = GameMgr.Singleton.CurrentState.InputSet.KeyBindings[key][i];
                            e = new InputEvent(order, false);
                            if (GameMgr.Singleton.CurrentState.InputSet.OrdersListeners.ContainsKey(order))
                            {
                                GameMgr.Singleton.CurrentState.InputSet.OrdersListeners[order](e);
                            }
                        }
                    }
                }


                //Mouse update
#if UNITY_STANDALONE || UNITY_WEBPLAYER

                if (GameMgr.Singleton.CurrentState.InputSet.MouseSupported)
                {
                    m_mouseState.Update();
                    GameMgr.Singleton.CurrentState.InputSet.MouseListeners(m_mouseState);
                }
                

#endif
            }



            /// <summary>
            /// Method used to register an Order listener. An order listener should be registered inside the callbacks of an state's inputset
            /// </summary>
            /// <param name="state">State to reference the desired InputSet</param>
            /// <param name="order">Order you want to listen</param>
            /// <param name="listener">Funciton callback you're registering</param>
            public void RegisterOrderListener(string state, string order, onOrderReceived listener)
            {
                if (GameMgr.Singleton.States.ContainsKey(state))
                {
                    GameMgr.Singleton.States[state].InputSet.RegisterOnOrderReceived(order, listener);
                }
            }

            /// <summary>
            /// Method used to unregister an order listener
            /// </summary>
            /// <param name="state">State to reference the desired InputSet</param>
            /// <param name="order">Order you want to listen</param>
            /// <param name="listener">Funciton callback you're unregistering</param>
            public void UnregisterOrderListener(string state, string order, onOrderReceived listener)
            {
                if (GameMgr.Singleton.States.ContainsKey(state))
                {
                    GameMgr.Singleton.States[state].InputSet.UnregisterOnOrderReceived(order, listener);
                }
            }

            /// <summary>
            /// Method used to register Mouse listeners, to a given InputSet from a state
            /// </summary>
            /// <param name="state">State name to reference the desired InputSet</param>
            /// <param name="listener">Function callback you're going to register</param>
            public void RegisterMouseListener(string state, onMouseMoved listener)
            {
                if (GameMgr.Singleton.States.ContainsKey(state))
                {
                    GameMgr.Singleton.States[state].InputSet.RegisterOnMouseMoved(listener);
                }
            }

            /// <summary>
            /// Methods used to un register a mouse listener.
            /// </summary>
            /// <param name="state">State name to reference the desired InputSet</param>
            /// <param name="listener">Function callback to unregister</param>
            public void UnregisterMouseListener(string state, onMouseMoved listener)
            {
                if (GameMgr.Singleton.States.ContainsKey(state))
                {
                    GameMgr.Singleton.States[state].InputSet.UnregisterOnMouseMoved(listener);
                }
            }

            #endregion

        }
    }
}


