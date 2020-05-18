using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRInput : MonoBehaviour {

	public enum bindings {VRGripRight,	VRGripLeft,
		VRTriggerRight,	VRTriggerLeft,
		VRA,	VRB,		VRX,	VRY,
		VRStickClickLeft,	VRStickClickRight,
		VRHorizontalLeft,	VRVerticalLeft,
		VRHorizontalRight,	VRVerticalRight,
		VRMenu
	}
	string VRGripPrimary;
	string VRGripSecondary;
	string VRTriggerPrimary;
	string VRTriggerSecondary;
	string VRSubmit;
	string VRCancel;
	string VRLastUsed;
	string VRShowPlayer;
	string VRTeleport1;
	string VRTeleport2;
	string VRStickAxisX1;
	string VRStickAxisY1;
	string VRStickAxisX2;
	string VRStickAxisY2;
	string VRMenu;

	public void LoadControlMappings(string mappingDefault)
	{
		if (string.IsNullOrEmpty (mappingDefault))
			mappingDefault = "oculusR";
		ChangeControlMappings (mappingDefault);
	}
	public void ChangeControlMappings(string newMapping)
	{
//		if (Persistence.persistence.controlSettings == newMapping)
//			return;
//		else
//			Persistence.persistence.controlSettings = newMapping;
		
		switch (newMapping) {
		case ("oculusL"):
			VRGripPrimary = "VRGripLeft";
			VRGripSecondary = "VRGripRight";
			VRTriggerPrimary = "VRTriggerLeft";
			VRTriggerSecondary = "VRTriggerRight";
			VRSubmit = "VRX";
			VRCancel = "VRY";
			VRLastUsed = "VRA";
			VRShowPlayer = "VRB";
			VRTeleport1 = "VRStickClickLeft";
			VRTeleport2= "VRStickClickRight";
			VRStickAxisX1= "VRHorizontalLeft";
			VRStickAxisY1= "VRVerticalLeft";
			VRStickAxisX2= "VRHorizontalRight";
			VRStickAxisY2= "VRVerticalRight";
			VRMenu= "VRMenu";
			break;
		case("oculusR"):
			LoadDefaultMappings ();
			break;
		case("viveL"):
			break;
		case("viveR"):
			break;
		default:
			Debug.Log ("Mapping unrecognized");
			LoadDefaultMappings ();
			break;
		}
		PlayerPrefs.SetString ("mappingDefault", newMapping);
	}

	//public EVRButtonId GetButtonId(string buttonMapping)
	//{
	//	switch (buttonMapping) {
	//	case (bindings.VRGripLeft.ToString()):
	//		return EVRButtonId.k_EButton_Grip;
	//		break;
	//	case (bindings.VRGripSecondary.ToString()):
	//		return EVRButtonId.k_EButton_Grip;
	//		break;
	//	case (bindings.VRTriggerPrimary.ToString()):
	//		return EVRButtonId.k_EButton_SteamVR_Trigger;
	//		break;
	//	case (bindings.VRTriggerSecondary.ToString()):
	//		return EVRButtonId.k_EButton_SteamVR_Trigger;
	//		break;
	//	case (bindings.VRSubmit.ToString()):
	//		return EVRButtonId.k_EButton_SteamVR_Trigger;
	//		break;
	//	}
	//}

	private void LoadDefaultMappings(){
		VRGripPrimary = "VRGripRight";
		VRGripSecondary = "VRGripLeft";
		VRTriggerPrimary = "VRTriggerRight";
		VRTriggerSecondary = "VRTriggerLeft";
		VRSubmit = "VRA";
		VRCancel = "VRB";
		VRLastUsed = "VRX";
		VRShowPlayer = "VRY";
		VRTeleport1 = "VRStickClickRight";
		VRTeleport2= "VRStickClickLeft";
		VRStickAxisX1= "VRHorizontalRight";
		VRStickAxisY1= "VRVerticalRight";
		VRStickAxisX2= "VRHorizontalLeft";
		VRStickAxisY2= "VRVerticalLeft";
		VRMenu= "VRMenu";
	}
}
