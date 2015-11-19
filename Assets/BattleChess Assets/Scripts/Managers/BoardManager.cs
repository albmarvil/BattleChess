using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	    
    #region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static BoardManager m_instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static BoardManager Singleton { get { return m_instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static BoardManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various BoardManager [" + name + "]");
            this.enabled = false;
        }
    }
	
	/// <summary>
    /// This is like the Release but done by the MonoBehaviour
    /// </summary>
    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
    }

    #endregion




    #region Public params

    #endregion

    #region Private params

    [SerializeField]
    private GameObject m_ChessBoard = null;

    private Dictionary<string, GameObject> m_boardWaypoints = new Dictionary<string, GameObject>();

    #endregion

    #region Public methods

    public GameObject ChessBoard
    {
        get { return m_ChessBoard; }
    }

    public Dictionary<string, GameObject> Waypoints
    {
        get { return m_boardWaypoints; }
    }

    public void registerBoardWaypoint(string name, GameObject go)
    {
        m_boardWaypoints.Add(name, go);
    }

    public void unregisterBoardWaypoint(string name)
    {
        m_boardWaypoints.Remove(name);
    }

    public static string statusIndexesToCode(int i, int j)
    {
        string number = (i + 1).ToString();
        string character = "";
        switch (j)
        {
            case 0:
                character = "A";
                break;
            case 1:
                character = "B";
                break;
            case 2:
                character = "C";
                break;
            case 3:
                character = "D";
                break;
            case 4:
                character = "E";
                break;
            case 5:
                character = "F";
                break;
            case 6:
                character = "G";
                break;
            case 7:
                character = "H";
                break;
        }

        return character + number;
    }

    public static void codeToStatusIndexes(string code, out int i, out int j)
    {
        char number = code[1];
        char character = code[0];

        i = int.Parse(number.ToString());
        i = i - 1;

        j = 0;

        if (character == 'A')
        {
            j = 0;
        }
        else if (character == 'B')
        {
            j = 1;
        }
        else if (character == 'C')
        {
            j = 2;
        }
        else if (character == 'D')
        {
            j = 3;
        }
        else if (character == 'E')
        {
            j = 4;
        }
        else if (character == 'F')
        {
            j = 5;
        }
        else if (character == 'G')
        {
            j = 6;
        }
        else if (character == 'H')
        {
            j = 7;
        }
    }

    #endregion

    #region Private methods

    #endregion

    #region Monobehavior calls


    private void OnDisable()
    {
        m_boardWaypoints.Clear();
        m_ChessBoard = null;
    }

    #endregion


}



