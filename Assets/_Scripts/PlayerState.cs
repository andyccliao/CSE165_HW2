using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemSpawner))]
[RequireComponent(typeof(Teleport))]
public class PlayerState : MonoBehaviour {

//	public bool isSelecting = false;
	public Teleport teleportRef;
//	public bool spawnMode = false;
//	public bool rayCast = true;
//	public bool gogoHands = false;
//	public GameObject lineRendererGobject;
//	private LineRenderer lineRenderComponent;

	private ItemSpawner itemSpawner;

	public Text textComponent;

	private EPlayerStates playerState;

	enum EPlayerStates {
		Teleport = 0,
		Spawn = 1,
		Select1 = 2,
		Select2 = 3,
		MeasuringTool = 4,
		SIZE = 5 // MAKE SURE THIS IS AT THE END
	};

	void Start(){
		playerState = EPlayerStates.Teleport;
//		lineRenderComponent = lineRendererGobject.GetComponent<LineRenderer> ();
		itemSpawner = GetComponent<ItemSpawner>();
		itemSpawner.enabled = false;
	}
	// Update is called once per frame
	void Update () {

		// tap Y to toggle between selection mode and teleport mode
		if (OVRInput.GetDown (OVRInput.Button.Four)) {
			playerState = (EPlayerStates)(((int)(((int)playerState) + 1)) % ((int)EPlayerStates.SIZE));
		}

		switch (playerState) {
		case EPlayerStates.Teleport:
			teleportRef.enabled = true;
			itemSpawner.enabled = false;
			SetText ("Teleport Mode");
			break;
		case EPlayerStates.Spawn:
			teleportRef.enabled = false;
			itemSpawner.enabled = true;
			SetText ("Spawn Mode");
			break;
		case EPlayerStates.Select1:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			SetText ("Select1 Mode");
			break;
		case EPlayerStates.Select2:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			SetText ("Select2 Mode");
			break;
		case EPlayerStates.MeasuringTool:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			SetText ("Measuring Mode");
			break;
		default:
			teleportRef.enabled = true;
			itemSpawner.enabled = false;
			SetText ("Teleport Mode");
			break;
		}
			
	}


	void SetText(string text){
		if (textComponent != null) {
			textComponent.text = text;
		}
	}
}
