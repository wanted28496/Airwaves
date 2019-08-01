using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;

public class CSVManager : MonoBehaviour {

	#region Public Variables

	public TextAsset levelData;
	[SerializeField] public List<GameObject> channelsGameObject;
	public static int[][] code;
	public Telephone telephone;

	#endregion

	#region Private Variables

	private List<int> correctCodes;
	private char lineSeprator = '\n';
	private char fieldSeperator = ',';
	private int level;
	private int channelNo;
	private ChannelType type;
	private string asset;
	private string extraAsset;
	private string telephoneNo;
	private string telephoneAsset;
	[SerializeField] private Text codeText;

	#endregion

	#region Component Methods

	// Start is called before the first frame update
	void Awake() {
		int length;
		if(SkipLevel.isDemo) {
			code = new int[1][];
			length = 0;
		} else {
			code = new int[5][];
			length = 5;
		}
		for (int i = 0; i < length; i++) {
			code[i] = new int[4];
		}
		foreach (var channel in channelsGameObject) {
			channel.SetActive(false);
		}
		CodeChecker.correctCode = new List<int>();
		ReadData();
	}


	public void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishLoading;
	}

	public void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishLoading;
	}

	/// <summary>
	/// Event Callback once the scene is loaded
	/// </summary>
	/// <param name="scene"> Scene Assigned from the event </param>
	/// <param name="mode"> Mode assigned from the event </param>
	public void OnLevelFinishLoading(Scene scene, LoadSceneMode mode) {
		CodeChecker.currentCode = "";
		codeText.text = "____";
		telephone.RingBeforeIntro(SkipLevel.level);
		//ChangeLevel(1);
	}

	#endregion

	#region Public Helper Methods

	public void ChangeCodeText() {
		codeText.text = "____";
	}

	public void ChangeLevel(int levelNumber) {
		foreach (var obj in channelsGameObject) {
			obj.SetActive(false);
		}

		List<GameObject> channelData = updateChannelData(levelNumber);

		int length = channelData.Count;

		for (int i = 0; i < length; i++) {
			channelsGameObject[i] = channelData[i];
			channelsGameObject[i].SetActive(true);
		}

	}

	#endregion


	#region Private Helper Methods

	/// <summary>
	/// Read the data from the CSV file for code values
	/// </summary>
	private void ReadData() {
		string[] levelDatas = levelData.text.Split(lineSeprator);
		int lineCount = 0;
		int prevLevel = -1;
		int[] levelCode = new int[1];
		foreach (string data in levelDatas) {
			string[] fields = data.Split(fieldSeperator);
			if (lineCount == 0) {
				lineCount++;
				continue;
			}


			level = int.Parse(fields[0].ToString()) - 1;
			if (prevLevel != level) {
				prevLevel = level;
				levelCode = new int[4];
			}

			if (fields[7].ToString() == "TRUE") {
				int codeLocation = int.Parse(fields[9].ToString()) - 1;
				int codeValue = int.Parse(fields[8].ToString());
				levelCode[codeLocation] = codeValue;
				code[level] = levelCode;
			}
		}
		UpdateCodes();
		//ChangeLevel(1);
	}

	/// <summary>
	/// assigns correct codes to list in CodeChecker.cs
	/// </summary>
	private void UpdateCodes() {
		foreach (var levelCode in code) {
			int correctLevelCode = 0;
			foreach (var intcode in levelCode) {
				correctLevelCode = (correctLevelCode * 10) + intcode;
			}
			CodeChecker.correctCode.Add(correctLevelCode);
		}

		CodeChecker.finalLevel = CodeChecker.correctCode.Count - 1;
	}


	/// <summary>
	/// Reads the CSV file to assign the Channel data to the channel object depending upon the level
	/// </summary>
	/// <param name="levelNumber">The level for which data is needed</param>
	/// <returns>Returns the list of new channel game objects including channel data</returns>
	public List<GameObject> updateChannelData(int levelNumber) {
		Telephone.telephoneDirectory.Clear();
		Resources.UnloadUnusedAssets();
		string[] levelDatas = levelData.text.Split(lineSeprator);
		int lineCount = 0;
		int prevChannel = -1;
		List<GameObject> channelListGameObjects = new List<GameObject>();
		foreach (string data in levelDatas) {
			string[] fields = data.Split(fieldSeperator);
			if (lineCount == 0) {
				lineCount++;
				continue;
			}
			level = int.Parse(fields[0].ToString()) - 1;
			if (level < levelNumber - 1) {
				continue;
			} else if (level > levelNumber - 1) {
				break;
			}

			channelNo = int.Parse(fields[1].ToString()) - 1;
			if (prevChannel == channelNo) {
				continue;
			}
			prevChannel = channelNo;
			channelListGameObjects.Add(channelsGameObject[channelNo]);

			asset = fields[3].ToString();

			if (fields[4].ToString() != "NULL") {
				extraAsset = fields[4].ToString();
			} else {
				extraAsset = null;
			}

			if (fields[5].ToString() != "NULL") {
				telephoneNo = fields[5].ToString();
			} else {
				telephoneNo = null;
			}

			if (fields[6].ToString() != "NULL") {
				telephoneAsset = fields[6].ToString();
				Telephone.telephoneDirectory.Add(telephoneNo, Resources.Load<AudioClip>(telephoneAsset));
			} else {
				telephoneAsset = null;
			}

			///Creating new Channel object and reseting everything
			Channel c = channelsGameObject[channelNo].GetComponent<Channel>();
			c.img.enabled = false;
			c.vidp.enabled = false;
			c.aud.enabled = false;
			c.video = false;
			string channelType = fields[2].ToString();
			/// Handles video related fields
			if (channelType == "Video") {
				c.channelType = ChannelType.Video;
				c.vidp.enabled = true;
				c.vidp.clip = Resources.Load<VideoClip>(asset);
				c.aud.enabled = true;
				c.vidp.isLooping = true;
				c.aud.clip = Resources.Load<AudioClip>(extraAsset);
				c.video = true;
			} else if (channelType == "Text") { /// Handles text related fields
				c.channelType = ChannelType.Text;
				c.img.sprite = Resources.Load<Sprite>(asset);
				TextAsset textFile = Resources.Load<TextAsset>(extraAsset);
				c.childText.text = textFile.text;
				c.enigmaInput.UpdateText();
				c.img.enabled = true;
				c.text = true;
				c.childText.fontSize = int.Parse(fields[14].ToString());
				c.child.GetComponent<RectTransform>().sizeDelta = new Vector2(float.Parse(fields[14].ToString()), float.Parse(fields[15].ToString()));
				float R, G, B;
				float.TryParse(fields[15].ToString(), out R);
				float.TryParse(fields[16].ToString(), out G);
				float.TryParse(fields[17].ToString(), out B);
				c.childText.color = new Color(R, G, B);
				c.enigmaInput.maxSetting = 10;
				c.enigmaInput.minSetting = -10;
				//System.Random rng = new System.Random();
				c.enigmaInput.correctSettingLeft = Convert.ToInt32(UnityEngine.Random.Range(-10, 10));
				c.enigmaInput.correctSettingRight = Convert.ToInt32(UnityEngine.Random.Range(-10, 10));
				///TODO::Randomized the correct value for left and right input
				//c.childText.enabled = true;
				//c.child.transform.
			} else {
				c.channelType = ChannelType.Image;
				c.img.enabled = true;
				c.img.sprite = Resources.Load<Sprite>(asset);
			}
			///Makes flags if the channel is Side B channel
			string sideB = fields[18].ToString();
			if (sideB.Contains("NULL")) {
				c.sideB = SideBType.n;
			} else if (sideB.Contains("A")) {
				c.sideB = SideBType.a;
			} else if (sideB.Contains("C")) {
				c.sideB = SideBType.c;
			} else if (sideB.Contains("R")) {
				c.sideB = SideBType.r;
			}


			/// Uncomment the code for Alternate Control
			/// 

			if (level == 0 && channelNo == 0) {
				c.leftValue = InputCallback.info[2] * 10;
				c.rightValue = InputCallback.info[3] * 10;
			} else if (level == 0 && channelNo == 1) {
				if (InputCallback.info[2] >= 0) {
					c.leftValue = -5;
				} else {
					c.leftValue = 5;
				}
				if (InputCallback.info[3] >= 0) {
					c.rightValue = -5;
				} else {
					c.rightValue = 5;
				}
			} else {
				c.leftValue = float.Parse(fields[10].ToString());
				c.rightValue = float.Parse(fields[11].ToString());
			}

			//float x = float.Parse(fields[10].ToString());
			//float y = float.Parse(fields[11].ToString());

			//c.leftPos = new Vector2(x, y);
			//c.rightPos = new Vector2(y, x);


			//TODO::If we use antenas Uncomment the code below

			//float rightX = float.Parse(fields[10].ToString());
			//float rightY = float.Parse(fields[11].ToString());

			//float leftX = float.Parse(fields[12].ToString());
			//float leftY = float.Parse(fields[13].ToString());

			//c.leftPos = new Vector2(leftX, leftY);
			//c.rightPos = new Vector2(rightX, rightY);

		}
		return channelListGameObjects;
	}

	#endregion


	public enum ChannelType {
		Image,
		Video,
		Text,
	}

	public enum SideBType {
		a,/// America Side B Flag
		c,/// Cuba Side B Flag
		r,/// Russia Side B Flag
		n,/// Not a Side B Flag
	}


	/// Fields list Summary
	/// [0] level number
	/// [1] Channel No
	/// [2] Channel Type
	/// [3] Channel Main Assest Location (Image or Video)
	/// [4] Channel Extra Assest Location (Text or Audio)
	/// [5] Telephone Number
	/// [6] Telephone Audio Location
	/// [7] Boolean value whether the channel contain code or not
	/// [8] Code value if it has code
	/// [9] Code location from 1 to 4
	/// [10] Right X value if using Vector 2 or right value if using float
	/// [11] Right Y value if using Vector 2 or left value if using float
	/// [12] Left X value if using Vector 2, not used in float
	/// [13] Left Y value if using Vector 2, not used in float
	/// [14] Width of Child Text field
	/// [15] Height of Child text field
	/// [16] font size of child text
	/// [17] R value of the child text color
	/// [18] G value of the child text color
	/// [19] B value of the child text color
}
