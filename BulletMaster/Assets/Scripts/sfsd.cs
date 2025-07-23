using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class sfsd : MonoBehaviour
{
    private void Update()
    {
        gameObject.GetComponent<TMP_Text>().text = YG2.lang;
    }
}
