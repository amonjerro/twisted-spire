using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKickableTarget
{
    // Overridden by child class to run specific logic when activated
    public abstract void OnKicked();
}
