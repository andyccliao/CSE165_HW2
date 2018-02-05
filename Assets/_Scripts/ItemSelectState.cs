using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectState : MonoBehaviour {


	public List<Material[]> rendererCopy = new List<Material[]>();
	// Use this for initialization
	void Start () {
		SaveMaterials ();
	}

	private void SaveMaterials(){
		Debug.Log ("SAVE MATERIALS");
		var renderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer renderer in renderers) {
			var copy = renderer.materials;
			rendererCopy.Add(copy);
		}
	}
	public void ResetMaterials(){
		Debug.Log ("RESETTING MATERIALS");
		var renderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer renderer in renderers) {
			var original = rendererCopy [0];
			rendererCopy.RemoveAt (0);
			renderer.materials = original;
		}
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
}
