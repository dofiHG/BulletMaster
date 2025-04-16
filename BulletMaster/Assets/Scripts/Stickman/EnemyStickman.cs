using UnityEngine;

public class EnemyStickman : Stickman
{
    [SerializeField] private int _gunNumber;

    private void Start()
    {
        Setup();
        GiveWeapon(_gunNumber);
    }
}
