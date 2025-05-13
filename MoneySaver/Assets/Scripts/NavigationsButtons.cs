using UnityEngine;
using UnityEngine.UI;

public class NavigationsButtons : MonoBehaviour
{
    [SerializeField] private Button _budget;
    [SerializeField] private Button _savings;
    [SerializeField] private Button _goals;
    [SerializeField] private Button _expenses;

    [SerializeField] private GameObject _panelsHolder;

    public void OpenPanel(GameObject mainPanel)
    {
        foreach (Transform panel in _panelsHolder.transform)
        { panel.gameObject.SetActive(false); }

        mainPanel.SetActive(true);
    }
}
