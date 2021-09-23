using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public Transform _enemies;
    float position = 100.0f;
    public Animator animator;

    public GameObject _enemy;
    private List<GameObject> _listEnemy = new List<GameObject>();
    private const int _enemyNum = 60;
    // Start is called before the first frame updatee
    void Start()
    {
        for (int i = 0; i < _enemyNum; i++)
        {
            GameObject tmpEnemy = Instantiate(_enemy, this.transform);
            _listEnemy.Add(tmpEnemy);
            _listEnemy[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {        
        //position -= 0.1f;
        //animator.speed = 1f;
        //_enemies.localPosition = new Vector3(position, _enemies.localPosition.y, position);
    }
}
