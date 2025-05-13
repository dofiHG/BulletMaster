using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class IncomeExpenceManager : MonoBehaviour
{
    [SerializeField] private GameObject _editPanel;
    [SerializeField] private GameObject _chosePanel;

    [SerializeField] private TMP_Text _caption;

    public void OpenPanel(GameObject selfPanel)
    {
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;

        selfPanel.SetActive(false);
        _editPanel.SetActive(true);
        string caption = pressedButton.GetComponentInChildren<TMP_Text>().text;

        _caption.text = "Добавить по статье: " + caption;
        Add.instance.state = caption;
    }

    public void ClosePanel(GameObject selfPanel) => selfPanel.SetActive(false);
}
