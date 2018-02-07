using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	public GameObject desk;
	public GameObject chair;
	public GameObject locker;
	public GameObject storage;
	public GameObject TV;
	public int size = 100;
	private Queue deskArr;
	private Queue chairArr;
	private Queue lockerArr;
	private Queue storageArr;
	private Queue TVArr;


	// Use this for initialization
	void Awake () {
		deskArr = new Queue();
     	chairArr = new Queue();
     	lockerArr = new Queue();
     	storageArr = new Queue();
     	TVArr = new Queue();     

		for (int i = 0; i < 100; i++) {

			var deskGo = GameObject.Instantiate (desk);
			deskGo.SetActive (false);
			deskArr.Enqueue (deskGo);

			var chairGo = GameObject.Instantiate (chair);
			chairGo.SetActive (false);
			chairArr.Enqueue (chairGo);

			var lockerGo = GameObject.Instantiate (locker);
			lockerGo.SetActive (false);
			lockerArr.Enqueue (lockerGo);

			var storageGo = GameObject.Instantiate (storage);
			storageGo.SetActive (false);
			storageArr.Enqueue (storageGo);

			var tvGo = GameObject.Instantiate (TV);
			tvGo.SetActive (false);
			TVArr.Enqueue(tvGo);
		}
	}

	public void DespawnObject(GameObject gObject){
		switch (gObject.tag) {
		case "deskTag":
			deskArr.Enqueue (gObject);
			break;
		case "tvTag":
			TVArr.Enqueue (gObject);
			break;
		case "chairTag":
			chairArr.Enqueue (gObject);
			break;
		case "lockerTag":
			lockerArr.Enqueue (gObject);
			break;
		case "storageTag":
			storageArr.Enqueue (gObject);
			break;
		default:
			return;
		}

		gObject.SetActive (false);

	}

	public GameObject SpawnObject(GameObject gObject, Vector3 position, bool inheritRotation = false){
		GameObject go;
		switch (gObject.tag) {
		case "deskTag":
			go = (deskArr.Count == 0) ? null : deskArr.Dequeue () as GameObject;
			break;
		case "tvTag":
			go = (TVArr.Count == 0) ? null : TVArr.Dequeue () as GameObject;
			break;
		case "chairTag":
			go = (chairArr.Count == 0) ? null : chairArr.Dequeue () as GameObject;
			break;
		case "lockerTag":
			go = (lockerArr.Count == 0) ? null : lockerArr.Dequeue () as GameObject;
			break;
		case "storageTag":
			go = (storageArr.Count == 0) ? null : storageArr.Dequeue () as GameObject;
			break;
		default:
			return null;
		}

		if (go != null) {
			go.SetActive (true);
			go.transform.position = position;
			if(inheritRotation){
				go.transform.rotation = gObject.transform.rotation;
			}
		}

		return go;

	}
//	GameObject SpawnDesk(Vector3 position){
//		if (deskArr.Count > 0) {
//			var desk = deskArr.Dequeue () as GameObject;
//			desk.transform.position = position;
//			desk.SetActive (true);
//		} else
//			return null;
//	}
//	GameObject SpawnChair(Vector3 position){}
//	GameObject SpawnLocker(Vector3 position){}
//	GameObject SpawnStorage(Vector3 position){}
//	GameObject SpawnTV(Vector3 position){}
	
	// Update is called once per frame
	void Update () {
		
	}
}
