using Spark.Gameplay.Entities.Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamage : MonoBehaviour
{
    public bool IsAttack = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!IsAttack) return;
        other.transform.GetComponent<IDamagable>()?.TakeDamage(.5f);
    }
}
