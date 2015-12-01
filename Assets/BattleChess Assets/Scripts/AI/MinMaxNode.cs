///----------------------------------------------------------------------
/// @file MinMaxNode.cs
///
/// This file contains the declaration of MinMaxNode class.
/// 
/// This class is used as parent class for specific implementations of MinMax Algorithm 
/// implemented in MinMaxJob.cs
///
/// @author Alberto Martinez Villaran <tukaram92@gmail.com>
/// @date 30/11/2015
///----------------------------------------------------------------------


using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Node type, used to indicate if a node is MIN or MAX in MinMax Algorithm
/// </summary>
public enum NodeType
{
    MIN,
    MAX
}


public abstract class MinMaxNode
{

    #region Private params
    /// <summary>
    /// Parent node. If null, this node doesn't has parents (fisrt node on MINMAX algorithm)
    /// </summary>
    private MinMaxNode m_parent = null;

    /// <summary>
    /// Best child evaluated in MinMax Algorithm, if null, this node hasn't been evaluated yet
    /// </summary>
    private List<MinMaxNode> m_bestChildren = new List<MinMaxNode>();

    /// <summary>
    /// Depth of the node. 0 depth means that the node is in the MinMax's Algorithm border
    /// </summary>
    private int m_Depth = 0;

    /// <summary>
    /// Node type
    /// </summary>
    private NodeType m_NodeType = NodeType.MAX;

    #endregion

    #region Public Methods
    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="depth">Depth from the origin node</param>
    /// <param name="parentNode">Parent node</param>
    /// <param name="nodeType">Node type</param>
    public MinMaxNode(int depth, ChessNode parentNode, NodeType nodeType)
    {
        m_Depth = depth;
        m_parent = parentNode;
        m_NodeType = nodeType;
    }

    /// <summary>
    /// Depth of the node
    /// </summary>
    public int Depth
    {
        get { return m_Depth; }
        set { m_Depth = value; }
    }

    /// <summary>
    /// Parent node
    /// </summary>
    public MinMaxNode Parent
    {
        get { return m_parent; }
        set { m_parent = value; }
    }

    /// <summary>
    /// Best child evaluated in MinMax Algorithm
    /// </summary>
    public List<MinMaxNode> BestChildren
    {
        get { return m_bestChildren; }
        //set { m_bestChild = value; }
    }

    /// <summary>
    /// Node type
    /// </summary>
    public NodeType NodeType
    {
        get { return m_NodeType; }
        set { m_NodeType = value; }
    }

    /// <summary>
    /// Abstract function used to get all the possible children of the node
    /// </summary>
    /// <returns>All child nodes</returns>
    public abstract List<MinMaxNode> getChildren();

    /// <summary>
    /// Abstract function used to evaluate the end nodes in MinMax Algorithm.
    /// </summary>
    /// <returns>Static evaluation value</returns>
    public abstract float StaticValueFunction();

    /// <summary>
    /// Predicate used to know if the node is a "End Node" in MinMax Algorithm
    /// </summary>
    /// <returns>True if the node is means the "End"</returns>
    public bool IsEndNode()
    {
        return m_Depth <= 0 || EndNodeSpecificCondition();
    }

    /// <summary>
    /// Abstract predicate used to implement specific conditions on EndNode predicate
    /// </summary>
    public abstract bool EndNodeSpecificCondition();

    #endregion

}
