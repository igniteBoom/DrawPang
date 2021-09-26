using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;
 
public class GameSceneController : MonoBehaviour
{
    // 위 아래 20개씩 오브젝트풀
    // 몬스터 생성을 어떻게 해야할까?? mesh, materials, 제스쳐 갯수, 제스쳐 난이도
    // Start is called before the first  frame update
    public ObscuredInt _gameLev = 5;
    private bool _cheaterDetected = false;
    
    public EnemiesController _enemiesController;
    public PlayerController _playerController;
    private float _levTime = 5.0f; 
    void Start()
    {
        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
    }
    private void OnCheaterDetected()
    {
        _cheaterDetected = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cheaterDetected) Application.Quit();

        if (_levTime > 0)
            _levTime -= Time.deltaTime;
        else if (_levTime <= 0)
        {
            _enemiesController.SetEnemy(_gameLev);
            _levTime = 5.0f;
            //Time.timeScale = 5.0f;
        }

        Debug.Log("time : " + _levTime);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _enemiesController.KillEnemy();
        }
    }
}
