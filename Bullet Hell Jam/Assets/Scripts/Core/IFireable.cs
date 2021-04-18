public interface IFireable
{
    float ShootCooldown { get; set; }
    BulletSpawner Spawner { get; }
}
