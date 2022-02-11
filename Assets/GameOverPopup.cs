using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class GameOverPopup : PopupBase
{
    [SerializeField]
    private int _perfectCount;
    [SerializeField]
    private int _greatCount;
    [SerializeField]
    private int _goodCount;
    [SerializeField]
    private int _missCount;
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _time;
    [SerializeField]
    private int _gold;

    public TextMeshProUGUI _perfectText;
    public TextMeshProUGUI _greatText;
    public TextMeshProUGUI _goodText;
    public TextMeshProUGUI _missText;
    public TextMeshProUGUI _scoreText;
    public TextMeshProUGUI _timeText;
    public TextMeshProUGUI _goldText;

    public GameObject[] _objStar;
    public void InitUI(int perfectCount, int greatCount, int goodCount, int missCount, int score, int time, int gold)
    {
        _perfectCount = perfectCount;
        _greatCount = greatCount;
        _goodCount = goodCount;
        _missCount = missCount;
        _score = score;
        _time = time;
        _gold = gold;

        _perfectText.text = _perfectCount.ToString();
        _greatText.text = _greatCount.ToString();
        _goodText.text = _goodCount.ToString();
        _missText.text = _missCount.ToString();
        _scoreText.text = _score.ToString();
        _timeText.text = _time.ToString();
        _goldText.text = _gold.ToString();

    }
    private void OnEnable()
    {
    }

    public void OnClickRestart()
    {
        LoadingSceneController.LoadScene("GameScene");
    }
}
