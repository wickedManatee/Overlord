    using System.Collections;
using UnityEngine;
using System;
using Valve.VR.InteractionSystem;
using UnityEngine.AI;

public class MagnetParent : MonoBehaviour {
	public Hand hand1;
	public Hand hand2;
	public Transform holdingPosition;

	private void OnTriggerEnter(Collider other)
	{
        if (other.transform.parent != null)
        {
            if (other.transform.parent.GetComponent<MagnetChild>() != null
                && other.transform.parent.GetComponent<MagnetChild>().isMagnetized == false
                && hand1.currentAttachedObject != other.transform.parent.gameObject
                && hand2.currentAttachedObject != other.transform.parent.gameObject)
            { //if magnet falls into magnet zone, attach
                AttachObjectToMagnet(other.transform.parent.gameObject);
            }
        }
//		else {
//			string temp = String.Format("Parent holsterable? {0}\n" +
//				"Is holstered? {1}\n" +
//				"Hand1 holding {2}\n" +
//				"Hand2 holding {3}\n" +
//				"Object parent {4}",
//				other.GetComponentInParent<MagnetChild>(),
//				other.GetComponentInParent<MagnetChild>().isMagnetized,
//				hand1.currentAttachedObject,
//				hand2.currentAttachedObject,
//				other.transform.parent.gameObject);
//			hand1.ShowDebugMessage (temp);
//			hand2.ShowDebugMessage (temp);
//		}
	}

	private void OnTriggerStay(Collider other)
	{
        if (other.transform.parent != null)
        {
            if (other.transform.parent.GetComponent<MagnetChild>() != null
                && other.transform.parent.GetComponent<MagnetChild>().isMagnetized == false
                && hand1.currentAttachedObject != other.transform.parent.gameObject
                && hand2.currentAttachedObject != other.transform.parent.gameObject)
            { //if magnet let go while holding in zone, attach
                AttachObjectToMagnet(other.transform.parent.gameObject);
            }
        }
//		else {
//			string temp = String.Format("Parent holsterable? {0}\n" +
//				"Is holstered? {1}\n" +
//				"Hand1 holding {2}\n" +
//				"Hand2 holding {3}\n" +
//				"Object parent {4}",
//				other.GetComponentInParent<MagnetChild>(),
//				other.GetComponentInParent<MagnetChild>().isMagnetized,
//				hand1.currentAttachedObject,
//				hand2.currentAttachedObject,
//				other.transform.parent.gameObject);
//			hand1.ShowDebugMessage (temp);
//			hand2.ShowDebugMessage (temp);
//		}
	}

	private void OnTriggerExit(Collider other)
	{
        Debug.Log("Attempting to Demag " + other.name +
            "| hand1: " + hand1.currentAttachedObject +
            "| hand2: " + hand2.currentAttachedObject +
            "| other: " + other.transform.parent.gameObject);

        if (other.transform.parent != null)
        {
            if (other.transform.parent.GetComponent<MagnetChild>() != null
                && other.transform.parent.GetComponent<MagnetChild>().isMagnetized
                && (hand1.currentAttachedObject == other.transform.parent.gameObject
                    || hand2.currentAttachedObject == other.transform.parent.gameObject))
            { //if magnet leaves zone after being magnetized, detach
                DetachObjectFromMagnet(other.transform.parent.gameObject);
            }
        }
        else if (other.tag.Equals("Enemy"))
        {
            if (other.GetComponent<MagnetChild>() != null
            && other.GetComponent<MagnetChild>().isMagnetized
            && (hand1.currentAttachedObject == other.gameObject
                || hand2.currentAttachedObject == other.gameObject))
                //if magnet leaves zone after being magnetized, detach
                DetachObjectFromMagnet(other.gameObject);
        }
        //		else {
        //			string temp = String.Format("Parent holsterable? {0}\n" +
        //				"Is holstered? {1}\n" +
        //				"Hand1 holding {2}\n" +
        //				"Hand2 holding {3}\n" +
        //				"Object parent {4}",
        //				other.GetComponentInParent<MagnetChild>(),
        //				other.GetComponentInParent<MagnetChild>().isMagnetized,
        //				hand1.currentAttachedObject,
        //				hand2.currentAttachedObject,
        //				other.transform.parent.gameObject);
        //			hand1.ShowDebugMessage (temp);
        //			hand2.ShowDebugMessage (temp);
        //		}

    }

	public void AttachObjectToMagnet(GameObject other)
	{
		other.transform.SetParent(this.transform);
		other.transform.localPosition = holdingPosition.localPosition;
		other.GetComponent<Rigidbody> ().isKinematic = true;
		other.GetComponent<Rigidbody> ().useGravity = false;
		other.GetComponent<MagnetChild> ().isMagnetized = true;
        other.GetComponent<MagnetChild>().currentMagnetParent = transform.gameObject;
        
        if (other.GetComponent<NavMeshAgent>() != null)
        {
            other.GetComponent<NavMeshAgent>().enabled = false;
        }
        //other.GetComponentInChildren<Collider>().enabled = false;
        //animation

        //hand1.ShowDebugMessage ("Attached");
        //hand2.ShowDebugMessage ("Attached");
    }

	public void DetachObjectFromMagnet(GameObject other)
	{
		other.transform.SetParent(transform.root);
		//other.transform.localPosition = holdingPosition;
		//Quaternion equipRot = Quaternion.Euler (holdingRotation);
		//other.transform.localRotation = equipRot;
		//other.GetComponentInChildren<Collider>().enabled = true;
		other.GetComponent<Rigidbody> ().isKinematic = false;
		other.GetComponent<Rigidbody> ().useGravity = true;
		other.GetComponent<MagnetChild> ().isMagnetized = false;
        other.GetComponent<MagnetChild>().currentMagnetParent = null;

        if (hand1.currentAttachedObject == other.gameObject) {
			hand1.AttachObject (other.gameObject);
			//hand1.ShowDebugMessage ("Detached");
		} 
		else if (hand2.currentAttachedObject == other.gameObject) {
			hand2.AttachObject (other.gameObject);
			//hand2.ShowDebugMessage ("Detached");
		}

	}
}
