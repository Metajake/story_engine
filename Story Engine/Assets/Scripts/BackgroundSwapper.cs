using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BackgroundSwapper : MonoBehaviour
{

	public Texture2D[] backgrounds;
	public Texture2D[] dateBackgrounds;
	private Image myImage;
	private Timelord myTimeLord;
	private SceneCatalogue mySceneCatalogue;
	private UIManager myUIManager;

	// Use this for initialization
	void Start()
	{
		myImage = GetComponent<Image>();
		myTimeLord = GameObject.FindObjectOfType<Timelord>();
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		myUIManager = GameObject.FindObjectOfType<UIManager>();
	}

	// Update is called once per frame
	void Update()
	{
		Texture2D nextBackground = getNextBackground();
		myImage.sprite = createSpriteFromTex2D(nextBackground);
	}

	private Texture2D getNextBackground(){
        if (mySceneCatalogue.getIsInInteriorScene() == true)
        {
            return dateBackgroundsForThisScene()[0];
        }
		return backgroundsForThisScene()[myTimeLord.timeStep % backgroundsForThisScene().Length];
	}

	public static Sprite createSpriteFromTex2D(Texture2D from){
		return Sprite.Create(from, new Rect(0, 0, from.width, from.height), Vector2.zero);
	}

	Texture2D[] backgroundsForThisScene(){
		return backgroundsMatchingSceneName(backgrounds);
	}

	Texture2D[] dateBackgroundsForThisScene()
    {
		return backgroundsMatchingSceneName(dateBackgrounds);
    }

	Texture2D[] backgroundsMatchingSceneName(Texture2D[] backgroundsToCheck){
		string sceneName = this.mySceneCatalogue.getCurrentSceneName();
        List<Texture2D> workingListOfBackgrounds = new List<Texture2D>();
        foreach (Texture2D background in backgroundsToCheck)
        {
            if (background.name.ToLower().Contains(sceneName.ToLower().Replace(' ', '_')))
            {
                workingListOfBackgrounds.Add(background);
            }
        }
        return workingListOfBackgrounds.ToArray();
	}
}
