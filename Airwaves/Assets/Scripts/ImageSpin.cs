using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSpin : MonoBehaviour {
	[SerializeField] private float rotationSpeed = 0.01f;

	private Vector3 newRot;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		newRot = this.GetComponent<RectTransform>().localRotation.eulerAngles;
		newRot.z += rotationSpeed;

		this.GetComponent<RectTransform>().localRotation = Quaternion.Euler(newRot);
	}
}
