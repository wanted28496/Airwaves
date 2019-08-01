using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Telephone : MonoBehaviour {

	#region Public Variables

	public static string ioAudioFileName = "Audio/Level";
	public Text telephone;
	public static Dictionary<string, AudioClip> telephoneDirectory = new Dictionary<string, AudioClip>();
	public AudioSource audioSource;
	public AudioSource staticComponentAudioSource;
	public static phoneTypes presentType = phoneTypes.End;
	public CSVManager csv;

	#endregion

	#region Private Variables

	private string currentCode = "";
	private int index;
	private AudioClip wrongAudio;
	private static AudioClip ringAudio;
	private int level = 0;

	#endregion

	#region Public Helper Functions

	/// <summary>
	/// Rings the phone before intro and assigns the intro audio clip based on the level number
	/// </summary>
	/// <param name="levelNumber"> level number </param>
	public void RingBeforeIntro(int levelNumber) {

		level = levelNumber;
		if (ringAudio == null) {
			ringAudio = Resources.Load<AudioClip>("Audio/Ring");
		}
		staticComponentAudioSource.volume = 0.0f;
		audioSource.loop = true;
		audioSource.clip = ringAudio;
		audioSource.Play();
		presentType = phoneTypes.IntroRinging;
	}

	/// <summary>
	/// Stop the audio and starts static sound
	/// </summary>
	private void StopAudio() {
		audioSource.Stop();
		audioSource.loop = false;
		staticComponentAudioSource.volume = 1.0f;
	}

	/// <summary>
	/// Rings the phone before outro and assigns the intro audio clip based on the level number
	/// </summary>
	/// <param name="levelNumber"> level number</param>
	public void RingBeforeOutro(int levelNumber) {
		//Playing Ringing Sound
		//Getting Ring Audio
		staticComponentAudioSource.volume = 0.0f;
		level = levelNumber;
		audioSource.loop = true;
		audioSource.clip = ringAudio;
		audioSource.Play();
		presentType = phoneTypes.OutroRinging;

	}

	/// <summary>
	/// Play the intro sound
	/// </summary>
	public void PlayIntro() {
		////Play Intro Sound
		CodeChecker.level = level;
		level += 1;
		string audiuoFileName;
		if (SkipLevel.isDemo) {
			audiuoFileName = "Audio/DemoIntro";
		} else {
			audiuoFileName = ioAudioFileName + level.ToString() + "Intro";
		}
		audioSource.PlayOneShot(Resources.Load<AudioClip>(audiuoFileName));
		presentType = phoneTypes.Intro;
	}


	/// <summary>
	/// play the Outro Sound
	/// </summary>
	public void PlayOutro() {
		////Playing Outro Sound
		string audiuoFileName;
		if (SkipLevel.isDemo) {
			audiuoFileName = "Audio/DemoOutro";
		} else {
			audiuoFileName = ioAudioFileName + level.ToString() + "Outro";
		}

		audioSource.PlayOneShot(Resources.Load<AudioClip>(audiuoFileName));
		presentType = phoneTypes.Outro;
	}

	#endregion

	#region Component Methods 

	void Start() {
		index = 0;
		if (wrongAudio == null) {
			wrongAudio = Resources.Load<AudioClip>("Audio/WrongNumberDialed");
		}
		if (ringAudio == null) {
			ringAudio = Resources.Load<AudioClip>("Audio/Ring");
		}
	}

	public void OnEnable() {
		wrongAudio = Resources.Load<AudioClip>("Audio/WrongNumberDialed");
		ringAudio = Resources.Load<AudioClip>("Audio/Ring");
	}

	public void OnDisable() {
		Resources.UnloadAsset(wrongAudio);
		Resources.UnloadAsset(ringAudio);
	}

	// Update is called once per frame
	void Update() {
		//StopRingAndPlayIntro();
		if (audioSource.isPlaying) {
			staticComponentAudioSource.volume = 0.0f;
		}

		if (InputCallback.If_Phone && presentType == phoneTypes.OutroRinging) {
			audioSource.Stop();
			PlayOutro();
		}

		if (!InputCallback.If_Phone && presentType == phoneTypes.Outro) {
			audioSource.Stop();
			csv.ChangeCodeText();
			RingBeforeIntro(level);
		}

		if (InputCallback.If_Phone && presentType == phoneTypes.IntroRinging) {
			audioSource.Stop();
			PlayIntro();
		}

		if (!InputCallback.If_Phone && presentType == phoneTypes.Intro) {
			audioSource.Stop();
			staticComponentAudioSource.volume = 1.0f;
			csv.ChangeLevel(level);
			CodeChecker.currentCode = "";
			print("Level Loaded");
			presentType = phoneTypes.End;
		}

		if (index != InputCallback.info_Phone.Count && InputCallback.If_Phone && presentType == phoneTypes.End) {
			index = InputCallback.info_Phone.Count;
			if (InputCallback.info_Phone.Count > 0) {
				currentCode += InputCallback.info_Phone[index - 1].ToString();
				foreach (var num in InputCallback.info_Phone)
					Debug.Log(num);
			}
		}
		if (!InputCallback.If_Phone) {
			currentCode = "";
			InputCallback.info_Phone.Clear();
		}
		if (Input.GetKeyDown(KeyCode.Backspace) && presentType == phoneTypes.End) {
			if (currentCode.Length != 0) {
				currentCode = currentCode.TrimEnd(currentCode[currentCode.Length - 1]);
			}
			if (InputCallback.info_Phone.Count != 0) {
				InputCallback.info_Phone.RemoveAt(InputCallback.info_Phone.Count - 1);
			}
		}
		//if (Input.GetKeyDown(KeyCode.Keypad0)) {
		//	currentCode += "0";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad1)) {
		//	currentCode += "1";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad2)) {
		//	currentCode += "2";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad3)) {
		//	currentCode += "3";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad4)) {
		//	currentCode += "4";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad5)) {
		//	currentCode += "5";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad6)) {
		//	currentCode += "6";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad7)) {
		//	currentCode += "7";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad8)) {
		//	currentCode += "8";
		//}
		//if (Input.GetKeyDown(KeyCode.Keypad9)) {
		//	currentCode += "9";
		//}
		telephone.text = currentCode;

		if (currentCode.Length == 10) {
			if (telephoneDirectory.ContainsKey(currentCode)) {
				if (currentCode == "3632559277") {
					SideB.cuba = true;
				}
				audioSource.PlayOneShot(telephoneDirectory[currentCode]);
				currentCode = "";
				telephone.text = "";
				index = 0;
			} else {
				audioSource.PlayOneShot(wrongAudio);
				currentCode = "";
				telephone.text = "";
			}
			//InputCallback.info_Phone.Clear();
		} else if (currentCode.Length > 10) {
			currentCode = "";
			telephone.text = "";
			//InputCallback.info_Phone.Clear();
		}

	}
	#endregion

	/// <summary>
	/// Enum states for all phone types
	/// </summary>
	public enum phoneTypes {
		OutroRinging = 1,
		Outro = 2,
		IntroRinging = 4,
		Intro = 8,
		End = 32,
	};
}
