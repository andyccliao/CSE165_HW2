using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

	public GameObject[] itemsToSpawn;
	private GameObject itemSpawnPreview;
	int index = 0;
	public ItemManager itemManager;
	public GameObject leftControllerRef;
	public GameObject rightControllerRef;



	void Awake(){
		if (itemsToSpawn.Length > 0) {
			for (var i = 0; i < itemsToSpawn.Length; i++) {
				itemsToSpawn [i].SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (itemsToSpawn.Length > 0) {
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
			if (rightControllerRef != null &&
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
	}

	void OnDisable() {
		setItemSpawnPreviewActive (false);
	}


	void setItemSpawnPreviewActive(bool flag){
		if (itemSpawnPreview != null) {
			itemSpawnPreview.SetActive (flag);
		}
	}
}
