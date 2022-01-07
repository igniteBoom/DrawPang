using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public EnemyBase _enemyBase;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter" + collision.gameObject.name);

        if(collision.gameObject.transform.parent.gameObject.name == "Player")
            _enemyBase._enemyState = EnemyBase.ENEMYSTATE.ATTACK;
        //if (collision.gameObject.transform.parent.gameObject.name == "Player")
        //{
        //    _enemyState = ENEMYSTATE.ATTACK;
        //    Debug.Log("OnCollisionEnter");
        //}
    }
}
