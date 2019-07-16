using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class trailerChannelSwap : MonoBehaviour {

	[SerializeField] private Image displayImg;
	[SerializeField] private Sprite[] images;
	private int currentIndex = 0;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Next")) {
			currentIndex++;
			displayImg.sprite = images[currentIndex];
		}
	}
}
