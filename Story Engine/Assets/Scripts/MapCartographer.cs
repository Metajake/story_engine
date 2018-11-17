using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// variable based dialogue
public class MapCartographer : MonoBehaviour, IKnownLocationsChangedObserver {
	private const int numberOfRows = 3;
	private const int numberOfColumns = 4;
	public GameObject mapLocationButtonPrefab;
	private SceneCatalogue mySceneCatalogue;
	private GameObject myMapPanel;
    private UIManager myUIManager;

	// Use this for initialization
	void Start ()
	{
		myMapPanel = GameObject.Find("MapPanel");

        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		myUIManager = GameObject.FindObjectOfType<UIManager>();

        createLocationButtons();

        mySceneCatalogue.Subscribe(this);

	}

	public void createLocationButtons()
	{

		Button[] allButtons = myMapPanel.GetComponentsInChildren<Button>();

		foreach(Button b in allButtons){
            if(!(b.gameObject.name.ToLower()=="closemapbutton")){
				Destroy(b.gameObject);
            }
		}

        for (int j = 0; j < numberOfRows; j++)
        {

            for (int k = 0; k < numberOfColumns; k++)
            {

                int mapButtonIndex = j * numberOfColumns + k;

                if (mapButtonIndex >= mySceneCatalogue.getLocationCount())
                {
                    return;
                }

                if (!mySceneCatalogue.locations[mapButtonIndex].isKnown)
                {
                    continue;
                }

                GameObject buttonObject = GameObject.Instantiate(mapLocationButtonPrefab, myMapPanel.transform);

                buttonObject.transform.Translate(new Vector3(-400 + k * 200, 200 - j * 200));

                buttonObject.GetComponentInChildren<Text>().text = mySceneCatalogue.getLocationNames()[mapButtonIndex];

                UnityAction buttonAction = () => onLocationClick(mapButtonIndex);
                buttonObject.GetComponent<Button>().onClick.AddListener(buttonAction);
                
			}		

		}
        
	}


	// Update is called once per frame
	void Update () {
		
	}
    
	public void onLocationClick(int sceneNumber){
		mySceneCatalogue.setCurrentSceneNumber(sceneNumber);
        myUIManager.toggleMap();
	}

    public void BeNotifiedOfLocationChange()
    {
        createLocationButtons();
    }
}
