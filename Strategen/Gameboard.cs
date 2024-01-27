using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategen {
    class DeployAction {
        public UnitType unitType;
        public bool team;
        public int x;
        public int y;
        public DeployAction(UnitType unitType, bool team, int x, int y) {
            this.team = team;
            this.unitType = unitType;

            this.x = x;
            this.y = y;
        }
    }

    public class Gameboard {
        private BoardInterface redStrat; //team true
        private BoardInterface blueStrat; //team false

        private int turnNumber = 0;

        private float redCores = 10;
        private float blueCores = 10;

        private float redHealth = 100;
        private float blueHealth = 100;

        private Match match;

        private List<List<Unit>> units = new List<List<Unit>>();

        private List<DeployAction> deployCache = new List<DeployAction>();

        public Gameboard(Match match, StrategyBase redStrat, StrategyBase blueStrat) {
            this.redStrat = new BoardInterface(redStrat, this, true);
            this.blueStrat = new BoardInterface(blueStrat, this, false);
            this.match = match;

            for (int x = 0; x < 16; x++) {
                units.Add(new List<Unit>());
                for (int y = 0; y < 16; y++) {
                    units[x].Add(new Unit());
                }
            }
        }

        public int GetTurn() {
            return turnNumber;
        }

        public float getHealth(bool team) {
            return team ? redHealth : blueHealth;
        }

        public bool DeployUnit(bool team, UnitType unitType, int x, int y) {
            int newX = x;
            int newY = y;
            if (!team) {
                newX = 15 - x;
                newY = 15 - y;
            }
            if ((team ? redCores : blueCores) >= 3) {
                if (units[newX][newY].GetUnitType() == UnitType.EMPTY) {
                    deployCache.Add(new DeployAction(unitType, team, newX, newY));
                    if (team) {
                        redCores = redCores - 3;
                    } else {
                        blueCores = blueCores - 3;
                    }
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
            
        }

        public UnitDetails GetUnitDetails(StrategyBase sender, int x, int y) {
            return new UnitDetails(UnitType.EMPTY, 0);
        }

        public float GetCores(bool team) {
            return team ? redCores : blueCores;
        }


        public void Update() {
            turnNumber++;
            redCores++; blueCores++;
            redStrat.RunUpdate(0);
            blueStrat.RunUpdate(0);
            for (int i = 0; i < deployCache.Count; i++) {
                units[deployCache[i].x][deployCache[i].y].SetType(deployCache[i].unitType);
                units[deployCache[i].x][deployCache[i].y].SetTeam(deployCache[i].team);
            }
            match.Render(units, turnNumber);
        }
    }

    public class BoardInterface {
        private bool team;
        private StrategyBase strategy;
        private Gameboard gameboard;

        public BoardInterface(StrategyBase strategy, Gameboard gameboard, bool team) {
            this.strategy = strategy;
            this.gameboard = gameboard;
            this.team = team;
            this.strategy.SetInterface(this);
        }
        public void Initialize() {
            strategy.OnStart();
        }
        public void RunUpdate(int turnNumber) {
            strategy.Update(turnNumber);
        }
        public float GetCores() {
            return gameboard.GetCores(team);
        }

        public bool DeployUnit(UnitType unitType, int x, int y) {
            return gameboard.DeployUnit(team, unitType, x, y) ? true : false;
        }
    }
}
