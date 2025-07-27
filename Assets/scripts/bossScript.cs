using UnityEngine;

public class bossScript : zombieScript
{
    protected override void Start()
    {
        health = 300f; // Más vida
        damagePerAttack = 25f; // Golpea más fuerte
        base.Start();
    }
}
