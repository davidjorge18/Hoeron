using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject {
    
    public int wallDamage = 1;
    public int enemyDamage = 2;
    
    public float restartLevelDelay = 1f;
    
    private Animator animator;
    private int hp;
    
	// Use this for initialization
	protected override void Start () {
		
        animator = GetComponent<Animator>();
        
        hp = GameManager.instance.playerHP;
        
        base.Start();
        
        
	}
    
    private void OnDisable()
    {
        
        GameManager.instance.playerHP = hp;
        
    }
	
	// Update is called once per frame
	void Update () {
		
        if(!GameManager.instance.playersTurn) return;
        
        int horizontal = 0;
        int vertical = 0;
        
        
        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");
        
        if(horizontal != 0)
            vertical = 0;
        
        
        if(horizontal != 0 || vertical != 0)
            AttemptMove<Enemy> (horizontal, vertical);
        
	}
    
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        base.AttemptMove <T> (xDir, yDir);
        
        RaycastHit2D hit;
        
        CheckIfGameOver();
        
        GameManager.instance.playersTurn = false;
    }
    
    private void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke("StartNewLevel", 1.0f);
            enabled = false;
        }
    }
    
    private void StartNewLevel()
    {
        enabled = true;
        Reset();
        transform.position = new Vector3(0, 0, 0);
        GameManager.instance.InitGame();
    }
    
    
    protected override void OnCantMove <T> (T component)
    {
        Enemy hitEnemy = component as Enemy;
        hitEnemy.DamageEnemy(enemyDamage);
        animator.SetTrigger("playerHit");
        
    }
   
    
    private void CheckIfGameOver()
    {
        
        if(hp<=0)
            GameManager.instance.GameOver();
        
    }
    
    public void LoseHp(int loss)
    {
        animator.SetTrigger("playerDmgd");
        hp -= loss;
        CheckIfGameOver();
    }
    
}
