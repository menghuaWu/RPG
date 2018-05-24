    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {


    public GameObject HitParticle;
    public GameObject DeadLife;
    public float totalHealth;
    public Animator Mummyani;
    public float expGranted;
    public float atkDamage;
    public float atkSpeed;
    public float moveSpeed;
    public bool dead;
    public bool walk;
    public bool attack;
    public const int AI_MOVE_DISTANCE = 5;
    public const int AI_ATTACK_DISTANCE = 1;

    GameObject life;
    GameObject[] playerPCTRL;
    private float currentHealth;

    PlayerController pc;


    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
        AnimationEvent.OnSlashAnimationPlayerHit += DamagePlayer;

        playerPCTRL = GameObject.FindGameObjectsWithTag("Player");

        currentHealth = totalHealth;

    }
	
	// Update is called once per frame
	void Update () {

        if (!dead && (Vector3.Distance(transform.position, playerPCTRL[0].transform.position) <= AI_MOVE_DISTANCE && Vector3.Distance(transform.position, playerPCTRL[0].transform.position) > AI_ATTACK_DISTANCE))
        {
           
            MoveToPlayer();
        }
        if (!dead && Vector3.Distance(transform.position, playerPCTRL[0].transform.position) <= AI_ATTACK_DISTANCE)
        {
            StartCoroutine(AttackRunTime());            
        }
        
        
	}

    void MoveToPlayer()
    {
        walk = true;
        Mummyani.SetBool("Walk", walk);
        transform.LookAt(playerPCTRL[0].transform);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void GetHit(float damage)
    {
        if (dead)
        {
            return;
        }
        Mummyani.SetBool("BeDamage", true);
        GameObject part = Instantiate(HitParticle,gameObject.transform);
        currentHealth -= damage;
        totalHealth = currentHealth;
        if (totalHealth <= 0)
        {
            totalHealth = 0;
            Die();
            return;
        }
        StartCoroutine(RecoverForHit());
        Destroy(part,1);
    }

    void Die()
    {
        dead = true;
        DropLoot();
        foreach (GameObject pc in playerPCTRL)
        {
            pc.GetComponent<PlayerController>().SetExperience(expGranted / playerPCTRL.Length);
        }
        Mummyani.SetBool("Dead",dead);
        life = Instantiate(DeadLife, gameObject.transform);
        Destroy(gameObject, 3f);
               

    }
    void DropLoot()
    {
        // Get money
    }

    

    IEnumerator RecoverForHit()
    {
        yield return new WaitForSeconds(0.1f);
        Mummyani.SetBool("BeDamage", false);
    }

    IEnumerator AttackRunTime()
    {
        walk = false;
        attack = true;
        Mummyani.SetBool("Walk", walk);
        Mummyani.SetBool("Attack", attack);
        
        yield return new WaitForSeconds(1/atkSpeed);
        attack = false;
        Mummyani.SetBool("Attack", attack);
    }

    void DamagePlayer()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pc.GetHit(atkDamage);
    }

    private void OnDestroy()
    {
        Destroy(life, 1);
    }
}
