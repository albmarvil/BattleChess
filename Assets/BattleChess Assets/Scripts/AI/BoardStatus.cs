using UnityEngine;
using System.Collections.Generic;

public enum ChessPiece
{
    NONE,
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
    WHITE = WHITE_KING | WHITE_BISHOP | WHITE_KNIGHT | WHITE_PAWN | WHITE_QUEEN | WHITE_ROOK,
    BLACK = BLACK_BISHOP |BLACK_KING | BLACK_KNIGHT | BLACK_PAWN | BLACK_QUEEN | BLACK_ROOK
}


public class BoardStatus {


    #region Public params

    #endregion

    #region Private params

    private Dictionary<string, ChessPiece> m_status = new Dictionary<string, ChessPiece>();

    #endregion

    #region Public methods

    public BoardStatus()
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                m_status.Add(BoardManager.statusIndexesToCode(i, j), ChessPiece.NONE);
            }
        }

        setToStartingStatus();
    }

    public BoardStatus(BoardStatus status)
    {
        m_status = status.m_status;
    }

    public Dictionary<string, ChessPiece> Status
    {
        get { return m_status; }
    }

    public void setToStartingStatus()
    {
        m_status = new Dictionary<string, ChessPiece>();

       
        #region White pieces

        m_status.Add("A1", ChessPiece.WHITE_ROOK);
        m_status.Add("A2", ChessPiece.WHITE_KNIGHT);
        m_status.Add("A3", ChessPiece.WHITE_BISHOP);
        m_status.Add("A4", ChessPiece.WHITE_QUEEN);
        m_status.Add("A5", ChessPiece.WHITE_KING);
        m_status.Add("A6", ChessPiece.WHITE_BISHOP);
        m_status.Add("A7", ChessPiece.WHITE_KNIGHT);
        m_status.Add("A8", ChessPiece.WHITE_ROOK);

        for (int i = 0; i < 8; ++i)
        {
             m_status.Add(BoardManager.statusIndexesToCode(0,i), ChessPiece.WHITE_PAWN);
        }

        #endregion

        #region Black pieces

        m_status.Add("H1", ChessPiece.BLACK_ROOK);
        m_status.Add("H2", ChessPiece.BLACK_KNIGHT);
        m_status.Add("H3", ChessPiece.BLACK_BISHOP);
        m_status.Add("H4", ChessPiece.BLACK_QUEEN);
        m_status.Add("H5", ChessPiece.BLACK_KING);
        m_status.Add("H6", ChessPiece.BLACK_BISHOP);
        m_status.Add("H7", ChessPiece.BLACK_KNIGHT);
        m_status.Add("H8", ChessPiece.BLACK_ROOK);

        for (int i = 0; i < 8; ++i)
        {
            m_status.Add(BoardManager.statusIndexesToCode(7, i), ChessPiece.BLACK_PAWN);
        }

        #endregion
    }

    public List<BoardStatus> getAllBoardMovements()
    {
        List<BoardStatus> result = new List<BoardStatus>();

        foreach (string tile in m_status.Keys)
        {
            ChessPiece piece = m_status[tile];
            List<string> movements = getAllPieceMovements(piece, tile);
            foreach (string movement in movements)
            {
                BoardStatus newBoard = new BoardStatus(this);
                newBoard.Status[movement] = piece;
                newBoard.Status[tile] = ChessPiece.NONE;
                result.Add(newBoard);
            }
        }

        return result;
    }

    #region Piece movements

    public List<string> getAllPieceMovements(ChessPiece piece, int i, int j)
    {
        List<string> result = new List<string>();

        
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
        }

        return result;
    }

    public List<string> getAllPieceMovements(ChessPiece piece, string tileCode)
    {
        int i = 0;
        int j = 0;
        BoardManager.codeToStatusIndexes(tileCode, out i, out j);
        return getAllPieceMovements(piece, i, j);
    }

    public List<string> getKingMovements(ChessPiece color, int i, int j)
    {
        List<string> result = new List<string>();

        for (int offsetH = -1; offsetH <= 1; ++offsetH)
        {
            int horPos = i + offsetH;
            if (horPos >= 0 && horPos <= 7)
            {
                for (int offsetV = -1; offsetV <= 1; ++offsetV)
                {
                    if (offsetH != 0 && offsetV != 0)
                    {
                        int verPos = j + offsetV;

                        if (verPos >= 0 && verPos <= 7)
                        {
                            string code = BoardManager.statusIndexesToCode(horPos, verPos);

                            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
                            {
                                result.Add(code);
                            }
                        }
                    }
                }
            }
        }

        return result;
    }

    public List<string> getQueenMovements(ChessPiece color, int i, int j)
    {
        List<string> result = new List<string>();

        result.AddRange(getKingMovements(color, i, j));
        result.AddRange(getRookMovements(color, i, j));
        result.AddRange(getBishopMovements(color, i, j));

        return result;
    }

    public List<string> getBishopMovements(ChessPiece color, int i, int j)
    {
        List<string> result = new List<string>();

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

                if (cont || (m_status[code]) == 0 )
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

                if (cont || (m_status[code]) == 0)
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

                if (cont || (m_status[code]) == 0)
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

                if (cont || (m_status[code]) == 0)
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

    public List<string> getKnightMovements(ChessPiece color, int i, int j)
    {
        List<string> result = new List<string>();

        //up-left
        int h = i + 2;
        int v = j - 1;
        if (h >= 0 && h < 8 && v >= 0 && v < 8)
        {
            string code = BoardManager.statusIndexesToCode(h, v);
            if(m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
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
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
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
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
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
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
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
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
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
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
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
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
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
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
            {
                result.Add(code);
            }
        }

        return result;
    }

    public List<string> getRookMovements(ChessPiece color, int i, int j)
    {
        List<string> result = new List<string>();

        ///up
        for (int up = i + 1; up < 8; ++up)
        {
            string code = BoardManager.statusIndexesToCode(up, j);
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
            {
                result.Add(code);
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
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
            {
                result.Add(code);
            }
            else
            {
                break;
            }
        }

        //right
        for (int right = j + 1; j < 8; ++right)
        {
            string code = BoardManager.statusIndexesToCode(i, right);
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
            {
                result.Add(code);
            }
            else
            {
                break;
            }
        }

        //left
        for (int left = j - 1; j >= 0; --left)
        {
            string code = BoardManager.statusIndexesToCode(left, i);
            if (m_status[code] == ChessPiece.NONE || (m_status[code] & color) == 0)
            {
                result.Add(code);
            }
            else
            {
                break;
            }
        }

        return result;
    }

    public List<string> getPawnMovements(ChessPiece color, int i, int j)
    {
        List<string> result = new List<string>();

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
                    if ((m_status[code] & color) == 0)
                    {
                        result.Add(code);
                    }
                }

                v = j + 1;
                if (h >= 0 && h < 8 && v >= 0 && v < 8)
                {
                    string code = BoardManager.statusIndexesToCode(h, v);
                    if ((m_status[code] & color) == 0)
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
                        string code = BoardManager.statusIndexesToCode(h, v);
                        if ((m_status[code] & color) == 0)
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
                    if ((m_status[code] & color) == 0)
                    {
                        result.Add(code);
                    }
                }

                v = j + 1;
                if (h >= 0 && h < 8 && v >= 0 && v < 8)
                {
                    string code = BoardManager.statusIndexesToCode(h, v);
                    if ((m_status[code] & color) == 0)
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
                        string code = BoardManager.statusIndexesToCode(h, v);
                        if ((m_status[code] & color) == 0)
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

    #endregion

    #region Private methods

    #endregion

    #region Monobehavior calls

    #endregion

}
