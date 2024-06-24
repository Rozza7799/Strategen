class ReactiveStrategy : StrategyBase {

    public override void OnStart() {
        name = "Reactive";
        author = "Josh";
    }

    public override void Update(int turnNumber) {

        for (int x = 0; x < 15; x++) {
            for (int y = 8; y < 15; y++) {
                if (gameboard.GetUnitDetails(x, y).GetUnitType() == UnitType.BARRICADE && gameboard.GetUnitDetails(x, y).team == true) {
                    gameboard.DeployUnit(UnitType.BARRICADE, x, y - 2);
                    if (gameboard.GetCores() >= 5) {
                        gameboard.DeployUnit(UnitType.KNIGHT, x, y - 2);
                    }

                }
            }
        }


        if (gameboard.GetCores() >= 5) {
            for (int i = 0; i < 15; ++i) {
                gameboard.DeployUnit(UnitType.KNIGHT, i, 7);
            }

        }
    }
}
