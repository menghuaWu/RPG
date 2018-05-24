using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public int level = 1;
    public float Health;
    private float maxHealth;
    private Text levelText;
    public float Experience { get; private set; }
    private Transform experienceBar;

    [Header("Movement")]
    public bool canMove = false;
    public float moveSpeed;
    public float velocity;

    [Header("Combat")]
    public bool canAtt = false;
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;

    private Rigidbody playerRigidbody;
    private Animator playerAni;
    private float time = 0;


    private List<Transform> enemyInRange = new List<Transform>();

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        AnimationEvent.OnSlashAnimationHit += DealDamage;
        experienceBar = UIController.instance.transform.Find("BackGround/Experience");
        levelText = UIController.instance.transform.Find("BackGround/LevelText").GetComponent<Text>();
        maxHealth = 100;
        Health = maxHealth;

    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        GetInput();
        if (!canAtt)
        {
            Move();
        }
        
       
    }

    #region Get Keyboard key
    void GetInput()
    {
        if (Input.GetMouseButton(0))
        {
            
            SetAttack();
        }
               
        if (Input.GetKey(KeyCode.A))
        {
            
            SetVelocity(-1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            
            SetVelocity(0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            SetVelocity(1);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            SetVelocity(0);
        }

    }
    #endregion

    #region Movement
    void Move()
    {
        if (velocity == 0)
        {
            playerAni.SetInteger("Condition", 0);
            return;
        }
        else 
        {
            if (canMove)
            {

                playerAni.SetInteger("Condition", 1);
                playerRigidbody.MovePosition(transform.position + (Vector3.right * velocity * moveSpeed * Time.deltaTime));
            }
        }
         
    }

    void SetVelocity(float dir)
    {
        canMove = true;
        canAtt = false;
        if (dir < 0)
        {
            transform.LookAt(transform.position + Vector3.left);
        }
        else if (dir > 0)
        {
            transform.LookAt(transform.position + Vector3.right);
        }
        velocity = dir;
    }
    #endregion

    #region Combat
    void SetAttack()
    {
        if (canAtt)
        {
            return;
        }
        StartCoroutine(AttackRunTime());
        
    }

    void DealDamage()
    {
        GetEnemyInRange();
        foreach (Transform enemy in enemyInRange)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec == null)
            {
                continue;
            }
            ec.GetHit(attackDamage);

        }
    }

    IEnumerator AttackRunTime()
    {
        canMove = false;
        canAtt = true;
        playerAni.SetBool("Attack", canAtt);

        

        yield return new WaitForSeconds(1/ attackSpeed);
        canAtt = false;
        playerAni.SetBool("Attack", canAtt);
        time = 0;
        canMove = true;
    }

    void GetEnemyInRange()
    {
        enemyInRange.Clear();
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * 0.5f),0.5f))
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                enemyInRange.Add(c.transform);
            }
        }
    }
    #endregion

    public void SetExperience(float exp)
    {
        Experience += exp;
        float experienceNeeded = GameLogic.ExperienceForNextLevel(level);
        
        float previousExperience = GameLogic.ExperienceForNextLevel(level - 1);
        
        while (Experience >= experienceNeeded)
        {
            LevelUp();
            experienceNeeded = GameLogic.ExperienceForNextLevel(level);
            
            previousExperience = GameLogic.ExperienceForNextLevel(level - 1);
            
        }
        float fill = (Experience - previousExperience) / experienceNeeded;
        Debug.Log(fill);
        experienceBar.Find("Fill").GetComponent<Image>().fillAmount = fill;
    }

    void LevelUp()
    {
        level++;
        levelText.text = "Lv. " + level.ToString("00");
    }

    public void GetHit(float damage)
    {
        maxHealth -= damage;
        Health = maxHealth;
    }
}
