using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;

public class EnemiesController : MonoBehaviour
{
    public GameSceneController _gameSceneScontroller;
    private bool _cheaterDetected = false;
    public GameObject _enemy, _gesture;
    public Transform _enemyTr, _gestureTr;
    public Camera _uiCamera;
    private List<GameObject> _listEnemy = new List<GameObject>();
    private List<GameObject> _listGesture = new List<GameObject>();
    [SerializeField] 
    private const int _enemyNum = 60;

    public GameObject[] _perfectEffect, _greatEffect, _goodEffect, _missEffect;
    // Start is called before the first frame updatee
    void Start()
    {
        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
        InitVariable();
        
        //Enemy 생성
        for (int i = 0; i < _enemyNum; i++)
        {
            GameObject tmpEnemy = Instantiate(_enemy, _enemyTr);
            tmpEnemy.name = "Enemy[" + i + "]";
            tmpEnemy.GetComponent<EnemyBase>()._uiCamera = this._uiCamera;
            _listEnemy.Add(tmpEnemy);
            _listEnemy[i].gameObject.SetActive(false);
        }

        //EnemyGesture 생성
        for (int i = 0; i < _enemyNum; i++)
        {
            GameObject tmpGesture = Instantiate(_gesture, _gestureTr);
            tmpGesture.name = "Gesture[" + i + "]";
            //tmpGesture.GetComponent<EnemyBase>()._uiCanvas.worldCamera = this._uiCamera;
            _listGesture.Add(tmpGesture);
            _listGesture[i].gameObject.SetActive(false);
        }
    }
    private void OnCheaterDetected()
    {
        _cheaterDetected = true;
    }

    
    public void InitVariable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_cheaterDetected) Application.Quit();
    }

    public void SetEnemy(int gameLev)
    {
        for (int i = 0; i < _enemyNum; i++)
        {
            if(_listEnemy[i].gameObject.activeSelf == false)  // i = 0 테스트 할라고 하나만 심어놓음
            {
                //EnemyBase의 gestureGroup을 따로 떼어내면서 새로 생성해준 Gesture그룹을 연결해준다.
                _listEnemy[i].GetComponent<EnemyBase>()._gestureGroup = _listGesture[i].GetComponent<GestureBase>()._gestureGroup;
                _listEnemy[i].GetComponent<EnemyBase>().SetEnemyInit(gameLev);
                return;
            }
        }
    }

    public void KillEnemy() //테스트 함수
    {
        List<GameObject> tmpEnemy = new List<GameObject>();
        for (int i = 0; i < _enemyNum; i++)
        {
            if (_listEnemy[i].gameObject.activeSelf == true)
            {
                tmpEnemy.Add(_listEnemy[i]);
            }
        }

        int tmpIndex = Random.Range(0, tmpEnemy.Count);
        tmpEnemy[tmpIndex].gameObject.SetActive(false);
    }

    public void StopEnemy()
    {
        for (int i = 0; i < _enemyNum; i++)
        {
            if (_listEnemy[i].gameObject.activeSelf == true)
            {
                _listEnemy[i].GetComponent<EnemyBase>().NONE_Enemy();
            }
        }
    }

    /// <summary>
    /// 제스쳐 한번 그릴때 Enemy들의 제스쳐가 있는지 없는지 체크
    /// </summary>
    /// <param name="result"></param>
    /// <param name="score"></param>
    public void DrawGestureResultCheck(string result, float score)
    {
        Debug.Log("result : " + result);
        //List<GameObject> tmpEnemy = new List<GameObject>();
        bool IsCheck = false;
        for (int i = 0; i < _enemyNum; i++)
        {
            if (_listEnemy[i].gameObject.activeSelf == true)
            {
                //tmpEnemy.Add(_listEnemy[i]);
                if(_listEnemy[i].GetComponent<EnemyBase>().DrawGesture(result)) IsCheck = true;
            }
        }

        if (IsCheck)
        {
            OnTextEffect(score);
        }
        else
        {
            OnTextEffectMiss();
        }
    }

    public void DrawGestureScoreCheck()
    {

    }

    public void OnTextEffect(float score)
    {
        //+시간
        Debug.Log("+ 얼마일까? " + _gameSceneScontroller.PlusTimeOffset * score);

        _gameSceneScontroller.TotalTime += _gameSceneScontroller.PlusTimeOffset * score;
        if (_gameSceneScontroller.TotalTime >= 60f) _gameSceneScontroller.TotalTime = 60f;
        //이펙트 출력
        if (score >= _gameSceneScontroller.PerfectPct)
        {
            for(int i = 0; i < _perfectEffect.Length; i++)
            {
                if(!_perfectEffect[i].activeSelf)
                {
                    _perfectEffect[i].SetActive(true);
                    _perfectEffect[i].transform.SetAsLastSibling();
                    SumScore(2);
                    break;
                }
            }
        }
        else if(score >= _gameSceneScontroller.GreatPct)
        {
            for (int i = 0; i < _greatEffect.Length; i++)
            {
                if (!_greatEffect[i].activeSelf)
                {
                    _greatEffect[i].SetActive(true);
                    _greatEffect[i].transform.SetAsLastSibling();
                    SumScore(1);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < _goodEffect.Length; i++)
            {
                if (!_goodEffect[i].activeSelf)
                {
                    _goodEffect[i].SetActive(true);
                    _goodEffect[i].transform.SetAsLastSibling();
                    SumScore(0);
                    break;
                }
            }
        }
    }

    public void OnTextEffectMiss()
    {
        for (int i = 0; i < _missEffect.Length; i++)
        {
            if (!_missEffect[i].activeSelf)
            {
                _missEffect[i].SetActive(true);
                _missEffect[i].transform.SetAsLastSibling();
                break;
            }
        }
    }

    /// <summary>
    /// 0 : good, 1: great, 2:perfect
    /// </summary>
    /// <param name="type"></param>
    public void SumScore(int type)
    {
        switch(type)
        {
            case 0:
                _gameSceneScontroller.MyScore += _gameSceneScontroller.GoodScore;
                break;
            case 1:
                _gameSceneScontroller.MyScore += _gameSceneScontroller.GreatScore;
                break;
            case 2:
                _gameSceneScontroller.MyScore += _gameSceneScontroller.PerfectScore;
                break;
        }
    }
}
