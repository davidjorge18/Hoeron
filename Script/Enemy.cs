using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
    
    public int playerDamage;
    public int turn = 2;

    private int inturn;
    private Animator animator;
    private Transform target;
    private bool skipMove;
    
    public int enemyHp = 2;
    
    
    
	// Use this for initialization
	protected override void Start () 
    {
		GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        inturn = turn;
        base.Start();
	}
	
    public void DamageEnemy (int loss)
    {
        
        animator.SetTrigger("enemyDmgd");
        
        enemyHp -= loss;

        if (enemyHp <= 0)
        {
            GameManager.instance.RemoveEnemyFromList(this);
            Destroy(gameObject);
        }
        
    }
    
    
    
    protected override void AttemptMove <T> (int xDir, int yDir)
    {

        if (turn == 0)
        {
            base.AttemptMove<T>(xDir, yDir);
           
            Debug.Log(turn);
            skipMove = true;
            turn = inturn;
        }
        else
        {
            skipMove = false;
            turn = turn - 1;
            Debug.Log(turn);
            return;

        }
    }
    
    public void MoveEnemy()
    {
        
        int xDir = 0;
        int yDir = 0;
        
        if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;
        
        AttemptMove <Player> (xDir, yDir);
        
    }
    
    protected override void OnCantMove <T> (T component)
    {
        Player hitPlayer = component as Player;
        
        animator.SetTrigger("enemyAttack");
        
        hitPlayer.LoseHp(playerDamage);
        
        
    }
    
    
}
