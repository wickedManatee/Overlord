using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {
	public GameObject player;
    public int amount;
	public Items.iType itemType;
	public Items.hType healthType;
	public Items.arType armorType;
	public Items.amType ammoType;
	public Items.gType gunType;
	public Items.oType objType;

	void OnTriggerEnter(Collider other)
	{
		if (other == player.GetComponent<Collider>()) {
			if (itemType == Items.iType.health) {
                Persistence.persistence.gm.player.AddHealth(healthType.ToString());
                Persistence.persistence.gm.UpdateHUDStats();
            }
			else if (itemType == Items.iType.armor) {
                Persistence.persistence.gm.player.AddArmor(armorType.ToString());
                Persistence.persistence.gm.UpdateHUDStats();
            }
			else if (itemType == Items.iType.gun) {
                Persistence.persistence.gm.invController.AddItem(gunType.ToString(), 1);
			}
			else if (itemType == Items.iType.ammo) {
                Persistence.persistence.gm.invController.AddItem(ammoType.ToString(), amount);
			}
			else if (itemType == Items.iType.objective) {
                Persistence.persistence.gm.invController.AddItem(objType.ToString(), 1);
            }
			Destroy(this.gameObject);
		}
	}
}
