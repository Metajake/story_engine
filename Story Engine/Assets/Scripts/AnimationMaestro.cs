using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AnimationMaestro : MonoBehaviour
{
    private SceneCatalogue mySceneCatalogue;

    private Text textPanel;
    GameObject dateActionButton;

    void Start()
    {
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();

        textPanel = GameObject.Find("TextPanel").GetComponentInChildren<Text>();
        dateActionButton = GameObject.Find("DateActionButton");
    }

    public void updatePotentialPartnersSprites(List<Character> potentialConversationPartners)
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
            Text partnerLoveAmount = GameObject.Find("Character " + (i + 1) + " LoveAmount").GetComponent<Text>();

            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.sprite =
                    BackgroundSwapper.createSpriteFromTex2D(potentialConversationPartners[i].image);
                partnerNameplate.text = potentialConversationPartners[i].givenName + " " +
                                        potentialConversationPartners[i].surname;
                if (potentialConversationPartners[i] is DateableCharacter)
                {
                    partnerLoveAmount.text =
                        "In Love Amount: " + potentialConversationPartners[i].inLoveAmount.ToString();
                }
                else
                {
                    partnerLoveAmount.text = "";
                }
            }
            else
            {
                disablePartnerSelectionUI(partnerPortrait, partnerNameplate, partnerLoveAmount);
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
            disablePartnerSelectionUI(partnerPortrait, partnerNameplate, partnerLoveAmount);
        }
    }

    private static void disablePartnerSelectionUI(Image partnerPortrait, Text partnerNameplate, Text partnerLoveAmount)
    {
        partnerPortrait.color = new Color(partnerPortrait.color.r, partnerPortrait.color.g, partnerPortrait.color.b, 0);
        partnerNameplate.text = "";
        partnerLoveAmount.text = "";
    }

    public void fadeInCharacters(List<Character> potentialConversationPartners, float fadeDuration = 0.6f)
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.color = new Color(255, 255, 255, 0);
                StartCoroutine(fadeImageTo(partnerPortrait, 1.0f, fadeDuration));
            }
        }
    }

    public void fadeInCharacterImage(int imagePositionToFadeIn, float fadeDuration = 0.6f)
    {
        Image partnerPortrait = GameObject.Find("Character " + (imagePositionToFadeIn) + " Portrait").GetComponent<Image>();
        partnerPortrait.color = new Color(255, 255, 255, 0);
        StartCoroutine(fadeImageTo(partnerPortrait, 1.0f, fadeDuration));
    }

    public void fadeOutCharacters(List<Character> potentialConversationPartners)
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.color = new Color(255, 255, 255, 1);
                StartCoroutine(fadeImageTo(partnerPortrait, 0.0f, 0.6f));
            }
        }
    }

    IEnumerator fadeImageTo(Image characterImage, float aValue, float aTime)
    {
        float alpha = characterImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b,
                Mathf.SmoothStep(alpha, aValue, t));
            characterImage.color = newColor;
            yield return null;
        }

        characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, aValue);
    }

    public IEnumerator delayGameCoroutine(float seconds, Action passedFunction)
    {
        yield return new WaitForSeconds(seconds);
        passedFunction();
    }

    public void writeDescriptionText(string toWrite, Text toWriteTo)
    {
        GameObject.Find("DescriptionTextScroller").GetComponent<TextScroller>().SetText(toWrite);
    }

    internal void showNeutralDescriptionText()
    {
        writeDescriptionText(mySceneCatalogue.neutralResultForCurrentLocationDescription(), textPanel);
    }

    internal void abandonDateDescription()
    {
        writeDescriptionText("Bye, lame.", textPanel);
        dateActionButton.SetActive(false);
    }
}