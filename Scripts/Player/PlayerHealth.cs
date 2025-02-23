using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : GenericHealth
{
    [SerializeField] private SignalZelda healthSignal;

    public override void Damage(float amountToDamage)
    {
        base.Damage(amountToDamage);
        maxHealth.RuntimeValue = currentHealth;
        healthSignal.Raise();
    }
}
