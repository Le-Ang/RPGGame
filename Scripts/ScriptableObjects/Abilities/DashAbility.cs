using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Dash Ability", fileName = "Dash Ability")]
public class DashAbilities : GenericAbility
{
    public float dashForce;

    public override void Ability(Vector2 playerPosition, Vector2 playerFacingDirection,
        Animator playerAnimator = null, Rigidbody2D playerRigidbody = null)
    {
        if(playerMagic.initialValue >= magicCost)
        {
            playerMagic.initialValue -= magicCost;
            usePlayerMagic.Raise();
        }
        else
        {
            return;
        }
        if (playerRigidbody)
        {
            Vector3 dashVector = playerRigidbody.transform.position +
                (Vector3)playerFacingDirection.normalized * dashForce;
            playerRigidbody.DOMove(dashVector, duration);
        }
    }
}
