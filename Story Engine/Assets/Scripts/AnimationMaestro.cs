using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMaestro : MonoBehaviour
{
    private GameState myGameState;

    // Use this for initialization
    void Start()
    {
        myGameState = GameObject.FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void placePotentialPartnersInUI(List<Character> potentialConversationPartners)
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.sprite = BackgroundSwapper.createSpriteFromTex2D(potentialConversationPartners[i].image);
                partnerNameplate.text = potentialConversationPartners[i].givenName + " " + potentialConversationPartners[i].surname;
                //partnerPortrait.color = new Color(255, 255, 255, 1);
            }
            else
            {
                disablePartnerSelectionUI(partnerPortrait, partnerNameplate);
            }
        }
    }

    public void clearPotentialPartners()
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
            disablePartnerSelectionUI(partnerPortrait, partnerNameplate);
        }
    }

    private static void disablePartnerSelectionUI(Image partnerPortrait, Text partnerNameplate)
    {
        partnerPortrait.color = new Color(partnerPortrait.color.r, partnerPortrait.color.g, partnerPortrait.color.b, 0);
        partnerNameplate.text = "";
    }

    public void fadeInCharacters(List<Character> potentialConversationPartners)
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.color = new Color(255, 255, 255, 0);
                StartCoroutine(fadeImageTo(partnerPortrait, 1.0f, 0.6f));
            }
        }
    }

    IEnumerator fadeImageTo(Image characterImage, float aValue, float aTime)
    {
        //Use in Update. Example: StartCoroutine(fadeImageTo(partnerPortrait, 1.0f, 1.0f));
        float alpha = characterImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, Mathf.Lerp(alpha, aValue, t));
            characterImage.color = newColor;
            yield return null;
        }
    }
}