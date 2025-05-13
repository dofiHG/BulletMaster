using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityNpgsql;

public class Authorization : MonoBehaviour
{
    public static Authorization instance;

    public int userID;

    [SerializeField] private TMP_InputField _loginInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _registerButton;
    [SerializeField] private TextMeshProUGUI _loginButtonText;
    [SerializeField] private TextMeshProUGUI _registerButtonText;
    [SerializeField] private GameObject _authPanel;
    [SerializeField] private GameObject _loadData;

    private string connectionString = "Host=localhost;Username=postgres;Password=root;Database=Money";
    private string originalLoginText;
    private string originalRegisterText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        originalLoginText = _loginButtonText.text;
        originalRegisterText = _registerButtonText.text;

        _loginButton.onClick.AddListener(OnLoginClicked);
        _registerButton.onClick.AddListener(OnRegisterClicked);
    }

    private void OnLoginClicked()
    {
        string login = _loginInput.text;
        string password = _passwordInput.text;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            StartCoroutine(ShowTempMessage(_loginButtonText, "Введите логин и пароль", 3f));
            return;
        }

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(
                    "SELECT id, password_hash FROM users WHERE login = @login",
                    conn))
                {
                    cmd.Parameters.AddWithValue("login", login);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userID = reader.GetInt32(0);
                            string dbPasswordHash = reader.GetString(1);

                            if (password == dbPasswordHash)
                            {
                                _authPanel.SetActive(false);
                                _loadData.SetActive(true);
                            }
                                
                            
                            else
                                StartCoroutine(ShowTempMessage(_loginButtonText, "Неправильный логин или пароль", 3f));
                        }
                        else
                            StartCoroutine(ShowTempMessage(_loginButtonText, "Неправильный логин или пароль", 3f));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Database error: {ex.Message}");
            StartCoroutine(ShowTempMessage(_loginButtonText, "Ошибка соединения", 3f));
        }
    }

    private void OnRegisterClicked()
    {
        string login = _loginInput.text;
        string password = _passwordInput.text;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            StartCoroutine(ShowTempMessage(_registerButtonText, "Введите логин и пароль", 3f));
            return;
        }

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var checkCmd = new NpgsqlCommand(
                    "SELECT COUNT(*) FROM users WHERE login = @login",
                    conn))
                {
                    checkCmd.Parameters.AddWithValue("login", login);
                    long userCount = (long)checkCmd.ExecuteScalar();

                    if (userCount > 0)
                    {
                        StartCoroutine(ShowTempMessage(_registerButtonText, "Такой пользователь уже существует", 3f));
                        return;
                    }
                }

                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO users (login, password_hash) VALUES (@login, @password) RETURNING id",
                    conn))
                {
                    cmd.Parameters.AddWithValue("login", login);
                    cmd.Parameters.AddWithValue("password", password);

                    int newUserId = (int)cmd.ExecuteScalar();
                    StartCoroutine(ShowTempMessage(_registerButtonText, "Вы зарегистрированы!", 3f));
                    _loginInput.text = "";
                    _passwordInput.text = "";
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Database error: {ex.Message}");
            StartCoroutine(ShowTempMessage(_registerButtonText, "Ошибка регистрации", 3f));
        }
    }

    private IEnumerator ShowTempMessage(TextMeshProUGUI buttonText, string message, float duration)
    {
        string originalText = buttonText.text;
        buttonText.text = message;
        yield return new WaitForSeconds(duration);
        buttonText.text = originalText;
    }
}
