using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int _score = 0;
    
    public static ScoreManager Instance;
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    private TextMeshProUGUI[] textList;
    void Awake()
    {
        if (Instance != null) Destroy(this);
        
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Score text: " + _scoreText);
    }

    public void IncrementScore(int score)
    {
        for (int i = 0; i < _scoreText.text.Length; i++)
        {
            Debug.Log(_scoreText.text.Substring(i) + " ici");
        }
        _score = 0;
        int[] intArray = _scoreText.text.Substring(9).ToIntArray();
        for (int i = 0; i < intArray.Length; i++)
        {
            Debug.Log(intArray[i] + " la");
            _score += (intArray[i] - 48) * (int)Mathf.Pow(10, intArray.Length - i - 1);
        }
        _score += score;
        Debug.Log(_score + "boum");
        _scoreText.text = "SCORE : \n" + _score;
        Debug.Log(_scoreText+ "bro pk ?");
    }
    public int GetScore() { return _score; }
}
