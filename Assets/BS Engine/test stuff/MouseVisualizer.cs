using UnityEngine;
using System.Collections;
using BSEngine.Input;

public class MouseVisualizer : MonoBehaviour {


    Transform m_transform = null;

	// Use this for initialization
	void Start () {
        m_transform = gameObject.GetComponent<Transform>();

        InputMgr.Singleton.RegisterMouseListener("Game", onMousemoved);
	}

    public void onMousemoved(MouseState state)
    {
        Vector3 newPos = state.AbsolutePosition;
        newPos.z = 10.0f;
        m_transform.position = Camera.main.ScreenToWorldPoint(newPos);
    }
}
