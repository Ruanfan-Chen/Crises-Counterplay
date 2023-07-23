using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInvulnerable { }

public interface ISpeedBonus
{
    float GetValue();
}

public interface IDisarmed { }

public interface IMoveDisabled { }