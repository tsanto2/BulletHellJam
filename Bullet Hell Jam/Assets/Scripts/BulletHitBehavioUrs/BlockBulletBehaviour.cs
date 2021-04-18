using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBulletBehaviour : IBulletHitBehaviour
{

    private PlayerController pc;

    public BlockBulletBehaviour()
    {
        pc = GameObject.FindObjectOfType<PlayerController>();
    }

    public void Perform(GameObject bullet)
    {
        ObjectPool.Instance.ReturnObject(bullet);
    }
}
