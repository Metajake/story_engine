using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMaestro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void activatePartnerUIInLocation(List<Character> potentialConversationPartners)
    {
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.sprite = BackgroundSwapper.createSpriteFromTex2D(potentialConversationPartners[i].image);
                partnerPortrait.color = new Color(255, 255, 255, 1);
                partnerNameplate.text = potentialConversationPartners[i].givenName + " " + potentialConversationPartners[i].surname;

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
}
