using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;

public class Add : MonoBehaviour
{
    public static Add instance;
    [HideInInspector] public bool isIncome;
    [HideInInspector] public string state;

    [SerializeField] private Button _addBtn;
    [SerializeField] private Transform _incomesHistory;
    [SerializeField] private Transform _expensesHistory;
    [SerializeField] private GameObject _incomePrefab;
    [SerializeField] private GameObject _expensePrefab;
    [SerializeField] private GameObject _addPanel;
    [SerializeField] private TMP_InputField _inputSum;

    public static Action<bool> addIvent;
    private string connectionString = "Host=localhost;Username=postgres;Password=root;Database=Money";

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _addBtn.onClick.AddListener(AddIncomeOrExpense);
    }

    private void AddIncomeOrExpense()
    {
        string sum = _inputSum.text;
        if (string.IsNullOrEmpty(sum)) return;

        if (!decimal.TryParse(sum, out decimal amount))
        {
            Debug.LogError("�������� ������ �����");
            return;
        }

        int userId = 1;
        DateTime currentDate = DateTime.Now.Date;
        string formattedDate = currentDate.ToString("dd.MM.yyyy");

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                if (isIncome)
                {
                    // ���������� ������ � ���� ������
                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO incomes (user_id, category, amount, date) VALUES (@userId, @category, @amount, @date)",
                        conn))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        cmd.Parameters.AddWithValue("category", state);
                        cmd.Parameters.AddWithValue("amount", amount);
                        cmd.Parameters.AddWithValue("date", currentDate);
                        cmd.ExecuteNonQuery();
                    }

                    // �������� ����������� ��������
                    GameObject incomePrefab = Instantiate(_incomePrefab, _incomesHistory);
                    incomePrefab.transform.Find("State").GetComponent<TMP_Text>().text = state;
                    incomePrefab.transform.Find("Count").GetComponent<TMP_Text>().text = amount.ToString();

                    // ���������� ����, ���� ���� ��������������� UI �������
                    Transform dateElement = incomePrefab.transform.Find("Date");
                    if (dateElement != null)
                    {
                        dateElement.GetComponent<TMP_Text>().text = formattedDate;
                    }
                }
                else
                {
                    // ���������� ������� � ���� ������
                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO expenses (user_id, category, amount, date) VALUES (@userId, @category, @amount, @date)",
                        conn))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        cmd.Parameters.AddWithValue("category", state);
                        cmd.Parameters.AddWithValue("amount", amount);
                        cmd.Parameters.AddWithValue("date", currentDate);
                        cmd.ExecuteNonQuery();
                    }

                    // �������� ����������� ��������
                    GameObject expensePrefab = Instantiate(_expensePrefab, _expensesHistory);
                    expensePrefab.transform.Find("State").GetComponent<TMP_Text>().text = state;
                    expensePrefab.transform.Find("Count").GetComponent<TMP_Text>().text = amount.ToString();

                    // ���������� ����, ���� ���� ��������������� UI �������
                    Transform dateElement = expensePrefab.transform.Find("Date");
                    if (dateElement != null)
                    {
                        dateElement.GetComponent<TMP_Text>().text = formattedDate;
                    }
                }
                addIvent?.Invoke(isIncome);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ���� ������: {ex.Message}");
        }

        _inputSum.text = string.Empty;
        _addPanel.SetActive(false);
    }
}
