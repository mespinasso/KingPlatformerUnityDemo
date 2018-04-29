using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {

	public AudioClip fxCoin;

	private Player playerScript;

	void Start () {
		playerScript = GameObject.Find ("Player").GetComponent<Player> ();
	}

	// Tratamento de colisão com trigger
	void OnTriggerEnter2D (Collider2D other) {

		// Verifica se trigger do player foi atingido pelo inimigo para inflingir dano
		if (other.CompareTag("Enemy")) {
			playerScript.DamagePlayer ();
		}

		if (other.CompareTag ("InstantDeath")) {
			playerScript.KillPlayer ();
		}

		if (other.CompareTag ("Coin")) {
			AudioManager.instance.PlaySound (fxCoin);
			Destroy (other.gameObject);

			playerScript.RegisterCollectedCoin ();
		}

		if (other.CompareTag ("LevelCompleted")) {
			playerScript.ShowResultScreen ();
		}
	}
}
