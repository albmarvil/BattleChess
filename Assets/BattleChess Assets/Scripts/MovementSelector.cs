///----------------------------------------------------------------------
/// @file MovementSelector.cs
///
/// This file contains the declaration of MovementSelector class.
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 02/12/2015
///----------------------------------------------------------------------




using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSEngine.Input;
using BSEngine;
using BSEngine.Utils;

public class MovementSelector : MonoBehaviour {


    #region Public params
    /// <summary>
    /// Reference to the blue marker prefab
    /// </summary>
    public GameObject m_blueMarker = null;

    /// <summary>
    /// Reference to the red marker prefab
    /// </summary>
    public GameObject m_redMarker = null;

    #endregion

    #region Private params

    /// <summary>
    /// Mouse position
    /// </summary>
    private Vector3 m_mousePos = Vector3.zero;

    /// <summary>
    /// List of the current markers showing over the board
    /// </summary>
    private List<GameObject> m_markers = new List<GameObject>();

    private Movement m_movement = new Movement();

    #endregion

    #region Public methods

    /// <summary>
    /// Coroutine used to wait for player movement
    /// </summary>
    public IEnumerator WaitForMovement()
    {
        while (m_movement.Destination == "NONE")
        {
            yield return null;
        }
    }

    /// <summary>
    /// Current movement made by the player
    /// </summary>
    public Movement Movement
    {
        get { return m_movement; }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// OnClick mouse callback
    /// 
    /// It will cast a physic ray and draw the needed markers of the corresponding hit piece.
    /// </summary>
    private void onClickReceived(InputEvent e)
    {
        if (e.isOk && TurnManager.Singleton.CurrentTurn.PlayerType == PlayerType.HUMAN)
        {
            if (m_movement.Origin == "NONE" && m_movement.PieceMoved == ChessPiece.NONE)
            {
                selectPiece();
            }
            else if (m_movement.Origin != "NONE" && m_movement.PieceMoved != ChessPiece.NONE && m_movement.Destination == "NONE")
            {
                selectMarkerDestination();
            }
        }
    }

    /// <summary>
    /// OnMouse Moved callback.
    /// 
    /// It keeps track of the mouse position
    /// </summary>
    /// <param name="state"></param>
    private void onMouseMoved(MouseState state)
    {
        m_mousePos = state.AbsolutePosition;
    }


    /// <summary>
    /// Private method used to select the piece to move.
    /// 
    /// It will cast a ray over the piece and draw all the possible places where that piece can move
    /// </summary>
    private void selectPiece()
    {
        foreach (GameObject obj in m_markers)
        {
            PoolMgr.Singleton.Destroy(obj);
        }
        m_markers.Clear();

        Ray ray = Camera.main.ScreenPointToRay(m_mousePos);

        RaycastHit hit;

        int mask = int.MaxValue;
        if (TurnManager.Singleton.CurrentTurn.PlayerColor == ChessPiece.WHITE)
        {
            mask = (1 << LayerMask.NameToLayer("WHITE_PIECE"));
        }
        else if (TurnManager.Singleton.CurrentTurn.PlayerColor == ChessPiece.BLACK)
        {
            mask = (1 << LayerMask.NameToLayer("BLACK_PIECE"));
        }


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            PieceInfo tag = hit.collider.gameObject.GetComponent<PieceInfo>();
            
            Assert.assert(tag != null, "GameObject retrieved by raycast isn't a valid pieces, check the mask or the layer config");
            ChessPiece piece = tag.Piece;

            m_movement.PieceMoved = piece;
            m_movement.Origin = tag.TileCode;

            HashSet<string> movements = BoardManager.Singleton.CurrentStatus.getAllPieceMovements(piece, tag.TileCode);

            foreach (string movement in movements)
            {
                BoardStatus st = new BoardStatus(BoardManager.Singleton.CurrentStatus);
                st.movePieceToDestination(tag.TileCode, movement);
                if (!st.Check(TurnManager.Singleton.CurrentTurn.PlayerColor))
                {
                    Vector3 pos = BoardManager.Singleton.Waypoints[movement].transform.position;
                    pos.y = pos.y + 0.55f;

                    Quaternion rotation = Quaternion.AngleAxis(-90.0f, Vector3.right);

                    GameObject marker = null;

                    bool condition = BoardManager.Singleton.CurrentStatus.Status[movement] != ChessPiece.NONE;

                    if (condition)
                    {
                        marker = PoolMgr.Singleton.Instatiate(m_redMarker, pos, rotation);
                    }
                    else
                    {
                        marker = PoolMgr.Singleton.Instatiate(m_blueMarker, pos, rotation);
                    }

                    marker.GetComponent<PieceInfo>().TileCode = movement;

                    m_markers.Add(marker);
                }
            }
        }
    }

    /// <summary>
    /// Method used to complete the movement.
    /// 
    /// After selecting a piece a marker should be selected as destination position
    /// </summary>
    private void selectMarkerDestination()
    {
        Ray ray = Camera.main.ScreenPointToRay(m_mousePos);

        RaycastHit hit;
        int mask = (1 << LayerMask.NameToLayer("MOVEMENT_MARKER"));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            PieceInfo tag = hit.collider.gameObject.GetComponent<PieceInfo>();
            //if (tag != null)
            //{
                Assert.assert(tag != null, "GameObject retrieved by raycast isn't a valid marker, check the mask or the layer config");
                m_movement.Destination = tag.TileCode;
            //}
        }
        else 
        {
            selectPiece();
        }
    }

    /// <summary>
    /// On turn start callback, used to clean markers on the board
    /// </summary>
    /// <param name="t"></param>
    private void onTurnStart(Turn t)
    {
        for (int i = 0; i < m_markers.Count; ++i)
        {
            PoolMgr.Singleton.Destroy(m_markers[i]);
        }
        m_markers.Clear();

        m_movement.Origin = "NONE";
        m_movement.Destination = "NONE";
        m_movement.PieceMoved = ChessPiece.NONE;
    }

    #endregion

    #region Monobehavior calls

    private void Start()
    {
        if (InputMgr.Singleton != null)
        {
            InputMgr.Singleton.RegisterOrderListener("Game", "CLICK", onClickReceived);
            InputMgr.Singleton.RegisterMouseListener("Game", onMouseMoved);
        }

        if (TurnManager.Singleton != null)
        {
            TurnManager.Singleton.registerOnTurnStartListener(onTurnStart);
        }
    }

    private void OnDestroy()
    {
        if (InputMgr.Singleton != null)
        {
            InputMgr.Singleton.UnregisterOrderListener("Game", "CLICK", onClickReceived);
            InputMgr.Singleton.UnregisterMouseListener("Game", onMouseMoved);
        }
    }

    #endregion

}
