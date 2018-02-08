using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectState : MonoBehaviour {


	public Material[][] materialsCopy;
	private Material[][] validMaterialCopy;
	private Material[][] invalidMaterialCopy;
	public Material validMaterial;
	public Material invalidMaterial;
	public List<GameObject> collided;
	public Vector3 originalPos;
	public Quaternion originalRot;
	public bool canBePlaced = true;
	// Use this for initialization
	void Awake () {
		validMaterial = Resources.Load ("Materials/SelectedMaterial") as Material;
		invalidMaterial = Resources.Load ("Materials/BadMaterial") as Material;
		Debug.Log (validMaterial);
		Debug.Log (invalidMaterial);
		originalPos = transform.position;
		originalRot = transform.rotation;
		collided = new List<GameObject> ();
		SaveMaterials ();
	}

	void OnTriggerEnter(Collider col){
		Debug.Log (col.tag);
		canBePlaced = false;
		if(/*!collided.Contains(col.gameObject)*/ !col.CompareTag("Floor"))
			collided.Add (col.gameObject);
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
		initValidMaterials ();
		initInvalidMaterials ();
	}

	private void initInvalidMaterials(){
		Debug.Log ("Initializing Invalid Materials");
		invalidMaterialCopy = new Material[materialsCopy.Length][];
		for (var i = 0; i < materialsCopy.Length; i++) {
			var copy = new Material [materialsCopy [i].Length];
			for (var j = 0; j < copy.Length; j++) {
				copy [j] = invalidMaterial;
			}
			invalidMaterialCopy [i] = copy;
		}
	}
	private void initValidMaterials(){
		Debug.Log ("Initializing valid Materials");
		validMaterialCopy = new Material[materialsCopy.Length][];
		for (var i = 0; i < materialsCopy.Length; i++) {
			var copy = new Material [materialsCopy [i].Length];
			for (var j = 0; j < copy.Length; j++) {
				copy [j] = validMaterial;
			}
			validMaterialCopy [i] = copy;
		}
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

	public void SetInvalidMaterial(){
		var renderers = GetComponentsInChildren<Renderer> ();

		for (var i = 0; i < renderers.Length; i++) {
			
			renderers[i].materials = invalidMaterialCopy[i];

		}
	}

	public void SetValidMaterial(){
		var renderers = GetComponentsInChildren<Renderer> ();

		for (var i = 0; i < renderers.Length; i++) {

			renderers[i].materials = validMaterialCopy[i];

		}
	}

	public void ResetOriginalState(){
		transform.position = originalPos;
		transform.rotation = originalRot;
	}

    public void OnDestroy()
    {
        ResetMaterials();
    }
}
