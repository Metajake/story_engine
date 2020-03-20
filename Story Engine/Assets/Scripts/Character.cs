using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterLocation
{
    public string locationName;
    public bool isInside;
    public bool isActive;
}

public class Character : MonoBehaviour {

    public Texture2D image;
    public Texture2D portrait;
    public string givenName;
    public string surname;
    public int inLoveAmount;
    public bool isPresent;
    public int returnTime;
    public int relocationInterval;
    public List<CharacterLocation> locations;
    
    public virtual bool checkIsPresent(){
        inLoveAmount = 0;
        return this.isPresent;
    }
    
    public void returnToPresent()
    {
        this.isPresent = true;
    }
}
