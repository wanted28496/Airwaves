using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntennaeManager : MonoBehaviour {

	#region public Variables
	public Text rightText;
	public Text leftText;
	public static bool canMove = true;
	public Vector2 RightPos {
		get {
			return rightCurrentPos;
		}
	}

	public Vector2 LeftPos {
		get {
			return leftCurrentPos;
		}
	}

	public float rightPos {
		get {
			return rightCurrentValue;
		}
	}

	public float leftPos {
		get {
			return leftCurrentValue;
		}
	}

	#endregion

	#region Private Serialized Variables
	[SerializeField] private Vector2 leftCurrentPos = new Vector2(0, 0);
	[SerializeField] private Vector2 rightCurrentPos = new Vector2(0, 0);
	[SerializeField] private float leftCurrentValue = 0;
	[SerializeField] private float rightCurrentValue = 0;
	[SerializeField] private const float posModifier = 10.0f;
	[SerializeField] private float movementModifier;
	#endregion

	public delegate void AntennaChange();
	public static event AntennaChange OnChange;

	#region Component Methods
	// Update is called once per frame
	void Update() {
		if (canMove) {
			if (InputCallback.info[3] != 0 || InputCallback.info[2] != 0) {
				//	Vector2 pos = new Vector2(Input.GetAxisRaw("LeftX"), Input.GetAxisRaw("LeftY"));
				//	leftCurrentPos = pos * posModifier;
				leftCurrentValue = InputCallback.info[2] * posModifier;
				rightCurrentValue = InputCallback.info[3] * posModifier;
				if (OnChange != null) {
					OnChange();
				}
			} else {
				//	leftCurrentPos = new Vector2(0, 0);
				leftCurrentValue = 0.0f;
				rightCurrentValue = 0.0f;
				if (OnChange != null) {
					OnChange();
				}
			}

			///Uncomment when using XBox controllers

			//if (Input.GetAxisRaw("RightX") != 0 || Input.GetAxisRaw("RightY") != 0) {
			//	Vector2 pos = new Vector2(Input.GetAxisRaw("RightX"), Input.GetAxisRaw("RightY"));
			//	//Debug.Log("position: " + pos);
			//	rightCurrentPos = pos * posModifier;
			//	//Debug.Log("Right Current Pos: " + rightCurrentPos);
			//	//rightText.text = "Right Pos: " + rightCurrentPos.ToString();
			//	if (OnChange != null) {
			//		OnChange();
			//	}
			//} else {
			//	rightCurrentPos = new Vector2(0, 0);
			//	if (OnChange != null) {
			//		OnChange();
			//	}
			//}
			//if (Input.GetAxisRaw("LeftX") != 0 || Input.GetAxisRaw("LeftY") != 0) {
			//	Vector2 pos = new Vector2(Input.GetAxisRaw("LeftX"), Input.GetAxisRaw("LeftY"));
			//	//Debug.Log("position: " + pos);
			//	leftCurrentPos = pos * posModifier;
			//	//Debug.Log("Right Current Pos: " + rightCurrentPos);
			//	//rightText.text = "Right Pos: " + rightCurrentPos.ToString();
			//	if (OnChange != null) {
			//		OnChange();
			//	}
			//} else {
			//	leftCurrentPos = new Vector2(0, 0);
			//	if (OnChange != null) {
			//		OnChange();
			//	}
			//}
		}
		//leftText.text = "Left Pos: " + leftCurrentValue + ":" + rightCurrentValue;
		//Debug.Log(leftText.text);
		if (Input.GetButtonDown("lock")) {
			canMove = (canMove) ? false : true;
		}
	}
	#endregion
}
