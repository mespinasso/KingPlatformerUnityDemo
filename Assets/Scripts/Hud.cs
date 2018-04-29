using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Hud : MonoBehaviour {

	public Sprite[] sprites;
	public Image healthBar;
	public TextMeshProUGUI coinScore;

	public TextMeshProUGUI resultCoinScore;
	public GameObject resultScreen;

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

	/// <summary>
	/// Reinicia a fase atual.
	/// </summary>
	void ReplayLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
	}

	/// <summary>
	/// Volta ao menu inicial.
	/// </summary>
	void MainMenu() {
		if (AudioManager.instance != null) {
			Destroy (AudioManager.instance.gameObject);
		}

		SceneManager.LoadScene (0);
	}

	/// <summary>
	/// Mostra o painel com o resultado do jogo.
	/// </summary>
	public void ShowResults(int coinAmount) {
		resultCoinScore.SetText ("x " + coinAmount.ToString ());
		resultScreen.SetActive (true);
	}
}
