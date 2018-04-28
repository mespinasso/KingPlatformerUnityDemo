using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class Hud : MonoBehaviour {

	public Sprite[] sprites;
	public Image healthBar;
	public TextMeshProUGUI coinScore;

	public static Hud instance;

	void Awake () {

		if (instance == null) {
			instance = this;
		} else {
			Destroy (instance);
		}
	}

	/// <summary>
	/// Atualiza o mostrador de quantidade de moedas coletadas com o valor passado.
	/// </summary>
	/// <param name="coinAmount">Quantidade de moedas coletadas.</param>
	public void RefreshCoinScore (int coinAmount) {
		coinScore.SetText ("x " + coinAmount.ToString ());
	}

	/// <summary>
	/// Atualiza o mostrador de saúde do jogador com o valor passado.
	/// </summary>
	/// <param name="playerHealth">Valor da saúde do jogador [deve estar entre 0 e 3].</param>
	public void RefreshHealthBar (int playerHealth) {
		healthBar.sprite = sprites [playerHealth];
	}
}
