using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour {

	public bool isSelecting = false;
	public Teleport teleportRef;
	public bool spawnMode = false;
	public bool rayCast = true;
	public bool gogoHands = false;
//	public GameObject lineRendererGobject;
//	private LineRenderer lineRenderComponent;
	public GameObject leftControllerRef;
	public GameObject rightControllerRef;
	public ItemManager itemManager;

	public GameObject[] itemsToSpawn;
	int index = 0;

	private GameObject itemSpawnPreview;

	public Text textComponent;

	void Awake(){
		if (itemsToSpawn.Length > 0) {
			for (var i = 0; i < itemsToSpawn.Length; i++) {
				itemsToSpawn [i].SetActive (false);
			}
		}
	}

	void Start(){
//		lineRenderComponent = lineRendererGobject.GetComponent<LineRenderer> ();
	}
	// Update is called once per frame
	void Update () {

		// tap Y to toggle between selection mode and teleport mode
		if (OVRInput.GetDown (OVRInput.Button.Four)) {
			isSelecting = !isSelecting;
			Debug.Log (isSelecting);
		}

		// set teleport mode to on or off
		if (teleportRef != null ){//&& lineRendererGobject != null) {
//			lineRendererGobject.SetActive (!isSelecting);
			teleportRef.enabled = !isSelecting;
		}

		//if we are selecting
		if (isSelecting) {

			// tap X to toggle between spawn mode and regular selection
			if (OVRInput.GetDown (OVRInput.Button.Three)) {
				spawnMode = !spawnMode;
			}
				
			//if we are not in spawn mode
			if (!spawnMode) {
				// hide the spawn preview
				setItemSpawnPreviewActive (false);

				// tap A to select RayCast Selection Mode
				if (OVRInput.GetDown (OVRInput.Button.One)) {
					rayCast = true;
					gogoHands = false;
				// tap B to select gogoHands Selection Mode
				} else if (OVRInput.GetDown (OVRInput.Button.Two)) {
					rayCast = false;
					gogoHands = true;
				}
			}

			// if we are in spawn mode
			if (spawnMode && itemsToSpawn.Length > 0) {
				//select itemSpawn Preview
				itemSpawnPreview = itemsToSpawn [index];

				//detect whether or not the user wants to view next or previous item
				if (OVRInput.GetDown (OVRInput.Button.One)) {
					index = (index - 1 + itemsToSpawn.Length) % itemsToSpawn.Length;
					itemSpawnPreview.SetActive (false);
					itemSpawnPreview = itemsToSpawn [index];
				} else if (OVRInput.GetDown (OVRInput.Button.Two)) {
					index = (index + 1) % itemsToSpawn.Length;
					itemSpawnPreview.SetActive (false);
					itemSpawnPreview = itemsToSpawn [index];
				}
				//set the current previewed item active
				itemSpawnPreview.SetActive (true);

				// manipulate preview to be in position of where we want item to spawn
				RaycastHit hit;
//				if (lineRenderComponent != null && 
				if(	rightControllerRef != null && 
					Physics.Raycast (rightControllerRef.transform.position, rightControllerRef.transform.forward, out hit)) {

					if (Vector3.Distance (rightControllerRef.transform.position, hit.point) <= 5.0f) {
						itemSpawnPreview.transform.position = new Vector3 (
							hit.point.x, 
							itemSpawnPreview.transform.position.y,
							hit.point.z
						);

						var itemPreviewState = itemSpawnPreview.GetComponent<ItemPreviewState> ();
						if (itemPreviewState != null) {
							if (itemPreviewState.CanBePlaced) {
								if (OVRInput.GetDown (OVRInput.Button.SecondaryThumbstick)) {
									itemManager.SpawnObject (itemSpawnPreview, itemPreviewState.transform.position);
								} 
							}
						}
					}
				}
			} 
				
		} else {
			setItemSpawnPreviewActive (false);
		}

		SetText ();
	}

	void setItemSpawnPreviewActive(bool flag){
		if (itemSpawnPreview != null) {
			itemSpawnPreview.SetActive (flag);
		}
	}

	void SetText(){
		if (textComponent != null) {
			if (!isSelecting) {
				textComponent.text = "Teleport Mode";
			} else {
				if (!spawnMode) {
					if (rayCast) {
						textComponent.text = "RayCast Select";
					} else if (gogoHands) {
						textComponent.text = "GoGo-Hands Select";
					}
				} else if (spawnMode) {
					textComponent.text = "Spawn Mode";
				}
			}
		}
	}
}
