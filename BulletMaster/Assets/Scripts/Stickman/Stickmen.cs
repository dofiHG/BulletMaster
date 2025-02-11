using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    /*[SerializeField] private ParticleSystem _bloodParticles;
    [SerializeField] private AudioSource _deathSound;*/
    [SerializeField] private Transform _weaponParent;

    [SerializeField] protected int _weaponNumber;
    protected bool _isAlive = true;

    private void Start()
    {
        GiveWeapon();
    }

    protected void GiveWeapon()
    {
        if ( _weaponNumber != -1)
        {
            Instantiate(LevelSettings.instance.weapons[_weaponNumber].gameObject, _weaponParent);
            gameObject.GetComponent<Animator>().SetInteger("Weapon", 1);
        }
    }

    protected Quaternion RorateAngle()
    {
        // �������� ��������� ������� �� ������
        Vector3 mousePosition = Input.mousePosition;

        // ����������� �������� ���������� � �������
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // �������� ����������� �� ��������� � �������
        Vector3 direction = new Vector3(mousePosition.x, transform.position.y, mousePosition.z) - transform.position;

        // ��������� ���� ��������
        Quaternion rotation = Quaternion.LookRotation(direction);

        return rotation;
    }
}
