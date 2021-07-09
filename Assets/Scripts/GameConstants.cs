using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
    // Mario basic starting values
    public int playerMaxSpeed = 10;
    public int playerMaxJumpSpeed = 20;
    public int playerDefaultForce = 25;

    // for Consumable Blink Interval
    public float powerUpBlinkInterval = 0.5f;

    // for RedMushroom.cs (Jump)
    public int powerupRedSlot = 0;
    public int powerUpRedTimeSteps = 10;
    public int powerUpRedIncrease = 10;

    // for GreenMushroom.cs (Speed)
    public int powerupGreenSlot = 1;
    public int powerUpGreenTimeSteps = 20;
    public int powerUpGreenMultiplier = 2;
    
    // for Debris.cs
    public int breakTimeStep = 30;
    public int breakDebrisTorque = 10;
    public int breakDebrisForce = 10;
    
    // for BreakBrick.cs
    public int debrisCount = 10;

    // for EnemyController.cs
    public int maxOffset = 5;
    public float enemyPatroltime = 2.0f;
    public float enemySpawnPointCenterX = 25.0f;
    public float enemySpawnPointCenterY = -2.0f;
    public float groundSurface = -2.0f;
    public int bounce = 6;
}
