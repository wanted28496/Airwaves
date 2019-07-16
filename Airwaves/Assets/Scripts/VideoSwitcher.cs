using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Class that handles level with more than 1 video players and switches between them
/// </summary>
public class VideoSwitcher : MonoBehaviour {
	[SerializeField] private VideoPlayer vidp;
	// Start is called before the first frame update
	void Start() {
		if (this.GetComponent<VideoPlayer>() != null) {
			vidp = this.GetComponent<VideoPlayer>();
		}
	}

	// Update is called once per frame
	void Update() {
		//Set it to enabled only when alpha is 1
		if (vidp.targetCameraAlpha > 0.0) {
			vidp.enabled = true;
		} else {
			vidp.enabled = false;
		}
	}
}
