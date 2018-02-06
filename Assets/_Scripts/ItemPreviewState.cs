using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPreviewState : MonoBehaviour {

	public bool CanBePlaced = true;
	public Material selectedMaterial;
	public Material invalidMaterial;
	private Material[][] selectedMaterialCopy;
	private Material[][] invalidMaterialCopy;
	public LinkedList<GameObject> collided;

	void Start(){
		collided = new LinkedList<GameObject> ();
		var renderers = GetComponentsInChildren<Renderer> ();
		initInvalidMaterials (renderers);
		initValidMaterials (renderers);

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

		for (int i=0; i < renderers.Length; i++) {
			renderers[i].materials = selectedMaterialCopy[i];
		}
	}

	void SetInvalidMaterial(){
		var renderers = GetComponentsInChildren<Renderer> ();

		for (int i=0; i < renderers.Length; i++) {
			renderers[i].materials = invalidMaterialCopy[i];
		}
	}

	private void initInvalidMaterials(Renderer[] renderers){
		Debug.Log ("Initializing Invalid Materials");
		invalidMaterialCopy = new Material[renderers.Length][];
		for (var i = 0; i < renderers.Length; i++) {
			var copy = new Material [renderers [i].materials.Length];
			for (var j = 0; j < copy.Length; j++) {
				copy [j] = invalidMaterial;
			}
			invalidMaterialCopy [i] = copy;
		}
	}
	private void initValidMaterials(Renderer[] renderers){
		Debug.Log ("Initializing valid Materials");
		selectedMaterialCopy = new Material[renderers.Length][];
		for (var i = 0; i < renderers.Length; i++) {
			var copy = new Material [renderers [i].materials.Length];
			for (var j = 0; j < copy.Length; j++) {
				copy [j] = selectedMaterial;
			}
			selectedMaterialCopy [i] = copy;
		}
	}
}
