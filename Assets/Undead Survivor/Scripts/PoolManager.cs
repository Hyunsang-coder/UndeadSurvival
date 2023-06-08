using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩 보관 변수
    public GameObject[] PrefabArray;

    // pool 담당 리스트
    [SerializeField] List<GameObject>[] poolArray;

    private void Awake()
    {
        // 배열과 리스트 초기화 
        poolArray = new List<GameObject>[PrefabArray.Length];

        for (int i = 0; i < poolArray.Length; i++)
        {
            poolArray[i] = new List<GameObject>();
        }
    }

    public GameObject GetObject(int index)
    {
        GameObject selected = null;

        // select an object that is not used

        // if there is one, assign it to selected

        foreach (GameObject item in poolArray[index])
        {
            if (!item.activeSelf) {
                selected = item;
                selected.SetActive(true);
                break;
            }
        }


        // if not, create a new one and assign it to selected 
        if (!selected) {
            selected = Instantiate(PrefabArray[index], transform);
        }

        //pool에 등록!
        poolArray[index].Add(selected);

        return selected;
    }

}
