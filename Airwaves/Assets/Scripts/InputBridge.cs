using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class InputBridge : MonoBehaviour {
	public string portName;
	[SerializeField] private SerialPort serial;
	[SerializeField] private string buf;
	// Start is called before the first frame update
	void Awake() {
		serial = new SerialPort();
		serial.PortName = portName;
		serial.BaudRate = 9600;
		serial.ReadTimeout = 5;
		serial.Open();
	}

	// Update is called once per frame
	void Update() {
		if (serial.IsOpen) {
			buf = serial.ReadLine();
			print(buf);
		}
	}
}
