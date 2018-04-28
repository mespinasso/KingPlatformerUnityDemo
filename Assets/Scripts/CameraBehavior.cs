using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

	public float smoothTimeX = 0f;
	public float smoothTimeY = 0f;

	private float shakeTimer;
	private float shakeOffset;
	private Vector2 speed;
	private Transform player;

	void Start () {
		
		player = GameObject.Find ("Player").GetComponent<Transform> ();
	}

	void Update () {

		if (shakeTimer > 0f) {
			Vector2 shakePos = Random.insideUnitCircle * shakeOffset;
			transform.position = new Vector3 (transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);

			shakeTimer -= Time.deltaTime;
		}
	}

	void FixedUpdate () {

		if (player != null) {
			float posX = Mathf.SmoothDamp (transform.position.x, player.position.x, ref speed.x, smoothTimeX);
			float posY = Mathf.SmoothDamp (transform.position.y, player.position.y, ref speed.y, smoothTimeY);

			transform.position = new Vector3 (posX, posY, transform.position.z);
		}
	}

	public void ShakeCamera (float timer, float offset) {

		shakeTimer = timer;
		shakeOffset = offset;
	}
}
