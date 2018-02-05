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

	public List<GameObject> selectedObjects = new List<GameObject>();
	public List<Material> originalMaterial = new List<Material>();
	private bool isSelecting = false;

	void Start(){
		lineRenderComponent = lineRendererGobject.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		var fRightHandInput = OVRInput.Get (OVRInput.Axis1D.SecondaryHandTrigger);
		var rightHandTrigger = 0.0f;
		RaycastHit hit;
		if (lineRenderComponent != null &&
		    fRightHandInput > 0.8 &&
		    Physics.Raycast (rightControllerRef.transform.position, rightControllerRef.transform.forward, out hit)) {
			rightHandTrigger = OVRInput.Get (OVRInput.Axis1D.SecondaryIndexTrigger);
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
							item.SetMaterial (selectedMaterial);
							selectedObjects.Add (hit.transform.gameObject);
						} else { // this object was previously selected
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
