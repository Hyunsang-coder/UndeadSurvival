using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button button;
    GameObject parentObject;
    GameObject HUD;

    private void Awake() {
        button = GetComponent<Button>();
        parentObject = transform.parent.gameObject;
        HUD = FindObjectOfType<HUD>(true).transform.parent.gameObject;
    }
    void Start()
    {
        button.onClick.AddListener(()=>
        {
            parentObject.SetActive(false);
            HUD.SetActive(true);
            GameManager.Instance.GameStart();
        });
    }

}
