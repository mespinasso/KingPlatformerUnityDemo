using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	// Public Variables
	public float speed;
	public float jumpForce;
	public int health;
	public float attackCadency;
	public Transform groundCheck;
	public Transform attackSpawn;
	public GameObject attackPrefab;
	public GameObject crownPrefab;

	public AudioClip fxHurt;
	public AudioClip fxJump;
	public AudioClip fxAttack;

	// Private Variables
	private int collectedCoins = 0;
	private bool isInvencible = false;
	private bool isTouchingGround = false;
	private bool isJumping = false;
	private bool isFacingRight = true;
	private float nextAttack = 0f;
	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private Animator anim;
	private CameraBehavior camScript;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		camScript = GameObject.Find ("Main Camera").GetComponent<CameraBehavior> ();
	}

	void Update () {
		
		// Verifica se o player está tocando o chão
		isTouchingGround = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		// Player pula quando está tocando o chão (comportamento tratado no FixedUpdate)
		if (Input.GetButtonDown ("Jump") && isTouchingGround) {
			isJumping = true;
		}

		// Player ataca quando está tocando o chão e depois de um intervalo entre o último ataque e o atual
		if (Input.GetButtonDown ("Fire1") && isTouchingGround && Time.time > nextAttack) {
			Attack ();
		}

		Animate ();
	}

	// Tratando movimento de objetos com física
	void FixedUpdate() {
		
		// Movimenta o player no eixo x
		float move = Input.GetAxis ("Horizontal");
		rb.velocity = new Vector2 (move * speed, rb.velocity.y);

		// Inverte a posição para onde o player olha de acordo com a direção que se movimenta
		if ((move < 0f && isFacingRight) || (move > 0f && !isFacingRight)) {
			FlipPlayer ();
		}

		// Player pula
		if (isJumping) {
			AudioManager.instance.PlaySound (fxJump);

			rb.AddForce (new Vector2 (0f, jumpForce));
			isJumping = false;
		}
	}

	/// <summary>
	/// Inverte o player.
	/// </summary>
	void FlipPlayer() {
		isFacingRight = !isFacingRight;
		transform.localScale = new Vector2 (transform.localScale.x * -1, transform.localScale.y);
	}

	/// <summary>
	/// Anima o player.
	/// </summary>
	void Animate() {
		anim.SetFloat ("VelY", rb.velocity.y);
		anim.SetBool ("IsJumpingOrFalling", rb.velocity.y != 0f);
		anim.SetBool ("IsWalking", rb.velocity.x != 0f && rb.velocity.y == 0f);
	}

	/// <summary>
	/// Registra a coleta de uma moeda.
	/// </summary>
	public void RegisterCollectedCoin() {
		collectedCoins++;
		Hud.instance.RefreshCoinScore (collectedCoins);
	}

	/// <summary>
	/// Player executa um ataque.
	/// </summary>
	void Attack() {
		anim.SetTrigger ("Slash");
		AudioManager.instance.PlaySound (fxAttack);

		// Define tempo de espera para o próximo ataque
		nextAttack = Time.time + attackCadency;

		// Instancia um ataque
		GameObject attackInstance = Instantiate (attackPrefab, attackSpawn.position, attackSpawn.rotation);

		// Verifica a direção para a qual o player está voltado e inverte a direção do ataque se necessário
		if (!isFacingRight) {
			attackInstance.transform.eulerAngles = new Vector3 (180, 0, 180);
		}
	}

	/// <summary>
	/// Inflinge dano ao player.
	/// </summary>
	public void DamagePlayer() {

		// Player perde saúde e fica momentaneamente invencível
		if (!isInvencible) {
			isInvencible = true;
			health--;

			// Atualiza a quantidade de vidas no HUD
			Hud.instance.RefreshHealthBar (health);
			// Executa o som de dano recebido
			AudioManager.instance.PlaySound (fxHurt);

			// Ativa o efeito visual de dano recebido
			StartCoroutine (DamageEffect ());

			// Se o player não tem mais saúde, é destruído
			if (health < 1) {
				KillPlayer ();
			}
		}
	}

	/// <summary>
	/// Mata o player e reinicia a fase.
	/// </summary>
	public void KillPlayer() {
		health = 0;

		// Atualiza a quantidade de vidas no HUD
		Hud.instance.RefreshHealthBar (health);
		// Executa o som de dano recebido
		AudioManager.instance.PlaySound (fxHurt);

		// Executa animação de morte
		PlayerDeath ();

		// Desativa o player e reinicia a fase após 3 segundos
		gameObject.SetActive (false);
		Invoke ("ReloadLevel", 3f);
	}

	/// <summary>
	/// Efeito visual de morte do player.
	/// </summary>
	void PlayerDeath() {

		// Instancia uma coroa na mesma posição do player e dá a ela um impulso para cima
		GameObject crownInstance = Instantiate (crownPrefab, transform.position, transform.rotation);
		Rigidbody2D rbCrown = crownInstance.GetComponent<Rigidbody2D> ();
		rbCrown.AddForce (Vector2.up * 350);
	}

	/// <summary>
	/// Efeito visual de dano recebido.
	/// </summary>
	/// <returns>IEnumerator</returns>
	IEnumerator DamageEffect() {

		// Camera sacode
		camScript.ShakeCamera (0.5f, 0.1f);

		// Player fica piscando
		for (float i = 0; i < 1f; i += 0.1f) {
			sr.enabled = false;
			yield return new WaitForSeconds (0.1f);

			sr.enabled = true;
			yield return new WaitForSeconds (0.1f);
		}

		// Invencibilidade termina ao fim do processo
		isInvencible = false;
	}

	/// <summary>
	/// Recarrega a fase.
	/// </summary>
	void ReloadLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
	}
}
