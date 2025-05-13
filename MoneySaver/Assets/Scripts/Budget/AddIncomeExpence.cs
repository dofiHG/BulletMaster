using UnityEngine;
using UnityEngine.UI;

public class AddIncomeExpence : MonoBehaviour
{
    [SerializeField] private Button _addIncome;
    [SerializeField] private Button _addExpence;

    [SerializeField] private GameObject _incomePanel;
    [SerializeField] private GameObject _expencePanel;

    private void Start()
    {
        _addExpence.onClick.AddListener(OpenExpencePanel);   
        _addIncome.onClick.AddListener(OpenIncomePanel);   
    }

    private void OpenIncomePanel()
    {
        _incomePanel.SetActive(true);
        Add.instance.isIncome = true;
    }

    private void OpenExpencePanel()
    {
        _expencePanel.SetActive(true);
        Add.instance.isIncome = false;
    }
}
