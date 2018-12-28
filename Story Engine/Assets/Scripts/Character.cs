using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public Texture2D image;
    public string givenName;
    public string surname;
    public string currentSceneName;
    public bool isInside;
    public int inLoveAmount;
    public bool[] activeTimes;
    public bool isPresent;
    public int returnTime;

    public virtual bool checkIsPresent(){
        inLoveAmount = 0;
        return this.isPresent;
    }
    
    public void returnToPresent()
    {
        this.isPresent = true;
    }
}
