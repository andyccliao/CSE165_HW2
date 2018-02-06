using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection2 : MonoBehaviour {

	public GameObject leftControllerRef;
	public GameObject rightControllerRef;
	private TouchState leftControllerTouch;
	private TouchState rightControllerTouch;
	public float rotateSpeed = 100.0f;
	public float translateSpeed = 1.0f;
	public PlayerState playerState;
	private bool allowSelection = true;
	private Vector3 prevPos;
	private Quaternion prevRot;

	void Start(){
		leftControllerTouch = leftControllerRef.GetComponent<TouchState> ();
		rightControllerTouch = rightControllerRef.GetComponent<TouchState> ();
		playerState = GetComponent<PlayerState> ();
		prevPos = rightControllerRef.transform.position;
		prevRot.eulerAngles = new Vector3(0, rightControllerRef.transform.rotation.eulerAngles.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
		var fRightHandInput = OVRInput.Get (OVRInput.RawButton.RHandTrigger);
		float fRightIndexTrigger = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);


		if (fRightHandInput && rightControllerTouch.isTouching) {
			if (allowSelection) {
			var tag = rightControllerTouch.objectTouching.transform.tag;
				if (CheckTag (tag)) {
					var item = rightControllerTouch.objectTouching.GetComponent<ItemSelectState> ();
					if (item == null) { // this object has not been selected
						item = rightControllerTouch.objectTouching.AddComponent (typeof(ItemSelectState)) as ItemSelectState;
						item.enabled = true;
						item.SetValidMaterial ();
						playerState.selectedObjects.Add (rightControllerTouch.objectTouching);
					} else { // this object was previously selected
						Debug.Log ("Unselect this");
						item.ResetOriginalState ();
						var i = playerState.selectedObjects.IndexOf (rightControllerTouch.objectTouching);
						playerState.selectedObjects.RemoveAt (i);
						item.ResetMaterials ();
						Destroy (item);
					}

					allowSelection = false;
				}
			}
		} else {
			allowSelection = true;
		}

		if (playerState.selectedObjects.Count > 0) {

			if (fRightIndexTrigger > 0.8f) {
//			var leftThumbstick = OVRInput.Get (OVRInput.RawAxis2D.LThumbstick);
//			var rightThumbstick = OVRInput.Get (OVRInput.RawAxis2D.RThumbstick);
				Vector3 midLoc = Vector3.zero;
				foreach (var selected in playerState.selectedObjects) {
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

				midLoc /= playerState.selectedObjects.Count;


				Quaternion deltaRot = Quaternion.Inverse (prevRot) * rightControllerRef.transform.rotation;
				Vector3 translation = rightControllerRef.transform.position - prevPos;
				Vector3 flatTranslation = new Vector3 (translation.x, 0, translation.z);
				foreach (var selected in playerState.selectedObjects) {
					float finalRot = (deltaRot.eulerAngles.y > 180) ? deltaRot.eulerAngles.y - 360 : deltaRot.eulerAngles.y;
					selected.transform.RotateAround(midLoc, Vector3.up, finalRot * rotateSpeed);

					selected.transform.Translate (flatTranslation * translateSpeed, Space.World);
				}
			}

			// place all items only if all objects can be placed, else return all objects to original position
			if (OVRInput.Get (OVRInput.RawButton.A)) {
				bool groupPlacable = true;
				foreach (var item in playerState.selectedObjects) {
					var itemSelectState = item.GetComponent<ItemSelectState> ();
					groupPlacable = groupPlacable && itemSelectState.canBePlaced;
				}

				if (!groupPlacable) {
					foreach (var item in playerState.selectedObjects) {
						var itemSelectState = item.GetComponent<ItemSelectState> ();
						itemSelectState.ResetOriginalState ();
					}
				}
				foreach (var item in playerState.selectedObjects) {
					var itemSelectState = item.GetComponent<ItemSelectState> ();
					itemSelectState.ResetMaterials ();
					Destroy (itemSelectState);
				}
				playerState.selectedObjects.Clear();
			}
		}
		prevPos = rightControllerRef.transform.position;
		prevRot.eulerAngles = new Vector3(0, rightControllerRef.transform.rotation.eulerAngles.y, 0);
	}

	bool CheckTag(string tag){
		return tag.Equals ("deskTag") ||
		tag.Equals ("tvTag") ||
		tag.Equals ("chairTag") ||
		tag.Equals ("lockerTag") ||
		tag.Equals ("storageTag");
	}

	void OnDisable() {

	}
}
