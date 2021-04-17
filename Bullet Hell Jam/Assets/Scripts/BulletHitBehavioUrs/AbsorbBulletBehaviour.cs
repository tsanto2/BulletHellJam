
using UnityEngine;

public class AbsorbBulletBehaviour : IBulletHitBehaviour
{

    private PlayerController pc;

    public AbsorbBulletBehaviour()
    {
        pc = GameObject.FindObjectOfType<PlayerController>();
    }

    public void Perform(GameObject bullet)
    {
        ObjectPool.Instance.ReturnObject(bullet);
        pc.Health += 1;
    }
}
