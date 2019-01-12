using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMaestro : MonoBehaviour
{
    private SceneCatalogue mySceneCatalogue;
    private CharacterManager myDialogueManager;
    private GameState myGameState;

    private Text textPanel;
    GameObject dateActionButton;

    // Use this for initialization
    void Start()
    {
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myDialogueManager = GameObject.FindObjectOfType<CharacterManager>();
        myGameState = GameObject.FindObjectOfType<GameState>();

        textPanel = GameObject.Find("TextPanel").GetComponentInChildren<Text>();
        dateActionButton = GameObject.Find("DateActionButton");
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void updateCharacterPanelSprites(List<Character> potentialConversationPartners)
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
            Text partnerLoveAmount = GameObject.Find("Character " + (i + 1) + " LoveAmount").GetComponent<Text>();

            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.sprite = BackgroundSwapper.createSpriteFromTex2D(potentialConversationPartners[i].image);
                partnerNameplate.text = potentialConversationPartners[i].givenName + " " + potentialConversationPartners[i].surname;
                if (potentialConversationPartners[i] is DateableCharacter)
                {
                    partnerLoveAmount.text = "In Love Amount: " + potentialConversationPartners[i].inLoveAmount.ToString();
                }
                else
                {
                    partnerLoveAmount.text = "";
                }
                if(partnerPortrait.color.a == 0)
                {
                    fadeTo(potentialConversationPartners, true);
                }
                //partnerPortrait.color = new Color(255, 255, 255, partnerPortrait.color.a);

            }
            else
            {
                hideUnusedCharacterUI(partnerPortrait, partnerNameplate, partnerLoveAmount);
            }
        }
    }

    public void clearPotentialPartners()
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
            Text partnerLoveAmount = GameObject.Find("Character " + (i + 1) + " LoveAmount").GetComponent<Text>();
            hideUnusedCharacterUI(partnerPortrait, partnerNameplate, partnerLoveAmount);
        }
    }

    private static void hideUnusedCharacterUI(Image partnerPortrait, Text partnerNameplate, Text partnerLoveAmount)
    {
        partnerPortrait.color = new Color(partnerPortrait.color.r, partnerPortrait.color.g, partnerPortrait.color.b, 0);
        partnerNameplate.text = "";
        partnerLoveAmount.text = "";
    }

    public void fadeTo(List<Character> potentialConversationPartners, bool isFadeIn)
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.color = new Color(255, 255, 255, isFadeIn ? 0 : 1);
                StartCoroutine(fadeImageTo(fadeComplete, partnerPortrait, isFadeIn ? 1 : 0, 0.6f));
            }
        }
    }

    public void fadeComplete()
    {
        Debug.Log("Fade Complete");
    }

    IEnumerator fadeImageTo(System.Action callback, Image characterImage, float aValue, float aTime)
    {
        float alpha = characterImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, Mathf.SmoothStep(alpha, aValue, t));
            characterImage.color = newColor;
            yield return null;
        }
        characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, aValue);
        callback();
    }

    public void updateLocationDescription()
    {
        setDescriptionText(mySceneCatalogue.getLocationDescription(), textPanel);
    }

    public void setDescriptionText(string toWrite, Text toWriteTo)
    {
        toWriteTo.text = toWrite;
    }

    internal void showNeutralDescriptionText()
    {
        setDescriptionText(mySceneCatalogue.neutralResultForCurrentLocationDescription(), textPanel);
    }

    internal void abandonDateDescription()
    {
        setDescriptionText("Bye, lame.", textPanel);
        dateActionButton.SetActive(false);
    }

}