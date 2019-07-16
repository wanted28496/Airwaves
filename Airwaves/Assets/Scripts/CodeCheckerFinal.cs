using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CodeCheckerFinal : MonoBehaviour {

	private List<int> numbers = new List<int>();
	public static string currentCode = "____";
	public static int correctCode = 9251;
	private int maxTries = 3;
	private int attempts = 0;

	public TelephoneFinal telephone;
	// Use this for initialization
	void Start() {
		for (int i = 0; i <= 9; i++) {
			numbers.Add(i);
		}
	}


	// Update is called once per frame
	void Update() {
		CheckForInput();
	}


	/// <summary>
	/// Checks the code input from the user and leads it to next level
	/// </summary>
	private void CheckForInput() {
		int result;
		/// To remove last digit from code
		if (Input.GetKeyDown(KeyCode.KeypadPeriod)) {
			if (currentCode.Length != 0) {
				currentCode = currentCode.TrimEnd(currentCode[currentCode.Length - 1]);
				this.gameObject.GetComponent<Text>().text = currentCode;
			}
		}
		//if (!Input.GetKeyDown(KeyCode.Keypad0) && !Input.GetKeyDown(KeyCode.Keypad1) && !Input.GetKeyDown(KeyCode.Keypad2) && !Input.GetKeyDown(KeyCode.Keypad3) &&
		//	!Input.GetKeyDown(KeyCode.Keypad4) && !Input.GetKeyDown(KeyCode.Keypad5) && !Input.GetKeyDown(KeyCode.Keypad6) && !Input.GetKeyDown(KeyCode.Keypad7) &&
		//	!Input.GetKeyDown(KeyCode.Keypad8) && !Input.GetKeyDown(KeyCode.Keypad9)) {
		if (int.TryParse(Input.inputString, out result)) {
			if (numbers.Contains(result)) {
				char[] charsToTrim = { '_' };
				currentCode = currentCode.Trim(charsToTrim);
				currentCode += Input.inputString;
				if (currentCode.Length >= 4) {
					int codeInt;
					int.TryParse(currentCode, out codeInt);
					if (codeInt == correctCode) {
						//Ring The Phone and Play Outro and end the game
						telephone.RingBeforeOutro();
						currentCode = "____";
						this.gameObject.GetComponent<Text>().text = currentCode;
					} else {
						attempts++;
						currentCode = "____";
						this.gameObject.GetComponent<Text>().text = currentCode;
						if (attempts >= maxTries) {
							SceneManager.LoadScene(2);

						}
					}

				} else {
					currentCode += "____";
					currentCode = currentCode.Substring(0, 3);
					this.gameObject.GetComponent<Text>().text = currentCode;
				}
			}
		}
		//}
	}
}
