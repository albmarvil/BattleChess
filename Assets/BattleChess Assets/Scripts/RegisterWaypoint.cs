using UnityEngine;
using System.Collections;

public class RegisterWaypoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BoardManager.Singleton.registerBoardWaypoint(gameObject.name, gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
