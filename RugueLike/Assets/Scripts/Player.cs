﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject
{
    public float restartLevelDelay = 1f;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public int wallDamage = 1;
	
	private Animator animator;
	private int food;
	
	//Start overrides the Start function of MovingObject
	protected override void Start ()
	{
		animator = GetComponent<Animator>();
		base.Start ();
	}
	
	
	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable ()
	{
		GameManager.instance.playerFoodPoints = food;
	}
	
	
	private void Update ()
	{
		if(!GameManager.instance.playersTurn) return;
		
		int horizontal = 0;
		int vertical = 0;

		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		vertical = (int) (Input.GetAxisRaw ("Vertical"));
		
		if(horizontal != 0)
		{
			vertical = 0;
		}
		if(horizontal != 0 || vertical != 0) {
			AttemptMove<Wall> (horizontal, vertical);
		}
	}

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		food--;
		base.AttemptMove <T> (xDir, yDir);
		
		CheckIfGameOver ();
		
		GameManager.instance.playersTurn = false;
	}

	protected override void OnCantMove <T> (T component) {
		Wall hitWall = component as Wall;
		hitWall.DamageWall (wallDamage);
		animator.SetTrigger ("playerChop");
	}
	
	private void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		}
		
		else if(other.tag == "Food") {
			food += pointsPerFood;
			other.gameObject.SetActive (false);
		}
		else if(other.tag == "Soda") {
			food += pointsPerSoda;
			other.gameObject.SetActive (false);
		}
	}
	
	public void LoseFood (int loss) {
		animator.SetTrigger ("playerHit");
		food -= loss;
		CheckIfGameOver ();
	}

	private void CheckIfGameOver () {
		if (food <= 0) {
			GameManager.instance.GameOver ();
		}
	}
}
