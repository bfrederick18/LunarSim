﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarSim
{
    public enum GameState 
    { 
        MainMenu, 
        Simulation, 
        MapCreator 
    }

    public enum LunarianState
    {
        Idle,
        Wandering,
        WalkingOne,
        WalkingTwo
    }
}
