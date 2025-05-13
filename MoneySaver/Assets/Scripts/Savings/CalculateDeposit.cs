using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculateDeposit : MonoBehaviour
{
    [SerializeField] private TMP_Text _depositPercent;
    [SerializeField] private TMP_InputField _date;
    [SerializeField] private TMP_InputField _sum;
    [SerializeField] private Button _calculate;
    [SerializeField] private TMP_Text _result;

    private void Start() => _calculate.onClick.AddListener(Calculate);

    private void Calculate()
    {
        float depositPercent = -1;
        float date = -1;
        float sum = -1;

        try
        {
            date = float.Parse(_date.text);
            sum = float.Parse(_sum.text);
            depositPercent = float.Parse(_depositPercent.text);

            if (depositPercent > 0 && date > 0 && sum > 0)
                _result.text = ((depositPercent * date / 12 * 1.03 * sum)/10).ToString("#.##");
            else
                throw new Exception();
        }

        catch { _result.text = "Заполните все поля правильно!"; }
    }
}
