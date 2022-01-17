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
    public Transform _pos;
    public Vector3 _regenPos;
    public float _speed, _knockback, _knockbackinit, _knockbackP;
    public RectTransform _gestureTransform;
    public RectTransform _gestureNumberTransform;
    public Camera _uiCamera;

    private void Awake()
    {
        //_rigdbody = _onObj[0].GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        InitVariable();

        StartCoroutine("CheckState");
        StartCoroutine("CheckAction");

        _uiCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
    public void InitVariable()
    {
        _isDie = false;

        _speed = 5f;
        _knockback = 0f;
        _knockbackinit = 0.005f; // ~5
        _knockbackP = 0.95f;
    }

    private void Update()
    {
        Vector3 p = _uiCamera.WorldToScreenPoint(_pos.position);
        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameSceneController._instance._rectTranfromCanvasUI, p,_uiCamera, out localPos);

        _gestureGroup.GetComponent<RectTransform>().anchoredPosition3D = new Vector2(localPos.x, localPos.y - 20);
        _gestureGroup._gestureNumberTransform.GetComponent<RectTransform>().anchoredPosition3D = new Vector2(localPos.x + 50, localPos.y + 70);

        if (_gestureGroup.gameObject.activeSelf && _gestureGroup.EnemyLife <= 0) _enemyState = ENEMYSTATE.DIE;
    }

    public void SetEnemyInit(int gameLev)
    {
        _pos.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        if (Random.Range(0, 2) == 0)
        {
            _regenPos = new Vector3(Random.Range(-37, 37), -70, 0);  // -0.9
            _pos.localPosition = _regenPos;  // -0.9
        }
        else
        {
            _regenPos = new Vector3(Random.Range(-37, 37), 50, 0);  // 0.3
            _pos.localPosition = _regenPos;  // 0.3
        }
        Debug.Log("초기 포지션? : " + _pos.localPosition);
        _gestureGroup.InitGestureGroup();
                
        StartCoroutine(activeEnemy());
        MOVE_Enemy();
    }

    /// <summary>
    /// Enemy가 생성되고 위치가 바뀌는게 보여서 만듬
    /// </summary>
    /// <returns></returns>
    IEnumerator activeEnemy()
    {
        yield return new WaitForEndOfFrame();
        _gestureGroup._gestureBase.gameObject.SetActive(true);
        _pos.gameObject.SetActive(true);
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
        if (_aniState.GetCurrentAnimatorStateInfo(0).IsName("Damage3") && _aniState.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
        {
            _aniState.Play("Damage3", -1, 0);
        }
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
                    _gestureGroup._gestureBase.gameObject.SetActive(false);
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
                    /* 콜라이더로 해결!
                    float tmpx, tmpy;
                    float dis = 10; //원점에서의 거리

                    Vector3 v = _regenPos - new Vector3(1f, 0f);                    
                    tmpx = dis * Mathf.Cos(Mathf.Atan2(v.y, v.x));
                    tmpy = dis * Mathf.Sin(Mathf.Atan2(v.y, v.x));
                    */
                    _pos.localPosition = Vector3.MoveTowards(_pos.transform.localPosition, new Vector3(0f, 0f, 0f), Time.deltaTime * _speed);
                    break;
                case ENEMYSTATE.ATTACK:
                    ATTACK_Enemy();
                    break;
                case ENEMYSTATE.DAMAGE:
                    Vector3 v = new Vector3(0f, 0f, 0f) - _regenPos;
                    v = v * _knockback;
                    _pos.localPosition = _pos.localPosition - v;
                    //if(_pos.localPosition.x > 0)
                    //    _pos.localPosition = new Vector3(_pos.localPosition.x + _knockback, _pos.localPosition.y, _pos.localPosition.z);
                    //else if(_pos.localPosition.x < 0)
                    //    _pos.localPosition = new Vector3(_pos.localPosition.x - _knockback, _pos.localPosition.y, _pos.localPosition.z);
                    //if(_pos.localPosition.y > 0)
                    //    _pos.localPosition = new Vector3(_pos.localPosition.x, _pos.localPosition.y + _knockback, _pos.localPosition.z);
                    //else if(_pos.localPosition.y < 0)
                    //    _pos.localPosition = new Vector3(_pos.localPosition.x, _pos.localPosition.y - _knockback, _pos.localPosition.z);
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
        Debug.Log("OnCollisionEnter" + collision.gameObject.name);
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
            if (IsCollect) DAMAGE_Enemy();
        }
        return IsCollect;
        //Debug.Log("EnemyBase DrawGesture : " + result);
    }
}
