using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BackgroundSwapper : MonoBehaviour {

	public Texture2D[] backgrounds;
	public Texture2D[] dateBackgrounds;
    private Image myImage;
	private RelationshipCounselor myRelationshipCounselor;
	private Timelord myTimeLord;
	private SceneCatalogue mySceneCatalogue;

	// Use this for initialization
	void Start () {
		myImage = GetComponent<Image>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
		myTimeLord = GameObject.FindObjectOfType<Timelord>();
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
	}
	
	// Update is called once per frame
	void Update () {
		Texture2D nextBackground = backgroundsForThisScene()[myTimeLord.timeStep % backgroundsForThisScene().Length];
		if (mySceneCatalogue.getIsInDateScene() == true){
			nextBackground = dateBackgroundsForThisScene()[0];
		}
		myImage.sprite = createSpriteFromTex2D(nextBackground);
	}

	public static Sprite createSpriteFromTex2D(Texture2D from){
		return Sprite.Create(from, new Rect(0, 0, from.width, from.height), Vector2.zero);
	}

	Texture2D[] backgroundsForThisScene(){
		string sceneName = this.mySceneCatalogue.getCurrentSceneName();
		List<Texture2D> workingListOfBackgrounds = new List<Texture2D>();
		foreach (Texture2D background in backgrounds) { 
			if(background.name.ToLower().Contains(sceneName.ToLower())){
				workingListOfBackgrounds.Add(background);
			}
		}
        return workingListOfBackgrounds.ToArray();
	}

	Texture2D[] dateBackgroundsForThisScene()
    {
		int sceneNumber = this.mySceneCatalogue.getCurrentSceneNumberModulus();

        List<Texture2D> workingListOfBackgrounds = new List<Texture2D>();
        workingListOfBackgrounds.Add(dateBackgrounds[sceneNumber]);
        return workingListOfBackgrounds.ToArray();
    }
}
