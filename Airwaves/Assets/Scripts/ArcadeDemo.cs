﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeDemo : MonoBehaviour {

	#region Private Variables

	private float lastLeftInput;
	private float lastRightInput;
	private float lastELeftInput;
	private float lastERightInput;

	#endregion

	#region Component Methods

	// Use this for initialization
	void Start() {
		lastELeftInput = InputCallback.info[0];
		lastERightInput = InputCallback.info[1];
		lastLeftInput = InputCallback.info[2];
		lastRightInput = InputCallback.info[3];

	}

	// Update is called once per frame
	void Update() {

		bool final = false;
		if (Input.inputString == "A" || Input.inputString == "a") {
			SkipLevel.level = 1;
		} else if (Input.inputString == "C" || Input.inputString == "c") {
			SkipLevel.level = 2;
		} else if (Input.inputString == "R" || Input.inputString == "r") {
			SkipLevel.level = 3;
		} else if (Input.inputString == "F" || Input.inputString == "f") {
			final = true;
		} else if(Input.inputString == "D" || Input.inputString == "d" || Input.GetKeyDown(KeyCode.KeypadPlus)) {
			SkipLevel.level = 0;
			SkipLevel.isDemo = true;
		} else {
			SkipLevel.level = 0;
		}


		if (Input.inputString != "" && !final && !SkipLevel.isDemo) {
			SceneManager.LoadScene(1);
		} else if (Input.inputString != "" && final) {
			SceneManager.LoadScene(4);
		} else if(Input.inputString != "" && SkipLevel.isDemo) {
			SceneManager.LoadScene(5);
		}

		lastELeftInput = InputCallback.info[0];
		lastERightInput = InputCallback.info[1];
		lastLeftInput = InputCallback.info[2];
		lastRightInput = InputCallback.info[3];
	}
	#endregion
}
