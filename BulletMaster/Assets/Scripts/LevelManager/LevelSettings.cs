using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings instance;
    public GameObject[] weapons;
    [HideInInspector] public Weapon weapon;
    [HideInInspector] public bool canShoot;
        
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform _bulletsContainer;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;

    private List<EnemyStickman> _enemies = new List<EnemyStickman>();
    private int _enemiesCount;
    private int _bulletsCount;
    private bool win;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        canShoot = true;
        win = false;

        foreach (Transform enemy in _enemiesContainer)
            _enemies.Add(enemy.gameObject.GetComponent<EnemyStickman>());

        foreach (Transform bullet in _bulletsContainer)
            _bulletsCount++;

        _enemiesCount = _enemies.Count;

        SubscribeEvents();
    }

    private void OnDisable() => UnsubscribeEvents();

    private void SubscribeEvents()
    {
        foreach (Stickman enemy in _enemies)
            enemy.onDied += OnEnemyDied;

        weapon.shoot += OnShot;
    }

    private void OnEnemyDied()
    {
        _enemiesCount--;

        if (_enemiesCount == 0)
        {
            canShoot = false;
            win = true;
            StartCoroutine(OnWin());
        }
    }

    private void OnShot()
    {
        _bulletsCount--;

        Destroy(_bulletsContainer.GetChild(0).gameObject);

        if (_bulletsCount == 0)
        {
            weapon.shoot -= OnShot;
            weapon.enabled = false;
            canShoot = false;
            if (!win)
                StartCoroutine(OnLose());
        }
    }

    private void UnsubscribeEvents()
    {
        foreach (Stickman enemy in _enemies)
            enemy.onDied -= OnEnemyDied;

        weapon.shoot -= OnShot;
    }

    private IEnumerator OnLose()
    {
        yield return new WaitForSeconds(3);
        _losePanel.SetActive(true);
    }

    private IEnumerator OnWin()
    {
        yield return new WaitForSeconds(3);
        _winPanel.SetActive(true);
    }
}
