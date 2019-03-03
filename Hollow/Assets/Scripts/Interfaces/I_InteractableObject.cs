using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_InteractableObject
{
    void InRange(bool inRange);
    void Interacted();
}
