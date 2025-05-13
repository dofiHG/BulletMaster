using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvestmentGame : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button _calcullateButton;
    [SerializeField] private GridLayoutGroup _matrixGrid;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private GameObject _cellPrefab;

    [Header("Settings")]
    [SerializeField] private Vector2 _cellSize = new Vector2(120, 60);
    [SerializeField] private Color _positiveColor = Color.green;
    [SerializeField] private Color _negativeColor = Color.red;
    [SerializeField] private Color _headerColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private Color _highlightColor = new Color(1f, 0.9f, 0.4f);

    private string[] _strategyNames = { "Надёжные", "Средние", "Рискованные" };
    private string[] _marketStates = { "Рост", "Стагнация", "Кризис" };
    private float[] _marketProbabilities = new float[3];
    private int[,] _payoffMatrix = new int[3, 3];
    private TMP_Text[,] _cellTexts = new TMP_Text[4, 4];

    private void Start()
    {
        InitializeGrid();
        _calcullateButton.onClick.AddListener(SimulateInvestment);

        UpdateRiskLevel();
        GeneratePayoffMatrix();
        UpdateMatrixUI();
    }

    private void InitializeGrid()
    {

        foreach (Transform child in _matrixGrid.transform)
            Destroy(child.gameObject);

        _matrixGrid.cellSize = _cellSize;
        _matrixGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _matrixGrid.constraintCount = 4;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                GameObject cell = Instantiate(_cellPrefab, _matrixGrid.transform);
                _cellTexts[row, col] = cell.GetComponent<TMP_Text>();
                _cellTexts[row, col].alignment = TextAlignmentOptions.Center;

                if (row == 0 || col == 0)
                {
                    _cellTexts[row, col].fontStyle = FontStyles.Bold;
                }
            }
        }
    }

    private void UpdateRiskLevel()
    {
        float risk = Random.Range(0f, 1f);
        _marketProbabilities[0] = 0.6f - risk * 0.3f; 
        _marketProbabilities[1] = 0.3f;                    
        _marketProbabilities[2] = 0.1f + risk * 0.3f;  

        float sum = _marketProbabilities[0] + _marketProbabilities[1] + _marketProbabilities[2];
        for (int i = 0; i < 3; i++)
        {
            _marketProbabilities[i] /= sum;
        }
    }

    private void GeneratePayoffMatrix()
    {
        _payoffMatrix[0, 0] = Random.Range(4, 8);    
        _payoffMatrix[1, 0] = Random.Range(8, 15);   
        _payoffMatrix[2, 0] = Random.Range(15, 25);  

        _payoffMatrix[0, 1] = Random.Range(2, 6);   
        _payoffMatrix[1, 1] = Random.Range(0, 4);     
        _payoffMatrix[2, 1] = Random.Range(-15, 0); 

        _payoffMatrix[0, 2] = Random.Range(1, 5); 
        _payoffMatrix[1, 2] = Random.Range(-8, 1); 
        _payoffMatrix[2, 2] = Random.Range(-40, -20);
    }

    private void UpdateMatrixUI()
    {
        _cellTexts[0, 0].text = "";
        for (int col = 0; col < 3; col++)
        {
            _cellTexts[0, col + 1].text = $"{_marketStates[col]}\n({_marketProbabilities[col]:P0})";
        }

        for (int row = 0; row < 3; row++)
        {
            _cellTexts[row + 1, 0].text = _strategyNames[row];

            for (int col = 0; col < 3; col++)
            {
                _cellTexts[row + 1, col + 1].text = _payoffMatrix[row, col].ToString();
                _cellTexts[row + 1, col + 1].color = _payoffMatrix[row, col] >= 0 ? _positiveColor : _negativeColor;
            }
        }
    }

    private void SimulateInvestment()
    {
        UpdateRiskLevel();
        GeneratePayoffMatrix();
        UpdateMatrixUI();
        AnalyzeStrategies();
    }

    private void AnalyzeStrategies()
    {
        float[] expectedUtilities = new float[3];
        int bestStrategy = 0;
        float maxUtility = float.MinValue;

        for (int strategy = 0; strategy < 3; strategy++)
        {
            expectedUtilities[strategy] = 0f;
            for (int state = 0; state < 3; state++)
            {
                expectedUtilities[strategy] += _payoffMatrix[strategy, state] * _marketProbabilities[state];
            }

            if (expectedUtilities[strategy] > maxUtility)
            {
                maxUtility = expectedUtilities[strategy];
                bestStrategy = strategy;
            }
        }

        for (int row = 1; row < 4; row++)
        {
            _cellTexts[row, 0].transform.parent.GetComponent<Image>().color =
                (row == bestStrategy + 1) ? _highlightColor : Color.white;
        }

        string analysisResult = "<b>Результаты анализа:</b>\n\n";
        analysisResult += $"• Вероятности: Рост {_marketProbabilities[0]:P0}, " +
                         $"Стагнация {_marketProbabilities[1]:P0}, " +
                         $"Кризис {_marketProbabilities[2]:P0}\n\n";

        analysisResult += "<b>Ожидаемая доходность:</b>\n";
        for (int i = 0; i < 3; i++)
        {
            analysisResult += $"• {_strategyNames[i]}: <b>{expectedUtilities[i]:F1}</b>\n";
        }

        analysisResult += $"\n<b>Рекомендуемая стратегия:</b> <color=green>{_strategyNames[bestStrategy]}</color> " +
                         $"(ожидаемая доходность: {maxUtility:F1})";

        _resultText.text = analysisResult;
    }
}