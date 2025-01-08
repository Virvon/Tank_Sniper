namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyBoot : EnemyCar
    {
        protected override uint CalculateDamga(ExplosionInfo explosionInfo) =>
            explosionInfo.Damage;
    }
}
