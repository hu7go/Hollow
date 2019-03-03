using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, I_InteractableObject
{
    Interact currentInteraction;
    GameObject currentOther;

    private bool interacted = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            currentOther = other.gameObject;
            InRange(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            InRange(false);
        }
    }

    public void InRange(bool inRange)
    {
        if (currentOther.GetComponent<Interact>())
        {
            currentInteraction = currentOther.GetComponent<Interact>();
            currentInteraction.inRange = inRange;
            if (inRange)
            {
                currentInteraction.IO = this;
                if (GameController.Instance.hideHints)
                {
                    return;
                }
            }
            else
            {
                currentInteraction.IO = null;
            }
        }
    }

    public void Interacted()
    {
        if (!interacted)
        {
            interacted = true;
            Nextlevel();
        }
    }

    public void Nextlevel ()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.StartNextLevel();
        else
            Debug.Log("There is currently no game manager in this scene!");
    }
}
