using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex
{
    RedMushroom = 0,
    GreenMushroom = 1
}

public class PowerupManagerEV : MonoBehaviour
{
    // reference of all player stats affected
    public IntVariable marioJumpSpeed;
    public IntVariable marioMaxSpeed;
    public PowerupInventory powerupInventory;
    public List<GameObject> powerupIcons;

    void Start()
    {
        if (!powerupInventory.gameStarted)
        {
            powerupInventory.gameStarted = true;
            powerupInventory.Setup(powerupIcons.Count);
            resetPowerup();
        }
        else
        {
            // re-render the contents of the powerup from the previous time
            for (int i = 0; i < powerupInventory.Items.Count; i++)
            {
                Powerup p = powerupInventory.Get(i);
                if (p != null)
                {
                    AddPowerupUI(i, p.powerupTexture);
                }
            }
        }
    }
    
    public void resetPowerup()
    {
        for (int i = 0; i < powerupIcons.Count; i++)
        {
            powerupIcons[i].SetActive(false);
        }
    }
    
    void AddPowerupUI(int index, Texture t)
    {
        powerupIcons[index].GetComponent<RawImage>().texture = t;
        powerupIcons[index].SetActive(true);
    }

    public void AddPowerup(Powerup p)
    {
        powerupInventory.Add(p, (int)p.index);
        AddPowerupUI((int)p.index, p.powerupTexture);
    }

    public void OnApplicationQuit()
    {
        powerupInventory.Clear();
    }

    public void AttemptConsumePowerup(KeyCode k) {
        int index = -1;
        Powerup p = null;
        if (k == KeyCode.Z) {
            index = 0;
        }
        else if (k == KeyCode.X) {
            index = 1;
        }
        if (index != -1) {
            p = powerupInventory.Get(index);
            if (p != null) {
                powerupInventory.Remove(index);
                ConsumePowerup(p, index);
            }
        }
    }

    IEnumerator removeEffect(Powerup p, int index) {
        int intervalMultiplier = 2;
		for (int i = 0; i < p.duration*intervalMultiplier; i++) {
			powerupIcons[index].SetActive(!powerupIcons[index].activeSelf);
			yield return new WaitForSeconds(1.0f/intervalMultiplier);
		}
		powerupIcons[index].SetActive(false);
        marioMaxSpeed.ApplyChange(-p.absoluteSpeedBooster);
        marioJumpSpeed.ApplyChange(-p.absoluteJumpBooster);
	}

    void ConsumePowerup(Powerup p, int index) {
        marioMaxSpeed.ApplyChange(p.absoluteSpeedBooster);
        marioJumpSpeed.ApplyChange(p.absoluteJumpBooster);
        StartCoroutine(removeEffect(p, index));
    }
}