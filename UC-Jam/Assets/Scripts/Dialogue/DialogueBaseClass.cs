using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueBaseClass : MonoBehaviour
{
    public virtual IEnumerator WriteText(string input, TextMeshProUGUI textHolder, float delay)
    {
        if (textHolder == null)
        {
            Debug.LogError("WriteText(): textHolder is null!");
            yield break;
        }

        if (string.IsNullOrEmpty(input))
        {
            Debug.LogError("WriteText(): input string is null or empty!");
            yield break;
        }

        textHolder.text = "";

        for (int i = 0; i < input.Length; i++)
        {
            textHolder.text += input[i];

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound("voiceBeeps");
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
