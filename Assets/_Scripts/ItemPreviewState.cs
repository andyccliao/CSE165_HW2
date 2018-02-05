using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPreviewState : MonoBehaviour {

	public bool CanBePlaced = true;
	public Material selectedMaterial;
	public Material invalidMaterial;
	public LinkedList<GameObject> collided;

	void Start(){
		collided = new LinkedList<GameObject> ();
		SetSelectedMaterial ();
	}

	public void Reset(){
		CanBePlaced = true;
		collided = new LinkedList<GameObject> ();
		SetSelectedMaterial ();
	}

	void OnTriggerEnter(Collider col){
		Debug.Log ("Colliding with " + col.gameObject.tag);
		collided.AddLast (col.gameObject);
		SetInvalidMaterial ();
		CanBePlaced = false;
	}

	void OnTriggerExit(Collider col){
		collided.Remove (col.gameObject);

		if (collided.Count == 0) {
			SetSelectedMaterial ();
			CanBePlaced = true;
		}
	}

	void OnDisable(){
		Reset ();
	}

	void SetSelectedMaterial(){
		var renderers = GetComponentsInChildren<Renderer> ();

		foreach (Renderer renderer in renderers) {
			var copy = renderer.materials;
			for (var i = 0; i < copy.Length; i++) {
				copy [i] = selectedMaterial;
			}
			renderer.materials = copy;
		}
	}

	void SetInvalidMaterial(){
		var renderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer renderer in renderers) {
			var copy = renderer.materials;
			for (var i = 0; i < copy.Length; i++) {
				copy [i] = invalidMaterial;
			}
			renderer.materials = copy;
		}
	}
}
