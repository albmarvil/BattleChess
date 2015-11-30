///----------------------------------------------------------------------
/// @file MinMaxJob.cs
///
/// This file contains the declaration of MinMaxJob class.
/// 
/// This class is an implementation of MinMax Algortithm using prepared to be used in threading
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 30/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using BSEngine.Threading;

public class MinMaxJob : ThreadedJob
{

    #region Private params

    /// <summary>
    /// Origin Node of the problem
    /// </summary>
    private MinMaxNode m_origin;

    /// <summary>
    /// Number of processed nodes by the Algorithm
    /// </summary>
    private int m_processedNodes;

    #endregion

    #region Public Methods
    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="depth">Maximum depth when running the algorithm</param>
    public MinMaxJob(int depth)
    {
        m_origin = new ChessNode(BoardManager.Singleton.CurrentStatus, depth, null, NodeType.MAX);
        m_processedNodes = 0;
    }

    /// <summary>
    /// Public property to acces to a Random Status node selected by the algorithm
    /// </summary>
    public MinMaxNode RandomResult
    {
        get 
        {
            System.Random rnd = new System.Random();
            int index = rnd.Next(m_origin.BestChildren.Count);
            return m_origin.BestChildren[index]; 
        }
    }

    /// <summary>
    /// Public property to acces to the Status node selected by the algorithm
    /// </summary>
    public MinMaxNode Result
    {
        get { return m_origin.BestChildren[0]; }
    }

    /// <summary>
    /// Number of nodes processed by the MinMax Algorithm
    /// </summary>
    public int ProcessedNodes
    {
        get { return m_processedNodes; }
    }

    #endregion

    #region Private methods
    /// <summary>
    /// Recursive function of MinMax Algorithm.
    /// 
    /// This version of the algorithm uses Apha-Beta bound optimization.
    /// </summary>
    /// <param name="node">Reference node to start the algorithm</param>
    /// <param name="alpha">Current alpha value</param>
    /// <param name="beta">Current beta value</param>
    /// <param name="nodeType">NodeType: MIN or MAX node</param>
    /// <returns>Optimal value for the Current Node</returns>
    private float MinMax(MinMaxNode node, float alpha, float beta, NodeType nodeType)
    {
        //Debug.Log("recursivo - depth: "+depth+" alpha: "+alpha+" beta: "+beta+" NodeType: "+nodeType);

        m_processedNodes++;

        if (node.IsEndNode())
        {
            //Debug.Log("retorno valor de funcion estatica");
            return node.StaticValueFunction();
        }
        else if (nodeType == NodeType.MAX)
        {
            foreach (MinMaxNode child in node.getChildren())
            {
                float newAlpha = MinMax(child, alpha, beta, NodeType.MIN);
                //Debug.Log("Alpha score: " + newAlpha);
                if (newAlpha > alpha)
                {
                    node.BestChildren.Clear();
                    node.BestChildren.Add(child);
                    alpha = newAlpha;

                    //Debug.LogWarning("New best Alpha score: " + newAlpha);
                }
                else if (newAlpha == alpha)
                {
                    node.BestChildren.Add(child);
                }

                if (beta <= alpha)
                {
                    //Debug.LogWarning("Poda alpha!!");
                    break;
                }
            }
            //Debug.Log("retorno alpha");
            return alpha;
        }
        else
        {
            foreach (MinMaxNode child in node.getChildren())
            {
                float newBeta = MinMax(child, alpha, beta, NodeType.MAX);
                //Debug.Log("Beta score: " + newBeta);
                if (newBeta < beta)
                {
                    node.BestChildren.Clear();
                    node.BestChildren.Add(child);
                    beta = newBeta;

                    //Debug.LogWarning("new Best Beta Score: " + newBeta);
                }
                else if (newBeta == beta)
                {
                    node.BestChildren.Add(child);
                }
                
                if (beta <= alpha)
                {
                    //Debug.LogWarning("Poda Beta!!!");
                    break;
                }
            }
            //Debug.Log("retorno beta");
            return beta;
        }
    }

    /// <summary>
    /// Function called when the thread starts.
    /// 
    /// It contains the first call to the MinMax Algortihm
    /// </summary>
    protected override void ThreadFunction()
    {
        //Debug.Log("Hola");
        MinMax(m_origin, float.NegativeInfinity, float.PositiveInfinity, NodeType.MAX);
    }

    /// <summary>
    /// Function called when the thread ends
    /// </summary>
    protected override void OnFinished()
    {
        //Debug.Log("FIN para " + m_origin.NodeType);
    }

    #endregion

}
