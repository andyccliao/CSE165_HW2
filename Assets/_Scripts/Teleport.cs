using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

	public GameObject lineRendererGobject;
	private LineRenderer lineRenderComponent;
	public GameObject player;
	private bool isTeleporting = false;

	// Use this for initialization
	void Start () {
		lineRenderComponent = lineRendererGobject.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		var fRightHandInput = OVRInput.Get (OVRInput.Axis1D.SecondaryHandTrigger);
		var rightHandTrigger = 0.0f;
		RaycastHit hit;
		if (lineRenderComponent != null && 
			fRightHandInput > 0.8 && 
			Physics.Raycast (transform.position, transform.forward, out hit)
		) {
			rightHandTrigger = OVRInput.Get (OVRInput.Axis1D.SecondaryIndexTrigger);
			Debug.Log (hit.point);

			var startPoint = transform.position;
			var endPoint = hit.point;

			if (!isTeleporting) {
				lineRendererGobject.SetActive (true);
			}

			lineRenderComponent.SetPosition (0, startPoint);
			lineRenderComponent.SetPosition (1, endPoint);


			if (player != null && rightHandTrigger > 0.8) {
				if (!isTeleporting) {
					player.transform.position = new Vector3 (
						hit.point.x, 
						player.transform.position.y, 
						hit.point.z);
					isTeleporting = true;
					lineRendererGobject.SetActive (false);
				}
			} else {
				if (isTeleporting) {
					isTeleporting = false;
				}
			}

		} else {
			lineRendererGobject.SetActive (false);
			//Debug.Log ("No Line");
		}
	}

	void OnDisable() {
		if(lineRendererGobject != null)
			lineRendererGobject.SetActive (false);
	}
}
