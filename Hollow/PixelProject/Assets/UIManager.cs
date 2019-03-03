using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Canvas mainCanvas;

    public PlayerHealth playerHealth;
    public Armor playerArmor;
    public List<GameObject> hpPoints;
    public List<GameObject> armorPoints;

    public GameObject armorPoint;

    private int currentHealth;

    private void Start()
    {
        currentHealth = hpPoints.Count;
    }

    public void SetArmor(int armorValue)
    {
        for (int i = 0; i < armorValue; i++)
        {
            GameObject newArmorPoint = Instantiate(armorPoint, mainCanvas.transform);
           
            newArmorPoint.GetComponent<RectTransform>().anchoredPosition = new Vector3(70 - (-i * 50),-150,0);
            armorPoints.Add(newArmorPoint);
        }
    }

    public void ArmorDamage(int damageAmount)
    {
        //Remove points of armor equal to the damage dealt
        for (int i = 0; i < damageAmount; i++)
        {
            armorPoints[armorPoints.Count - 1].GetComponent<ArmorPoint>().GotRemoved();
            armorPoints.Remove(armorPoints[armorPoints.Count - 1]);
        }
    }

    public void HealthChange(int health, bool damage = true)
    {
        if (damage)
        {
            for (int i = 0; i < health; i++)
            {
                hpPoints[currentHealth - 1].SetActive(false);
                currentHealth -= 1;
            }
        }
        else
        {
            for (int i = 0; i < hpPoints.Count; i++)
            {
                if (!hpPoints[i].activeInHierarchy)
                {
                    hpPoints[i].SetActive(true);
                    currentHealth += 1;
                    return;
                }
            }
        }
    }
}
