using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;

public class AddNewGoal : MonoBehaviour
{
    [SerializeField] private GameObject _goalPrefab;
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private Button _addBtn;
    [SerializeField] private TMP_InputField _name;
    [SerializeField] private TMP_InputField _startDate;
    [SerializeField] private TMP_InputField _finishDate;

    private string connectionString = "Host=localhost;Username=postgres;Password=root;Database=Money";

    private void Start() => _addBtn.onClick.AddListener(AddGoal);

    private void AddGoal()
    {
        string title = _name.text;

        if (!DateTime.TryParse(_startDate.text, out DateTime createdDate))
        {
            Debug.LogError("Неверный формат даты создания");
            return;
        }

        if (!DateTime.TryParse(_finishDate.text, out DateTime deadline))
        {
            Debug.LogError("Неверный формат даты окончания");
            return;
        }

        // Создаем визуальный элемент
        GameObject newGoal = Instantiate(_goalPrefab, _contentContainer);
        newGoal.transform.Find("Name").GetComponent<TMP_Text>().text = title;
        newGoal.transform.Find("StartDate").GetComponent<TMP_Text>().text = createdDate.ToShortDateString();
        newGoal.transform.Find("FinishDate").GetComponent<TMP_Text>().text = deadline.ToShortDateString();

        // Добавляем в базу данных
        AddGoalToDatabase(title, createdDate, deadline);
    }

    private void AddGoalToDatabase(string title, DateTime createdDate, DateTime deadline)
    {
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        INSERT INTO goals 
                            (user_id, title, created_date, deadline) 
                        VALUES 
                            (@user_id, @title, @created_date, @deadline)";

                    cmd.Parameters.AddWithValue("user_id", 1); // ID пользователя по умолчанию
                    cmd.Parameters.AddWithValue("title", title);
                    cmd.Parameters.AddWithValue("created_date", createdDate);
                    cmd.Parameters.AddWithValue("deadline", deadline);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Debug.Log("Цель успешно добавлена");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка базы данных: {ex.Message}");
        }
    }
}
