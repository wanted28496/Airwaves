using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class RestartAfterVideo : MonoBehaviour {

	[SerializeField] private float countdown;

	// Use this for initialization
	void Start() {
		countdown = (float)this.GetComponent<VideoPlayer>().clip.length;
	}

	// Update is called once per frame
	void Update() {
		countdown -= Time.deltaTime;
		if (countdown <= 0) {
			SceneManager.LoadScene(3);
		}
	}
}
