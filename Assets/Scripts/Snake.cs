using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

	public int health;
	public float speed;
	public Transform wallCheck;
	public Transform currentGroundCheck;
	public Transform forwardGroundCheck;

	private bool isInvencible = false;
	private bool isFacingRight = true;
	private bool didTouchWall = false;
	private bool stoppedTouchingGround = false;
	private SpriteRenderer sr;
	private Rigidbody2D rb;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update () {

		// Verifica se a cobra encontou numa parede
		didTouchWall = Physics2D.Linecast (transform.position, wallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		stoppedTouchingGround = 
			!Physics2D.Linecast (transform.position, forwardGroundCheck.position, 1 << LayerMask.NameToLayer ("Ground")) &&
			Physics2D.Linecast (transform.position, currentGroundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		// Inverte a posição da cobra caso ela toque numa parede
		if (didTouchWall || stoppedTouchingGround) {
			FlipSnake ();
		}
	}

	// Tratando movimento de objetos com física
	void FixedUpdate() {
		rb.velocity = new Vector2 (speed, rb.velocity.y);
	}

	/// <summary>
	/// Inverte a posição da cobra.
	/// </summary>
	void FlipSnake() {
		isFacingRight = !isFacingRight;
		speed *= -1;

		transform.localScale = new Vector2 (transform.localScale.x * -1, transform.localScale.y);
	}

	// Tratamento de colisão com trigger
	void OnTriggerEnter2D (Collider2D other) {

		// Verifica se a cobra foi atingida pelo ataque do player
		if (other.CompareTag ("Attack")) {
			GetDamaged ();
		}
	}

	/// <summary>
	/// Cobra sofre dano de ataque do player
	/// </summary>
	void GetDamaged() {
		if (!isInvencible) {
			health--;
			isInvencible = true;

			// Após perder saúde, é ativado o efeito visual de dano recebido
			StartCoroutine (DamageEffect ());

			// Se a cobra não tem mais saúde, é destruída
			if (health < 1) {
				Destroy (gameObject);
			}
		}
	}

	/// <summary>
	/// Efeito visual de dano recebido pela cobra.
	/// </summary>
	/// <returns>IEnumerator</returns>
	IEnumerator DamageEffect() {
		float actualSpeed = speed;

		// Cobra sofre um impulso para cima e para trás ao receber dano
		rb.AddForce (new Vector2(0f, 100f));
		speed = speed * -1;

		// Cor da cobra muda para vermelho
		sr.color = Color.red;

		// Aguarda-se um curto intervalo de tempo
		yield return new WaitForSeconds (0.2f);

		// Velocidade e cor da cobra voltam ao normal
		speed = actualSpeed;
		sr.color = Color.white;

		isInvencible = false;
	}
}
