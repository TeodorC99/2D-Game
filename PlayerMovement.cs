using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Photon.MonoBehaviour
{
	//Creating animator and controller
	public CharacterController2D controller;
	public Animator animator;

	//Creating runing and jumping related variables
	float runSpeed = 60f;
	float horizontalMove = 0f;
	public bool jump = false;
	public float dash = 0f;
	public bool flip = true;

	//Crating attack related variables
	public Transform attackPoint;
	float attackRange = 0.5f;
	public LayerMask enemyLayers;
	int attackDamage = 5;

	//Creating health related variables
	public int maxHealth = 100;
	public int currentHealth = 0;
	public HealthBarScript healthBar;
	bool imune = false;

	//Creating stamina related variables
	public StaminaBarScript staminaBar;
	public int maxStamina = 100;
	public int currentStamina = 0;

	bool isBlocking = false;

	public PhotonView photonView;

	public Text PlayerNameText;

	public GameObject disUI;

	public bool block;

	public GameObject Touchscreen;


	// Start is called before the first frame update
	void Start()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);

		currentStamina = maxStamina;
		staminaBar.SetMaxStamina(maxStamina);

		if (photonView.isMine)
		{
			Touchscreen.SetActive(true);
		}
	}

	void Awake()
	{
		if (photonView.isMine)
		{
			PlayerNameText.text = PhotonNetwork.playerName;
		}
		else {
			PlayerNameText.text = photonView.owner.name;
			PlayerNameText.color = Color.red;
		}
	}



	// Update is called once per frame
	void Update()
	{
		if (photonView.isMine)
		{
			if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
			{
				flip = true;
			}

			else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
			{
				flip = false;
			}


			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

			//animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

			if (Input.GetButtonDown("Jump"))
			{
				jump = true;
				animator.SetTrigger("Jump");
				animator.SetBool("IsJumping", true);

			}

			if (Input.GetButtonDown("Fire1"))
			{
				animator.SetTrigger("IsAttacking");
				photonView.RPC("Attack", PhotonTargets.AllBuffered);
			}

			if (Input.GetButtonDown("Fire2"))
			{

				if (currentStamina >= 40)
				{
					if (flip)
					{
						dash = 40f;
					}
					else { dash = -40f; }

					photonView.RPC("Magic", PhotonTargets.AllBuffered);
				}
			}

			if (Input.GetButtonDown("Fire3") && currentStamina > 0)
			{

				photonView.RPC("Block", PhotonTargets.AllBuffered);


			}

			if (Input.GetButtonUp("Fire3")|| currentStamina <= 0)
			{
				photonView.RPC("StopBlock", PhotonTargets.AllBuffered);

			}

			if (this.transform.position.y <= -25)
			{

				Die();
				Debug.Log("Death");
			}
		}
	}

	[PunRPC]
	void Block() {
		animator.SetTrigger("Block");
		animator.SetBool("IsBlocking", true);

		imune = true;

		isBlocking = true;
		runSpeed = 0f;
		StartCoroutine(StaminaTake());

	}

	[PunRPC]
	void StopBlock() 
	{
		isBlocking = false;
		animator.SetBool("IsBlocking", false);
		StartCoroutine(StaminaRegen());
		runSpeed = 60f;
		imune = false;


	}

	[PunRPC]
	void Magic() {

		animator.SetTrigger("Magic");
		currentStamina = currentStamina - 40;
		staminaBar.SetStamina(currentStamina);
	}
	
	private IEnumerator StaminaTake() { 

		while (currentStamina > 0 && isBlocking) {
			currentStamina = currentStamina - 1;
			staminaBar.SetStamina(currentStamina);
			yield return new WaitForSeconds(0.1f);
		}
	
	}

	void Die() {
		//Plays an animation
		animator.SetTrigger("Die");
		disUI.SetActive(true);
	}

	private IEnumerator StaminaRegen() {

		yield return new WaitForSeconds(2f);

		while (currentStamina < maxStamina && !isBlocking)
		{
			currentStamina += maxStamina / 100;
			staminaBar.SetStamina(currentStamina);
			yield return new WaitForSeconds(0.1f);
		}
	}

	[PunRPC]
	void Attack() {

		//Check for enemies
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

		//Damage them
		foreach (Collider2D enemy in hitEnemies)
		{
			if (enemy.GetComponent<PlayerMove2>() != null)
			{
				enemy.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, attackDamage);
			}
			if (enemy.GetComponent<PlayerMovement>() != null)
			{
				enemy.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, attackDamage);
			}
			else
			{
				Debug.Log("NE");
			}
		}
	}

	[PunRPC]
	public void TakeDamage(int damage) {
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);

		if (currentHealth <= 0)
		{
			Die();
		}

	}

	public void OnLanding() {

		animator.SetBool("IsJumping", false);
	
	}

	void FixedUpdate()
	{
		// Move our character
		if (dash == 0)
		{
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		}
		else {
			controller.Move(dash, false, false);
			dash = 0f;
		}

		jump = false;
	}



	void OnDrawGizmosSelected() {

		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);

	}
}