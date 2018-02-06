using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchState : MonoBehaviour {

	public bool isTouching;
	public GameObject objectTouching;

	void OnTriggerEnter(Collider col){
		isTouching = true;
		objectTouching = col.gameObject;
	}

	void OnTriggerExit(Collider col){
		isTouching = false;
	}
}
