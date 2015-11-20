using UnityEngine;
using System.Collections.Generic;
using BSEngine.Input;

public class MovementTester : MonoBehaviour {

    private Vector3 position = Vector3.zero;

    private List<GameObject> marcadores = new List<GameObject>();

    public GameObject marcador = null;

    public void onClickReceived(InputEvent e)
    {
        if (e.isOk)
        {
            foreach (GameObject obj in marcadores)
            {
                BSEngine.PoolMgr.Singleton.Destroy(obj);
            }
            marcadores.Clear();

            Ray ray = Camera.main.ScreenPointToRay(position);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                PieceTag tag = hit.collider.gameObject.GetComponent<PieceTag>();
                if (tag != null)
                {
                    ChessPiece piece = tag.Piece;

                    List<string> movements = BoardManager.Singleton.CurrentStatus.getAllPieceMovements(piece, tag.TileCode);

                    foreach (string movement in movements)
                    {
                        Vector3 pos = BoardManager.Singleton.Waypoints[movement].transform.position;
                        pos.y = pos.y + 0.55f;

                        marcadores.Add(BSEngine.PoolMgr.Singleton.Instatiate(marcador, pos, Quaternion.identity));
                    }
                }
            }
        }
    }

    public void onMouseMoved(MouseState state)
    {
        position = state.AbsolutePosition;
    }

	// Use this for initialization
	void Start () {
        InputMgr.Singleton.RegisterOrderListener("Game", "CLICK", onClickReceived);
        InputMgr.Singleton.RegisterMouseListener("Game", onMouseMoved);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
