﻿///----------------------------------------------------------------------
/// @file BoardManager.cs
///
/// This file contains the declaration of BoardManager class.
/// 
/// BoardManager alaways keep track of the current logic status of the board,
/// also has a register of all the "Board Waypoints" existing
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 20/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;
using BSEngine;

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

    /// <summary>
    /// Reference to the White King piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_WhiteKing = null;

    /// <summary>
    /// Reference to the White Queen piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_WhiteQueen = null;

    /// <summary>
    /// Reference to the White Knight piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_WhiteKnight = null;

    /// <summary>
    /// Reference to the White Bishop piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_WhiteBishop = null;

    /// <summary>
    /// Reference to the White Rook piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_WhiteRook = null;

    /// <summary>
    /// Reference to the White Pawn piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_WhitePawn = null;

    /// <summary>
    /// Reference to the Black King piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_BlackKing = null;

    /// <summary>
    /// Reference to the Black Queen piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_BlackQueen = null;

    /// <summary>
    /// Reference to the Black Knight piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_BlackKnight = null;

    /// <summary>
    /// Reference to the Black Bishop piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_BlackBishop = null;

    /// <summary>
    /// Reference to the Black Rook piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_BlackRook = null;

    /// <summary>
    /// Reference to the Black Pawn piece prefab
    /// </summary>
    [SerializeField]
    private GameObject m_BlackPawn = null;

    /// <summary>
    /// GameObject reference to the ChessBoard
    /// </summary>
    [SerializeField]
    private GameObject m_ChessBoard = null;

    /// <summary>
    /// Waypoints of the ChessBoard
    /// </summary>
    private Dictionary<string, GameObject> m_boardWaypoints = new Dictionary<string, GameObject>();

    /// <summary>
    /// Pieces located in the board
    /// </summary>
    private Dictionary<string, GameObject> m_PiecesInBoard = new Dictionary<string, GameObject>();

    /// <summary>
    /// Current logical board status
    /// </summary>
    private BoardStatus m_currentStatus;

    #endregion

    #region Public methods

    /// <summary>
    /// Property to acces the chessBoard
    /// </summary>
    public GameObject ChessBoard
    {
        get { return m_ChessBoard; }
    }

    /// <summary>
    /// Property to acces the Board Waypoints
    /// </summary>
    public Dictionary<string, GameObject> Waypoints
    {
        get { return m_boardWaypoints; }
    }

    /// <summary>
    /// Property to acces the Current Board Status
    /// </summary>
    public BoardStatus CurrentStatus
    {
        get { return m_currentStatus; }
        set { m_currentStatus = value; }
    }

    /// <summary>
    /// Method used to register a Board Waypoint in the manager
    /// 
    /// Each waypoint must have a code, corresponding to his tile
    /// </summary>
    /// <param name="name">Traditional code of the tile</param>
    /// <param name="go">Borad Waypoint</param>
    public void registerBoardWaypoint(string name, GameObject go)
    {
        m_boardWaypoints.Add(name, go);
    }

    /// <summary>
    /// Used to Unregister a Board Waypoint in the manager
    /// </summary>
    /// <param name="name">Traditional chess code</param>
    public void unregisterBoardWaypoint(string name)
    {
        m_boardWaypoints.Remove(name);
    }

    /// <summary>
    /// Function used to start a match
    /// </summary>
    public void startMatch()
    {
        m_currentStatus = new BoardStatus();
    }

    /// <summary>
    /// Method used to update the current status with the given movement
    /// </summary>
    /// <param name="movement">Movement data to update</param>
    public void UpdateCurrentStatus(BoardStatus movement)
    {
        //CurrentStatus.movePieceToDestination(movement.Origin, movement.Destination);
        CurrentStatus = movement;
    }


    /// <summary>
    /// Static method used to translate indexes on the chess board to
    /// the traditional code.
    /// 
    /// Rows are translated into a number [1-8]
    /// Columns are translated into a character [A-H]
    /// </summary>
    /// <param name="i">Row index</param>
    /// <param name="j">Column Index</param>
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

    /// <summary>
    /// Static method
    /// Translates a coords code to indexes
    /// 
    /// </summary>
    /// <param name="code">code to trasnlate</param>
    /// <param name="i">(OUT)Row index</param>
    /// <param name="j">(OUT)Column index</param>
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



    #region HACKS

    // This region contains all the hack methods used for testing and graphic representation of the logic status board
    
    /// <summary>
    /// Method used to create a piece from its corresponding prefab in the board
    /// </summary>
    /// <param name="piece">Piece to create</param>
    /// <param name="tile">Tile Code where create the piece</param>
    private void createPiece(ChessPiece piece, string tile)
    {
        Vector3 waypointPos = m_boardWaypoints[tile].transform.position;
        waypointPos.y = waypointPos.y + 0.65f;

        Quaternion rotation = m_WhiteBishop.transform.rotation;

        GameObject inst = null;
        switch (piece)
        {

            case ChessPiece.WHITE_BISHOP:
                inst = PoolMgr.Singleton.Instatiate(m_WhiteBishop, waypointPos, rotation);
                break;
            case ChessPiece.WHITE_KING:
                inst = PoolMgr.Singleton.Instatiate(m_WhiteKing, waypointPos, rotation);
                break;
            case ChessPiece.WHITE_KNIGHT:
                inst = PoolMgr.Singleton.Instatiate(m_WhiteKnight, waypointPos, rotation);
                break;
            case ChessPiece.WHITE_PAWN:
                inst = PoolMgr.Singleton.Instatiate(m_WhitePawn, waypointPos, rotation);
                break;
            case ChessPiece.WHITE_QUEEN:
                inst = PoolMgr.Singleton.Instatiate(m_WhiteQueen, waypointPos, rotation);
                break;
            case ChessPiece.WHITE_ROOK:
                inst = PoolMgr.Singleton.Instatiate(m_WhiteRook, waypointPos, rotation);
                break;
            case ChessPiece.BLACK_BISHOP:
                inst = PoolMgr.Singleton.Instatiate(m_BlackBishop, waypointPos, rotation);
                break;
            case ChessPiece.BLACK_KING:
                inst = PoolMgr.Singleton.Instatiate(m_BlackKing, waypointPos, rotation);
                break;
            case ChessPiece.BLACK_KNIGHT:
                inst = PoolMgr.Singleton.Instatiate(m_BlackKnight, waypointPos, rotation);
                break;
            case ChessPiece.BLACK_PAWN:
                inst = PoolMgr.Singleton.Instatiate(m_BlackPawn, waypointPos, rotation);
                break;
            case ChessPiece.BLACK_QUEEN:
                inst = PoolMgr.Singleton.Instatiate(m_BlackQueen, waypointPos, rotation);
                break;
            case ChessPiece.BLACK_ROOK:
                inst = PoolMgr.Singleton.Instatiate(m_BlackRook, waypointPos, rotation);
                break;
            case ChessPiece.NONE:
                break;
        }

        inst.GetComponent<PieceInfo>().TileCode = tile;
        m_PiecesInBoard.Add(tile, inst);
    }


    /// <summary>
    /// Method used to remove a piece GameObject from the board
    /// </summary>
    /// <param name="tile">Tile Code to clear</param>
    private void removePiece(string tile)
    {

        PoolMgr.Singleton.Destroy(m_PiecesInBoard[tile]);
        m_PiecesInBoard.Remove(tile);
    }

    /// <summary>
    /// Method used to Create all the corresponding pieces of the current logic status on the board
    /// </summary>
    public void ShowBoard()
    {
        foreach (string tile in m_currentStatus.Status.Keys)
        {
            if (m_currentStatus.Status[tile] != ChessPiece.NONE)
                createPiece(m_currentStatus.Status[tile], tile);
        }
    }

    /// <summary>
    /// Method used to remove all the piece's gameobject from the board
    /// </summary>
    public void ClearBoard()
    {
        foreach (string key in m_PiecesInBoard.Keys)
        {
            PoolMgr.Singleton.Destroy(m_PiecesInBoard[key]);
        }
        m_PiecesInBoard.Clear();
    }

    #endregion


    #region Private methods

    #endregion

    #region Monobehavior calls

    /// <summary>
    /// At the begining a status is created (setting the starting config by default)
    /// </summary>
    //private void OnEnable()
    //{
    //    m_currentStatus = new BoardStatus();
    //}

    /// <summary>
    /// Clear the board status
    /// </summary>
    private void OnDisable()
    {
        m_boardWaypoints.Clear();
        m_ChessBoard = null;
    }

    #endregion


}



