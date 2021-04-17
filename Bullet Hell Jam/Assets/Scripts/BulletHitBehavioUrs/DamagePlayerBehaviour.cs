using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerBehaviour : IBulletHitBehaviour
{

    private PlayerController pc;

    public DamagePlayerBehaviour()
    {
        pc = GameObject.FindObjectOfType<PlayerController>();
    }

    public void Perform(GameObject bullet)
    {
        ObjectPool.Instance.ReturnObject(bullet);
        pc.TakeDamage(1);
    }
}
