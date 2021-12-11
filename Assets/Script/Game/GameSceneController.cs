using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    // 위 아래 20개씩 오브젝트풀
    // 몬스터 생성을 어떻게 해야할까?? mesh, materials, 제스쳐 갯수, 제스쳐 난이도
    // Start is called before the first  frame update
    [SerializeField] 
    public ObscuredInt _gameLev = 1;
    private bool _cheaterDetected = false;
    
    public EnemiesController _enemiesController;
    public PlayerController _playerController;
    public Slider _slider;

    [SerializeField]
    private ObscuredBool _isOver = false;
    [SerializeField]
    private ObscuredFloat _totalTime = 60.0f;
    [SerializeField]
    private ObscuredFloat _maxLevTime = 20.0f;
    [SerializeField]
    private ObscuredFloat _levTime;
    [SerializeField]
    private ObscuredFloat _initRespawnTime = 5.0f;
    [SerializeField]
    private ObscuredFloat _respawnTime;
    [SerializeField]
    private ObscuredFloat _respawnDivisionScale = 1.1f;
    void Start()
    {
        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
        _slider.value = 1f;
        _levTime = _maxLevTime;
        _respawnTime = _initRespawnTime;
    }
    private void OnCheaterDetected()
    {
        _cheaterDetected = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cheaterDetected) Application.Quit();

        if (_isOver)
        {
            //게임 종료시 처리
            _enemiesController.StopEnemy();
        }
        else
        {
            ///Total 시간 카운터
            _totalTime -= Time.deltaTime;
            if (_totalTime <= 0) _isOver = true;
            _slider.value = _totalTime / 60f;

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
}