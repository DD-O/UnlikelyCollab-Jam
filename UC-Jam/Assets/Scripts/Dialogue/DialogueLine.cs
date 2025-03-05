using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueLine : DialogueBaseClass
{
    [SerializeField] private string input;
    [SerializeField] private float delay;
    private TextMeshProUGUI textHolder;

    public enum FollowerType { Circle, Triangle, Heart, Star } // Enum for dropdown
    [SerializeField] private FollowerType follower; // Dropdown in Inspector 

    private void Awake()
    {
        textHolder = GetComponent<TextMeshProUGUI>();

        if (textHolder == null)
        {
            Debug.LogError("TextMeshProUGUI component is missing from this GameObject!", this);
            return; // Stop execution if there's no TextMeshProUGUI
        }

        StartCoroutine(WriteText(input, textHolder, delay));
    }

    public override IEnumerator WriteText(string input, TextMeshProUGUI textHolder, float delay)
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
                string soundToPlay = GetFollowerSound();
                SoundManager.Instance.PlaySound(soundToPlay);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private string GetFollowerSound()
    {
        switch (follower) 
        {
            case FollowerType.Circle:
                return "circleVoice";
            case FollowerType.Triangle:
                return "triangleVoice";
            case FollowerType.Heart:
                return "heartVoice";
            case FollowerType.Star:
                return "starVoice";
            default:
                return "circleVoice"; // Default sound
        }
    }
}
