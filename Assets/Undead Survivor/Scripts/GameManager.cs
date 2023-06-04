using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Player Player { get; private set; }
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        Player = FindObjectOfType<Player>().GetComponent<Player>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
