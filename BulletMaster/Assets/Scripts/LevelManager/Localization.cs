using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class Localization : MonoBehaviour
{
    [SerializeField] private string ru, en, ge, tr;

    private TMP_Text _currentText;

    private void Start()
    {
        _currentText = GetComponent<TMP_Text>();
        Localizate(YG2.lang);
    }

    private void Localizate(string lang)
    {
        switch (lang)
        {
            case "ru":
                _currentText.text = ru;
                break;
            case "en":
                _currentText.text = en;
                break;
            case "ge":
                _currentText.text = ge;
                break;
            case "tr":
                _currentText.text = tr;
                break;
            default:
                _currentText.text = en;
                break;
        }
    }
}
