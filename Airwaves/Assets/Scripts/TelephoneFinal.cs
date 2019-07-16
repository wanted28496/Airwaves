using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TelephoneFinal : MonoBehaviour {


	private string currentCode = "";
	private string correctCode = "4965438847";

	[Header("Components for Telephone")]
	public Text telephone;
	public AudioSource audioSource;
	public AudioSource staticComponentAudioSource;
	public static phoneTypesFinal presentType = phoneTypesFinal.IntroRinging;

	[Header("Audio Clips for final Level")]
	public List<AudioClip> introAudio;
	public List<AudioClip> outroAudio;
	public AudioClip callAudio;

	[Header("Sprites to Change for getting final digit")]
	public Sprite correctNumberSprite;
	public Sprite incorrectNumberSprite;
	public Sprite defaulSprite;
	public Image RussianImage;
	public GameObject sideBChannel;

	private int index;
	private const int maxLenght = 4;
	private IsSpriteChangeAble changeChannel = IsSpriteChangeAble.canChange;

	private AudioClip wrongAudio;
	private static AudioClip ringAudio;

	// Start is called before the first frame update


	//public AudioClip GetIntroAudioClip() {
	//return Resources.Load<AudioClip>(introAudioFileName + levelNumber.ToString());
	//}

	public void RingBeforeIntro() {

		if (ringAudio == null) {
			ringAudio = Resources.Load<AudioClip>("Audio/Ring");
		}
		staticComponentAudioSource.volume = 0.0f;
		audioSource.loop = true;
		audioSource.clip = ringAudio;
		audioSource.Play();
		presentType = phoneTypesFinal.IntroRinging;
	}

	private void StopAudio() {
		audioSource.Stop();
		audioSource.loop = false;
		staticComponentAudioSource.volume = 1.0f;
	}

	public void RingBeforeOutro() {
		//Playing Ringing Sound
		//Getting Ring Audio
		staticComponentAudioSource.volume = 0.0f;
		audioSource.loop = true;
		audioSource.clip = ringAudio;
		audioSource.Play();
		presentType = phoneTypesFinal.OutroRinging;

	}

	public void PlayIntro(AudioClip audio) {
		////Play Intro Sound
		audioSource.PlayOneShot(audio);
		presentType = phoneTypesFinal.Intro;
	}

	public void PlayOutro(AudioClip audioFileName) {
		////Playing Outro Sound
		audioSource.PlayOneShot(audioFileName);
		presentType = phoneTypesFinal.Outro;
	}

	void Start() {
		index = 0;
		if (wrongAudio == null) {
			wrongAudio = Resources.Load<AudioClip>("Audio/WrongNumberDialed");
		}
		if (ringAudio == null) {
			ringAudio = Resources.Load<AudioClip>("Audio/Ring");
		}
		RingBeforeIntro();
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

		if (InputCallback.If_Phone && presentType == phoneTypesFinal.OutroRinging) {
			audioSource.Stop();
			PlayOutro(outroAudio[Extras.activeChannel]);
		}

		if (!InputCallback.If_Phone && presentType == phoneTypesFinal.Outro) {
			audioSource.Stop();
			presentType = phoneTypesFinal.gameEnd;
			SceneManager.LoadScene(3);
		}

		if (InputCallback.If_Phone && presentType == phoneTypesFinal.IntroRinging) {
			audioSource.Stop();
			if (Extras.ioIndex < maxLenght) {
				PlayIntro(introAudio[Extras.ioIndex]);
				Extras.ioIndex++;
			} else {
				presentType = phoneTypesFinal.End;
			}
		}

		if (!InputCallback.If_Phone && presentType == phoneTypesFinal.Intro) {
			audioSource.Stop();
			staticComponentAudioSource.volume = 1.0f;
			if (Extras.ioIndex == 3 && SideB.america && SideB.cuba && SideB.russia) {
				RingBeforeIntro();
				sideBChannel.SetActive(true);
			} else if (Extras.ioIndex >= 3) {
				Extras.ioIndex = 0;
				presentType = phoneTypesFinal.End;
			} else {
				RingBeforeIntro();
			}
		}


		if (index != InputCallback.info_Phone.Count && InputCallback.If_Phone && presentType == phoneTypesFinal.End) {
			index = InputCallback.info_Phone.Count;
			if (InputCallback.info_Phone.Count > 0) {
				currentCode += InputCallback.info_Phone[index - 1].ToString();
				changeChannel = IsSpriteChangeAble.canChange;
			}
		}
		if (!InputCallback.If_Phone) {
			currentCode = "";
			InputCallback.info_Phone.Clear();
		}
		if (Input.GetKeyDown(KeyCode.Backspace) && presentType == phoneTypesFinal.End) {
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
		//Debug.Log(changeChannel);
		if (Extras.activeChannel == 2 && changeChannel == IsSpriteChangeAble.canChange) {
			if (currentCode.Length != 0) {
				Debug.Log(Convert.ToInt32(currentCode[currentCode.Length - 1]));
				if (Convert.ToInt32(currentCode[currentCode.Length - 1]) - 48 == 5) {
					RussianImage.sprite = correctNumberSprite;
				} else {
					RussianImage.sprite = incorrectNumberSprite;
				}
				changeChannel = IsSpriteChangeAble.changeToDefault;
			}
		}

		if (changeChannel == IsSpriteChangeAble.changeToDefault) {
			changeChannel = IsSpriteChangeAble.changeSprite;
			StartCoroutine(checkForTimer());
		}

		if (currentCode.Length == 10) {
			if (currentCode.Equals(correctCode)) {
				audioSource.PlayOneShot(callAudio);
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

	public IEnumerator checkForTimer() {
		yield return new WaitForSeconds(0.5f);
		RussianImage.sprite = defaulSprite;
	}

	public enum phoneTypesFinal {
		OutroRinging = 1,
		Outro = 2,
		IntroRinging = 4,
		Intro = 8,
		End = 32,
		gameEnd = 64,
	};

	public enum IsSpriteChangeAble {
		changeSprite,
		changeToDefault,
		canChange,
	}

}
