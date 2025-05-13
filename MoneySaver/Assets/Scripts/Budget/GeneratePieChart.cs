using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityNpgsql;
using TMPro;
using System;

public class PieChartController : MonoBehaviour
{
    [Header("Income Chart Settings")]
    [SerializeField] private GameObject pieChartIncomePanel;
    [SerializeField] private GameObject pieChartSegmentPrefab;
    [SerializeField] private RectTransform pieChartIncomeContainer;
    [SerializeField] private Transform incomeLegendContainer;
    [SerializeField] private GameObject legendItemPrefab;

    [Header("Expense Chart Settings")]
    [SerializeField] private GameObject pieChartExpensePanel;
    [SerializeField] private RectTransform pieChartExpenseContainer;
    [SerializeField] private Transform expenseLegendContainer;

    [Header("Income Filter Buttons")]
    [SerializeField] private Button incomeDayFilterButton;
    [SerializeField] private Button incomeMonthFilterButton;
    [SerializeField] private Button incomeYearFilterButton;

    [Header("Expense Filter Buttons")]
    [SerializeField] private Button expenseDayFilterButton;
    [SerializeField] private Button expenseMonthFilterButton;
    [SerializeField] private Button expenseYearFilterButton;

    private string connectionString = "Host=localhost;Username=postgres;Password=root;Database=Money";
    private int currentUserId = 1;
    private DateTime currentDate = DateTime.Now;

    // Отдельные фильтры для доходов и расходов
    private string incomeFilter = "";
    private string expenseFilter = "";

    void Start()
    {
        // Назначаем обработчики для кнопок доходов
        incomeDayFilterButton.onClick.AddListener(() => SetIncomeFilter("day"));
        incomeMonthFilterButton.onClick.AddListener(() => SetIncomeFilter("month"));
        incomeYearFilterButton.onClick.AddListener(() => SetIncomeFilter("year"));

        // Назначаем обработчики для кнопок расходов
        expenseDayFilterButton.onClick.AddListener(() => SetExpenseFilter("day"));
        expenseMonthFilterButton.onClick.AddListener(() => SetExpenseFilter("month"));
        expenseYearFilterButton.onClick.AddListener(() => SetExpenseFilter("year"));

        CreatePieCharts();
        Add.addIvent += UpdatePieChart;
    }

    private void SetIncomeFilter(string filterType)
    {
        incomeFilter = filterType;
        CreateIncomePieChart();
        UpdateIncomeButtonsAppearance();
    }

    private void SetExpenseFilter(string filterType)
    {
        expenseFilter = filterType;
        CreateExpensePieChart();
        UpdateExpenseButtonsAppearance();
    }

    private void UpdateIncomeButtonsAppearance()
    {
        // Сбрасываем все кнопки доходов к активному состоянию
        incomeDayFilterButton.interactable = true;
        incomeMonthFilterButton.interactable = true;
        incomeYearFilterButton.interactable = true;

        // Делаем активную кнопку неактивной (выделенной)
        switch (incomeFilter)
        {
            case "day":
                incomeDayFilterButton.interactable = false;
                break;
            case "month":
                incomeMonthFilterButton.interactable = false;
                break;
            case "year":
                incomeYearFilterButton.interactable = false;
                break;
        }
    }

    private void UpdateExpenseButtonsAppearance()
    {
        // Сбрасываем все кнопки расходов к активному состоянию
        expenseDayFilterButton.interactable = true;
        expenseMonthFilterButton.interactable = true;
        expenseYearFilterButton.interactable = true;

        // Делаем активную кнопку неактивной (выделенной)
        switch (expenseFilter)
        {
            case "day":
                expenseDayFilterButton.interactable = false;
                break;
            case "month":
                expenseMonthFilterButton.interactable = false;
                break;
            case "year":
                expenseYearFilterButton.interactable = false;
                break;
        }
    }

    public void CreatePieCharts()
    {
        CreateIncomePieChart();
        CreateExpensePieChart();
    }

    private void CreateIncomePieChart()
    {
        ClearContainer(pieChartIncomeContainer);
        ClearLegend(incomeLegendContainer);

        Dictionary<string, float> incomeByCategory = GetIncomeByCategory();

        pieChartIncomePanel.SetActive(true);
        CreateChart(incomeByCategory, pieChartIncomeContainer, incomeLegendContainer);
    }

    private void CreateExpensePieChart()
    {
        ClearContainer(pieChartExpenseContainer);
        ClearLegend(expenseLegendContainer);

        Dictionary<string, float> expenseByCategory = GetExpenseByCategory();

        pieChartExpensePanel.SetActive(true);
        CreateChart(expenseByCategory, pieChartExpenseContainer, expenseLegendContainer);
    }

    private Dictionary<string, float> GetIncomeByCategory()
    {
        return GetDataByCategory("incomes", incomeFilter);
    }

    private Dictionary<string, float> GetExpenseByCategory()
    {
        return GetDataByCategory("expenses", expenseFilter);
    }

    private Dictionary<string, float> GetDataByCategory(string tableName, string filter)
    {
        Dictionary<string, float> result = new Dictionary<string, float>();

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;

                string dateCondition = GetDateCondition(filter);
                cmd.CommandText = $@"SELECT category, SUM(amount) as total 
                                    FROM {tableName} 
                                    WHERE user_id = {currentUserId} {dateCondition}
                                    GROUP BY category";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string category = reader.GetString(0);
                        decimal decimalValue = reader.GetDecimal(1);
                        float total = (float)decimalValue;
                        result.Add(category, total);
                    }
                }
            }

            conn.Close();
        }

        return result;
    }

    private string GetDateCondition(string filter)
    {
        switch (filter)
        {
            case "day":
                return $"AND date = '{currentDate:yyyy-MM-dd}'";
            case "month":
                return $"AND EXTRACT(MONTH FROM date) = {currentDate.Month} " +
                       $"AND EXTRACT(YEAR FROM date) = {currentDate.Year}";
            case "year":
                return $"AND EXTRACT(YEAR FROM date) = {currentDate.Year}";
            default:
                return "";
        }
    }

    // Остальные методы остаются без изменений
    private void CreateChart(Dictionary<string, float> dataByCategory, RectTransform container, Transform legendContainer)
    {
        float totalAmount = dataByCategory.Sum(x => x.Value);
        float currentAngle = 0f;

        foreach (var item in dataByCategory)
        {
            float segmentAngle = (item.Value / totalAmount) * 360f;
            Color segmentColor = GetRandomColor();

            CreatePieSegment(currentAngle, segmentAngle, segmentColor, item.Key, item.Value, container);
            CreateLegendItem(item.Key, segmentColor, legendContainer);

            currentAngle += segmentAngle;
        }
    }

    private void CreatePieSegment(float startAngle, float segmentAngle, Color color, string category, float amount, RectTransform container)
    {
        GameObject segment = Instantiate(pieChartSegmentPrefab, container);
        Image segmentImage = segment.GetComponent<Image>();
        segmentImage.color = color;
        segmentImage.type = Image.Type.Filled;
        segmentImage.fillMethod = Image.FillMethod.Radial360;
        segmentImage.fillOrigin = (int)Image.Origin360.Top;
        segmentImage.fillClockwise = true;
        segmentImage.fillAmount = segmentAngle / 360f;
        segmentImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -startAngle);
        segmentImage.pixelsPerUnitMultiplier = 0.95f;

        if (segment.transform.childCount > 0)
        {
            Text segmentText = segment.GetComponentInChildren<Text>();
            if (segmentText != null)
            {
                segmentText.text = $"{category}\n{amount:F2}";
                float textAngle = startAngle + segmentAngle / 2f;
                float radius = container.rect.width * 0.35f;
                segmentText.rectTransform.anchoredPosition = new Vector2(
                    Mathf.Sin(textAngle * Mathf.Deg2Rad) * radius,
                    Mathf.Cos(textAngle * Mathf.Deg2Rad) * radius
                );
                segmentText.rectTransform.localRotation = Quaternion.identity;
            }
        }
    }

    private void CreateLegendItem(string category, Color color, Transform legendContainer)
    {
        GameObject legendItem = Instantiate(legendItemPrefab, legendContainer);

        Image colorImage = legendItem.transform.Find("Color")?.GetComponent<Image>();
        if (colorImage != null)
        {
            colorImage.color = color;
        }

        TMP_Text legendText = legendItem.transform.Find("Caption")?.GetComponent<TMP_Text>();
        if (legendText != null)
        {
            legendText.text = category;
        }
    }

    private void ClearContainer(RectTransform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    private void ClearLegend(Transform legendContainer)
    {
        foreach (Transform child in legendContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private Color GetRandomColor()
    {
        return new Color(
            UnityEngine.Random.Range(0.2f, 0.9f),
            UnityEngine.Random.Range(0.2f, 0.9f),
            UnityEngine.Random.Range(0.2f, 0.9f),
            1f
        );
    }

    public void UpdatePieChart(bool isIncome)
    {
        if (isIncome)
        {
            CreateIncomePieChart();
        }
        else
        {
            CreateExpensePieChart();
        }
    }

    void OnDestroy()
    {
        Add.addIvent -= UpdatePieChart;
    }
}