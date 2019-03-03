using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthReturn = 1;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            PlayerHealth player = other.gameObject.GetComponentInChildren<PlayerHealth>();
            player.HealthChange(healthReturn, false);
            player.currentHealth += healthReturn;
            Destroy(gameObject);
        }
    }
}
