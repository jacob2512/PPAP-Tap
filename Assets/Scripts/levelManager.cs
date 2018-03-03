using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class levelManager : MonoBehaviour {

	[SerializeField] private GameObject logoObj;	
	[SerializeField] private GameObject faceObj;
	[SerializeField] private GameObject startObj;

	[SerializeField] private GameObject tutText;
	[SerializeField] private GameObject goObj;

	[SerializeField] private GameObject appleObj;
	[SerializeField] private GameObject pineappleObj;
	[SerializeField] private GameObject penObj;
	[SerializeField] private GameObject progressbarObj;
	[SerializeField] private GameObject progressObj;
	[SerializeField] private GameObject ughObj;

	[SerializeField] private GameObject winObj;
	[SerializeField] private GameObject lossObj;
	[SerializeField] private GameObject restartObj;

	[SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip tutMusic;
    [SerializeField] private AudioClip gameAudio;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip lossAudio;

    private AudioSource audioSource;

	private Vector3 penOriginalPos;
	private Vector3 pineappleOriginalPos;
	private Vector3 appleOriginalPos;

	private int gameState;
	private int currentProgress;
	private float invokeTime;

	private const float totalParts = 16f;

	private const float penInvokeTime = 0.8f;
	private const float penActiveTime = 0.8f;

	private const float penInvokeTimeFast = 0.6f;
	private const float penActiveTimeFast = 0.7f;

	private const float penPineappleDisableTime = 0.9f;

	private const float applePenEnableTime = 0.1f;

	void Start ()
	{
		penOriginalPos = penObj.transform.position;
		pineappleOriginalPos = pineappleObj.transform.position;
		appleOriginalPos = appleObj.transform.position;

		audioSource = GetComponent<AudioSource>();

		goToStart();
	}

	void disableObj(GameObject gameObj)
	{
		gameObj.SetActive(false);
	}
	void enableObj(GameObject gameObj)
	{
		gameObj.SetActive(true);
	}

	bool objIsClicked(GameObject gameObj)
	{
		return gameObj.GetComponent<clickAction>().isClicked();
	}

	void setObjClicked(GameObject gameObj, bool value)
	{
		gameObj.GetComponent<clickAction>().setClicked(value);
	}

	Vector3 getProgressBarExtents()
	{
		return progressbarObj.GetComponent<SpriteRenderer>().sprite.bounds.extents;
	}

	void incrementGameState()
	{
		gameState++;
		//print(gameState);
	}

	void incrementProgress()
	{
		currentProgress++;
		//print(currentProgress);
		
		progressObj.transform.localScale = new Vector3(currentProgress/totalParts,1,1);
		//print(progressObj.transform.localScale);
		
		progressObj.transform.position = new Vector3(
			progressbarObj.transform.position.x - getProgressBarExtents().x 
				+ currentProgress * getProgressBarExtents().x / totalParts + 0.04f,
			progressObj.transform.position.y,
			progressObj.transform.position.z);
	}

	void goToStart()
	{
		CancelInvoke("Animate");
		disableObj(tutText);
		disableObj(goObj);

		disableObj(progressbarObj);

		progressObj.transform.localScale = new Vector3(0,1,1);
		disableObj(progressObj);
		
		penObj.transform.position = penOriginalPos;
		pineappleObj.transform.position = pineappleOriginalPos;
		appleObj.transform.position = appleOriginalPos;

		disableObj(penObj);
		disableObj(appleObj);
		disableObj(pineappleObj);
		disableObj(ughObj);

		disableObj(winObj);
		disableObj(lossObj);
		disableObj(restartObj);

		audioSource.Stop();
		playAudioLoop(introMusic);

		enableObj(logoObj);
		enableObj(faceObj);
		enableObj(startObj);		
		
		gameState = -2;
		currentProgress = 0;
		invokeTime = 1.4f;
	}

	void goToTutorial()
	{
		disableObj(faceObj);
		disableObj(startObj);
		disableObj(logoObj);

		audioSource.Stop();
		playAudioLoop(tutMusic);

		enableObj(goObj);
		enableObj(tutText);

		tutText.GetComponent<RepeatAnimation>().RepeatAnim(3.2f, 3.2f);
	}

	void setupGame()
	{
		disableObj(tutText);
		disableObj(goObj);

		audioSource.Stop();
		playAudioOnce(gameAudio);

		enableObj(progressObj);
		enableObj(progressbarObj);

		Invoke("IHaveAPen",invokeTime); //-> 1
		invokeTime += 1.6f;
		Invoke("IHaveAnApple",invokeTime); //-> 2
		invokeTime += 1.8f;
		Invoke("Ugh",invokeTime);
		invokeTime += 0.7f;
		Invoke("Apple_Pen",invokeTime); //-> 3 -> 4
		invokeTime += penInvokeTime + penActiveTime + 0.8f;

		Invoke("IHaveAPen",invokeTime); //-> 5
		invokeTime += 1.6f;
		Invoke("IHavePineapple",invokeTime); //-> 6
		invokeTime += 1.8f;
		Invoke("Ugh",invokeTime);
		invokeTime += 0.7f;
		Invoke("Pineapple_Pen",invokeTime); //-> 7 -> 8
		invokeTime += penInvokeTime + penActiveTime + 0.2f;

		Invoke("ApplePen",invokeTime); //-> 9 -> 10
		invokeTime += penInvokeTimeFast + penActiveTimeFast + 0.4f;
		Invoke("PineapplePen",invokeTime); //-> 11 -> 12
		invokeTime += penInvokeTimeFast + penActiveTimeFast + 1.3f;
		Invoke("Ugh",invokeTime);
		invokeTime += 0.9f;
		Invoke("PenPineappleApplePen",invokeTime); //-> 13 -> 13 -> 14 -> 14
		invokeTime += penPineappleDisableTime + applePenEnableTime + 1.2f;

		Invoke("Results",invokeTime);
	}

	private void CheckState(GameObject gameObject, int currentState)
	{
		if(objIsClicked(gameObject) && gameState == currentState)
		{
			disableObj(gameObject);
			setObjClicked(gameObject,false);
			incrementProgress();
		}
	}

	void Update () 
	{
		//print(gameState);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(gameState == -2)
			{
				Application.Quit();
			}
			else
			{
				goToStart();
				CancelInvoke("Animate");
			}
		}

		if(objIsClicked(startObj) && gameState == -2)
		{
			incrementGameState();
			setObjClicked(startObj,false);
			goToTutorial();
		}

		if(objIsClicked(goObj) && gameState == -1)
		{
			incrementGameState();
			setObjClicked(goObj,false);
			setupGame();
		}

		//Verse 1
		CheckState(penObj, 1);
		CheckState(appleObj, 2);
		//UGH!
		CheckState(appleObj, 3);
		CheckState(penObj, 4);
		
		//Verse 2
		CheckState(penObj, 5);
		CheckState(pineappleObj, 6);
		//UGH!
		CheckState(pineappleObj, 7);
		CheckState(penObj, 8);

		//Verse 3
		CheckState(appleObj, 9);
		CheckState(penObj, 10);
		CheckState(pineappleObj, 11);
		CheckState(penObj, 12);

		//UGH!

		//Climax
		CheckState(penObj, 13);
		CheckState(pineappleObj, 13);
		CheckState(appleObj, 14);
		CheckState(penObj, 14);

		if(objIsClicked(restartObj) && gameState == 14)
		{
			disableObj(restartObj);
			setObjClicked(restartObj,false);
			goToStart();
		}
	}

	void disableAllItems()
	{
		disableObj(penObj);
		disableObj(appleObj);
		disableObj(pineappleObj);
		disableObj(ughObj);
	}

	void IHaveAPen()
	{
		incrementGameState();
		disableAllItems();
		enableObj(penObj);
	}

	void IHaveAnApple()
	{
		incrementGameState();
		disableAllItems();
		enableObj(appleObj);
	}

	void Ugh()
	{
		disableAllItems();
		enableObj(ughObj);
	}

	void Apple_Pen()
	{	
		incrementGameState();
		disableAllItems();
		enableObj(appleObj);
		Invoke("_Pen", penInvokeTime);
		Invoke("disableAllItems",penInvokeTime + penActiveTime);
		
	}

	void _Pen()
	{
		incrementGameState();
		disableAllItems();
		enableObj(penObj);
	}

	void IHavePineapple()
	{
		incrementGameState();
		disableAllItems();
		enableObj(pineappleObj);
	}

	void Pineapple_Pen()
	{
		incrementGameState();
		disableAllItems();
		enableObj(pineappleObj);
		Invoke("_Pen", penInvokeTime);
		Invoke("disableAllItems",penInvokeTime + penActiveTime);
	}

	void ApplePen()
	{
		incrementGameState();
		disableAllItems();
		enableObj(appleObj);
		Invoke("_Pen",penInvokeTimeFast);
		Invoke("disableAllItems",penInvokeTimeFast + penActiveTimeFast);
	}

	void PineapplePen()
	{
		incrementGameState();
		disableAllItems();
		enableObj(pineappleObj);
		Invoke("_Pen",penInvokeTimeFast);
		Invoke("disableAllItems",penInvokeTimeFast + penActiveTimeFast);
	}	
	
	void PenPineappleApplePen()
	{
		disableAllItems();
		
		Pen_Pineapple_Fast();
		Invoke("disableAllItems",penPineappleDisableTime);
		Invoke("Apple_Pen_Fast",penPineappleDisableTime + applePenEnableTime);
	}

	void Pen_Pineapple_Fast()
	{
		incrementGameState();
		penObj.transform.position = appleObj.transform.position;
		enableObj(penObj);
		
		pineappleObj.transform.position = new Vector3(penObj.transform.position.x * 0.25f
			,penObj.transform.position.y,penObj.transform.position.z);
		enableObj(pineappleObj);
	}

	void Apple_Pen_Fast()
	{
		incrementGameState();
		appleObj.transform.position = new Vector3(penOriginalPos.x * 0.25f
			,penOriginalPos.y,penOriginalPos.z);
		enableObj(appleObj);
		
		penObj.transform.position = penOriginalPos;
		enableObj(penObj);
	}

	void Results()
	{
		disableAllItems();
		//print(currentProgress+" == "+totalParts);
		
		if(currentProgress == totalParts) //WIN
		{
			audioSource.Stop();
			playAudioLoop(winMusic);
			enableObj(winObj);
		}
		else //LOSS
		{
			audioSource.Stop();
			playAudioOnce(lossAudio);
			enableObj(lossObj);
		}
		enableObj(restartObj);
	}

    public void playAudioLoop(AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void playAudioOnce(AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.Play();
    }

}