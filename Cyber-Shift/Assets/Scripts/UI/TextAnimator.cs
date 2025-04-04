using UnityEngine;
using TMPro;
using System.Collections;

public class TextAnimator : MonoBehaviour
{
    public TMP_Text textComponent;
    public string targetText = "123";
    public float letterDelay = 0.2f;
    public float symbolDelay = 0.1f;
    public int minSymbols = 3;
    public int maxSymbols = 4;
    public int symbolCycles = 3;

    private char[] symbols = new char[]
    {
        '!', '@', '#', '$', '%', '^', '&', '(', ')',
        '-', '_', '+', '=', '{', '}', '|', '\\',
        ':', ';', '"', '\'', '<', '>', '?', '/', '~'
    };

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        textComponent.text = "";

        for (int i = 0; i < targetText.Length; i++)
        {
            string baseText = targetText.Substring(0, i + 1);
            textComponent.text = baseText;

            if (letterDelay > 0)
                yield return new WaitForSecondsRealtime(letterDelay);
            else
                yield return null;

            for (int j = 0; j < symbolCycles; j++)
            {
                string randomSymbols = GenerateRandomSymbols(
                    Random.Range(minSymbols, maxSymbols + 1));

                textComponent.text = baseText + " " + randomSymbols;

                if (symbolDelay > 0)
                    yield return new WaitForSecondsRealtime(symbolDelay);
                else
                    yield return null;
            }
        }
        textComponent.text = targetText;
    }

    string GenerateRandomSymbols(int count)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < count; i++)
        {
            sb.Append(symbols[Random.Range(0, symbols.Length)]);
        }
        return sb.ToString();
    }
}