using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score = 0;
    
    public static ScoreManager Instance;
    [SerializeField] private TextMeshProUGUI _scoreText;
    void Awake()
    {
        if (Instance != null) Destroy(this);
        
        Instance = this;
    }

    public void IncrementScore(int score)
    {
        _score += score;
        _scoreText.text = "SCORE : " + _score;
    }
    public int GetScore() { return _score; }
}
