using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CSVManager : MonoBehaviour {

	public TextAsset levelData;
	[SerializeField]
	public List<GameObject> channelsGameObject;
	public static int[][] code;
	public Telephone telephone;
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

	// Start is called before the first frame update
	void Awake() {
		code = new int[5][];
		for (int i = 0; i < 5; i++) {
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

	public void OnLevelFinishLoading(Scene scene, LoadSceneMode mode) {
		CodeChecker.currentCode = "";
		codeText.text = "____";
		telephone.RingBeforeIntro(SkipLevel.level);
		//ChangeLevel(1);
	}


	public void ChangeCodeText() {
		codeText.text = "____";
	}

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

	//private void getChannelData(string[] fields, ChannelType type, out Channel channel) {
	//	Channel c = new Channel();

	//}

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

			Channel c = channelsGameObject[channelNo].GetComponent<Channel>();
			c.img.enabled = false;
			c.vidp.enabled = false;
			c.aud.enabled = false;
			c.video = false;
			string channelType = fields[2].ToString();
			if (channelType == "Video") {
				c.channelType = ChannelType.Video;
				c.vidp.enabled = true;
				c.vidp.clip = Resources.Load<VideoClip>(asset);
				c.aud.enabled = true;
				c.vidp.isLooping = true;
				c.aud.clip = Resources.Load<AudioClip>(extraAsset);
				c.video = true;
			} else if (channelType == "Text") {
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
				//c.childText.enabled = true;
				//c.child.transform.
			} else {
				c.channelType = ChannelType.Image;
				c.img.enabled = true;
				c.img.sprite = Resources.Load<Sprite>(asset);
			}
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


	public enum ChannelType {
		Image,
		Video,
		Text,
	}

	public enum SideBType {
		a,
		c,
		r,
		n,
	}

}
