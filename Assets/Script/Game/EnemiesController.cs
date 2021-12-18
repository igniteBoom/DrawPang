using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;

public class EnemiesController : MonoBehaviour
{
    public GameSceneController _gameSceneScontroller;
    private bool _cheaterDetected = false;
    public GameObject _enemy;
    private List<GameObject> _listEnemy = new List<GameObject>();
    [SerializeField] 
    private const int _enemyNum = 60;

    public GameObject[] _perfectEffect, _greatEffect, _goodEffect, _missEffect;
    // Start is called before the first frame updatee
    void Start()
    {
        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
        InitVariable();

        for (int i = 0; i < _enemyNum; i++)
        {
            GameObject tmpEnemy = Instantiate(_enemy, this.transform);
            tmpEnemy.name = "Enemy[" + i + "]";
            _listEnemy.Add(tmpEnemy);
            _listEnemy[i].gameObject.SetActive(false);
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
            if(_listEnemy[i].gameObject.activeSelf == false)
            {
                _listEnemy[i].GetComponent<EnemyBase>().SetEnemyInit(gameLev);
                return;
            }
        }
    }

    public void KillEnemy()
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
