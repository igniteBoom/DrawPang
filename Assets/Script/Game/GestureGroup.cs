﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GestureGroup : MonoBehaviour
{
    public GestureBase _gestureBase;
    public GameObject _gestureNumberTransform;
    private List<GameObject> _GestureList = new List<GameObject>();
    public int EnemyLife { get { return _GestureList.Count; } }
    public string CurrentGesture
    { 
        get 
        {
            if (_GestureList.Count > 0)
            {
                return _GestureList[_GestureList.Count - 1].GetComponent<Image>().sprite.name;
            }
            else return null;
        }
    }
    public GameObject _gestureItem;
    public int _gestureNum;
    public int _seeGestureNum;
    public TextMeshProUGUI _numberText;
    private void OnEnable()
    {
    }

    public void InitGestureGroup()
    {
        _gestureNum = 10;
        _seeGestureNum = 2;
        for (int i = 0; i < _gestureNum; i++)
        {
            GameObject tmpEnemy = Instantiate(_gestureItem, this.transform);
            tmpEnemy.name = "GestureItem[" + i + "]";
            _GestureList.Add(tmpEnemy);
            _GestureList[i].gameObject.SetActive(false);
            _GestureList[i].GetComponent<GestureItem>().InitGestureItem(GestureItem.GESTURETYPE.LEV1);
        }

        int tmpIndex = _GestureList.Count - 1;         // 24 - 2 == 22
        _numberText.text = _GestureList.Count.ToString();
        for (int i = 0; i < _GestureList.Count; i++)
        {
            _GestureList[i].transform.localPosition = new Vector3(_GestureList[i].GetComponent<RectTransform>().rect.width * (i - tmpIndex)
                + (i - tmpIndex) * 10, 0, 0);            // 0 - 22 * width       23 - 22 * width
            if (i >= _GestureList.Count - _seeGestureNum)
            {
                _GestureList[i].SetActive(true);
            }
            else
            {
                _GestureList[i].SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    void SortGestures(List<GameObject> gestureList)
    {
        //gestureList[0].transform.localPosition = new Vector3(250f, );
    }

    public void deleteGesture()
    {
        if (_GestureList.Count > 0)
        {
            Destroy(_GestureList[_GestureList.Count - 1]);
            _GestureList.RemoveAt(_GestureList.Count - 1);

            moveGesture();
        }
    }

    /// <summary>
    /// 제스쳐가 Enemy의 첫번째 제스쳐와 같은지 체크
    /// </summary>
    /// <param name="result">sprite name이 같나?</param>
    /// <param name="IsCollect">그린 제스쳐 결과와 Enemy 제스쳐 비교 bool값</param>
    public void DrawGesture(string result, out bool IsCollect)
    {
        if (_GestureList.Count > 0 && _GestureList[_GestureList.Count - 1].GetComponent<Image>().sprite.name == result)
        {
            IsCollect = true;
            deleteGesture();
        }
        else IsCollect = false;
        //Debug.Log("GestureGroup DrawGesture : " + result);
        //Debug.Log("_GestureList[_GestureList.Count - 1].name : " + _GestureList[_GestureList.Count - 1].GetComponent<Image>().sprite.name);
    }
    private void moveGesture()
    {
        int tmpIndex = _GestureList.Count - 1;
        _numberText.text = _GestureList.Count.ToString();
        for (int i = 0; i < _GestureList.Count; i++)
        {
            Debug.Log("_GestureList.Count : " + _GestureList.Count);            
            if (i > _GestureList.Count - _seeGestureNum)
            {
                _GestureList[i].SetActive(true);
            }
            else if(i == _GestureList.Count - _seeGestureNum)
            {
                StartCoroutine(delayFalseObject(0.1f, i));
                //_GestureList[i].SetActive(false);
            }
            else
            {
                _GestureList[i].SetActive(false);
            }
            _GestureList[i].GetComponent<RectTransform>().DOAnchorPosX(_GestureList[i].GetComponent<RectTransform>().rect.width * (i - tmpIndex) + (i - tmpIndex) * 10, 0.2f).SetEase(Ease.OutExpo);
        }
    }

    IEnumerator delayFalseObject(float time, int index)
    {
        yield return new WaitForSeconds(time);
        _GestureList[index].SetActive(true);
    }
}
