using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum DungeonBossAttackState {Idle, Punch, Lazer, Rocket, Speed, Deff, Buff}
public class DungeonBossAttack : MonoBehaviour
{
    public Transform target;
    public Transform firePoint;
    public GameObject projectile;
    public DungeonBossAttackState dBossAttackState;
    private DungeonBoss dungeonBoss;
    [SerializeField] private GameObject puchCollider;
    [SerializeField] private CapsuleCollider2D defCollider;
    [SerializeField] private PolygonCollider2D bossCollider;
    public int random;
    public float skillCooldown = 5f;
    private float currentCooldown;
    public bool canFire = false;
    public float punchCooldown = 2f;
    private float currentPunchCooldown;
    public bool canPunch = false;

    // Start is called before the first frame update
    void Start()
    {
        dBossAttackState = DungeonBossAttackState.Idle;
        currentCooldown = skillCooldown;
        currentCooldown = punchCooldown;
        dungeonBoss = gameObject.GetComponent<DungeonBoss>();
    }
    void Update()
    {
        CooldownSkill();
    }
    private  void CooldownSkill()
    {
        //Cooldown skill
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            canFire = true;
            random = Random.Range(0, 3);
            CheckAttack();
            currentCooldown = skillCooldown;
        }
        //Cooldown punch
        if (currentPunchCooldown > 0)
        {
            currentPunchCooldown -= Time.deltaTime;
        }
        else
        {
            canPunch = true;
            Punch();
            currentPunchCooldown = punchCooldown;
        }
    }
    private void Punch()
    {
        float distance = Vector3.Distance(gameObject.transform.position,
            dungeonBoss.player.transform.position);
        if (distance <= dungeonBoss.distanceAttack
            && dungeonBoss.dBossState == DungeonBossState.Move)
        {
            bossCollider.enabled = true;
            defCollider.enabled = false;
            dBossAttackState = DungeonBossAttackState.Punch;
            dungeonBoss.dBossState = DungeonBossState.Idle;
            dungeonBoss.anim.SetTrigger("Punch");
            canPunch = false;
            Invoke("DelayToMove", 1f);
        }
    }
    private void CheckAttack()
    {
        float distance = Vector3.Distance(gameObject.transform.position,
            dungeonBoss.player.transform.position);
        if (distance > dungeonBoss.distanceAttack
            && distance < dungeonBoss.distanceView)
        {
            //dungeonBoss.anim.SetTrigger("Exit Deff");
            //bossCollider.enabled = true;
            //defCollider.enabled = false;
            //Script for random weapon between roket and lazer
            //if random = 0 do lazer, random = 1 do rocket, random = 3 do speed 
            switch (random)
            {
                case 0:
                    //Lazer
                    dBossAttackState = DungeonBossAttackState.Lazer;
                    if (dBossAttackState == DungeonBossAttackState.Lazer
                        && dungeonBoss.dBossState == DungeonBossState.Move && canFire == true)
                    {
                        dungeonBoss.dBossState = DungeonBossState.Idle;
                        dungeonBoss.anim.SetTrigger("Lazer");
                        canFire = false;
                        Invoke("DelayToMove", 3.5f);
                    }
                    Debug.Log("Lazer");
                    break;
                case 1:
                    //Fire Rocket
                    dBossAttackState = DungeonBossAttackState.Rocket;
                    if (dBossAttackState == DungeonBossAttackState.Rocket
                        && dungeonBoss.dBossState == DungeonBossState.Move && canFire == true)
                    {
                        dungeonBoss.dBossState = DungeonBossState.Idle;
                        dungeonBoss.anim.SetTrigger("Rocket");
                        Invoke("FireRocket", 1f);
                        canFire = false;
                        Invoke("DelayToMove", 2f);
                    }
                    Debug.Log("Rocket");
                    break;
                case 2:
                    //Speed
                    dBossAttackState = DungeonBossAttackState.Speed;
                    if (dBossAttackState == DungeonBossAttackState.Speed
                        && dungeonBoss.dBossState == DungeonBossState.Move && canFire == true)
                    {
                        dungeonBoss.anim.SetTrigger("Speed");
                        dungeonBoss.speed = 4;
                        canFire = false;
                        Invoke("DelayToMove", 1f);
                    }
                    Debug.Log("Speed");
                    break;
                default:
                    Debug.Log("Not fire");
                    break;
            }
        }
            //else if(distance > dungeonBoss.distanceView)
            //{
            //    bossCollider.enabled = false;
            //    defCollider.enabled = true;
            //    Defence();
            //}
    }

    //to delay boss movement when they action
    private void DelayToMove()
    {
        dBossAttackState = DungeonBossAttackState.Idle;
        dungeonBoss.dBossState = DungeonBossState.Move;
        dungeonBoss.anim.SetTrigger("Movement");
        dungeonBoss.speed = 2;
    }

    //to fire the rocket
    private void FireRocket()
    {
        Instantiate(projectile, firePoint.position, Quaternion.identity).GetComponent<Rocket>();
    }

    //to buff the boss amor when boss health's is low
    private void Buff()
    {
        //if the boss health low, increase the amor
        dungeonBoss.dBossAmor = 1;
    }

    //to defence and summon some tiny enemy to attack the player
    private void Defence()
    {
        dungeonBoss.dBossState = DungeonBossState.Idle;
        dBossAttackState = DungeonBossAttackState.Deff;
        dungeonBoss.anim.SetTrigger("Deff");
    }
}
