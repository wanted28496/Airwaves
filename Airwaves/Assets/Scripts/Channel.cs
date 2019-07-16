using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class Channel : MonoBehaviour {

	#region Public Varaibles
	public Vector2 rightPos;
	public Vector2 leftPos;
	public float leftValue;
	public float rightValue;
	public Image img;
	public VideoPlayer vidp;
	public AudioSource aud;
	public EnigmaText enigmaInput;
	public GameObject child;
	public TextMeshPro childText;
	public AudioSource childAudio;
	public CSVManager.ChannelType channelType;
	public CSVManager.SideBType sideB;
	public float diff = 1.0f;

	#endregion

	#region Private Variable
	[SerializeField] public bool video;
	[SerializeField] public bool text;
	private AntennaeManager antm;
	private float diffMod = 1.0f;

	private GameObject stat;
	#endregion


	#region Component Methods
	// Use this for initialization
	void Start() {



		stat = GameObject.Find("Static");

		if (this.GetComponent<Image>() != null) {
			img = this.GetComponent<Image>();
		}
		if (this.GetComponent<VideoPlayer>() != null) {
			vidp = this.GetComponent<VideoPlayer>();
		}
		antm = GameObject.Find("AntennaeManager").GetComponent<AntennaeManager>();

	}
	private void OnEnable() {
		AntennaeManager.OnChange += calculateOpacity;
	}

	private void OnDisable() {
		AntennaeManager.OnChange -= calculateOpacity;
	}

	#endregion


	#region Private Helper Method


	/// <summary>
	/// Calculates the visibilites of each channel and changes alpha values based on channel position
	/// </summary>
	void calculateOpacity() {
		//Using Vector 2 Values for antena or XBox controller
		//Vector2 antLeft = antm.LeftPos;
		//Vector2 antRight = antm.RightPos;

		//Vector2 RDiff = rightPos - antRight;
		//Vector2 LDiff = leftPos - antLeft;

		//float totalRDiff = Mathf.Abs(RDiff.x) + Mathf.Abs(RDiff.y);
		//float totalLDiff = Mathf.Abs(LDiff.x) + Mathf.Abs(LDiff.y);
		//float diffPercent = 0;

		//Using FLoat Values for potentiometer Dialers
		float antenaLeft = antm.leftPos;
		float antenaRight = antm.rightPos;


		float rDiff = rightValue - antenaRight;
		float lDiff = leftValue - antenaLeft;


		float totalRDiff = Mathf.Abs(rDiff);
		float totalLDiff = Mathf.Abs(lDiff);


		float diffPercent = 0;

		//Debug.Log("Total R Diff: " + totalRDiff);
		//Debug.Log("Total L Diff: " + totalLDiff);

		if (totalRDiff <= 1.0f) {
			diffPercent += (1.0f - totalRDiff) * 0.5f;
		}
		if (totalLDiff <= 1.0f) {
			diffPercent += (1.0f - totalLDiff) * 0.5f;
		}

		if (diffPercent > 0.0f) {
			//diffPercent += 0.25f;
			diffPercent = Mathf.Clamp(diffPercent, 0, 1.0f);
		}
		if (diffPercent != 1.0f) {
			diff = 1.0f - diffPercent;
			//stat.GetComponent<AudioSource>().volume = diff;
		}

		if (diffPercent < 1.0f) {
			stat.GetComponent<AudioSource>().volume = 1.0f - diffPercent;
		}

		/// Handling the Video Aspect of channel
		if (video) {
			vidp.Play();
			vidp.targetCameraAlpha = diffPercent;
			if (vidp.GetDirectAudioMute(0)) {
				vidp.SetDirectAudioVolume(0, diffPercent);
			} else {
				vidp.GetComponent<AudioSource>().volume = diffPercent;
			}
		} else {	/// Handling Image aspect of Channel
			Color newCol = img.color;
			if (diffPercent > 0.25f) {
				newCol.a = diffPercent;
				if (sideB == CSVManager.SideBType.a) {
					SideB.america = true;
					Debug.Log("America: " + SideB.america);
					Debug.Log("Cuba: " + SideB.cuba);
					Debug.Log("Russia: " + SideB.russia);
				}
			} else {
				newCol.a = 0;
			}
			img.color = newCol;
		}

		/// Handles the text aspects of channel
		if (text && diffPercent > 0.25f) {
			if (sideB == CSVManager.SideBType.r) {
				SideB.russia = true;
				//Debug.Log("America: " + SideB.america);
				//Debug.Log("Cuba: " + SideB.cuba);
				//Debug.Log("Russia: " + SideB.russia);
			}
			childText.enabled = true;
			Color newCol = childText.color;
			newCol.a = diffPercent;
			enigmaInput.enabled = true;
			childText.color = newCol;
		} else {
			childText.enabled = false;
			enigmaInput.enabled = false;
		}
	}
	#endregion
}
