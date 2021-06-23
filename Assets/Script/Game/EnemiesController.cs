using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public Transform _enemies;
    float position = 100.0f;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        //position -= 0.1f;
        //animator.speed = 1f;
        //_enemies.localPosition = new Vector3(position, _enemies.localPosition.y, position);
    }
}
