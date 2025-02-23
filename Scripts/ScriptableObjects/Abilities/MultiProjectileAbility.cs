using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Multi-Projectile Ability", fileName = "New Multi Projectile Ability")]

public class MultiProjectileAbility : GenericAbility
{
    [SerializeField] private GameObject thisProjectile;
    [SerializeField] private int numberOfProjectiles;
    [SerializeField] private float projectileSpread;

    public override void Ability(Vector2 playerPosition, Vector2 playerFacingDirection,
        Animator playerAnimator = null, Rigidbody2D playerRigidbody = null)
    {
        float facingRotation = Mathf.Atan2(playerFacingDirection.y, 
            playerFacingDirection.x) * Mathf.Rad2Deg;
        float startRotation = facingRotation + projectileSpread / 2f;
        float angleIncrease = projectileSpread / ((float)numberOfProjectiles - 1f);
        
        for(int i = 0; i < numberOfProjectiles; i++)
        {
            float tempRotation = startRotation - angleIncrease * i;
            GameObject newProjectile = Instantiate(thisProjectile, playerPosition,
            Quaternion.Euler(0f, 0f, tempRotation));
            GenericProjectile temp = newProjectile.GetComponent<GenericProjectile>();
            if (temp)
            {
                temp.Setup(new Vector2(Mathf.Cos(tempRotation * Mathf.Deg2Rad),
                    Mathf.Sin(tempRotation * Mathf.Deg2Rad)));
            }
        }
        
    }
}
