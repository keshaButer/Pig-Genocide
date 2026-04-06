public class Bomb : Explosives
{
    protected override void OnExplode()
    {
        DealDamage();
    }
    public void Explode() => OnExplode();
}
