﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SampleAlgorithm : StrategyBase {

    public override void OnStart() {
        name = "Sample";
        author = "Tutorial";
    }

    public override void Update(int turnNumber) {
        if (gameboard.DeployUnit(UnitType.BARRICADE, 14, 7 )) { gameboard.log("Replaced Wall"); }
    
        if (turnNumber == 30) { gameboard.DeployUnit(UnitType.ARCHER, 5, 5); }

        if (turnNumber == 60) { gameboard.DeployUnit(UnitType.CATAPULT, 6, 5); }
        /*
        gameboard.DeployUnit(UnitType.BARRICADE, 5, 5);
        gameboard.DeployUnit(UnitType.BARRICADE, 6, 6);
        gameboard.DeployUnit(UnitType.BARRICADE, 7, 5);
        gameboard.DeployUnit(UnitType.BARRICADE, 8, 6);
        gameboard.DeployUnit(UnitType.BARRICADE, 9, 5);
        gameboard.DeployUnit(UnitType.BARRICADE, 10, 6);
        gameboard.DeployUnit(UnitType.BARRICADE, 11, 5);
        gameboard.DeployUnit(UnitType.BARRICADE, 12, 6);
        gameboard.DeployUnit(UnitType.BARRICADE, 13, 5);
        gameboard.DeployUnit(UnitType.BARRICADE, 14, 6);
        */
        
    }
}
