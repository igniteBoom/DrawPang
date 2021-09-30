using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public GameObject _enemy;
    private List<GameObject> _listEnemy = new List<GameObject>();
    private const int _enemyNum = 60;
    // Start is called before the first frame updatee
    void Start()
    {
        for (int i = 0; i < _enemyNum; i++)
        {
            GameObject tmpEnemy = Instantiate(_enemy, this.transform);
            tmpEnemy.name = "Enemy[" + i + "]";
            _listEnemy.Add(tmpEnemy);
            _listEnemy[i].gameObject.SetActive(false);
        }
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
    // Update is called once per frame
    void Update()
    {        
        //position -= 0.1f;
        //animator.speed = 1f;
        //_enemies.localPosition = new Vector3(position, _enemies.localPosition.y, position);
    }

    public void DrawGesture(string result)
    {
        Debug.Log("result : " + result);
        List<GameObject> tmpEnemy = new List<GameObject>();
        for (int i = 0; i < _enemyNum; i++)
        {
            if (_listEnemy[i].gameObject.activeSelf == true)
            {
                tmpEnemy.Add(_listEnemy[i]);
            }
        }

        for (int i = 0; i < tmpEnemy.Count; i++)
        {
            tmpEnemy[i].GetComponent<EnemyBase>().DrawGesture(result);
        }
    }
}
