using UnityEngine;

[CreateAssetMenu(fileName = "new Potion", menuName = "Potion")]
public class Potion : ScriptableObject
{
    public string itemName;
    public PotionEffect effect;
    public GameObject emptyPotion;
    public Sprite image;
    public int cost;
    public string description;
}
