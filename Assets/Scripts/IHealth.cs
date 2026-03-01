using System;
using UnityEngine;

public interface IHealth
{
    //Send new health with it
    public event Action<int> OnHealthChanced;
    int getHealth();
    int getMaxHealth();
    
}
