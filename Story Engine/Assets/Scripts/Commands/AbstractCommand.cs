using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCommand : MonoBehaviour, ICommand {

	public abstract void execute(bool toFastForwad);
    
}
