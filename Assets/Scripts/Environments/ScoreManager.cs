using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro leftText;

    [SerializeField]
    private TextMeshPro rightText;

    private int leftScore;
    private int rightScore;

    public void AddToLeft()
    {
        leftScore++;
        leftText.text = leftScore.ToString();
    }

    public void AddToRight()
    {
        rightScore++;
        rightText.text = rightScore.ToString();
    }
}
