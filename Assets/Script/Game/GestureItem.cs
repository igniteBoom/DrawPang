using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;

public class GestureItem : MonoBehaviour
{
    public enum GESTURETYPE
    {
        LEV0 = 0, //없는레벨
        LEV1,
        LEV2,
        LEV3,
        LEV4,
        LEV1to2,
        LEV1to3,
        LEV1to4,
        LEV2to3,
        LEV2to4,
        LEV3to4,
    }
    public GESTURETYPE _gestureType;// = GESTURETYPE.LEV1;

    private bool _cheaterDetected = false;

    public ObscuredInt _gestureIndex;
    public Sprite[] _gestureSprite;
    private Image _myImage;

    private void OnCheaterDetected()
    {
        _cheaterDetected = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);        
    }

    // Update is called once per frame
    void Update()
    {
        if (_cheaterDetected) Application.Quit();
    }

    public void SetSize(float width, float height)
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public void InitGestureItem(GESTURETYPE gestureType)
    {
        _gestureType = gestureType;
        _myImage = this.GetComponent<Image>();
        if (null == _myImage)
        {
            Debug.Log("Sprite Component null");
            return;
        }

        switch (gestureType)
        {
            case GESTURETYPE.LEV0:
                Debug.LogError("쓰지 않는 Type");
                break;
            case GESTURETYPE.LEV1:
                _myImage.sprite = _gestureSprite[Random.Range(0, 4)];
                break;
            case GESTURETYPE.LEV2:
                _myImage.sprite = _gestureSprite[Random.Range(4, 8)];
                break;
            case GESTURETYPE.LEV3:
                _myImage.sprite = _gestureSprite[Random.Range(8, 11)];
                break;
            case GESTURETYPE.LEV4:
                _myImage.sprite = _gestureSprite[Random.Range(11, 13)];
                break;
            case GESTURETYPE.LEV1to2:
                _myImage.sprite = _gestureSprite[Random.Range(0, 8)];
                break;
            case GESTURETYPE.LEV1to3:
                _myImage.sprite = _gestureSprite[Random.Range(0, 11)];
                break;
            case GESTURETYPE.LEV1to4:
                _myImage.sprite = _gestureSprite[Random.Range(0, 13)];
                break;
            case GESTURETYPE.LEV2to3:
                _myImage.sprite = _gestureSprite[Random.Range(4, 11)];
                break;
            case GESTURETYPE.LEV2to4:
                _myImage.sprite = _gestureSprite[Random.Range(4, 13)];
                break;
            case GESTURETYPE.LEV3to4:
                _myImage.sprite = _gestureSprite[Random.Range(8, 13)];
                break;
        }
    }
}
