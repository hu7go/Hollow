using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string itemName;
    public int damage;
    public int heavyAttackDamage;
    public float damageForce;
    public Sprite image;
    public int cost;
}
