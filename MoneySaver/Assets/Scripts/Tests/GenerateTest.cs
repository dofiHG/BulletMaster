using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;

public class TestManager : MonoBehaviour
{
    [SerializeField] private Transform _questionsPanel;
    [SerializeField] private GameObject _questionPrefab;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TMP_Text _resultText;

    private int _currentQuestion;
    private int _points;
    private string _connectionString = "Host=localhost;Username=postgres;Password=root;Database=Money";
    private NpgsqlConnection _conn;
    private List<string> _questions = new List<string>();
    private List<GameObject> _questionsPanels = new List<GameObject>();
    private List<string> _correctAnswers = new List<string>();
    private List<string> _invalidAnswers = new List<string>();

    public void LoadQuestions(int testId)
    {
        _currentQuestion = 0;
        _questions.Clear();
        _correctAnswers.Clear();
        _invalidAnswers.Clear();
        _conn = new NpgsqlConnection(_connectionString);
        _conn.Open();

        var cmd = new NpgsqlCommand("SELECT question_text, correct_answer, wrong_answer1, wrong_answer2, wrong_answer3 FROM questions WHERE test_id = 1", _conn);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            _questions.Add(reader.GetString(0));
            _correctAnswers.Add(reader.GetString(1));
            _invalidAnswers.Add($"{reader.GetString(2)}@{reader.GetString(3)}@{reader.GetString(4)}");
        }

        GenerateQustions();
    }

    private void GenerateQustions()
    {
        foreach (Transform question in _questionsPanel)
            Destroy(question.gameObject);
            
        for (int i = 0; i < _questions.Count; i++)
        {
            GameObject currentQuestion = Instantiate(_questionPrefab, _questionsPanel);
            _questionsPanels.Add(currentQuestion);
            currentQuestion.GetComponentInChildren<TMP_Text>().text = _questions[i];

            currentQuestion.transform.Find("Answers").Find("Ans1").GetComponentInChildren<TMP_Text>().text = _correctAnswers[i];
            currentQuestion.transform.Find("Answers").Find("Ans2").GetComponentInChildren<TMP_Text>().text = _invalidAnswers[i].Split("@")[0];
            currentQuestion.transform.Find("Answers").Find("Ans3").GetComponentInChildren<TMP_Text>().text = _invalidAnswers[i].Split("@")[1];
            currentQuestion.transform.Find("Answers").Find("Ans4").GetComponentInChildren<TMP_Text>().text = _invalidAnswers[i].Split("@")[2];
            currentQuestion.transform.Find("NextQ").GetComponent<Button>().onClick.AddListener(GoToNextQuestion);
        }
        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        foreach (GameObject question in _questionsPanels)
            question.gameObject.SetActive(false);

        _questionsPanels[_currentQuestion].SetActive(true);
    }

    private void GoToNextQuestion()
    {
        GameObject currentPanel = _questionsPanels[_currentQuestion];
        ToggleGroup toggleGroup = currentPanel.transform.Find("Answers").GetComponent<ToggleGroup>();
        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();

        if (selectedToggle == null)
            return;

        string selectedAnswer = selectedToggle.GetComponentInChildren<TMP_Text>().text;

        if (selectedAnswer == _correctAnswers[_currentQuestion])
            _points++;

        selectedToggle.isOn = false;

        _currentQuestion++;
        if (_currentQuestion != _questions.Count)
            ShowCurrentQuestion();
        else
            CalculateResult();
    }

    private void CalculateResult()
    {
        _questionsPanel.gameObject.SetActive(false);
        _resultPanel.SetActive(true);

        if (((double)_points / (double)(_questions.Count)) * 100 >= 80)
            _resultText.text = $"¬ы прошли тестирование, набрав {_points}/{_questions.Count} баллов!";
        else
            _resultText.text = $"¬ы не прошли тестирование, набрав {_points}/{_questions.Count} баллов!";
    }
}
