using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic{

	public static float ExperienceForNextLevel(int currentLevel)
    {
        if (currentLevel == 0) return 0;
        //return (currentLevel * currentLevel + currentLevel + 3) * 4;
        
        return ((currentLevel - 1) ^ 3 + 60) / 5 * ((currentLevel - 1) * 2 + 60);
    }

    
}
