using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnownLocationsChangedObservable {

    void Subscribe(IKnownLocationsChangedObserver observer);

    void Notify();

    void Unsubscribe(IKnownLocationsChangedObserver observer);

}