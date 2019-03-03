using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeBehaviour : MonoBehaviour
{
	public int health = 100;

	public void GiveDamage(int damage)
	{
		health -= damage;
	}
}
