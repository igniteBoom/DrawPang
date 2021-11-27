using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GestureGroup : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void InitGestureGroup()
    {
        _gestureNum = 10;
        for (int i = 0; i < _gestureNum; i++)
        {
            GameObject tmpEnemy = Instantiate(_gestureItem, this.transform);
            tmpEnemy.name = "GestureItem[" + i + "]";
            _GestureList.Add(tmpEnemy);
            _GestureList[i].gameObject.SetActive(false);
            _GestureList[i].GetComponent<GestureItem>().InitGestureItem(GestureItem.GESTURETYPE.LEV1);
        }

        int tmpIndex = _GestureList.Count - 2;          // 24 - 2 == 22
        for (int i = 0; i < _GestureList.Count; i++)
        {
            _GestureList[i].transform.localPosition = new Vector3(_GestureList[i].GetComponent<RectTransform>().rect.width * (i - tmpIndex)
                + (i - tmpIndex) * 10, 50, 0);            // 0 - 22 * width       23 - 22 * width
            if (i >= _GestureList.Count - 3)
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
        int tmpIndex = _GestureList.Count - 2;
        for (int i = 0; i < _GestureList.Count; i++)
        {
            _GestureList[i].GetComponent<RectTransform>().DOAnchorPosX(_GestureList[i].GetComponent<RectTransform>().rect.width * (i - tmpIndex) + (i - tmpIndex) * 10, 0.2f).SetEase(Ease.OutBounce);
            if (i > _GestureList.Count - 3)
            {
                _GestureList[i].SetActive(true);
            }
            else if(i == _GestureList.Count - 3)
            {
                StartCoroutine(delayFalseObject(0.1f, i));
                //_GestureList[i].SetActive(false);
            }
            else
            {
                _GestureList[i].SetActive(false);
            }
        }
    }

    IEnumerator delayFalseObject(float time, int index)
    {
        yield return new WaitForSeconds(time);
        _GestureList[index].SetActive(true);
    }
}
