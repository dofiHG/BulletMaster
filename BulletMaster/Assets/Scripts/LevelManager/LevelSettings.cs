using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings instance;

    [HideInInspector] public Weapon weapon;
    [HideInInspector] public bool canShoot;
    public int reboundBulletsCount;
    public GameObject[] weapons;

    [SerializeField] private GameObject _starsPanel;
    [SerializeField] private GameObject _bulletObject;
    [SerializeField] private GameObject _bulletImage;
    [SerializeField] private PlayerStickman _playerScript;
    [SerializeField] private Transform _prisonersContainer;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private int _bulletsCount;
    [SerializeField] private int _bulletsToTwoStars;
    [SerializeField] private int _bulletsToOneStar;

    private List<EnemyStickman> _enemies = new List<EnemyStickman>();
    private List<PrisonerStickman> _prisoners = new List<PrisonerStickman>();
    private int _enemiesCount;
    private int win;
    private int _starsCount;
    private GameObject _winPanel;
    private GameObject _bulletPanel;
    private GameObject _losePanel;
    private GameObject _canvas;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        Setup();
        _starsCount = 3;
        canShoot = true;
        win = 0;

        foreach (Transform enemy in _enemiesContainer)
            _enemies.Add(enemy.gameObject.GetComponent<EnemyStickman>());

        if (_prisonersContainer != null)
        {
            foreach (Transform prisoner in _prisonersContainer)
                _prisoners.Add(prisoner.gameObject.GetComponent<PrisonerStickman>());
        }

        _enemiesCount = _enemies.Count;
        SubscribeEvents();
    }

    private void Setup()
    {
        _canvas = GameObject.Find("Canvas");
        _bulletPanel = _canvas.transform.Find("BulletsPanel").gameObject;
        _winPanel = _canvas.transform.Find("WinPanel").gameObject;
        _losePanel = _canvas.transform.Find("LosePanel").gameObject;

        _bulletPanel.SetActive(true);
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);

        foreach (Transform child in _bulletPanel.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < _bulletsCount; i++)
            Instantiate(_bulletImage, _bulletPanel.transform);
    }

    private void SubscribeEvents()
    {
        foreach (Stickman enemy in _enemies)
            enemy.onDied += OnEnemyDied;

        _playerScript.onDied += DeadPlayer;

        if (_prisonersContainer != null)
        {
            foreach (Stickman prisoner in _prisoners)
                prisoner.onDied += DeadPlayer;
        }

        weapon.shoot += OnShot;
    }

    private void DeadPlayer()
    {
        win = -1;
        StartCoroutine(OnLose());
    }

    private void OnEnemyDied()
    {
        _enemiesCount--;
        if (_enemiesCount == 0 && win != -1)
        {
            canShoot = false;
            win = 1;
            StartCoroutine(OnWin());
        }
    }

    private void OnShot()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        _bulletsCount--;
        Destroy(_bulletPanel.transform.GetChild(0).gameObject);

        if (_bulletsCount == _bulletsToTwoStars)
        {
            _starsPanel.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            _starsCount = 2;
        }
        if (_bulletsCount == _bulletsToOneStar)
        {
            _starsPanel.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            _starsCount = 1;
        }
            

        if (_bulletsCount == 0)
        {
            weapon.shoot -= OnShot;
            weapon.enabled = false;
            canShoot = false;
            StartCoroutine(OnLose());
        }
    }

    public void UnsubscribeEvents()
    {
        foreach (Stickman enemy in _enemies)
            enemy.onDied -= OnEnemyDied;

        _playerScript.onDied -= DeadPlayer;

        if (_prisonersContainer != null)
        {
            foreach (Stickman prisoner in _prisoners)
                prisoner.onDied -= DeadPlayer;
        }

        weapon.shoot -= OnShot;
    }

    public IEnumerator OnLose()
    {
        canShoot = false;
        yield return new WaitForSeconds(3);
        if (win == 0 || win == -1)
            _losePanel.SetActive(true);

        UnsubscribeEvents();
    }

    private IEnumerator OnWin()
    {
        yield return new WaitForSeconds(3);
        if (win == 1)
        {
            _bulletPanel.SetActive(false);
            _winPanel.SetActive(true);

            UnsubscribeEvents();
            YG2.saves.stars[SceneManager.GetActiveScene().buildIndex] = _starsCount;
            YG2.SaveProgress();
        }
    }
}
