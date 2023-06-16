using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionEnemies : MonoBehaviour
{
   
    Collider2D coll2D;

    private void Awake()
    {
        coll2D = GetComponent<Collider2D>();   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 다른 오브젝트와 콜리전 시 return
        if (!collision.CompareTag("EnemyArea")) return;
        
        Debug.Log("Enmemy repositions step1" + ": " + transform.position.ToString());

        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 transformPos = transform.position;

                
        if (coll2D.enabled)
            {
                float distance = Vector3.Distance(playerPos, transformPos);
                transform.Translate((playerPos - transform.position).normalized * distance*2 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                Debug.Log("Enmemy repositions step2" + ": " + transform.position.ToString());
            }
        }
}
