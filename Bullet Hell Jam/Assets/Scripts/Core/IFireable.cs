// If you have a better idea for a name for this, by all means, lemme know
public interface IFireable
{
    float ShootCooldown { get; set; }

    void Shoot(BulletPattern weapon);
}
