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
    /// Reference to the marker prefab
    /// </summary>
    public GameObject m_marker = null;

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

    public IEnumerator WaitForMovement()
    {
        while (m_movement.Destination == "NONE")
        {
            yield return null;
        }
    }

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
            //if (tag != null)
            //{
            Assert.assert(tag != null, "GameObject retrieved by raycast isn't a valid pieces, check the mask or the layer config");
                ChessPiece piece = tag.Piece;

                m_movement.PieceMoved = piece;
                m_movement.Origin = tag.TileCode;

                HashSet<string> movements = BoardManager.Singleton.CurrentStatus.getAllPieceMovements(piece, tag.TileCode);

                foreach (string movement in movements)
                {
                    Vector3 pos = BoardManager.Singleton.Waypoints[movement].transform.position;
                    pos.y = pos.y + 0.55f;

                    GameObject marker = PoolMgr.Singleton.Instatiate(m_marker, pos, Quaternion.identity);
                    marker.GetComponent<PieceInfo>().TileCode = movement;

                    m_markers.Add(marker);
                }
            //}
        }
    }

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
