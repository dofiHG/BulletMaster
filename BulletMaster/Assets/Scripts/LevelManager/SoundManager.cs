using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _audioSprites;
    [SerializeField] private Image _soundIcon;
    [SerializeField] private GameObject _menu;

    private Stickman _player;
    private Pistol _weaponScript;

    public void OnSoundBtnClick()
    {
        _soundIcon.sprite = _soundIcon.sprite == _audioSprites[0]? _audioSprites[1]: _audioSprites[0];
        AudioListener.volume = AudioListener.volume == 1? AudioListener.volume = 0: AudioListener.volume = 1 ;
    }

    public void OnMenuButtonClick()
    {    
        bool isActive = _menu.activeSelf == true ? false: true;
        _menu.SetActive(isActive);

        GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>().enabled = !isActive;
    }
}
