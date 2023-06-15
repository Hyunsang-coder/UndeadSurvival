using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int ID;
    [SerializeField] int prefabID;
    [SerializeField] float damage;
    [SerializeField] float shootingTimer;
    [SerializeField] int weaponCount;
    [SerializeField] int pierceCount;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.Instance.Player;

    }
    
    private void OnEnable() {
        player.OnShootingSpeedChanged += UpdateShootSpeed;
        
    }

    public  void UpdateShootSpeed(float speed){
        this.shootingTimer = speed;
    }

    private void Update()
    {
        if(!GameManager.Instance.isGameLive) return;
        
        switch(ID)
        {
            // shovel
            case 0:
                transform.Rotate(Vector3.back, 150 * Time.deltaTime);
                break;

            // bullet
            default:
                timer += Time.deltaTime;

                if(timer > shootingTimer)
                {
                    Fire();
                    timer = 0;
                }

                break;
        }

       
    }

    public void Init(ItemData data)
    {
        // Basic setup
        name = "Weapon" + data.itemID;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property setup + bonus 
        ID = data.itemID;
        damage = data.baseDamage * player.playerData[GameManager.Instance.playerID].damage;
        weaponCount = data.baseCount + player.playerData[GameManager.Instance.playerID].weaponCount;
        pierceCount = data.baseCount + player.playerData[GameManager.Instance.playerID].pierceCount;
        
        for (int index = 0; index < GameManager.Instance.Pool.PrefabArray.Length; index++)
        {
            if (data.prefab == GameManager.Instance.Pool.PrefabArray[index])
            {
                prefabID = index;
                break;
            }
        }

        switch(ID)
        {
            // 무기가 삽일 때
            case 0:
                Placement();
                break;

            // 무기가 총일 때 
            default:
                shootingTimer = player.shootingTimer;
                break;
        }

        // Weapon이 초기화 될때 손도 셋업 => melee = 0, range = 1
        Hand hand  = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.handSprite;
        hand.gameObject.SetActive(true);
    }

    public void LevelUp(float damageRate, int count)
    {
        damage = damage + (damage * damageRate);

        switch(ID)
        {
            // shovel
            case 0:
                weaponCount += count;
                Placement();
                break;

            // bullet
            default:
                pierceCount += count;
                break;
        }
        
    }

    void Placement()
    {

        if (ID == 1) return;

        for (int index = 0; index < weaponCount; index++)
        {
            Transform shovel;

            if (index < transform.childCount)
            {
                shovel = transform.GetChild(index);
            }
            else
            {
                shovel = GameManager.Instance.Pool.GetObject(prefabID).transform;
                shovel.parent = transform;
            }

            //위치와 로테이션 초기화 > 플레이어 위치로 이동
            shovel.localPosition = Vector3.zero;
            shovel.localRotation = Quaternion.identity;

            // Z축으로 조금씩 더 회전 + Y축으로 1.5만큼 이동 *Space.World는 부모의 로테이션과 상관없이 월드 기준 Y축 이동하기 위함 
            Vector3 rotVec = Vector3.forward * 360 * index / weaponCount;
            shovel.Rotate(rotVec);
            shovel.Translate(shovel.up * 1.5f, Space.World);

            shovel.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -1은 무한 관통, 방향은 임의의 수 할
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
        bullet.GetComponent<Bullet>().Init(damage, pierceCount, dir);

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }

    private void OnDisable() {
        player.OnShootingSpeedChanged -= UpdateShootSpeed;
    }

}
