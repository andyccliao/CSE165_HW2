using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection : MonoBehaviour {

	public Material selectedMaterial;
	public Material movingMaterial;
	public Material invalidMaterial;
	public GameObject lineRendererGobject;
	private LineRenderer lineRenderComponent;
	public GameObject rightControllerRef;
	public float rotateSpeed = 100.0f;
	public float translateSpeed = 1.0f;
	public List<GameObject> selectedObjects = new List<GameObject>();
	private bool isSelecting = false;

	void Start(){
		lineRenderComponent = lineRendererGobject.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		var fRightHandInput = OVRInput.Get (OVRInput.RawAxis1D.RHandTrigger);
		var rightHandTrigger = 0.0f;
		RaycastHit hit;
		if (lineRenderComponent != null &&
		    fRightHandInput > 0.8 &&
		    Physics.Raycast (rightControllerRef.transform.position, rightControllerRef.transform.forward, out hit)) {
			rightHandTrigger = OVRInput.Get (OVRInput.RawAxis1D.RIndexTrigger);
			//Debug.Log (hit.point);

			var startPoint = rightControllerRef.transform.position;
			var endPoint = hit.point;

			lineRendererGobject.SetActive (true);
			lineRenderComponent.SetPosition (0, startPoint);
			lineRenderComponent.SetPosition (1, endPoint);

			if (rightHandTrigger > 0.8) {
				if (!isSelecting) {
					var tag = hit.transform.tag;
					isSelecting = true;
					if (CheckTag (tag)) {
						var item = hit.transform.gameObject.GetComponent<ItemSelectState> ();
						if (item == null) { // this object has not been selected
							item = hit.transform.gameObject.AddComponent (typeof(ItemSelectState)) as ItemSelectState;
							item.enabled = true;
							item.SetValidMaterial ();
							selectedObjects.Add (hit.transform.gameObject);
						} else { // this object was previously selected
							if (!item.canBePlaced)
								item.ResetOriginalState ();
							var i = selectedObjects.IndexOf(hit.transform.gameObject);
							selectedObjects.RemoveAt (i);
							item.ResetMaterials ();
							Destroy (item);
						}
					}
				}
			}else {
				if (isSelecting) {
					isSelecting = false;
				}
			}
		} else {
			lineRendererGobject.SetActive (false);
		}

		if (selectedObjects.Count > 0) {
			var leftThumbstick = OVRInput.Get (OVRInput.RawAxis2D.LThumbstick);
			var rightThumbstick = OVRInput.Get (OVRInput.RawAxis2D.RThumbstick);
			Vector3 midLoc = Vector3.zero;
			foreach (var selected in selectedObjects) {
				var selectedState = selected.GetComponent<ItemSelectState> ();
				if (!selectedState.canBePlaced)
					selectedState.SetInvalidMaterial ();
				else
					selectedState.SetValidMaterial ();
				if (midLoc == Vector3.zero) {
					midLoc = selected.transform.position;
					continue;
				}
				midLoc += selected.transform.position;
			}

			midLoc /= selectedObjects.Count;


			foreach(var selected in selectedObjects){
				selected.transform.RotateAround (midLoc, Vector3.up, rotateSpeed * leftThumbstick.x * Time.deltaTime);

				Vector3 forward3d = new Vector3(rightControllerRef.transform.forward.x, 0, rightControllerRef.transform.forward.z).normalized;
				Vector3 right3d = new Vector3 (rightControllerRef.transform.right.x, 0, rightControllerRef.transform.right.z).normalized;
				selected.transform.Translate (forward3d * rightThumbstick.y * Time.deltaTime * translateSpeed, Space.World);
				selected.transform.Translate (right3d * rightThumbstick.x * Time.deltaTime * translateSpeed, Space.World);
			}

			//
		}
	}

	bool CheckTag(string tag){
		return tag.Equals ("deskTag") ||
		tag.Equals ("tvTag") ||
		tag.Equals ("chairTag") ||
		tag.Equals ("lockerTag") ||
		tag.Equals ("storageTag");
	}

	void OnDisable() {
		if(lineRendererGobject != null)
			lineRendererGobject.SetActive (false);
	}
}
