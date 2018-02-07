using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasureTool : MonoBehaviour
{
	public GameObject lLineRendererGobject;
    private LineRenderer lLineRenderComponent;
	public GameObject rLineRendererGobject;
    private LineRenderer rLineRenderComponent;
	public GameObject measureLineRendererGobject;
    private LineRenderer measureLineRenderComponent;
    public GameObject rightControllerRef;

	public GameObject leftControllerRef;
	public GameObject measureTextGo;
	private Text measureText;
	public GameObject pointer1;
	public GameObject pointer2;

	public int precision = 3;

	void Start(){
		lLineRenderComponent = lLineRendererGobject.GetComponent<LineRenderer> ();
		rLineRenderComponent = rLineRendererGobject.GetComponent<LineRenderer> ();
		measureLineRenderComponent = measureLineRendererGobject.GetComponent<LineRenderer> ();
		measureText = measureTextGo.GetComponentInChildren<Text>();
	}

	void OnDisable(){
		measureText.text = "";
	}

    // Update is called once per frame
    void Update()
    {
		var bLHandInput = OVRInput.Get (OVRInput.RawButton.LHandTrigger);
		var bRHandInput = OVRInput.Get (OVRInput.RawButton.RHandTrigger);
		RaycastHit hit;
		if (lLineRenderComponent != null &&
            bLHandInput &&
            Physics.Raycast(leftControllerRef.transform.position, leftControllerRef.transform.forward, out hit))
        {
            var lHandTrigger = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger);
            //Debug.Log (hit.point);

            var startPoint = leftControllerRef.transform.position;
            var endPoint = hit.point;

            lLineRendererGobject.SetActive(true);
            lLineRenderComponent.SetPosition(0, startPoint);
            lLineRenderComponent.SetPosition(1, endPoint);

            if (lHandTrigger)
            {
				pointer1.transform.position = hit.point;
				pointer1.SetActive(true);
            }
        } else {
			lLineRendererGobject.SetActive(false);
		}

        if (rLineRenderComponent != null &&
            bRHandInput &&
            Physics.Raycast(rightControllerRef.transform.position, rightControllerRef.transform.forward, out hit))
        {
            var rightHandTrigger = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
            //Debug.Log (hit.point);

            var startPoint = rightControllerRef.transform.position;
            var endPoint = hit.point;

            rLineRendererGobject.SetActive(true);
            rLineRenderComponent.SetPosition(0, startPoint);
            rLineRenderComponent.SetPosition(1, endPoint);

            if (rightHandTrigger)
            {
				
				pointer2.transform.position = hit.point;
				pointer2.SetActive(true);
				
            }
        } else {
			rLineRendererGobject.SetActive(false);
		}

		if(pointer1.activeSelf && pointer2.activeSelf){
			measureLineRendererGobject.SetActive (true);
			measureLineRenderComponent.SetPosition (0, pointer1.transform.position);
			measureLineRenderComponent.SetPosition (1, pointer2.transform.position);
			var dist = Vector3.Distance(pointer1.transform.position, pointer2.transform.position);
			var mag = Mathf.Pow(10, precision);
			measureText.text = ((float)((int)(mag * dist))/mag).ToString() + " meters";
			measureText.text += "\n" + ((float)((int)(mag * (dist * 3.28084)))/mag) + " feet";
		}

		if(OVRInput.GetDown(OVRInput.Button.Two)){
			measureLineRendererGobject.SetActive(false);
			pointer1.SetActive(false);
			pointer2.SetActive(false);
			measureText.text = "";
		}
    }
}
