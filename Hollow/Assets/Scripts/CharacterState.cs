using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface CharacterState
{
    CharacterState Jump();

    CharacterState Update(Transform transform);
}

/*
public class GroundedCharacterState : CharacterState
{
    public CharacterState Jump()
    {
        return new JumpCharacterState(new Vector2(0, 5));
    }
}
*/
