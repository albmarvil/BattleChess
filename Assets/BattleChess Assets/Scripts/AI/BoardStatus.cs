///----------------------------------------------------------------------
/// @file BoardStatus.cs
///
/// This file contains the declaration of BoardStatus class.
/// 
/// This class is used to code the logic status of the chess board.
/// 
/// A Board status can be asked for all the possible Board statuses that are originated from all the possible movements.
/// 
/// It also contains the logic of each piece movement, and can be asked if is an ending status. (CHECK MATE)
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 20/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Enum used to encode each piece and color
/// </summary>
public enum ChessPiece
{
    NONE = 0,
    WHITE_KING,
    BLACK_KING,
    WHITE_QUEEN,
    BLACK_QUEEN,
    WHITE_ROOK,
    BLACK_ROOK,
    WHITE_KNIGHT,
    BLACK_KNIGHT,
    WHITE_BISHOP,
    BLACK_BISHOP,
    WHITE_PAWN,
    BLACK_PAWN,
    WHITE, //= WHITE_KING | WHITE_BISHOP | WHITE_KNIGHT | WHITE_PAWN | WHITE_QUEEN | WHITE_ROOK,
    BLACK //= BLACK_BISHOP | BLACK_KING | BLACK_KNIGHT | BLACK_PAWN | BLACK_QUEEN | BLACK_ROOK
}

/// <summary>
/// Definition of a chess movement
/// </summary>
public struct Movement
{
    /// <summary>
    /// Origin position of the piece when moved
    /// </summary>
    private string m_origin;

    /// <summary>
    /// Destination position of the piece when moved
    /// </summary>
    private string m_destination;

    /// <summary>
    /// Piece moved
    /// </summary>
    private ChessPiece m_pieceMoved;

    /// <summary>
    /// Public constructor. Default constructor. Values set to default (empty, isn't a valid movement)
    /// </summary>
    //public Movement()
    //{
    //    m_origin = "";
    //    m_destination = "";
    //    m_pieceMoved = ChessPiece.NONE;
    //}

    /// <summary>
    /// Public constructor
    /// </summary>
    /// <param name="origin">Origin position</param>
    /// <param name="destination">Destination position</param>
    /// <param name="pieceMoved">Piece moved</param>
    public Movement(string origin, string destination, ChessPiece pieceMoved)
    {
        m_origin = origin;
        m_destination = destination;
        m_pieceMoved = pieceMoved;
    }

    public string Origin
    {
        get { return m_origin;  }
        set { m_origin = value; }
    }

    public string Destination
    {
        get { return m_destination; }
        set { m_destination = value; }
    }

    public ChessPiece PieceMoved
    {
        get { return m_pieceMoved; }
        set { m_pieceMoved = value; }
    }
}


public class BoardStatus {


    #region Private params

    /// <summary>
    /// Collection used as storage status.
    /// 
    /// Key --> string code: "A1"
    /// Value --> ChessPiece: NONE
    /// </summary>
    private Dictionary<string, ChessPiece> m_status = new Dictionary<string, ChessPiece>();

    /// <summary>
    /// List of the tile codes (positions on the board) of all white pieces
    /// </summary>
    private List<string> m_whitePiecesPosition = new List<string>();

    /// <summary>
    /// List of the tile codes(positions on the board) of all black pieces
    /// </summary>
    private List<string> m_blackPiecesPosition = new List<string>();

    #endregion

    #region Public methods

    /// <summary>
    /// Default constructor. It initializes to the starting status
    /// </summary>
    public BoardStatus()
    {
        setToStartingStatus();
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="status">Status to copy</param>
    public BoardStatus(BoardStatus status)
    {
        m_status = new Dictionary<string, ChessPiece>();
        foreach (string key in status.m_status.Keys)
        {
            m_status.Add(key, status.m_status[key]);
        }

        m_whitePiecesPosition = new List<string>();
        m_whitePiecesPosition.AddRange(status.m_whitePiecesPosition);

        m_blackPiecesPosition = new List<string>();
        m_blackPiecesPosition.AddRange(status.m_blackPiecesPosition);
    }

    /// <summary>
    /// Property to acces to the data collection of the status
    /// </summary>
    public Dictionary<string, ChessPiece> Status
    {
        get { return m_status; }
    }

    /// <summary>
    /// Used to reset the status to the pieces's starting position
    /// </summary>
    public void setToStartingStatus()
    {
        m_status = new Dictionary<string, ChessPiece>();

        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                m_status.Add(BoardManager.statusIndexesToCode(i, j), ChessPiece.NONE);
            }
        }
       
        #region White pieces

        m_status["A1"] = ChessPiece.WHITE_ROOK;
        m_whitePiecesPosition.Add("A1");
        m_status["B1"] = ChessPiece.WHITE_KNIGHT;
        m_whitePiecesPosition.Add("B1");
        m_status["C1"] = ChessPiece.WHITE_BISHOP;
        m_whitePiecesPosition.Add("C1");
        m_status["D1"] = ChessPiece.WHITE_QUEEN;
        m_whitePiecesPosition.Add("D1");
        m_status["E1"] = ChessPiece.WHITE_KING;
        m_whitePiecesPosition.Add("E1");
        m_status["F1"] = ChessPiece.WHITE_BISHOP;
        m_whitePiecesPosition.Add("F1");
        m_status["G1"] = ChessPiece.WHITE_KNIGHT;
        m_whitePiecesPosition.Add("G1");
        m_status["H1"] = ChessPiece.WHITE_ROOK;
        m_whitePiecesPosition.Add("H1");

        for (int i = 0; i < 8; ++i)
        {
            string code = BoardManager.statusIndexesToCode(1, i);
            m_status[code] = ChessPiece.WHITE_PAWN;
            m_whitePiecesPosition.Add(code);
        }

        #endregion

        #region Black pieces

        m_status["A8"] = ChessPiece.BLACK_ROOK;
        m_blackPiecesPosition.Add("A8");
        m_status["B8"] = ChessPiece.BLACK_KNIGHT;
        m_blackPiecesPosition.Add("B8");
        m_status["C8"] = ChessPiece.BLACK_BISHOP;
        m_blackPiecesPosition.Add("C8");
        m_status["D8"] = ChessPiece.BLACK_QUEEN;
        m_blackPiecesPosition.Add("D8");
        m_status["E8"] = ChessPiece.BLACK_KING;
        m_blackPiecesPosition.Add("E8");
        m_status["F8"] = ChessPiece.BLACK_BISHOP;
        m_blackPiecesPosition.Add("F8");
        m_status["G8"] = ChessPiece.BLACK_KNIGHT;
        m_blackPiecesPosition.Add("G8");
        m_status["H8"] = ChessPiece.BLACK_ROOK;
        m_blackPiecesPosition.Add("H8");

        for (int i = 0; i < 8; ++i)
        {
            string code = BoardManager.statusIndexesToCode(6, i);
            m_status[code] = ChessPiece.BLACK_PAWN;
            m_blackPiecesPosition.Add(code);
        }

        #endregion
    }

    /// <summary>
    /// Property to acces to the Black Pieces positions of the board
    /// </summary>
    public List<string> BlackPieces
    {
        get { return m_blackPiecesPosition; }
    }

    /// <summary>
    /// Property to acces to the White Pieces positions of the board
    /// </summary>
    public List<string> WhitePieces
    {
        get { return m_whitePiecesPosition; }
    }

    /// <summary>
    /// Property to get the White King Position on the board
    /// </summary>
    public string WhiteKing
    {
        get
        {
            string res = "-";
            foreach (string piece in m_whitePiecesPosition)
            {
                if (m_status[piece] == ChessPiece.WHITE_KING)
                {
                    res = piece;
                }
            }
            return res;
        }
    }

    /// <summary>
    /// Property to get the Black King position on the board
    /// </summary>
    public string BlackKing
    {
        get
        {
            string res = "-";
            foreach (string piece in m_blackPiecesPosition)
            {
                if (m_status[piece] == ChessPiece.BLACK_KING)
                {
                    res = piece;
                }
            }
            return res;
        }
    }

    /// <summary>
    /// Moves a piece in the board. This method doesn't check if it's a valid movement
    /// </summary>
    /// <param name="origin">Origin tile code</param>
    /// <param name="destination">Destination tile code</param>
    public void movePieceToDestination(string origin, string destination)
    {
        ChessPiece piece = m_status[origin];
        if (piece != ChessPiece.NONE)
        {
            
            ChessPiece destinationPiece = m_status[destination];
            if (destinationPiece != ChessPiece.NONE)
            {
                ChessPiece destinationPieceColor = getPieceColor(destinationPiece);
                if (destinationPieceColor == ChessPiece.WHITE)
                {
                    m_whitePiecesPosition.Remove(destination);
                }
                else if (destinationPieceColor == ChessPiece.BLACK)
                {
                    m_blackPiecesPosition.Remove(destination);
                }
            }

            m_status[destination] = piece;
            m_status[origin] = ChessPiece.NONE;

            ChessPiece pieceColor = getPieceColor(piece);

            if (pieceColor == ChessPiece.WHITE)
            {
                m_whitePiecesPosition.Remove(origin);
                m_whitePiecesPosition.Add(destination);
            }
            else if (pieceColor == ChessPiece.BLACK)
            {
                m_blackPiecesPosition.Remove(origin);
                m_blackPiecesPosition.Add(destination);
            }
        }
    }

    /// <summary>
    /// Returns all the BoardStatus that can be originated from this BoardStatus
    /// given a color to move
    /// </summary>
    /// <param name="color">Color to move</param>
    public List<BoardStatus> getAllBoardMovements(ChessPiece color)
    {
        List<BoardStatus> result = new List<BoardStatus>();

        List<string> piecesToMove = color == ChessPiece.WHITE ? WhitePieces : BlackPieces;


        foreach (string tile in piecesToMove)
        {
            ChessPiece piece = m_status[tile];
            HashSet<string> movements = getAllPieceMovements(piece, tile);
            foreach (string movement in movements)
            {
                BoardStatus newBoard = new BoardStatus(this);
                newBoard.movePieceToDestination(tile, movement);
                if (!newBoard.Check(color))
                {
                    result.Add(newBoard);
                }
            }
        }

        //Pawn promotion.

        //int i = color == ChessPiece.WHITE ? 7 : 1;

        //string code = "";

        //for (int j = 0; j < 8; ++j)
        //{
        //    code = BoardManager.statusIndexesToCode(i, j);
        //    if ((color == ChessPiece.WHITE && Status[code] == ChessPiece.WHITE_PAWN) || (color == ChessPiece.BLACK && Status[code] == ChessPiece.BLACK_PAWN))
        //    {
        //        result.AddRange(promotePawn(color, i, j));
        //    }
        //}

        return result;
    }

    #region Piece movements

    /// <summary>
    /// Gets all the possible movements of the given piece
    /// </summary>
    /// <param name="piece">Piece to move</param>
    /// <param name="i">Row of the piece to move</param>
    /// <param name="j">Column of the piece to move</param>
    /// <returns>List of tile codes where the piece can move</returns>
    public HashSet<string> getAllPieceMovements(ChessPiece piece, int i, int j)
    {
        HashSet<string> result = new HashSet<string>();

        if (WhiteKing != "-" && BlackKing != "-")
        {

            switch (piece)
            {
                case ChessPiece.WHITE_KING:
                    result = getKingMovements(ChessPiece.WHITE, i, j);
                    break;
                case ChessPiece.BLACK_KING:
                    result = getKingMovements(ChessPiece.BLACK, i, j);
                    break;
                case ChessPiece.WHITE_QUEEN:
                    result = getQueenMovements(ChessPiece.WHITE, i, j);
                    break;
                case ChessPiece.BLACK_QUEEN:
                    result = getQueenMovements(ChessPiece.BLACK, i, j);
                    break;
                case ChessPiece.WHITE_BISHOP:
                    result = getBishopMovements(ChessPiece.WHITE, i, j);
                    break;
                case ChessPiece.BLACK_BISHOP:
                    result = getBishopMovements(ChessPiece.BLACK, i, j);
                    break;
                case ChessPiece.WHITE_KNIGHT:
                    result = getKnightMovements(ChessPiece.WHITE, i, j);
                    break;
                case ChessPiece.BLACK_KNIGHT:
                    result = getKnightMovements(ChessPiece.BLACK, i, j);
                    break;
                case ChessPiece.WHITE_ROOK:
                    result = getRookMovements(ChessPiece.WHITE, i, j);
                    break;
                case ChessPiece.BLACK_ROOK:
                    result = getRookMovements(ChessPiece.BLACK, i, j);
                    break;
                case ChessPiece.WHITE_PAWN:
                    result = getPawnMovements(ChessPiece.WHITE, i, j);
                    break;
                case ChessPiece.BLACK_PAWN:
                    result = getPawnMovements(ChessPiece.BLACK, i, j);
                    break;
                case ChessPiece.NONE:
                    result.Clear();
                    break;
            }
        }

        return result;
    }

    /// <summary>
    /// Gets all the possible movements of the given piece
    /// </summary>
    /// <param name="piece">Piece to move</param>
    /// <param name="tileCode">Tile code of the piece to move</param>
    /// <returns>List of tile codes where the piece can move</returns>
    public HashSet<string> getAllPieceMovements(ChessPiece piece, string tileCode)
    {
        int i = 0;
        int j = 0;
        BoardManager.codeToStatusIndexes(tileCode, out i, out j);
        return getAllPieceMovements(piece, i, j);
    }

    /// <summary>
    /// Gets all the possible movements of the KING piece
    /// </summary>
    /// <param name="color">Color of the piece to move</param>
    /// <param name="i">Row of the piece</param>
    /// <param name="j">Column of the piece</param>
    /// <returns>List of tile codes where the piece can move</returns>
    public HashSet<string> getKingMovements(ChessPiece color, int i, int j)
    {
        HashSet<string> result = new HashSet<string>();

        //up-left
        int row = i + 1;
        int column = j - 1;
        if(row >= 0 && row < 8 && column >= 0 && column < 8){
            string code = BoardManager.statusIndexesToCode(row, column);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //up
        row = i + 1;
        column = j;
        if (row >= 0 && row < 8 && column >= 0 && column < 8)
        {
            string code = BoardManager.statusIndexesToCode(row, column);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //up-right
        row = i + 1;
        column = j + 1;
        if (row >= 0 && row < 8 && column >= 0 && column < 8)
        {
            string code = BoardManager.statusIndexesToCode(row, column);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //left
        row = i;
        column = j - 1;
        if (row >= 0 && row < 8 && column >= 0 && column < 8)
        {
            string code = BoardManager.statusIndexesToCode(row, column);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //right
        row = i;
        column = j + 1;
        if (row >= 0 && row < 8 && column >= 0 && column < 8)
        {
            string code = BoardManager.statusIndexesToCode(row, column);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //down-left
        row = i - 1;
        column = j - 1;
        if (row >= 0 && row < 8 && column >= 0 && column < 8)
        {
            string code = BoardManager.statusIndexesToCode(row, column);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //down
        row = i - 1;
        column = j;
        if (row >= 0 && row < 8 && column >= 0 && column < 8)
        {
            string code = BoardManager.statusIndexesToCode(row, column);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //down-right
        row = i - 1;
        column = j + 1;
        if (row >= 0 && row < 8 && column >= 0 && column < 8)
        {
            string code = BoardManager.statusIndexesToCode(row, column);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }
        

        return result;
    }

    /// <summary>
    /// Gets all the possible movements of the QUEEN piece
    /// </summary>
    /// <param name="color">Color of the piece to move</param>
    /// <param name="i">Row of the piece</param>
    /// <param name="j">Column of the piece</param>
    /// <returns>List of tile codes where the piece can move</returns>
    public HashSet<string> getQueenMovements(ChessPiece color, int i, int j)
    {
        HashSet<string> result = new HashSet<string>();

        result.UnionWith(getKingMovements(color, i, j));
        result.UnionWith(getRookMovements(color, i, j));
        result.UnionWith(getBishopMovements(color, i, j));

        return result;
    }

    /// <summary>
    /// Gets all the possible movements of the BISHOP piece
    /// </summary>
    /// <param name="color">Color of the piece to move</param>
    /// <param name="i">Row of the piece</param>
    /// <param name="j">Column of the piece</param>
    /// <returns>List of tile codes where the piece can move</returns>
    public HashSet<string> getBishopMovements(ChessPiece color, int i, int j)
    {
        HashSet<string> result = new HashSet<string>();

        //up-right
        int h = i;
        int v = j;
        string code = "";
        bool cont = true;
        do
        {
            ++h;
            ++v;
            if (h >= 0 && h < 8 && v >= 0 && v < 8)
            {
                code = BoardManager.statusIndexesToCode(h, v);
                cont = m_status[code] == ChessPiece.NONE;

                if (cont || getPieceColor(m_status[code]) != color )
                {
                    result.Add(code);
                } 
            }
            else
            {
                cont = false;
            }

        }while(cont);


        //up-left
        h = i;
        v = j;
        code = "";
        cont = true;
        do
        {
            --h;
            ++v;
            if (h >= 0 && h < 8 && v >= 0 && v < 8)
            {
                code = BoardManager.statusIndexesToCode(h, v);
                cont = m_status[code] == ChessPiece.NONE;

                if (cont || getPieceColor(m_status[code]) != color)
                {
                    result.Add(code);
                } 
            }
            else
            {
                cont = false;
            }

        } while (cont);

        //down-right
        h = i;
        v = j;
        code = "";
        cont = true;
        do
        {
            ++h;
            --v;
            if (h >= 0 && h < 8 && v >= 0 && v < 8)
            {
                code = BoardManager.statusIndexesToCode(h, v);
                cont = m_status[code] == ChessPiece.NONE;

                if (cont || getPieceColor(m_status[code]) != color)
                {
                    result.Add(code);
                } 
            }
            else
            {
                cont = false;
            }

        } while (cont);

        //down-left
        h = i;
        v = j;
        code = "";
        cont = true;
        do
        {
            --h;
            --v;
            if (h >= 0 && h < 8 && v >= 0 && v < 8)
            {
                code = BoardManager.statusIndexesToCode(h, v);
                cont = m_status[code] == ChessPiece.NONE;

                if (cont || getPieceColor(m_status[code]) != color)
                {
                    result.Add(code);
                } 
            }
            else
            {
                cont = false;
            }

        } while (cont);

        return result;
    }

    /// <summary>
    /// Gets all the possible movements of the KNIGHT piece
    /// </summary>
    /// <param name="color">Color of the piece to move</param>
    /// <param name="i">Row of the piece</param>
    /// <param name="j">Column of the piece</param>
    /// <returns>List of tile codes where the piece can move</returns>
    public HashSet<string> getKnightMovements(ChessPiece color, int i, int j)
    {
        HashSet<string> result = new HashSet<string>();

        //up-left
        int h = i + 2;
        int v = j - 1;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //up-right
        h = i + 2;
        v = j + 1;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //right-up
        h = i + 1;
        v = j + 2;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //right-down
        h = i - 1;
        v = j + 2;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //down-right
        h = i - 2;
        v = j + 1;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //down-left
        h = i - 2;
        v = j - 1;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //left-down
        h = i - 1;
        v = j - 2;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        //left-up
        h = i + 1;
        v = j - 2;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
            }
        }

        return result;
    }

    /// <summary>
    /// Gets all the possible movements of the ROOK piece
    /// </summary>
    /// <param name="color">Color of the piece to move</param>
    /// <param name="i">Row of the piece</param>
    /// <param name="j">Column of the piece</param>
    /// <returns>List of tile codes where the piece can move</returns>
    public HashSet<string> getRookMovements(ChessPiece color, int i, int j)
    {
        HashSet<string> result = new HashSet<string>();

        ///up
        for (int up = i + 1; up < 8; ++up)
        {
            string code = BoardManager.statusIndexesToCode(up, j);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
                if (m_status[code] != ChessPiece.NONE)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        //down
        for (int down = i - 1; down >= 0; --down)
        {
            string code = BoardManager.statusIndexesToCode(down, j);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
                if (m_status[code] != ChessPiece.NONE)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        //right
        for (int right = j + 1; right < 8; ++right)
        {
            string code = BoardManager.statusIndexesToCode(i, right);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
                if (m_status[code] != ChessPiece.NONE)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        //left
        for (int left = j - 1; left >= 0; --left)
        {
            string code = BoardManager.statusIndexesToCode(i, left);
            if (getPieceColor(m_status[code]) != color)
            {
                result.Add(code);
                if (m_status[code] != ChessPiece.NONE)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Gets all the possible movements of the PAWN piece
    /// </summary>
    /// <param name="color">Color of the piece to move</param>
    /// <param name="i">Row of the piece</param>
    /// <param name="j">Column of the piece</param>
    /// <returns>List of tile codes where the piece can move</returns>
    public HashSet<string> getPawnMovements(ChessPiece color, int i, int j)
    {
        HashSet<string> result = new HashSet<string>();
        
        switch (color)
        {
            case ChessPiece.WHITE:
                int h = i + 1;
                int v = j;
                if (h >= 0 && h < 8 && v >= 0 && v < 8)
                {
                    string code = BoardManager.statusIndexesToCode(h, v);
                    if (m_status[code] == ChessPiece.NONE)
                    {
                        result.Add(code);
                    }
                }

                
                v = j - 1;
                if (h >= 0 && h < 8 && v >= 0 && v < 8)
                {
                    string code = BoardManager.statusIndexesToCode(h, v);
                    if (m_status[code] != ChessPiece.NONE && getPieceColor(m_status[code]) != color)
                    {
                        result.Add(code);
                    }
                }

                v = j + 1;
                if (h >= 0 && h < 8 && v >= 0 && v < 8)
                {
                    string code = BoardManager.statusIndexesToCode(h, v);
                    if (m_status[code] != ChessPiece.NONE && getPieceColor(m_status[code]) != color)
                    {
                        result.Add(code);
                    }
                }

                if (i == 1)
                {
                    h = i + 2;
                    v = j;
                    if (h >= 0 && h < 8 && v >= 0 && v < 8)
                    {
                        string previous = BoardManager.statusIndexesToCode(h - 1, v);
                        string code = BoardManager.statusIndexesToCode(h, v);
                        if (m_status[code] == ChessPiece.NONE && m_status[previous] == ChessPiece.NONE)
                        {
                            result.Add(code);
                        }
                    }
                }
                

                break;
            case ChessPiece.BLACK:

                h = i - 1;
                v = j;
                if (h >= 0 && h < 8 && v >= 0 && v < 8)
                {
                    string code = BoardManager.statusIndexesToCode(h, v);
                    if (m_status[code] == ChessPiece.NONE)
                    {
                        result.Add(code);
                    }
                }

                
                v = j - 1;
                if (h >= 0 && h < 8 && v >= 0 && v < 8)
                {
                    string code = BoardManager.statusIndexesToCode(h, v);
                    if (m_status[code] != ChessPiece.NONE && getPieceColor(m_status[code]) != color)
                    {
                        result.Add(code);
                    }
                }

                v = j + 1;
                if (h >= 0 && h < 8 && v >= 0 && v < 8)
                {
                    string code = BoardManager.statusIndexesToCode(h, v);
                    if (m_status[code] != ChessPiece.NONE && getPieceColor(m_status[code]) != color)
                    {
                        result.Add(code);
                    }
                }

                if (i == 6)
                {
                    h = i - 2;
                    v = j;
                    if (h >= 0 && h < 8 && v >= 0 && v < 8)
                    {
                        string previous = BoardManager.statusIndexesToCode(h + 1, v);
                        string code = BoardManager.statusIndexesToCode(h, v);
                        if (m_status[code] == ChessPiece.NONE && m_status[previous] == ChessPiece.NONE)
                        {
                            result.Add(code);
                        }
                    }
                }

                break;
        }

        return result;
    }

    #endregion

    /// <summary>
    /// Returns the movement difference between to chess boards.
    /// 
    /// ***IMPORTANT*** Only works with a board that has 1 movement of difference. 
    /// Other boards will return bad results
    /// </summary>
    /// <param name="color">Color of the pieces that had been moved</param>
    /// <param name="destinationBoard">Destination Board to check</param>
    /// <returns>KeyValuePair with this structure: (Piece, (origin, destination))</returns>
    public Movement getMovementDifference(ChessPiece color, BoardStatus destinationBoard)
    {
        List<string> colorPieces = color == ChessPiece.WHITE ? WhitePieces : BlackPieces;
        List<string> otherColorPieces = color == ChessPiece.WHITE ? destinationBoard.WhitePieces : destinationBoard.BlackPieces;

        string origin = "";
        string destination = "";

        foreach (string myPiece in colorPieces)
        {
            if (!otherColorPieces.Contains(myPiece))
            {
                origin = myPiece;
                break;
            }
        }

        foreach (string otherPiece in otherColorPieces)
        {
            if (!colorPieces.Contains(otherPiece))
            {
                destination = otherPiece;
                break;
            }
        }

        ChessPiece pieceMoved = m_status[origin];


        return new Movement(origin, destination, pieceMoved);
    }

    /// <summary>
    /// Predicate. True if the given piece is a White piece
    /// </summary>
    /// <param name="piece">Piece to check</param>
    public bool isWhitePiece(ChessPiece piece)
    {
        return piece == ChessPiece.WHITE_BISHOP ||
               piece == ChessPiece.WHITE_KING ||
               piece == ChessPiece.WHITE_KNIGHT ||
               piece == ChessPiece.WHITE_PAWN ||
               piece == ChessPiece.WHITE_QUEEN ||
               piece == ChessPiece.WHITE_ROOK;
    }

    /// <summary>
    /// Predicate. True if the given piece is a Black piece
    /// </summary>
    /// <param name="piece">Piece to check</param>
    public bool isBlackPiece(ChessPiece piece)
    {
        return piece == ChessPiece.BLACK_BISHOP ||
               piece == ChessPiece.BLACK_KING ||
               piece == ChessPiece.BLACK_KNIGHT ||
               piece == ChessPiece.BLACK_PAWN ||
               piece == ChessPiece.BLACK_QUEEN ||
               piece == ChessPiece.BLACK_ROOK;
    }

    /// <summary>
    /// Method used to get the piece color
    /// </summary>
    /// <param name="piece">Piece to check</param>
    /// <returns>Color of the given piece( WHITE || BLACK || NONE)</returns>
    public ChessPiece getPieceColor(ChessPiece piece)
    {
        if (isWhitePiece(piece))
            return ChessPiece.WHITE;
        else if (isBlackPiece(piece))
            return ChessPiece.BLACK;
        else
            return ChessPiece.NONE;
    }

    /// <summary>
    /// Check predicate. Used to check if the board is in "Check" status
    /// 
    /// This method also returns the list with the position of the checking pieces to the given color
    /// </summary>
    /// <param name="color">Color to check</param>
    /// <param name="checking">(REF) list of the cheking pieces</param>
    public bool Check(ChessPiece color, ref List<string> checking)
    {
        List<string> rivalPieces = color == ChessPiece.WHITE ? BlackPieces : WhitePieces;

        string KingPos = color == ChessPiece.WHITE ? WhiteKing : BlackKing;

        HashSet<string> rivalMovements = new HashSet<string>();

        bool res = false;

        foreach (string rivalPiece in rivalPieces)
        {
            rivalMovements = getAllPieceMovements(m_status[rivalPiece], rivalPiece);

            res = res || rivalMovements.Contains(KingPos);
            if (res)
            {
                checking.Add(rivalPiece);
            }
        }

        return res;
    }

    /// <summary>
    /// Check predicate. Used to check if the board is in "Check" status
    /// </summary>
    /// <param name="color">Color to check</param>
    public bool Check(ChessPiece color)
    {
        List<string> rivalPieces = color == ChessPiece.WHITE ? BlackPieces : WhitePieces;

        string KingPos = color == ChessPiece.WHITE ? WhiteKing : BlackKing;

        HashSet<string> rivalMovements = new HashSet<string>();

        bool res = false;

        foreach (string rivalPiece in rivalPieces)
        {
            rivalMovements = getAllPieceMovements(m_status[rivalPiece], rivalPiece);

            res = res || rivalMovements.Contains(KingPos);
            if (res)
            {
                return res;
            }
        }

        return res;
    }

    /// <summary>
    /// Predicate used to check if the board is in a "CheckMate" status.
    /// 
    /// This status is obtained when the board is in check and none of the possible
    /// movements from this board solves this situation.
    /// </summary>
    /// <param name="color">Color to check the status</param>
    /// <returns>True if the board is in CHECK MATE</returns>
    public bool CheckMate(ChessPiece color)
    {
        List<BoardStatus> nextStatus = getAllBoardMovements(color);

        bool mate = nextStatus.Count == 0;

        foreach (BoardStatus st in nextStatus)
        {
            mate = mate || st.Check(color);
        }

        return mate;
    }

    /// <summary>
    /// Predicate used to check if the board is in "Draw" status
    /// 
    /// Now the condition used is "if both oponents have only 1 piece remaining (kings)"
    /// </summary>
    /// <returns>True if there is a DRAW</returns>
    public bool Draw()
    {
        return m_blackPiecesPosition.Count == 1 && m_whitePiecesPosition.Count == 1;
    }

    /// <summary>
    /// Predicate used to check if "Castling" it's possible for one color.
    /// 
    /// It also returns the position of the rook available to castling with the king
    /// </summary>
    /// <param name="color">Color to check the castling</param>
    /// <param name="code">(OUT) Tile code position of the rook for castling</param>
    /// <returns>True if a castling is possible</returns>
    public bool Castling(ChessPiece color, out string code)
    {
        code = "-";

        string KingPos = color == ChessPiece.WHITE ? WhiteKing : BlackKing;

        if (color == ChessPiece.WHITE && KingPos == "E1")
        {
            string rook1 = "-";
            string rook2 = "-";
            foreach (string piece in m_whitePiecesPosition)
            {
                if (m_status[piece] == ChessPiece.WHITE_ROOK)
                {
                    if (rook1 == "-")
                    {
                        rook1 = piece;
                    }
                    else if (rook1 != "-" && rook2 == "-")
                    {
                        rook2 = piece;
                        break;
                    }
                }
            }

            if (rook1 == "H1" || rook1 == "A1")
            {
                code = rook1;
                return true;
            }
            else if (rook2 == "H1" || rook2 == "A1")
            {
                code = rook2;
                return true;
            }
            else
            {
                return false;
            }

        }
        else if (color == ChessPiece.BLACK && KingPos == "E8")
        {
            string rook1 = "-";
            string rook2 = "-";
            foreach (string piece in m_blackPiecesPosition)
            {
                if (m_status[piece] == ChessPiece.BLACK_ROOK)
                {
                    if (rook1 == "-")
                    {
                        rook1 = piece;
                    }
                    else if (rook1 != "-" && rook2 == "-")
                    {
                        rook2 = piece;
                        break;
                    }
                }
            }

            if (rook1 == "H8" || rook1 == "A8")
            {
                code = rook1;
                return true;
            }
            else if (rook2 == "H8" || rook2 == "A8")
            {
                code = rook2;
                return true;
            }
            else
            {
                return false;
            }

        }

        return false;
    }


    public List<BoardStatus> promotePawn(ChessPiece color, int i, int j)
    {
        List<BoardStatus> result = new List<BoardStatus>();

        string code = BoardManager.statusIndexesToCode(i, j);

        switch (color)
        {
            case ChessPiece.WHITE:

                if (i == 7)
                {
                    BoardStatus st = new BoardStatus(this);
                    st.Status[code] = ChessPiece.WHITE_BISHOP;
                    result.Add(st);

                    st = new BoardStatus(this);
                    st.Status[code] = ChessPiece.WHITE_KNIGHT;
                    result.Add(st);

                    st = new BoardStatus(this);
                    st.Status[code] = ChessPiece.WHITE_QUEEN;
                    result.Add(st);

                    st = new BoardStatus(this);
                    st.Status[code] = ChessPiece.WHITE_ROOK;
                    result.Add(st);
                }
                break;

            case ChessPiece.BLACK:

                if (i == 1)
                {
                    BoardStatus st = new BoardStatus(this);
                    st.Status[code] = ChessPiece.BLACK_BISHOP;
                    result.Add(st);

                    st = new BoardStatus(this);
                    st.Status[code] = ChessPiece.BLACK_KNIGHT;
                    result.Add(st);

                    st = new BoardStatus(this);
                    st.Status[code] = ChessPiece.BLACK_QUEEN;
                    result.Add(st);

                    st = new BoardStatus(this);
                    st.Status[code] = ChessPiece.BLACK_ROOK;
                    result.Add(st);
                }
                break;
        }
        

        return result;
    }

    #endregion

}
