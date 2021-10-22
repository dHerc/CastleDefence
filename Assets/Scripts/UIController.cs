using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public bool towerClicked=false;
    public bool wallClicked=false;
    public bool archerClicked=false;
    public bool turretClicked=false;
    public bool barrelClicked=false;
    public bool upgradeMode = false;
    public RawImage towerImage;
    public RawImage wallImage;
    public RawImage archerImage;
    public RawImage turretImage;
    public RawImage barrelImage;
    public RawImage upgradeImage;

    public GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        weapon.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void towerButtonClicked()
    {
        if(towerClicked)
        {
            towerClicked=false;
            towerImage.color=Color.white;
        }else
        {
            towerClicked=true;
            towerImage.color=Color.green;

            wallClicked=false;
            wallImage.color=Color.white;
        }
    }

    public void wallButtonClicked()
    {
        if(wallClicked)
        {
            wallClicked=false;
            wallImage.color=Color.white;
        }else
        {
            wallClicked=true;
            wallImage.color=Color.green;

            towerClicked=false;
            towerImage.color=Color.white;
        }
    }

    public void archerButtonClicked()
    {
        if(archerClicked)
        {
            archerClicked=false;
            archerImage.color=Color.white;
        }else
        {
            archerClicked=true;
            archerImage.color=Color.green;

            turretClicked=false;
            turretImage.color=Color.white;

            barrelClicked=false;
            barrelImage.color=Color.white;
        }
    }
    public void turretButtonClicked()
    {
        if(turretClicked)
        {
            turretClicked=false;
            turretImage.color=Color.white;
        }else
        {
            turretClicked=true;
            turretImage.color=Color.green;

            archerClicked=false;
            archerImage.color=Color.white;

            barrelClicked=false;
            barrelImage.color=Color.white;
        }
    }
    public void barrelButtonClicked()
    {
        if(barrelClicked)
        {
            barrelClicked=false;
            barrelImage.color=Color.white;
        }else
        {
            barrelClicked=true;
            barrelImage.color=Color.green;

            archerClicked=false;
            archerImage.color=Color.white;

            turretClicked=false;
            turretImage.color=Color.white;
        }
    }
    public void upgradeButtonClicked()
    {
        if (upgradeMode)
        {
            upgradeMode = false;
            upgradeImage.color = Color.black;
        }
        else
        {
            upgradeMode = true;
            upgradeImage.color = Color.green;
        }
    }
    public void hideWeapon()
    {
        weapon.gameObject.SetActive(false);
    }
    public void showWeapon()
    {
        weapon.gameObject.SetActive(true);
    }
}
