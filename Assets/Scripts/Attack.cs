using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public float speed;
	public float destroyTime;

	void Start () {
		Destroy (gameObject, destroyTime);
	}

	void Update () {
		transform.Translate (Vector2.right * speed * Time.deltaTime);
	}
}
