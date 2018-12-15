using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventSubscriber{

    void eventOccured(IGameEvent occurringEvent);

}
