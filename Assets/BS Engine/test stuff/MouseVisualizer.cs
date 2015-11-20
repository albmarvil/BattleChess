using UnityEngine;
using System.Collections;
using BSEngine.Input;

public class MouseVisualizer : MonoBehaviour {


    public float m_DistanceFromCamera = 1.0f;

    Transform m_transform = null;

	// Use this for initialization
	void Start () {
        m_transform = gameObject.GetComponent<Transform>();

        InputMgr.Singleton.RegisterMouseListener("Game", onMousemoved);
	}

    public void onMousemoved(MouseState state)
    {
        Vector3 newPos = state.AbsolutePosition;
        newPos.z = m_DistanceFromCamera;//Camera.main.transform.position.z;

        //newPos += Camera.main.transform.forward * m_DistanceFromCamera; 

        m_transform.position = Camera.main.ScreenToWorldPoint(newPos);
    }
}
