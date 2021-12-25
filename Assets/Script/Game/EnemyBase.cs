using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        NONE = 0,
        MOVE,
        ATTACK,
        DAMAGE,
        DIE
    }

    public ENEMYSTATE _enemyState = ENEMYSTATE.NONE;
    public bool _isDie = false;
    public GestureGroup _gestureGroup;
    public Animator _aniState;
    private Transform _pos;
    public float _speed = 0.01f;
    public float _knockback;
    public float _knockbackinit = 1f; // ~5
    public float _knockbackP = 0.95f;
    public RectTransform _gestureTransform;
    public RectTransform _gestureNumberTransform;
    public Camera _mainCamera;
    public GameObject[] _onObj;
    public bool _isRespawnDirUp;
    private Vector3 _movement;
    private Rigidbody _rigdbody;

    private void Awake()
    {
        _rigdbody = _onObj[0].GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        //_aniState = _onObj[0].gameObject.GetComponent<Animator>();
        _pos = _onObj[0].transform;
        _isDie = false;

        StartCoroutine("CheckState");
        StartCoroutine("CheckAction");

        _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 p = _mainCamera.WorldToScreenPoint(_onObj[0].gameObject.transform.position);
        Debug.Log("초기 위치 : " + p);

        _gestureTransform.anchoredPosition3D = new Vector3(p.x, p.y, 0);// ; - 10f);// - 60f);
        _gestureNumberTransform.anchoredPosition3D = new Vector3(p.x, p.y, 0); //new Vector3(p.x + 40f, p.y + 60f);

        if (_gestureGroup.EnemyLife <= 0) _enemyState = ENEMYSTATE.DIE;
    }

    public void SetEnemyInit(int gameLev)
    {
        OnOffEnemyRender(false);
        this.gameObject.SetActive(true);
        if (Random.Range(0, 2) == 0)
        {
            _onObj[0].transform.localPosition = new Vector3(Random.Range(-1.6f, 1.6f), -3, 0);  // -0.9
            _isRespawnDirUp = false;
        }
        else
        {
            _onObj[0].transform.localPosition = new Vector3(Random.Range(-1.6f, 1.6f), 3, 0);  // 0.3
            _isRespawnDirUp = true;
        }
        Debug.Log("초기 포지션? : " + _onObj[0].transform.localPosition);
        _gestureGroup.InitGestureGroup();
                
        StartCoroutine(activeEnemy());
        MOVE_Enemy();
    }
    IEnumerator activeEnemy()
    {
        yield return new WaitForEndOfFrame();
        OnOffEnemyRender(true);
    }

    public void OnOffEnemyRender(bool isOnOff)
    {
        foreach(var item in _onObj)
        {
            item.SetActive(isOnOff);
        }
    }

    public void NONE_Enemy()
    {
        _enemyState = ENEMYSTATE.NONE;
        _aniState.SetInteger("animation", 0);
    }

    public void MOVE_Enemy()
    {
        _enemyState = ENEMYSTATE.MOVE;
        _aniState.SetInteger("animation", 0);
    }

    public void ATTACK_Enemy()
    {
        _enemyState = ENEMYSTATE.ATTACK;
        _aniState.SetInteger("animation", 2);
    }

    public void DAMAGE_Enemy()
    {
        _knockback = _knockbackinit;
        _enemyState = ENEMYSTATE.DAMAGE;
        _aniState.SetInteger("animation", 3);
    }

    public void DIE_Enemy()
    {
        _enemyState = ENEMYSTATE.DIE;
        _aniState.SetInteger("animation", 4);
    }

    IEnumerator Die_fade()
    {
        yield return null;
    }
    IEnumerator CheckState()
    {
        while(!_isDie)
        {
            if (_enemyState == ENEMYSTATE.DIE)
            {
                DIE_Enemy();// yield break;
                if(_aniState.GetCurrentAnimatorStateInfo(0).IsName("Die4") && _aniState.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
                {
                    _isDie = true;
                    this.gameObject.SetActive(false);
                }
            }
            if (_aniState.GetCurrentAnimatorStateInfo(0).IsName("Damage3") && _aniState.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                _enemyState = ENEMYSTATE.MOVE;
                _aniState.SetInteger("animation", 1);
            }
            yield return null;
        }
    }

    IEnumerator CheckAction()
    {
        while(!_isDie)
        {
            yield return null;

            switch(_enemyState)
            {
                case ENEMYSTATE.NONE:
                    break;
                case ENEMYSTATE.MOVE:
                    if (_isRespawnDirUp)
                        _onObj[0].transform.localPosition = Vector3.MoveTowards(_onObj[0].transform.transform.localPosition, new Vector3(0, 0.3f, 0f), Time.deltaTime * _speed);
                    else _onObj[0].transform.localPosition = Vector3.MoveTowards(_onObj[0].transform.transform.localPosition, new Vector3(0, -0.9f, 0f), Time.deltaTime * _speed);
                    break;
                case ENEMYSTATE.ATTACK:
                    break;
                case ENEMYSTATE.DAMAGE:
                    _pos.localPosition = new Vector3(_pos.localPosition.x + _knockback, _pos.localPosition.y, _pos.localPosition.z + _knockback);
                    _knockback *= _knockbackP;
                    break;
                case ENEMYSTATE.DIE:
                    break;
                default:
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.parent.gameObject.name == "Player")
        {
            _enemyState = ENEMYSTATE.ATTACK ;
            Debug.Log("OnCollisionEnter");
        }
    }

    public bool DrawGesture(string result)
    {
        bool IsCollect = false;
        if (_gestureGroup)
        {
            _gestureGroup.DrawGesture(result, out IsCollect);
        }
        return IsCollect;
        //Debug.Log("EnemyBase DrawGesture : " + result);
    }
}
