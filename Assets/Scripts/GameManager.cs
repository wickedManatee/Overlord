using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour {
	public PlayerController player;
    public InventoryController invController;
    public int playerStartingHealth;
    public int playerStartingArmor;

    public Hand hand1;
    public Hand hand2;

    public int currentScore;
	public GameObject objectives;
	public int currentObjective;
	public GameObject[] playerCanvasTexts;
    GameObject[] hud;
    public int overlordPointsPerSecond;

	public bool showDebugInventory;
	private TextMesh debugText;
    iDroidManager droidMgr;

	// Use this for initialization
	void Start () {
		Persistence.persistence.gm = this;
		currentObjective = 0;
		AssignCurrentObjective ();
        hud = GameObject.FindGameObjectsWithTag("PlayerHUD");
        AssignStartingHUD();
        droidMgr = GameObject.FindObjectOfType<iDroidManager>();
        InvokeRepeating("AddTimerPoints", 5f, 10f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		InvokeRepeating( "DebugInventory", 1f, 1f );
	}

    void AssignStartingHUD()
    {
        player.AddHealth(playerStartingHealth);
        player.AddArmor(playerStartingArmor);

        UpdateHUDStats();
    }

    public void UpdateHUDStats()
    {
        Items currentWep = player.GetCurrentWeapon();
        foreach (GameObject go in hud)
        {
            if (go.name.Contains("Health"))
                go.GetComponent<Text>().text = player.GetHealth().ToString();
            else if (go.name.Contains("Armor"))
                go.GetComponent<Text>().text = player.GetArmor().ToString();
            else if (go.name.Contains("Bullets"))
            {
                if (currentWep != null)
                    go.GetComponent<Text>().text = currentWep.GetBullets().ToString();
            }
            else if (go.name.Contains("Reserve"))
            {
                if (currentWep != null && currentWep.itemName != "hands" && currentWep.itemName != "bat")
                    go.GetComponent<Text>().text = Persistence.persistence.itemsCount[string.Concat(currentWep.itemName,"Ammo")].ToString();
            }
        }
    }

    void AssignCurrentObjective() {
		playerCanvasTexts.Where(x=> x.name == "txtObjective").FirstOrDefault().GetComponent<Text> ().text = objectives.transform.GetChild(currentObjective).GetComponent<ObjectiveController>().objText;
	}

	public void FinishObjective(){
		currentObjective+= 1;
		AssignCurrentObjective ();
        droidMgr.UpdateBankLabel(objectives.transform.GetChild(currentObjective).GetComponent<ObjectiveController>().objPoints);
	}

    private void AddTimerPoints()
    {
        droidMgr.UpdateBankLabel(overlordPointsPerSecond);
    }

	private void DebugInventory()
	{
		if (showDebugInventory) {
			if (debugText == null) {
				debugText = new GameObject ("_debug_text").AddComponent<TextMesh> ();
				debugText.fontSize = 120;
				debugText.characterSize = 0.01f;
				debugText.transform.parent = player.transform;

				debugText.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
			} else {
				debugText.text = "Inventory: \n";
			}


			debugText.transform.localPosition = new Vector3( 0.05f, 2.0f, 0.0f );
			debugText.alignment = TextAlignment.Left;
			debugText.anchor = TextAnchor.UpperLeft;

            foreach (KeyValuePair<int, Items> item in invController.inventory)
            {
                Items current = ((Items)item.Value);
                debugText.text = debugText.text +
                    "Item ["+current.inventorySlot+"]: " + current.itemName + "-" + Persistence.persistence.itemsCount[current.itemName]+ "\n";
            }
		}
		else
		{
			if ( debugText != null )
			{
				Destroy( debugText.gameObject );
			}
		}
	}
}
