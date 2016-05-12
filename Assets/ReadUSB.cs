using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ReadUSB : MonoBehaviour {

	public const int BAUD_RATE = 9600;
    string port = "COM5";
    //Requires a change to full api compatiability level
    SerialPort stream; //Set the port (com4) and the baud rate (9600, is standard on most devices)
	float[] lastRot = {0f,0f,0f}; //Need the last rotation to tell how far to spin the camera

	// Use this for initialization
	void Start () {
        
		stream = new SerialPort(port, BAUD_RATE);
		stream.Open(); //Open the Serial Stream.

        if(!stream.IsOpen) {
            Debug.LogError("Could not find " + port);
        }
    }

	// Update is called once per frame
	void Update () {
        string value = stream.ReadLine(); //Read the information
        string[] data = value.Split(' '); //My arduino script returns a 3 part value (IE: 12,30,18)
        transform.rotation = new Quaternion(float.Parse(data[2]), float.Parse(data[3]), -float.Parse(data[4]), float.Parse(data[1]));
        transform.rotation = Quaternion.Inverse (transform.rotation);

        stream.BaseStream.Flush(); //Clear the serial information so we assure we get new information.
	}

	void OnGUI()
	{
		string newString = "Connected: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z;
		GUI.Label(new Rect(10,10,300,100), newString); //Display new values
		// Though, it seems that it outputs the value in percentage O-o I don't know why.
	}
}
