using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeEvent: IGameEvent {
    private string eventType;

    void Start()
    {
        this.eventType = "SCENECHANGE";
    }

    public string ReturnType()
    {
        return this.eventType;
    }

}
