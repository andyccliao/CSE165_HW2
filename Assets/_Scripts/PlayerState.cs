using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemSpawner))]
[RequireComponent(typeof(Teleport))]
[RequireComponent(typeof(ItemSelection))]
[RequireComponent(typeof(ItemSelection2))]
[RequireComponent(typeof(CopyPaste))]
public class PlayerState : MonoBehaviour {

	public Teleport teleportRef;
	private ItemSpawner itemSpawner;
	private ItemSelection itemSelection;
	private ItemSelection2 itemSelection2;
	private CopyPaste copyPaste;
	public List<GameObject> selectedObjects = new List<GameObject>();
	public List<GameObject> selectedToCopyObjects = new List<GameObject>();
	public Text stateTextComponent;
	public Text instructionTextComponent;

	private EPlayerStates playerState;

	enum EPlayerStates {
		Teleport = 0,
		Spawn = 1,
		Select1 = 2,
		Select2 = 3,
		CopyPaste = 4,
		MeasuringTool = 5,
		SIZE = 6 // MAKE SURE THIS IS AT THE END
	};

	void Start(){
		playerState = EPlayerStates.Teleport;
		itemSpawner = GetComponent<ItemSpawner>();
		itemSelection = GetComponent<ItemSelection> ();
		itemSelection2 = GetComponent<ItemSelection2> ();
		copyPaste = GetComponent<CopyPaste>();
		itemSpawner.enabled = false;
	}
	// Update is called once per frame
	void Update () {

		// tap Y to toggle between selection mode and teleport mode
		if (OVRInput.GetDown (OVRInput.RawButton.Y)) {
			playerState = (EPlayerStates)(((int)(((int)playerState) + 1)) % ((int)EPlayerStates.SIZE));
		} else if (OVRInput.GetDown (OVRInput.RawButton.X)) {
			playerState = (EPlayerStates)(((int)(((int)playerState) - 1) + (int)EPlayerStates.SIZE) % ((int)EPlayerStates.SIZE));
		}

		switch (playerState) {
		case EPlayerStates.Teleport:
			teleportRef.enabled = true;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = false;
			copyPaste.enabled = false;
			SetStateText ("Teleport Mode");
			SetInstructionText ("Press hold Right Palm\nAim\nPull right trigger to teleport");
			break;
		case EPlayerStates.Spawn:
			teleportRef.enabled = false;
			itemSpawner.enabled = true;
			itemSelection.enabled = false;
			itemSelection2.enabled = false;
			copyPaste.enabled = false;
			SetStateText ("Spawn Mode");
			break;
		case EPlayerStates.Select1:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			itemSelection.enabled = true;
			itemSelection2.enabled = false;
			copyPaste.enabled = false;
			SetStateText ("Select1 Mode");
			break;
		case EPlayerStates.Select2:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = true;
			copyPaste.enabled = false;
			SetStateText ("Select2 Mode");
			break;
		case EPlayerStates.CopyPaste:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = false;
			copyPaste.enabled = true;
			SetStateText ("CopyPaste Mode");
			break;
		case EPlayerStates.MeasuringTool:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = true;
			copyPaste.enabled = false;
			SetStateText ("Measuring Mode");
			break;
		default:
			teleportRef.enabled = true;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = false;
			copyPaste.enabled = false;
			SetStateText ("Teleport Mode");
			break;
		}
			
	}


	void SetStateText(string text){
		if (stateTextComponent != null) {
			stateTextComponent.text = text;
		}
	}

	void SetInstructionText(string text){
		if (instructionTextComponent != null) {
			instructionTextComponent.text = text;
		}
	}
}
