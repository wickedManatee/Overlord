using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandController : MonoBehaviour {

	public Hand hand;

	[EnumFlags]
	[Tooltip( "The flags used to attach this object to the hand." )]
	public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

	[Tooltip( "Name of the attachment transform under in the hand's hierarchy which the object should should snap to." )]
	public string attachmentPoint;

	public bool attachEaseIn;

	public GameObject enemyPrefab;

	void Awake()
	{
		if ( attachEaseIn )
		{
			attachmentFlags &= ~Hand.AttachmentFlags.SnapOnAttach;
		}
	}

	void Update () {
		
		if (hand == null || hand.controller == null)
			return;
		
		if (hand.controller.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_Axis2) && IsHandInSpawningZone())
		{
			SpawnEnemy ();
		}
	}

	void SpawnEnemy()
	{
		//TODO: replace zombie with Persistance.current enemy selected
		GameObject newEnemy = Instantiate(enemyPrefab);
		hand.AttachObject( gameObject, Hand.AttachmentFlags.SnapOnAttach);
		newEnemy.name = "EnemyClone";
	}

	bool IsHandInSpawningZone() {
		return true;
		//TODO
	}
}
