using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int ID;
    [SerializeField] int prefabID;
    [SerializeField] float damage;
    [SerializeField] float rotationSpeed;

    [SerializeField] float splitTime;
    [SerializeField] int meleeWeaponCount;

    float timer;
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();        
    }
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        
        switch(ID)
        {
            case 0:
                transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if(timer > splitTime)
                {
                    Fire();
                    timer = 0;
                }

                break;
        }

        //test

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelUp(5f, 2);
        }
    }

    public void Init()
    {
        switch(ID)
        {
            case 0:
                //rotationSpeed = 150f;
                Placement();
                break;
            default:
                //timePerCount = 0.3f;
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage += damage;
        this.meleeWeaponCount += count;
        Placement();
    }

    void Placement()
    {

        if (ID == 1) return;
        for (int index = 0; index < meleeWeaponCount; index++)
        {
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.Pool.GetObject(prefabID).transform;
                bullet.parent = transform;
            }

            //위치와 로테이션 초기화 > 플레이어 위치로 이동
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // Z축으로 조금씩 더 회전 + Y축으로 1.5만큼 이동 *Space.World는 부모의 로테이션과 상관없이 월드 기준 Y축 이동하기 위함 
            Vector3 rotVec = Vector3.forward * 360 * index / meleeWeaponCount;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1은 무한 관통, 방향은 임의의 수 할
        }
    }

    void Fire()
    {
        if (!player.scanner.nearesttarget) return;

        Vector3 targetPos = player.scanner.nearesttarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;

        Transform bullet = GameManager.Instance.Pool.GetObject(prefabID).transform;
        bullet.position = transform.position;

        // Z축을 기준으로 dir 방향으로 rotate 
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, meleeWeaponCount, dir);
    }
}
