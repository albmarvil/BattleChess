using UnityEngine;
using System.Collections.Generic;
using BSEngine.Input;

public class TestScript : MonoBehaviour {

    public bool start = false;

    public void onNextMovementReceived(InputEvent e)
    {
        if (e.isOk)
        {
            ///Mover el estado a un hijo aleatorio una vez con cada color
            ///
            ///llamar a que se despinte y pinte en cada momento para todas las piezas
            ///

            //Blancas
            List<BoardStatus> children = BoardManager.Singleton.CurrentStatus.getAllBoardMovements(ChessPiece.WHITE);

            int index = Random.Range(0, children.Count);

            BoardStatus st = children[index];

            BoardManager.Singleton.CurrentStatus = st;

            BoardManager.Singleton.ClearBoard();
            BoardManager.Singleton.ShowBoard();

            //Vector3 pos = gameObject.transform.position;
            //pos.y = pos.y + 1;
            //BSEngine.PoolMgr.Singleton.Instatiate(prefab, pos, Quaternion.identity);
        }
    }

    public void onTestKey1Pressed(InputEvent e)
    {
        
    }

    private void Start()
    {
        InputMgr.Singleton.RegisterOrderListener("Game", "NEXT_MOVEMENT", onNextMovementReceived);
        InputMgr.Singleton.RegisterOrderListener("Game", "TESTKEY1", onTestKey1Pressed);

        start = true;
    }

    private void Update()
    {
        if (start)
        {
            BoardManager.Singleton.ClearBoard();
            BoardManager.Singleton.ShowBoard();
            start = false;
        }
        
    }
}
