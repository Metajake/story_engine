using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class characterLocation
{
    public string locationName;
    public bool isInside;
    public bool isActive;
}

public class Character : MonoBehaviour {

    public Texture2D image;
    public string givenName;
    public string surname;
    public int inLoveAmount;
    public bool isPresent;
    public int returnTime;
    public List<characterLocation> locations;

    public virtual bool checkIsPresent(){
        inLoveAmount = 0;
        return this.isPresent;
    }
    
    public void returnToPresent()
    {
        this.isPresent = true;
    }
}
