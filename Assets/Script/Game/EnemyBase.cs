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
    private Animator _aniState;
    private Transform _pos;
    public float _speed = 10f;
    public float _knockback;
    public float _knockbackinit = 1f; // ~5
    public float _knockbackP = 0.95f;
    public RectTransform _gestureTransform;
    public Camera _mainCamera;
    private Vector3 _movement;
    private Rigidbody _rigdbody;

    private void Awake()
    {
        _rigdbody = this.GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _aniState = gameObject.GetComponent<Animator>();
        _pos = transform;
        _isDie = false;

        StartCoroutine("CheckState");
        StartCoroutine("CheckAction");

        _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {   
        Vector3 p = _mainCamera.WorldToScreenPoint(gameObject.transform.localPosition);
        _gestureTransform.anchoredPosition3D = new Vector3(p.x, p.y - 60f);

        if (_gestureGroup.EnemyLife <= 0) _enemyState = ENEMYSTATE.DIE;
    }

    public void SetEnemyInit(int gameLev)
    {
        this.gameObject.SetActive(true);
        if(Random.Range(0,2) == 0)
            this.transform.localPosition = new Vector3(Random.Range(-20, 20), 0, -30);
        else this.transform.localPosition = new Vector3(Random.Range(-20, 20), 0, 30);
        MOVE_Enemy();
        _gestureGroup.InitGestureGroup();
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
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, 0, -6.5f), Time.deltaTime * _speed);
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
