namespace Assets.Sources.Gameplay.Enemies
{
    public abstract class DamagebleEnemy : Enemy, IDamageable
    {
        public abstract void TakeDamage(ExplosionInfo explosionInfo);
    }
}
