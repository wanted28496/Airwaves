using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour {

	[SerializeField] private float timer = 10f;
	[SerializeField] private Text countdownText;

	// Use this for initialization
	void Start() {
		countdownText = this.gameObject.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update() {
		if (timer > 0) {
			timer -= Time.deltaTime;
			countdownText.text = ((int)timer).ToString();
		} else if (timer <= 0) {
			timer = 0;
			SceneManager.LoadScene(1);
		}
	}
}
