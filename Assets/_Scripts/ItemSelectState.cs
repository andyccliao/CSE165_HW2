using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectState : MonoBehaviour {


	public Material[][] materialsCopy;
	public LinkedList<GameObject> collided;
	public Vector3 originalPos;
	public Quaternion originalRot;
	public bool canBePlaced = true;
	// Use this for initialization
	void Awake () {
		originalPos = transform.position;
		originalRot = transform.rotation;
		collided = new LinkedList<GameObject> ();
		SaveMaterials ();
	}

	void OnTriggerEnter(Collider col){
		canBePlaced = false;
		collided.AddLast (col.gameObject);
	}

	void OnTriggerExit(Collider col){
		collided.Remove (col.gameObject);

		if (collided.Count == 0) {
			canBePlaced = true;
		}
	}

	private void SaveMaterials(){
		Debug.Log ("SAVE MATERIALS");
		var original = (GetComponentsInChildren<Renderer> ());
		materialsCopy = new Material[original.Length][];
		for(var i = 0; i < original.Length; i++) {
			var materialCopy = original[i].materials;
			materialsCopy [i] = materialCopy;
		}
//		foreach (Renderer renderer in renderers) {
//			var copy = renderer.materials;
//			rendererCopy.Add(copy);
//		}
	}
	public void ResetMaterials(){
		Debug.Log ("RESETTING MATERIALS");
		var renderers = GetComponentsInChildren<Renderer> ();
		for (var i = 0; i < renderers.Length; i++) {
			var original = materialsCopy [i];
			renderers[i].materials = original;
		}
		Debug.Log ("RETTING FINISHED");
	}

	public void SetMaterial(Material mat){
		var renderers = GetComponentsInChildren<Renderer> ();

		foreach (Renderer renderer in renderers) {
			var copy = renderer.materials;
			for (var i = 0; i < copy.Length; i++) {
				copy [i] = mat;
			}
			renderer.materials = copy;
		}
	}

	public void ResetOriginalState(){
		transform.position = originalPos;
		transform.rotation = originalRot;
	}
}
