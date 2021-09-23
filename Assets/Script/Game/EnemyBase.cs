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
    private Animator _aniState;
    private Transform _pos;
    public float _speed = 1f;
    public float _knockback;
    public float _knockbackinit = 1f; // ~5
    public float _knockbackP = 0.95f;
    public RectTransform _gestureTransform;
    public Camera _mainCamera;

    private void OnEnable()
    {
        _aniState = gameObject.GetComponent<Animator>();
        _pos = transform;

        StartCoroutine("CheckState");
        StartCoroutine("CheckAction");

        _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {   
        Vector3 p = _mainCamera.WorldToScreenPoint(gameObject.transform.localPosition);
        _gestureTransform.anchoredPosition3D = new Vector3(p.x, p.y - 60f);
    }
    public void NONE_Enemy()
    {
        _enemyState = ENEMYSTATE.NONE;
        _aniState.SetInteger("animation", 0);
    }

    public void MOVE_Enemy()
    {
        _enemyState = ENEMYSTATE.MOVE;
        _aniState.SetInteger("animation", 1);
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

    IEnumerator CheckState()
    {
        while(!_isDie)
        {
            if (_enemyState == ENEMYSTATE.DIE) yield break;
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
                    _pos.localPosition = new Vector3(_pos.localPosition.x - _speed / 20, _pos.localPosition.y, _pos.localPosition.z - _speed / 20);
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
}
