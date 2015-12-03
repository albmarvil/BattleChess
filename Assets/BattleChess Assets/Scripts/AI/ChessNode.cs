///----------------------------------------------------------------------
/// @file ChessNode.cs
///
/// This file contains the declaration of ChessNode class.
/// 
/// This class is an implementation of MinMaxNode.
/// 
/// It contains all the generation required for Chess movements and children boards, and the definition of the static evaluation function
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 30/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Enum with the individual score to each piece in Chess evaluation
/// </summary>
public enum ChessPiecesValue
{
    QUEEN = 10,
    ROOK = 5,
    KNIGHT = 3,
    BISHOP = 3,
    PAWN = 1,
    KING = 0
}

public class ChessNode : MinMaxNode
{
    #region Private parmas
    /// <summary>
    /// Board status to the node
    /// </summary>
    private BoardStatus m_board = null;
    #endregion

    #region Public methods
    /// <summary>
    /// Porperty to acces to the board status of the node
    /// </summary>
    public BoardStatus Board
    {
        get { return m_board; }
    }

    /// <summary>
    /// Node constructor
    /// </summary>
    /// <param name="board">Board status</param>
    /// <param name="depth">Depth from the origin node</param>
    /// <param name="parentNode">Parent node</param>
    /// <param name="nodeType">Node type</param>
    public ChessNode(BoardStatus board, int depth, ChessNode parentNode, NodeType nodeType) : base(depth, parentNode, nodeType)
    {
        m_board = board;
    }

    /// <summary>
    /// Abstract function used to get all the possible children of the node
    /// </summary>
    /// <returns>All child nodes</returns>
    public override List<MinMaxNode> getChildren()
    {
        //System.Diagnostics.Stopwatch stp = new System.Diagnostics.Stopwatch();
        //stp.Start();

        ChessPiece playerMax = TurnManager.Singleton.CurrentTurn.PlayerColor;
        ChessPiece playerMin = playerMax == ChessPiece.WHITE ? ChessPiece.BLACK : ChessPiece.WHITE;

        ChessPiece color = ChessPiece.NONE;
        color = NodeType == global::NodeType.MAX ? playerMax : playerMin;

        List<BoardStatus> childrenBoards = m_board.getAllBoardMovements(color);

        List<MinMaxNode> res = new List<MinMaxNode>();
        ChessNode node = null;
        for (int i = 0; i < childrenBoards.Count; ++i)
        {
            node = new ChessNode(childrenBoards[i], Depth - 1, this, NodeType == global::NodeType.MIN ? global::NodeType.MAX : global::NodeType.MAX);
            res.Add(node);
        }

        //stp.Stop();
        //Debug.Log("getChildren() : Depth: " + Depth + " - T: " + stp.ElapsedMilliseconds + " Count: " + res.Count);
        return res;
    }

    /// <summary>
    /// Abstract function used to evaluate the end nodes in MinMax Algorithm.
    /// </summary>
    /// <returns>Static evaluation value</returns>
    public override float StaticValueFunction()
    {

        int whitePiecesValue = 0;
        foreach(string pos in m_board.WhitePieces)
        {
            whitePiecesValue += getPieceValue(m_board.Status[pos]);
        }


        float blackPiecesValue = 0.0f;
        foreach(string pos in m_board.BlackPieces)
        {
            blackPiecesValue += getPieceValue(m_board.Status[pos]);
        }

        float denominator = whitePiecesValue + blackPiecesValue;
        float numerator = 0.0f;

        if (TurnManager.Singleton.CurrentTurn.PlayerColor == ChessPiece.WHITE)
        {
            numerator = whitePiecesValue - blackPiecesValue;
        }
        else if (TurnManager.Singleton.CurrentTurn.PlayerColor == ChessPiece.BLACK)
        {
            numerator = blackPiecesValue - whitePiecesValue;
        }

        return numerator / denominator;
    }

    /// <summary>
    /// Abstract predicate used to implement specific conditions on EndNode predicate
    /// </summary>
    public override bool EndNodeSpecificCondition()
    {

        ChessPiece playerMax = TurnManager.Singleton.CurrentTurn.PlayerColor;
        ChessPiece playerMin = playerMax == ChessPiece.WHITE ? ChessPiece.BLACK : ChessPiece.WHITE;

        ChessPiece color = ChessPiece.NONE;
        color = NodeType == global::NodeType.MAX ? playerMax : playerMin;


        if (m_board.Draw())
        {
            return true;
        }
        else if (Parent != null)
        {
            return m_board.CheckMate(color);
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Private Methdos
    /// <summary>
    /// Private function used to get each individual piece score for static evaluation
    /// </summary>
    private int getPieceValue(ChessPiece piece)
    {
        switch (piece)
        {
            case ChessPiece.BLACK_QUEEN:
                return (int)ChessPiecesValue.QUEEN;
            case ChessPiece.WHITE_QUEEN:
                return (int)ChessPiecesValue.QUEEN;
            case ChessPiece.BLACK_ROOK:
                return (int)ChessPiecesValue.ROOK;
            case ChessPiece.WHITE_ROOK:
                return (int)ChessPiecesValue.ROOK;
            case ChessPiece.BLACK_KNIGHT:
                return (int)ChessPiecesValue.KNIGHT;
            case ChessPiece.WHITE_KNIGHT:
                return (int)ChessPiecesValue.KNIGHT;
            case ChessPiece.BLACK_BISHOP:
                return (int)ChessPiecesValue.BISHOP;
            case ChessPiece.WHITE_BISHOP:
                return (int)ChessPiecesValue.BISHOP;
            case ChessPiece.BLACK_PAWN:
                return (int)ChessPiecesValue.PAWN;
            case ChessPiece.WHITE_PAWN:
                return (int)ChessPiecesValue.PAWN;
            case ChessPiece.BLACK_KING:
                return (int)ChessPiecesValue.KING;
            case ChessPiece.WHITE_KING:
                return (int)ChessPiecesValue.KING;
        }

        return 0;
    }
    #endregion
}
