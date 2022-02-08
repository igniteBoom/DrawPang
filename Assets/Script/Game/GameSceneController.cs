using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;
using UnityEngine.UI;
using TMPro;
public class GameSceneController : MonoBehaviour
{
    static public GameSceneController _instance;
    // 위 아래 20개씩 오브젝트풀
    // 몬스터 생성을 어떻게 해야할까?? mesh, materials, 제스쳐 갯수, 제스쳐 난이도
    // Start is called before the first  frame update
    [SerializeField]
    public ObscuredInt _gameLev = 1;
    private bool _cheaterDetected = false;

    public GameObject _gesture;
    public RectTransform _rectTranfromCanvasUI;
    public RectTransform _rectTranformCanvasParticle;
    public EnemiesController _enemiesController;
    public PlayerController _playerController;
    public TextMeshProUGUI _scoreText;
    public Slider _lifeSlider, _timeSlider;

    [SerializeField]
    private ObscuredBool _isOver = false, _onceOver = false;
    [SerializeField]
    private ObscuredFloat _totalTime, _life, _maxLevTime, _levTime, _initRespawnTime, _respawnTime, _respawnDivisionScale, _perfectPct, _greatPct, _plusTimeOffset, _enemyDamageOffset;
    [SerializeField]
    private ObscuredInt _myScore, _perfectScore, _greatScore, _goodScore;

    public float PerfectPct {
        get { return _perfectPct; }
        set { _perfectPct = value; }
    }

    public float GreatPct {
        get { return _greatPct; }
        set { _greatPct = value; }
    }

    public int MyScore { 
        get { return _myScore; }
        set { _myScore = value; _scoreText.text = GetThousandCommaText(_myScore).ToString(); }
    }

    public int PerfectScore {
        get { return _perfectScore; }
        set { _perfectScore = value; }
    }

    public int GreatScore {
        get { return _greatScore; }
        set { _greatScore = value; }
    }

    public int GoodScore {
        get { return _goodScore; }
        set { _goodScore = value; }
    }

    public float PlusTimeOffset {
        get { return _plusTimeOffset; }
    }

    public float EnemyDamageOffset {
        get { return _enemyDamageOffset; }
    }

    public float TotalTime {
        get { return _totalTime; }
        set { _totalTime = value; }
    }

    public float Life {
        get { return _life; }
        set { _life = value; }
    }

    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
        InitVariable();
    }
    private void OnCheaterDetected()
    {
        _cheaterDetected = true;
    }
    public void InitVariable()
    {
        _isOver = false;
        _totalTime = 60.0f;
        _maxLevTime = 20.0f;
        _initRespawnTime = 5.0f;
        _respawnDivisionScale = 1.1f;
        _perfectPct = 0.95f;
        _greatPct = 0.9f;
        _myScore = 0;
        _scoreText.text = 0.ToString();//GetThousandCommaText(_myScore).ToString();
        _perfectScore = 100;
        _greatScore = 80;
        _goodScore = 50;
        _plusTimeOffset = 1f;
        _enemyDamageOffset = 0.05f;

        _life = 1f;
        _lifeSlider.value = 1f;
        _timeSlider.value = 1f;

        _levTime = _maxLevTime;
        _respawnTime = _initRespawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cheaterDetected) Application.Quit();

        if (_isOver)
        {
            //게임 종료시 처리
            _enemiesController.StopEnemy();
            _onceOver = true;
        }
        else
        {
            ///Total 시간 카운터
            _totalTime -= Time.deltaTime;
            if (_totalTime <= 0 || _life <= 0) _isOver = true;
                _timeSlider.value = _totalTime / 60f;
            _lifeSlider.value = _life;

            ///Game Lev 카운터
            if (_levTime > 0)
            {
                _levTime -= Time.deltaTime;
            }
            else if (_levTime <= 0)
            {
                _levTime = _maxLevTime;
                _gameLev++;

                ///게임 레벨이 오를때 스폰 시간을 Scale만큼 줄여준다.
                _initRespawnTime /= _respawnDivisionScale;
            }

            //Game Lev에 따른 몬스터 리젠
            if (_respawnTime > 0)
            {
                _respawnTime -= Time.deltaTime;
            }
            else if (_respawnTime <= 0)
            {
                _respawnTime = _initRespawnTime;
                _enemiesController.SetEnemy(_gameLev);
            }

            //Debug.Log("time : " + _levTime);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _enemiesController.KillEnemy();
            }
        }
    }
    public string GetThousandCommaText(int data)
    {
        return string.Format("{0:#,###}", data);
    }
}