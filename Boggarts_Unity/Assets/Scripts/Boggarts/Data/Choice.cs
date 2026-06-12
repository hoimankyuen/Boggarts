using UnityEngine;

[CreateAssetMenu(menuName = "Boggart Data/Choice")]
public class Choice : ScriptableObject
{
    [Tooltip("Denotes a good choice or a bad choice")] public bool IsGood;
    [Tooltip("Sound to play on selecting the choice")] public AudioClip Sound;
}