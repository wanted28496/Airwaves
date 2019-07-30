using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Static : MonoBehaviour {

	#region Private variable

	[SerializeField] private int pixWidth;
	[SerializeField] private int pixHeight;

	[SerializeField] private float xOrg;
	[SerializeField] private float yOrg;


	[SerializeField] private float scale = 1.0f;
	[SerializeField] private float scaleMin = 100.0f;
	[SerializeField] private float scaleMax = 500.0f;

	private Texture2D noiseTex;
	private Color[] pix;
	private Image img;
	private AudioSource source;
	#endregion

	public List<Channel> channelObject = new List<Channel>();
	public List<ChannelFinal> finaleObjects = new List<ChannelFinal>();

	#region Component Methods
	// Use this for initialization
	void Start() {
		img = GetComponent<Image>();
		source = GetComponent<AudioSource>();
		noiseTex = new Texture2D(pixWidth, pixHeight);
		pix = new Color[noiseTex.width * noiseTex.height];
		img.material.mainTexture = noiseTex;

	}

	// Update is called once per frame
	void Update() {
		scale = Random.Range(scaleMin, scaleMax);
		CalcNoise();
		source.volume = GetMaxOpacity();
		if (Input.GetKeyDown(KeyCode.Space)) {
			SceneManager.LoadScene(2);
		}

	}

	#endregion


	#region Private Helper Functions

	/// <summary>
	/// Creates the static Sound effect on the screen
	/// </summary>
	private void CalcNoise() {
		// For each pixel in the texture...
		float y = 0.0F;

		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);
				pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}

		// Copy the pixel data to the texture and load it into the GPU.
		noiseTex.SetPixels(pix);
		noiseTex.Apply();
	}

	/// <summary>
	/// Used to check which channel is on screen and returns the alpha value of that screen
	/// </summary>
	/// <returns></returns>
	private float GetMaxOpacity() {
		float max = 1;

		if (channelObject.Count > 0) {
			max = channelObject[0].diff;

			foreach (var channel in channelObject) {
				if (max > channel.diff) {
					max = channel.diff;
				}
			}
		} else if (finaleObjects.Count > 0) {
			max = finaleObjects[0].diff;

			foreach (var channel in finaleObjects) {
				if (max > channel.diff) {
					max = channel.diff;
				}
			}
		}
		return max;
	}
	#endregion
}
