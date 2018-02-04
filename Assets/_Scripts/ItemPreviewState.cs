using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemPreviewState : MonoBehaviour {

	public bool CanBePlaced = true;
	public Material selectedMaterial;
	public Material invalidMaterial;
	public LinkedList<GameObject> collided;

	void Start(){
		collided = new LinkedList<GameObject> ();
		//SetSelectedMaterial ();
	}

	public void Reset(){
		CanBePlaced = true;
		collided = new LinkedList<GameObject> ();
	}

	void OnTriggerEnter(Collider col){
		Debug.Log ("Colliding with " + col.gameObject.tag);
		collided.AddLast (col.gameObject);
		//SetInvalidMaterial ();
		CanBePlaced = false;
	}

	void OnTriggerExit(Collider col){
		collided.Remove (col.gameObject);

		if (collided.Count == 0) {
			//SetSelectedMaterial ();
			CanBePlaced = true;
		}
		

	}

	void SetSelectedMaterial(){
		var renderers = GetComponentsInChildren<Renderer> ();
		var newMaterials = new LinkedList<Material> ();
		foreach (Renderer renderer in renderers) {
			foreach (Material mat in renderer.materials)
				newMaterials.AddLast (selectedMaterial);
			renderer.materials = newMaterials.ToArray ();
		}
	}

	void SetInvalidMaterial(){
		var renderers = GetComponentsInChildren<Renderer> ();
		var newMaterials = new LinkedList<Material> ();
		foreach (Renderer renderer in renderers) {
			foreach (Material mat in renderer.materials)
				newMaterials.AddLast (invalidMaterial);
			renderer.materials = newMaterials.ToArray ();
		}
	}
}
