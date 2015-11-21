///----------------------------------------------------------------------
/// @file PieceInfo.cs
///
/// This file contains the declaration of PieceInfo class.
/// 
/// This component stores the logic info in the piece gameObject
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 21/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class PieceInfo : MonoBehaviour {

    
    #region Private params
    /// <summary>
    /// Piece type
    /// </summary>
    [SerializeField]
    private ChessPiece m_piece = ChessPiece.NONE;

    /// <summary>
    /// Piece current position
    /// </summary>
    private string m_tilePosition;

    #endregion

    #region Public methods

    public ChessPiece Piece
    {
        get { return m_piece; }
    }

    public string TileCode
    {
        get { return m_tilePosition; }
        set { m_tilePosition = value; }
    }

    #endregion

}
