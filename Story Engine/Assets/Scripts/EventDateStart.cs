using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDateStart : IGameEvent
{
    private string eventType = "DATESTARTEVENT";

    public string getEventType()
    {
        return this.eventType;
    }
}