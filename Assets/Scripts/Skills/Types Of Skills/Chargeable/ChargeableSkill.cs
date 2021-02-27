using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChargeableSkill : Active
{
    [SerializeField] protected float minDamage = 0f;
    [SerializeField] protected float maxDamage = 0f;
    [SerializeField] private float chargingRate = 0f;
    private float chargeAmount = 0f;

    protected float ChargeAmount { get => chargeAmount; set => chargeAmount = Mathf.Clamp(value, 0, 100); }
    protected float ChargingRate { get => chargingRate; set => chargingRate = value; }

    public abstract bool OnCharging ();
    public abstract bool OnEndCharging ();
}
