using System.Collections.Generic;
using UnityEngine;

public class GunSwitch : MonoBehaviour {
    public int selectedGun;
    public Dictionary<int, string> selectedGuns;
    public PlayerController player; 

	// Use this for initialization
	void Start () {
        selectedGuns = new Dictionary<int, string>();
        selectedGuns.Add(0, "hands");
        selectedGuns.Add(1, "bat");
        selectedGuns.Add(2, "handgun");
        selectedGuns.Add(3, "shotgun");
        selectedGuns.Add(4, "m4");
        selectedGuns.Add(5, "sniper");
        SelectWeapon ();
    }

	void Update () {
		int prevSelectedGun = selectedGun;
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f)
		{
            if (selectedGun >= transform.childCount-1)
            {
                selectedGun = 0;
            }
            else
                selectedGun++;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0f)
		{
			if (selectedGun <= 0)
				selectedGun = transform.childCount-1;
			else 
				selectedGun--;
		}

        if (Input.GetKeyDown(KeyCode.Alpha0))
            selectedGun = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedGun = 1;
		if (Input.GetKeyDown (KeyCode.Alpha2) && transform.childCount >= 3)
			selectedGun = 2;
		if (Input.GetKeyDown (KeyCode.Alpha3) && transform.childCount >= 4)
			selectedGun = 3;
		if (Input.GetKeyDown (KeyCode.Alpha4) && transform.childCount >= 5)
			selectedGun = 4;
		if (Input.GetKeyDown (KeyCode.Alpha5) && transform.childCount >= 6)
			selectedGun = 5;

        // if the weapon has changed, update what gun is active
        int itemCount = Persistence.persistence.itemsCount[selectedGuns[selectedGun]];
        if (prevSelectedGun != selectedGun && itemCount > 0)
        {
            SelectWeapon();
            Persistence.persistence.gm.UpdateHUDStats();
        }
	}

	void SelectWeapon() {
		int i = 0;
		foreach (Transform weapon in transform) {
			if (i == selectedGun)
				weapon.gameObject.SetActive (true);
			else
				weapon.gameObject.SetActive (false);
			i++;
		}
	}

    public void DropWeapon(int slot)
    {
        if (slot == selectedGun)
        {
            SelectBestGun();
        }
    }

    void SelectBestGun()
    {
        int i = 0;
        GameObject best = null;
        int currentWep = 0;
        foreach (Transform weapon in transform)
        {
            currentWep = Persistence.persistence.itemsCount[selectedGuns[i]];
            if (currentWep > 0)
            {
                if (best != null)
                    best.SetActive(false);
                best = weapon.gameObject;
                best.SetActive(true);
                Persistence.persistence.gm.UpdateHUDStats();
            }
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
