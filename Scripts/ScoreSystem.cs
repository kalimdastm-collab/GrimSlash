using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int score;

    public void AddScore(int amount)
    {
        score += amount;
    }
}
