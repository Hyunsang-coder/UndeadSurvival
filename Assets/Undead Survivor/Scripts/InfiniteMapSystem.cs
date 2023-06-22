using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMapSystem : MonoBehaviour
{
    Collider2D coll2D;

    private void Awake()
    {
        coll2D = GetComponent<Collider2D>();   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 다른 오브젝트와 콜리전 시 return
        if (!collision.CompareTag("Area")) return;
        

        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 transformPos = transform.position;

        // player area와 타일 간의 차이 
        float diffX = Mathf.Abs(playerPos.x - transformPos.x);
        float diffY = Mathf.Abs(playerPos.y - transformPos.y);


        // 인풋 대각선 값이 normalized 되었기 때문에 값을 1과 -1로 다시 바꿔 주기 
        float dirX = playerPos.x - transformPos.x < 0 ? -1 : 1;
        float dirY = playerPos.y - transformPos.y < 0 ? -1 : 1;


        switch(transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    // 40은 타일 맵 2개 너비
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if(diffY> diffX)
                {
                    // 40은 타일 맵 2개 너비
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            
            case "Enemy":
                if (coll2D.enabled)
                {
                    transform.Translate((playerPos - transform.position).normalized * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }

}
