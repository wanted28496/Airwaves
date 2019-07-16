using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CodeChecker : MonoBehaviour {

	#region Public Variables

	public static string currentCode = "____";
	[SerializeField] public static List<int> correctCode;
	[SerializeField] public static int level;
	[SerializeField] public static int finalLevel = 0;
	public CSVManager csv;
	public Telephone telephone;

	#endregion

	#region Private Variables
	private List<int> numbers = new List<int>();
	private int maxTries = 3;
	private int attempts = 0;
	#endregion

	#region Component Methods

	// Use this for initialization
	void Start() {

		for (int i = 0; i <= 9; i++) {
			numbers.Add(i);
		}
		if (correctCode == null) {
			correctCode = new List<int>();
			correctCode.Add(2658);
			correctCode.Add(5555);
		}
		this.gameObject.GetComponent<Text>().text = currentCode;
		if (finalLevel == 0) {
			finalLevel = 2;
		}
	}


	// Update is called once per frame
	void Update() {
		CheckForInput();
	}

	#endregion

	#region Private Helper Methods

	/// <summary>
	/// Checks the code input from the user and leads it to next level
	/// </summary>
	private void CheckForInput() {
		int result;
		if (Input.GetKeyDown(KeyCode.Delete)) {
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
					if (codeInt == correctCode[level]) {
						if (level < finalLevel - 1) {
							level++;
							string sceneName = string.Format("Level{0}", level + 1);
							//Ring The Phone and Play Outro and Intro and changes level
							telephone.RingBeforeOutro(level);
							//csv.ChangeLevel(level + 1);
							currentCode = "____";
							this.gameObject.GetComponent<Text>().text = currentCode;
						} else {
							SceneManager.LoadScene(4);
						}
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

	#endregion
}
