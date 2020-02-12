using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// variable based dialogue
public class MapCartographer : MonoBehaviour, IKnownLocationsChangedObserver {
	private const int numberOfRows = 3;
	private const int numberOfColumns = 3;
    int[] mapColumnLengths = new int[] { 4, 4, 3 };
	public GameObject mapLocationButtonPrefab;
	private SceneCatalogue mySceneCatalogue;
	private GameObject myMapPanel;
    private InputOrganizer myInputOrganizer;

	// Use this for initialization
	void Start ()
	{
		myMapPanel = GameObject.Find("MapPanel");

        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myInputOrganizer = GameObject.FindObjectOfType<InputOrganizer>();

        createLocationButtons();

        mySceneCatalogue.Subscribe(this);

	}

    // Update is called once per frame
    void Update()
    {

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

            for (int k = 0; k < mapColumnLengths[j]; k++)
            {
                //TODO make this int 4 more flexible (accommodating for various map sizes. Maybe make it the length of the largest row?)
                int mapButtonIndex = j * 4 + k;

                if (mapButtonIndex >= mySceneCatalogue.getLocationCount())
                {
                    return;
                }

                if (!mySceneCatalogue.locations[mapButtonIndex].isKnown)
                {
                    continue;
                }

                createLocationButton(j, k, mapButtonIndex);

            }

        }
        
	}

    private void createLocationButton(int j, int k, int mapButtonIndex)
    {
        GameObject buttonObject = GameObject.Instantiate(mapLocationButtonPrefab, myMapPanel.transform);

        //These position transform multiples should be half the width/height of the prefab button. I don't know why.
        buttonObject.transform.Translate(new Vector3(k * 117, j * -100));

        buttonObject.GetComponentInChildren<Text>().text = mySceneCatalogue.getLocationNames()[mapButtonIndex];

        UnityAction buttonAction = () => myInputOrganizer.BTN_onLocationClick(mapButtonIndex);
        buttonObject.GetComponent<Button>().onClick.AddListener(buttonAction);
    }

    public void highlightCurrentLocation()
    {
        Button[] allButtons = myMapPanel.GetComponentsInChildren<Button>();

        foreach (Button b in allButtons)
        {
            b.GetComponent<Image>().color = Color.white;

            if(mySceneCatalogue.getCurrentSceneName() == b.GetComponentInChildren<Text>().text)
            {
                b.GetComponent<Image>().color = Color.cyan; 
            }
        }
    }

    public void changeScene(int sceneNumber)
    {
        mySceneCatalogue.setCurrentSceneNumber(sceneNumber);
    }

    public void BeNotifiedOfLocationChange()
    {
        createLocationButtons();
    }
}
