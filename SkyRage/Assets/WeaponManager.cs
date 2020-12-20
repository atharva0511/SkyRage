
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public byte vehicleIndex = 0;
    

    public Weapons[] miniguns;
    public Weapons homingLauncher;
    public Weapons stunGun;
    public Weapons laserGun;
    public Weapons[] weapons;

    public bool hasMG = true;
    public bool hasHL = true;
    public bool hasSG = true;
    public bool hasLG = true;
    public bool dead = false;

    public GameObject minigunIcon;
    public GameObject laserGunIcon;
    public GameObject stunGunIcon;
    public GameObject homingIcon;

    public int maxRockets = 6;
    public int rockets = 6;
    public bool infiniteRocket = false;

    [Header("UI")]
    public GameObject fire1Button;
    public GameObject fire2Button;
    public GameObject swap1Button;
    public GameObject swap2Button;
    // Use this for initialization
    void Awake()
    {
        // check possessions
    }

	void Start () {
        //uncomment all while building 
        fire1Button.SetActive(false);
        fire2Button.SetActive(false);
        swap1Button.SetActive(true);
        swap2Button.SetActive(true);
        SetPossessions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Swap1()
    {
        ReleaseFire1();
        if (miniguns[0].equipped == true)
        {
            miniguns[0].equipped = false; miniguns[1].equipped = false;
            laserGun.equipped = true;
            laserGunIcon.SetActive(true);
            minigunIcon.SetActive(false);
        }
        else if(laserGun.equipped == true)
        {
            laserGun.equipped = false;
            miniguns[0].equipped = true; miniguns[1].equipped = true;
            minigunIcon.SetActive(true);
            laserGunIcon.SetActive(false);
        }
    }

    public void Swap2()
    {
        ReleaseFire2();
        if(homingLauncher.equipped == true)
        {
            homingLauncher.equipped = false;
            stunGun.equipped = true;
            stunGunIcon.SetActive(true);
            homingIcon.SetActive(false);
        }
        else if(stunGun.equipped == true)
        {
            stunGun.equipped = false;
            homingLauncher.equipped = true;
            stunGunIcon.SetActive(false);
            homingIcon.SetActive(true);
        }
    }

    public void SetPossessions()
    {
        switch (vehicleIndex)
        {
            case 0:
                hasMG = Upgrades.qDroneWeapons[0];
                hasHL = Upgrades.qDroneWeapons[1];
                hasSG = Upgrades.qDroneWeapons[2];
                hasLG = Upgrades.qDroneWeapons[3];
                if (PlayerData.levelProgression[2] >= 2)
                {
                    hasMG = true;
                }
                break;
            case 1:
                hasMG = Upgrades.hodWeapons[0];
                hasHL = Upgrades.hodWeapons[1];
                hasSG = Upgrades.hodWeapons[2];
                hasLG = Upgrades.hodWeapons[3];
                if (PlayerData.levelProgression[2] >= 1)
                {
                    hasMG = true;
                }
                break;
            case 2:
                hasMG = Upgrades.wDroneWeapons[0];
                hasHL = Upgrades.wDroneWeapons[1];
                hasSG = Upgrades.wDroneWeapons[2];
                hasLG = Upgrades.wDroneWeapons[3];
                if (PlayerData.levelProgression[2] >= 1)
                {
                    hasMG = true;
                }
                break;
            case 3:
                hasMG = Upgrades.slayerXWeapons[0];
                hasHL = Upgrades.slayerXWeapons[1];
                hasSG = Upgrades.slayerXWeapons[2];
                hasLG = Upgrades.slayerXWeapons[3];
                if (PlayerData.levelProgression[2] >= 1)
                {
                    hasMG = true;
                }
                break;
            default: break;
        }

        if (hasMG) { miniguns[0].gameObject.SetActive(true); miniguns[1].gameObject.SetActive(true); fire1Button.SetActive(true); }
        else { miniguns[0].gameObject.SetActive(false); miniguns[1].gameObject.SetActive(false); swap1Button.SetActive(false); }

        if (hasLG) { laserGun.gameObject.SetActive(true); fire1Button.SetActive(true); }
        else { laserGun.gameObject.SetActive(false); swap1Button.SetActive(false); }

        if (hasSG) {stunGun.gameObject.SetActive(true); fire2Button.SetActive(true); }
        else { stunGun.gameObject.SetActive(false); swap2Button.SetActive(false); }

        if (hasHL) { homingLauncher.gameObject.SetActive(true); fire2Button.SetActive(true); }
        else { homingLauncher.gameObject.SetActive(false); swap2Button.SetActive(false); }

        if (hasMG)
        {
            miniguns[0].equipped = true; miniguns[1].equipped = true;
            laserGun.equipped = false;
            minigunIcon.SetActive(true);
            laserGunIcon.SetActive(false);
        }
        else if (hasLG)
        {
            laserGun.equipped = true;
            miniguns[0].equipped = false; miniguns[1].equipped = false;
            laserGunIcon.SetActive(true);
            minigunIcon.SetActive(false);
        }

        if (hasHL)
        {
            homingLauncher.equipped = true;
            stunGun.equipped = false;
            stunGunIcon.SetActive(false);
            homingIcon.SetActive(true);
        }
        else if (hasSG)
        {
            stunGun.equipped = true;
            homingLauncher.equipped = false;
            stunGunIcon.SetActive(true);
            homingIcon.SetActive(false);
        }
    }


    public void PressedFire1()
    {
        if (dead) return;
        foreach (Weapons w in weapons)
        {
            w.PressedFire1();
        }
    }
    public void ReleaseFire1()
    {
        if (dead) return;
        foreach (Weapons w in weapons)
        {
            w.ReleaseFire1();
        }
    }
    public void PressedFire2()
    {
        if (dead) return;
        foreach (Weapons w in weapons)
        {
            w.PressedFire2();
        }
    }
    public void ReleaseFire2()
    {
        if (dead) return;
        foreach (Weapons w in weapons)
        {
            w.ReleaseFire2();
        }
    }

    public void UpdateDisp()
    {
        homingLauncher.UpdateDisp();
    }
}
