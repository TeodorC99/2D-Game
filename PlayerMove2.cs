using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove2 : Photon.MonoBehaviour
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
	int currentHealth = 0;
	public HealthBarScript healthBar;
	bool imune = false;

	//Creating stamina related variables
	public StaminaBarScript staminaBar;
	int maxStamina = 100;
	public int currentStamina = 0;

	public PhotonView photonView;

	public Text PlayerNameText;

	public GameObject fireObject;
	public GameObject firePoint;

	public GameObject disUI;

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
		else
		{
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
							dash = 100f;
						}
						else { dash = -100f; }

						photonView.RPC("Magic", PhotonTargets.AllBuffered);
					}
				}

			if (Input.GetButtonDown("Fire3"))
			{
				Fire();
			}

			if (this.transform.position.y <= -25)
			{

				Die();
				Debug.Log("Death");
				//disconectUI.SetActive(false);
			}
		}
	}

	public void Fire()
	{
		if (flip)
		{
			GameObject obj = PhotonNetwork.Instantiate(fireObject.name, new Vector2(firePoint.transform.position.x, attackPoint.transform.position.y), Quaternion.identity, 0);
		}

		if (!flip)
		{
			GameObject obj = PhotonNetwork.Instantiate(fireObject.name, new Vector2(firePoint.transform.position.x, attackPoint.transform.position.y), Quaternion.identity, 0);
			obj.GetComponent<PhotonView>().RPC("ChangeDir_left", PhotonTargets.AllBuffered);
		}

		animator.SetTrigger("Fire");
		currentStamina = currentStamina - 10;
		staminaBar.SetStamina(currentStamina);
		StartCoroutine(StaminaRegen());
	}


	[PunRPC]
	void Magic()
	{

		animator.SetTrigger("Magic");
		currentStamina = currentStamina - 40;
		staminaBar.SetStamina(currentStamina);
	}

	void Die()
	{
		//Plays an animation
		animator.SetTrigger("Die");
		disUI.SetActive(true);

	}

	private IEnumerator StaminaRegen()
	{

		yield return new WaitForSeconds(2f);

		while (currentStamina < maxStamina)
		{
			currentStamina += maxStamina / 100;
			staminaBar.SetStamina(currentStamina);
			yield return new WaitForSeconds(0.1f);
		}
	}

	[PunRPC]
	void Attack()
	{

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
	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);

		if (currentHealth <= 0)
		{
			Die();
		}

	}

	public void OnLanding()
	{

		animator.SetBool("IsJumping", false);

	}

	void FixedUpdate()
	{
		// Move our character
		if (dash == 0)
		{
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		}
		else
		{
			controller.Move(dash, false, false);
			dash = 0f;
		}

		jump = false;
	}



	void OnDrawGizmosSelected()
	{

		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);

	}
	[PunRPC]
	public void ButtonMoveRight()
	{
		controller.Move(5, false, jump);
	}

	[PunRPC]
	public void ButtonMoveLeft()
	{
		controller.Move(-5, false, jump);
	}
}
