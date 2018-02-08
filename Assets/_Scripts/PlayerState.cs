using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemSpawner))]
[RequireComponent(typeof(Teleport))]
[RequireComponent(typeof(ItemSelection))]
[RequireComponent(typeof(ItemSelection2))]
[RequireComponent(typeof(CopyPaste))]
[RequireComponent(typeof(MeasureTool))]
public class PlayerState : MonoBehaviour {

	public Teleport teleportRef;
	private ItemSpawner itemSpawner;
	private ItemSelection itemSelection;
	private ItemSelection2 itemSelection2;
	private CopyPaste copyPaste;
	private MeasureTool measureTool;
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
		measureTool = GetComponent<MeasureTool>();
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
			measureTool.enabled = false;
			SetStateText ("Teleport Mode");
			SetInstructionText ("Press hold Right Palm\nAim with Right Touch Controller\nPull right trigger to teleport");
            DeselectObjects();
			break;
		case EPlayerStates.Spawn:
			teleportRef.enabled = false;
			itemSpawner.enabled = true;
			itemSelection.enabled = false;
			itemSelection2.enabled = false;
			copyPaste.enabled = false;
			measureTool.enabled = false;
			SetStateText ("Spawn Mode");
			SetInstructionText ("Cycle through objects to spawn with A and B\nAim with Right Touch Controller\nPull right trigger to spawn that object");
            DeselectObjects();
			break;
		case EPlayerStates.Select1:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			itemSelection.enabled = true;
			itemSelection2.enabled = false;
			copyPaste.enabled = false;
			measureTool.enabled = false;
			SetStateText ("Select1 Mode");
			SetInstructionText ("Press and hold Right Palm\n" +
				"Aim with Right Touch Controller\n" + 
				"Pull right trigger to select object(s)\n " + 
				"Use right thumbstick to translate, left thumbstick to rotate\n" + 
				"Press A to Snap in place");
			break;
		case EPlayerStates.Select2:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = true;
			copyPaste.enabled = false;
			measureTool.enabled = false;
			SetStateText ("Select2 Mode");
			SetInstructionText ("Touch the object by pressing Right Palm once\n" + 
				"Translate/Rotate the object by holding trigger and moving the Right Controller\n" + 
				"Press A to Snap in place");
			break;
		case EPlayerStates.CopyPaste:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = false;
			copyPaste.enabled = true;
			measureTool.enabled = false;
			SetStateText ("CopyPaste Mode");
			SetInstructionText ("Press and hold Right Palm\nAim with Right Touch Controller\n" + 
				"Pull right trigger to copy object(s)\nUse right thumbstick to translate, left thumbstick to rotate\n" + 
				"Press A to Snap in place");
            DeselectObjects();
			break;
		case EPlayerStates.MeasuringTool:
			teleportRef.enabled = false;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = false;
			copyPaste.enabled = false;
			measureTool.enabled = true;
			SetStateText ("Measuring Mode");
			SetInstructionText ("Press and hold Right Palm and Left Palm\nAim with Left and Right Touch Controller\n" + 
				"Pull Right and Left triggers to set points\nPress B to reset points\n");
            DeselectObjects();
			break;
		default:
			teleportRef.enabled = true;
			itemSpawner.enabled = false;
			itemSelection.enabled = false;
			itemSelection2.enabled = false;
			copyPaste.enabled = false;
			measureTool.enabled = false;
			SetStateText ("Teleport Mode");
			SetInstructionText ("Press hold Right Palm\nAim with Right Touch Controller\nPull right trigger to teleport");
            DeselectObjects();
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

    void DeselectObjects()
    {
        foreach (var item in selectedObjects)
        {
            var itemSelectState = item.GetComponent<ItemSelectState>();
            /* Make itemSelectState reset materials and state when OnDestroy */
            //itemSelectState.ResetOriginalState();
            //itemSelectState.ResetMaterials();
            if (itemSelectState != null)
            {
                itemSelectState.ResetOriginalState();
                Destroy(itemSelectState);
            }
        }
        selectedObjects.Clear();
    }
}
