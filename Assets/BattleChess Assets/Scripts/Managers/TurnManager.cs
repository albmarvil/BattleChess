///----------------------------------------------------------------------
/// @file TurnManager.cs
///
/// This file contains the declaration of TurnManager class.
/// 
/// Turn manager controls all the turn flow.
/// 
/// The turn is controlled by coroutines, waiting for each action to be finished.
/// 
/// There are multiple callbacks for each different steps during a Turn
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 03/12/2015
///----------------------------------------------------------------------



using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BSEngine;

/// <summary>
/// Enum used to define different ind of players
/// The different CPU players are used for difficulty config
/// </summary>
public enum PlayerType
{
    HUMAN = 0,
    CPU,
    //CPU2 = 5,
    //CPU3 = 10
}

public delegate void onTurnStart(Turn t);
public delegate void onTurnAnimationFinished(Turn t);
public delegate void onTurnMovementDecisionFinished(Turn t);
public delegate void onTurnFinished(Turn t);
public delegate void onEndMatch(Turn t);

public struct Turn
{
    /// <summary>
    /// Turn order of the match
    /// </summary>
    private int m_turnOrder;

    /// <summary>
    /// Board status AFTER the movement
    /// </summary>
    private BoardStatus m_boardStatus;

    /// <summary>
    /// Movement description done in this turn
    /// </summary>
    private Movement m_movement;

    /// <summary>
    /// Player piece color of this turn movement
    /// </summary>
    private ChessPiece m_playerColor;

    /// <summary>
    /// Player tipe. Human or CPU
    /// </summary>
    private PlayerType m_playerType;

    public int TurnOrder
    {
        get { return m_turnOrder; }
        set { m_turnOrder = value; }
    }

    public BoardStatus BoardStatus
    {
        get { return m_boardStatus; }
        set { m_boardStatus = value; }
    }

    public Movement Movement
    {
        get { return m_movement; }
        set { m_movement = value; }
    }

    public ChessPiece PlayerColor
    {
        get { return m_playerColor; }
        set { m_playerColor = value; }
    }

    public PlayerType PlayerType
    {
        get { return m_playerType; }
        set { m_playerType = value; }
    }

    public override string ToString()
    {
        return "Turn: " + m_turnOrder + "("+m_playerColor+") - " + m_movement.PieceMoved + " from " + m_movement.Origin + " to " + m_movement.Destination;
    }
}

public class TurnManager : MonoBehaviour {

	    
    #region Singleton

    /// <summary>
    /// Singleton instance of the class
    /// </summary>
    private static TurnManager m_instance = null;

    /// <summary>
    /// Property to get the singleton instance of the class.
    /// </summary>
    public static TurnManager Singleton { get { return m_instance; } }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static TurnManager() { }

    /// <summary>
    /// This is like the Init but done by the MonoBehaviour
    /// </summary>
    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
        {
            Debug.LogError("Someone is trying to create various TurnManager [" + name + "]");
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
    /// <summary>
    /// Reference to the human movement selector
    /// </summary>
    public MovementSelector m_humanMovementSelector = null;


    public GameObject m_CheckToBlackLabel = null;

    public GameObject m_CheckToWhiteLabel = null;

    public GameObject m_CheckMateToBlackLabel = null;

    public GameObject m_CheckMateToWhiteLabel = null;

    public GameObject m_WhiteTurnLabel = null;

    public GameObject m_BlackTurnLabel = null;

    #endregion

    #region Private params

    /// <summary>
    /// Type of Player 1 (White pieces)
    /// </summary>
    private PlayerType m_player1;

    /// <summary>
    /// Type of Player 2 (Black pieces)
    /// </summary>
    private PlayerType m_player2;

    /// <summary>
    /// Turn record of the match
    /// </summary>
    private List<Turn> m_turnRecord = new List<Turn>();

    /// <summary>
    /// Delegate/Callback when a turn starts
    /// </summary>
    private onTurnStart m_onTurnStart = null;

    /// <summary>
    /// Delegate/Callback when turn animation finishes
    /// </summary>
    private onTurnAnimationFinished m_onTurnAnimationFinished = null;

    /// <summary>
    /// Delegate/Callback when turn movement decision finishes
    /// </summary>
    private onTurnMovementDecisionFinished m_onTurnMovementDecisionFinished = null;

    /// <summary>
    /// Delegate/Callback when turn finishes
    /// </summary>
    private onTurnFinished m_onTurnFinished = null;

    /// <summary>
    /// Delegate/Callback when match finishes
    /// </summary>
    private onEndMatch m_onEndMatch = null;

    #endregion

    #region Public methods

    public Turn CurrentTurn
    {
        get { return m_turnRecord[m_turnRecord.Count - 1]; }
        set { m_turnRecord[m_turnRecord.Count - 1] = value; }
    }

    public void startMatch(PlayerType player1, PlayerType player2)
    {
        m_player1 = player1;
        m_player2 = player2;

        BoardManager.Singleton.startMatch();

        m_turnRecord.Clear();

        Turn t1 = new Turn(); ;
        t1.TurnOrder = 1;
        t1.PlayerColor = ChessPiece.WHITE;
        t1.PlayerType = player1;

        m_turnRecord.Add(t1);

        ////HACK
        BoardManager.Singleton.ClearBoard();
        BoardManager.Singleton.ShowBoard();

        StartCoroutine(Turn());
    }

    public void registerOnTurnStartListener(onTurnStart listener)
    {
        m_onTurnStart += listener;
    }

    public void unregisterOnTurnStartListener(onTurnStart listener)
    {
        m_onTurnStart -= listener;
    }

    public void registerOnTurnAnimationFinishedListener(onTurnAnimationFinished listener)
    {
        m_onTurnAnimationFinished += listener;
    }

    public void unregisterOnTurnAnimationFinishedListener(onTurnAnimationFinished listener)
    {
        m_onTurnAnimationFinished -= listener;
    }

    public void registerOnTurnMovementDecisionFinishedListener(onTurnMovementDecisionFinished listener)
    {
        m_onTurnMovementDecisionFinished += listener;
    }

    public void unregisterOnTurnMovementDecisionFinishedListener(onTurnMovementDecisionFinished listener)
    {
        m_onTurnMovementDecisionFinished -= listener;
    }

    public void registerOnTurnFinishedListener(onTurnFinished listener)
    {
        m_onTurnFinished += listener;
    }

    public void unregisterOnTurnFinishedListener(onTurnFinished listener)
    {
        m_onTurnFinished -= listener;
    }

    public void registerOnEndMatchListener(onEndMatch listener)
    {
        m_onEndMatch += listener;
    }

    public void unregisterOnEndMatchListener(onEndMatch listener)
    {
        m_onEndMatch -= listener;
    }

    #endregion

    #region Private methods

    private IEnumerator Turn()
    {
        if(m_onTurnStart != null)
            m_onTurnStart(CurrentTurn);

        ///EJECUCION DE ANIMACION DE TURNO
        yield return null;

        if (CurrentTurn.PlayerColor == ChessPiece.WHITE)
        {
            m_WhiteTurnLabel.SetActive(true);
            m_BlackTurnLabel.SetActive(false);
        }
        else if (CurrentTurn.PlayerColor == ChessPiece.BLACK)
        {
            m_WhiteTurnLabel.SetActive(false);
            m_BlackTurnLabel.SetActive(true);
        }

        if(m_onTurnAnimationFinished != null)
            m_onTurnAnimationFinished(CurrentTurn);

        PlayerType currentPlayer = CurrentTurn.PlayerColor == ChessPiece.WHITE ? m_player1 : m_player2;
        ChessPiece playerColor = CurrentTurn.PlayerColor;
        ChessPiece oponentColor = playerColor == ChessPiece.WHITE ? ChessPiece.BLACK : ChessPiece.WHITE;

        Movement movement = new Movement();

        BoardStatus nextBoard = new BoardStatus();

        if (currentPlayer == PlayerType.HUMAN)
        {
            ///While HUMAN is thinking, CPU can think too
        
            ///WAIT FOR MOVEMENT
            yield return StartCoroutine(m_humanMovementSelector.WaitForMovement());
            movement = m_humanMovementSelector.Movement;
            nextBoard = new BoardStatus(BoardManager.Singleton.CurrentStatus);
            nextBoard.movePieceToDestination(movement.Origin, movement.Destination);
        }
        else
        {
            ///EJECUCION DE MIN MAX

            float TimeToThink = StorageMgr.Blackboard.Get<float>("TimeToThink");
            int MaxDepth = StorageMgr.Blackboard.Get<int>("MaxDepth");
            
            MinMaxJob job = new MinMaxJob(MaxDepth, TimeToThink);
            job.Start();

            yield return StartCoroutine(job.WaitFor());
            
            ////HACK
            ChessNode result = (ChessNode)job.RandomResult;
            if (result != null)
            {
                BoardStatus st = ((ChessNode)job.RandomResult).Board;
                movement = BoardManager.Singleton.CurrentStatus.getMovementDifference(CurrentTurn.PlayerColor, st);
                nextBoard = st;
            }
            else
            {
                Debug.LogError("Algoritmo MinMax sin resultado Nodos Procesados: "+job.ProcessedNodes);
            }
        }

        if(m_onTurnMovementDecisionFinished != null)
            m_onTurnMovementDecisionFinished(CurrentTurn);

        ///EJECUCION DEL MOVIMIENTO
        yield return null;

        BoardManager.Singleton.UpdateCurrentStatus(nextBoard);


        ///Registro de turno y turno siguiente
        Turn t = CurrentTurn;
        t.BoardStatus = BoardManager.Singleton.CurrentStatus;
        t.Movement = movement;
        CurrentTurn = t;

        Debug.Log(CurrentTurn);

        Turn next = new Turn();
        next.PlayerColor = CurrentTurn.PlayerColor == ChessPiece.WHITE ? ChessPiece.BLACK : ChessPiece.WHITE;
        next.TurnOrder = CurrentTurn.TurnOrder + 1;
        next.PlayerType = next.TurnOrder % 2 == 0 ? m_player2 : m_player1;
        m_turnRecord.Add(next);

        if(m_onTurnFinished != null)
            m_onTurnFinished(t);


        ////AVISO Y COMPROBACIONES DE FIN DE JUEGO

        ////HHACKS!
        m_CheckToWhiteLabel.SetActive(false);
        m_CheckToBlackLabel.SetActive(false);

        

        if (BoardManager.Singleton.CurrentStatus.Draw())
        {
            Debug.LogWarning("Turn: " + t.TurnOrder + " Draw!!!");
        }
        else if (BoardManager.Singleton.CurrentStatus.CheckMate(oponentColor))
        {
            if (oponentColor == ChessPiece.WHITE)
            {
                Debug.LogWarning("Turn: " + t.TurnOrder + " END MATCH - Check Mate to White. BLACK WINS");
                m_CheckMateToWhiteLabel.SetActive(true);

                if(m_onEndMatch != null)
                    m_onEndMatch(CurrentTurn);
            }
            else if (oponentColor == ChessPiece.BLACK)
            {
                Debug.LogWarning("Turn: " + t.TurnOrder + " END MATCH - Check Mate to Black. WHITE WINS");
                m_CheckMateToBlackLabel.SetActive(true);

                if(m_onEndMatch != null)
                    m_onEndMatch(CurrentTurn);
            }
            
        }
        else if (BoardManager.Singleton.CurrentStatus.Check(oponentColor))
        {
            if (oponentColor == ChessPiece.WHITE)
            {
                Debug.LogWarning("Turn: " + t.TurnOrder + " Check to White");
                m_CheckToWhiteLabel.SetActive(true);
                m_CheckToBlackLabel.SetActive(false);
                StartCoroutine(Turn());
            }
            else if (oponentColor == ChessPiece.BLACK)
            {
                Debug.LogWarning("Turn: " + t.TurnOrder + " Check to Black");
                m_CheckToWhiteLabel.SetActive(false);
                m_CheckToBlackLabel.SetActive(true);
                StartCoroutine(Turn());           
            }
            
        }
        else
        {
            StartCoroutine(Turn());
        }

        

        ////HACK
        BoardManager.Singleton.ClearBoard();
        BoardManager.Singleton.ShowBoard();

    }

    #endregion

    #region Monobehavior calls

    private void Start()
    {
        ///TO DO
        ///leer de Blackboard lo necesario para recueprar la configuracion de juego
        startMatch(PlayerType.HUMAN, PlayerType.CPU);



        //DataTable tiempos = new DataTable("Tiempos", SerializationMode.XML, true);
        //for (int i = 0; i <= StorageMgr.Blackboard.Get<int>("MaxDepth"); ++i)
        //{
        //    tiempos[i] = new DataTable("because yes", SerializationMode.NONE, false);
        //}

        //registerOnEndMatchListener(End);
    }

    //private void End(Turn t)
    //{
    //    DataTable tiempos = StorageMgr.Blackboard.Get<DataTable>("Tiempos");

    //    DataTable tiempos2 = new DataTable("Tiempos_AUX2", SerializationMode.XML, false);

    //    for (int i = 0; i <= StorageMgr.Blackboard.Get<int>("MaxDepth"); ++i)
    //    {
    //        float media = 0.0f;
    //        DataTable table = (DataTable) tiempos[i];
    //        foreach (string key in table)
    //        {
    //            media += (float)table[key];
    //        }
    //        media /= table.Count;
    //        tiempos2[i] = media;
    //    }
    //}

    #endregion


}
