using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;

public class FindBankKey : MonoBehaviour
{
    [SerializeField] private TMP_Text _percentText;

    private string url = "https://www.cbr.ru/hd_base/KeyRate/";

    private void Awake() => StartCoroutine(GetKeyRate());

    private IEnumerator GetKeyRate()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                _percentText.text = "Не удалось загрузить данные!";
                yield break;
            }

            string html = request.downloadHandler.text;
            ParseHTML(html);
        }
    }

    private void ParseHTML(string html)
    {
        string pattern = @"<table[^>]*class=""data""[^>]*>.*?<tr[^>]*>.*?<td[^>]*>(\d{2}\.\d{2}\.\d{4})</td>.*?<td[^>]*>(\d+,\d+)</td>.*?</tr>";

        MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.Singleline);

        if (matches.Count > 0)
        {
            Match lastMatch = matches[matches.Count - 1];
            string rate = lastMatch.Groups[2].Value;
            _percentText.text = rate;
        }
    }
}