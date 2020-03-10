using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventSubscriber{

    void eventOccurred(IGameEvent occurringEvent);

}
