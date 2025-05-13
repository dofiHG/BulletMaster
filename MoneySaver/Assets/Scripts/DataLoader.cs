using System;
using TMPro;
using UnityEngine;
using UnityNpgsql;
using static UnityEngine.UI.Image;

public class DataLoader : MonoBehaviour
{
    [Header("Income Settings")]
    [SerializeField] private Transform _incomesHistory;
    [SerializeField] private GameObject _incomePrefab;

    [Header("Expense Settings")]
    [SerializeField] private Transform _expensesHistory;
    [SerializeField] private GameObject _expensePrefab;

    [Header("Goal Settings")]
    [SerializeField] private Transform _goalsContainer;
    [SerializeField] private GameObject _goalPrefab;

    private string connectionString = "Host=localhost;Username=postgres;Password=root;Database=Money";

    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        int userId = Authorization.instance.userID;

        ClearAllData();

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                LoadIncomes(conn, userId);
                LoadExpenses(conn, userId);
                LoadGoals(conn, userId);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Database error: {ex.Message}");
        }
    }

    private void ClearAllData()
    {
        ClearTransformChildren(_incomesHistory);
        ClearTransformChildren(_expensesHistory);
        ClearTransformChildren(_goalsContainer);
    }

    private void ClearTransformChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void LoadIncomes(NpgsqlConnection conn, int userId)
    {
        using (var cmd = new NpgsqlCommand(
            "SELECT category, amount, date FROM incomes WHERE user_id = @userId ORDER BY date DESC",
            conn))
        {
            cmd.Parameters.AddWithValue("userId", userId);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string category = reader.GetString(0);
                    decimal amount = reader.GetDecimal(1);
                    DateTime date = reader.GetDateTime(2);

                    GameObject incomePrefab = Instantiate(_incomePrefab, _incomesHistory);
                    incomePrefab.transform.Find("State").GetComponent<TMP_Text>().text = category;
                    string amountString = amount.ToString();
                    incomePrefab.transform.Find("Count").GetComponent<TMP_Text>().text = amountString.Substring(0, amountString.Length - 3);

                    Transform dateElement = incomePrefab.transform.Find("Date");
                    if (dateElement != null)
                    {
                        dateElement.GetComponent<TMP_Text>().text = date.ToString("dd.MM.yyyy");
                    }
                }
            }
        }
    }

    private void LoadExpenses(NpgsqlConnection conn, int userId)
    {
        using (var cmd = new NpgsqlCommand(
            "SELECT category, amount, date FROM expenses WHERE user_id = @userId ORDER BY date DESC",
            conn))
        {
            cmd.Parameters.AddWithValue("userId", userId);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string category = reader.GetString(0);
                    decimal amount = reader.GetDecimal(1);
                    DateTime date = reader.GetDateTime(2);

                    GameObject expensePrefab = Instantiate(_expensePrefab, _expensesHistory);
                    expensePrefab.transform.Find("State").GetComponent<TMP_Text>().text = category;
                    string amountString = amount.ToString();
                    expensePrefab.transform.Find("Count").GetComponent<TMP_Text>().text = amountString.Substring(0, amountString.Length - 3);

                    Transform dateElement = expensePrefab.transform.Find("Date");
                    if (dateElement != null)
                    {
                        dateElement.GetComponent<TMP_Text>().text = date.ToString("dd.MM.yyyy");
                    }
                }
            }
        }
    }

    private void LoadGoals(NpgsqlConnection conn, int userId)
    {
        using (var cmd = new NpgsqlCommand(
            "SELECT id, title, created_date, deadline, is_completed FROM goals WHERE user_id = @userId ORDER BY deadline",
            conn))
        {
            cmd.Parameters.AddWithValue("userId", userId);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string title = reader.GetString(1);
                    DateTime createdDate = reader.GetDateTime(2);
                    DateTime deadline = reader.GetDateTime(3);
                    bool isCompleted = reader.GetBoolean(4);

                    GameObject goalPrefab = Instantiate(_goalPrefab, _goalsContainer);

                    goalPrefab.transform.Find("Name").GetComponent<TMP_Text>().text = title;
                    goalPrefab.transform.Find("StartDate").GetComponent<TMP_Text>().text = createdDate.ToString("dd.MM.yyyy");
                    goalPrefab.transform.Find("FinishDate").GetComponent<TMP_Text>().text = deadline.ToString("dd.MM.yyyy");

                    Transform statusElement = goalPrefab.transform.Find("Status");
                    if (statusElement != null)
                    {
                        statusElement.GetComponent<TMP_Text>().text = isCompleted ? "✔️" : "❌";
                    }
                }
            }
        }
    }
}
