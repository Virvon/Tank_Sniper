namespace Assets.Sources.Gameplay.Bullets
{
    public abstract class Laser : ExplodingBullet
    {
        public virtual Laser BindLifeTimes(float explosionLifeTime, float projectileLifeTime)
        {
            Destroy(gameObject, explosionLifeTime);
            Destroy(Projectile, projectileLifeTime);

            return this;
        }
    }
}
