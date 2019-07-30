using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCallback : MonoBehaviour {
	[Tooltip("The data from Enigma Machine")]
	public static float[] info;
	[Tooltip("The data from Antenna")]
	public float[] info_Antenna_left;
	public float[] info_Antenna_right;
	[Tooltip("The data from Phone")]
	public static List<int> info_Phone;
	public static bool If_Phone = false;
	private int current_index = -1;
	private static readonly int[] infoLength = { 4, 3 };




	// Start is called before the first frame update
	void Start() {
		info = new float[infoLength[0]];
		info_Antenna_left = new float[infoLength[1]];
		info_Antenna_right = new float[infoLength[1]];
		info_Phone = new List<int>();
		DecriptionF(info_Antenna_left, "s-137.53p-41.49p239.07pe", infoLength[1]);
		//DecriptionF(info_Antenna, "s304p52p12088p1.37p-1.43p-3.56pe", infoLength[1]);
	}
	void DecriptionF(float[] output, string msg, int Length) {

		char[] infoArray = msg.ToCharArray();
		int index = 0;
		int arrayindex = -1;
		int digits = 0;
		float sign = 1;
		float result = 0.0f;
		float other_count = 0;
		bool other_flag = false;
		foreach (char ch in infoArray) {
			if (other_flag) {
				other_count++;
			}
			arrayindex++;
			if (arrayindex == 0) {
				if (ch != 's') {
					print("error the start is wrong");
				}
				digits = 0;
				continue;
			}
			if (ch == 'p') {
				result *= Mathf.Pow(0.1f, other_count - 1);
				output[index] = sign * result;
				result = 0.0f;
				digits = 0;
				sign = 1.0f;
				other_flag = false;
				other_count = 0;
				index++;
				continue;
			}
			if (index == Length) {
				if (ch != 'e') {
					print("error the end is wrong");
					break;
				} else {
					break;
				}
			}

			if (digits == 0) {
				digits = 1;
				if (ch == '-') {
					sign = -1;
					continue;
				} else {
					sign = 1;
				}
			}

			if (digits == 1) {
				result = ch - 48;
				digits++;
				continue;
			}
			if (ch == '.') {
				other_flag = true;
				continue;
			}

			result = 10 * result + ch - 48;
			digits++;
		}
	}
	// Update is called once per frame
	void Update() {

	}
	void DecriptionUI(float[] output, string msg, int Length) {
		char[] infoArray = msg.ToCharArray();
		int index = 0;
		int arrayindex = -1;
		foreach (char ch in infoArray) {
			arrayindex++;
			if (arrayindex == 0) {
				if (ch != 's') {
					print("error the start is wrong");
					break;
				}
				continue;
			}
			if (ch == 'p') {
				index++;
				continue;
			}
			if (index == Length) {
				if (ch != 'e') {
					print("error the end is wrong");
					break;
				} else {
					break;
				}
			}
			output[index] = 10 * output[index] + ch - 48;
		}
	}
	void DecriptionUI(List<int> output, string msg) {
		char[] infoArray = msg.ToCharArray();
		if ((infoArray[3]) - 48 == 0) {
			If_Phone = false;
		} else {
			If_Phone = true;
		}
		if (infoArray[0] != 's' || infoArray[4] != 'e') {
			print("error in the phone inputs");
		} else {
			int temp_index = (infoArray[2] - 48) % 2;
			if ((current_index != -1) && temp_index == (current_index + 1) % 2) {
				info_Phone.Add(infoArray[1] - 48);
				print(infoArray[1] - 48);
				print(If_Phone);
				print("\n");
			} else {
				//print("digits are missing");
			}
			current_index = temp_index;
		}
	}



	private void InterpretInfo() {
		for (int i = 0; i < infoLength[0]; i++) {
			info[i] -= 512.0f;
			info[i] *= 0.00195f;
			Debug.Log(i + "   :   " + info[i]);
		}
	}
	private void ResetInfo() {
		for (int i = 0; i < infoLength[0]; i++) {
			info[i] = 0;
		}
	}
	private void ResetAntennaInfo() {
		for (int i = 0; i < infoLength[1]; i++) {
			info_Antenna_left[i] = 0;
			info_Antenna_right[i] = 0;
		}
	}
	private void ResetPhoneInfo() {
		info_Phone.Clear();
	}
	public void PrintInfo() {
		for (int i = 0; i < infoLength[0]; i++) {
			//print("the" + i.ToString() + " data is" + info[i].ToString() + "\n");

		}
	}
	public void PrintPhoneInfo() {
		print("Phone value is");
		foreach (int value in info_Phone) {
			print(value.ToString());
		}
		print("\n");
	}
	public void OnMessageArrived(string msg) {
		ResetInfo();
		DecriptionUI(info, msg, infoLength[0]);
		InterpretInfo();
		PrintInfo();
	}
	public void OnLeftAntennaArrived(string msg) {
		DecriptionF(info_Antenna_left, msg, infoLength[1]);
		//PrintAntennaInfo();
	}
	public void OnRightAntennaArrived(string msg) {
		DecriptionF(info_Antenna_right, msg, infoLength[1]);
		//PrintAntennaInfo();
	}
	public void OnPhoneArrived(string msg) {
		DecriptionUI(info_Phone, msg);
		//PrintPhoneInfo();
	}
}
