using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Armor", menuName = "Armor")]
public class Armor : ScriptableObject
{
	public string itemName;
    public Sprite image;
    public int armorValue;
    public int cost;
}
