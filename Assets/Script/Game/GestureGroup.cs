using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GestureGroup : MonoBehaviour
{
    public List<GameObject> _GestureList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        int tmpIndex = _GestureList.Count - 2;          // 24 - 2 == 22
        for(int i = 0; i < _GestureList.Count; i++)
        {
            _GestureList[i].transform.localPosition = new Vector3(_GestureList[i].GetComponent<RectTransform>().rect.width * (i - tmpIndex)
                + (i - tmpIndex) * 10, 0,0);            // 0 - 22 * width       23 - 22 * width
            if(i >= _GestureList.Count - 3)
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
