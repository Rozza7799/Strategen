using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

    private Strategen.Match match;

    private List<List<Unit>> units = new List<List<Unit>>();

    private List<DeployAction> deployCache = new List<DeployAction>();

    public Gameboard(Strategen.Match match, StrategyBase redStrat, StrategyBase blueStrat) {
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
        int cost = 0;
        if (unitType == UnitType.BARRICADE) { cost = 2; }
        if (unitType == UnitType.KNIGHT) { cost = 3; }
        if (unitType == UnitType.ARCHER) { cost = 4; }
        if (unitType == UnitType.CATAPULT) { cost = 10; }
        if (unitType == UnitType.DRAGON) { cost = 25; }

        if ((team ? redCores : blueCores) >= cost) {
            if (units[x][y].GetUnitType() == UnitType.EMPTY) {
                deployCache.Add(new DeployAction(unitType, team, x, y)); // This is fine because neither team can place ontop of eachother
                if (team) {
                    redCores -= cost;
                } else {
                    blueCores -= cost;
                }
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }

    }

    public UnitDetails GetUnitDetails(bool team, int x, int y) {
        UnitDetails unit = new UnitDetails(units[x][y].GetUnitType(), units[x][y].GetHealth());
        unit.team = unit.team == team;
        return unit;
    }

    public float GetCores(bool team) {
        return team ? redCores : blueCores;
    }


    public void Update() {
        //Update turn info
        deployCache.Clear();
        turnNumber++;
        redCores += 1; 
        blueCores += 1;
        //Let the strategies decide their actions
        redStrat.RunUpdate(turnNumber);
        blueStrat.RunUpdate(turnNumber);

        foreach (DeployAction d in deployCache) {
            if (d.unitType == UnitType.BARRICADE) {
                units[d.x][d.y] = new Barricade(d.team);
            } else if (d.unitType == UnitType.KNIGHT) {
                units[d.x][d.y] = new Knight(d.team);
            } else if (d.unitType == UnitType.DRAGON) {
                units[d.x][d.y] = new Dragon(d.team);
            } else if (d.unitType == UnitType.ARCHER) {
                units[d.x][d.y] = new Archer(d.team);
            } else if (d.unitType == UnitType.CATAPULT) {
                units[d.x][d.y] = new Catapult(d.team);
            }
        }
        
        //Update tick
        for (int x = 0; x < 16; x++) {
            for (int y = 0; y < 16; y++) {
                units[x][y].Update();
            }
        }

        //Damage Update
        //Get a list of all the damage events from all units
        List<TileDamage> damages = new List<TileDamage>(); 
        for (int x = 0; x < 15; x++) {
            for (int y = 0; y < 15; y++) {
                foreach (TileDamage damage in units[x][y].GetDamages()) {                    
                    damages.Add(new TileDamage(damage.x + x, damage.y + y, damage.damageType, damage.damage, damage.team));
                }
            }
        }

        //Apply the damage
        foreach (TileDamage damage in damages) {
            if (damage.x >= 0 && damage.x <= 15 && damage.y >= 0 && damage.y <= 15) {
                if (units[damage.x][damage.y].GetUnitType() != UnitType.EMPTY && damage.team == !units[damage.x][damage.y].getTeam()) {
                    units[damage.x][damage.y].Damage(damage.damage, damage.damageType);
                }
            }
        }

        //Check if unit has died
        for (int x = 0; x < 16; x++) {
            for (int y = 0; y < 16; y++) {
                if (units[x][y].GetUnitType() != UnitType.EMPTY && units[x][y].GetHealth() <= 0) {
                    units[x][y] = new Unit();
                }
            }
        }

        //Movement
        for (int x = 0; x < 16; x++) {
            for (int y = 0; y < 16; y++) {
                if (units[x][y].GetUnitType() != UnitType.EMPTY && units[x][y].IsMoving()) {
                    if (units[x][y].getTeam()) {
                        if (y + 1 > 15) {
                            blueHealth--;
                            units[x][y] = new Unit();
                        } else if (units[x][y+1].GetUnitType() == UnitType.EMPTY) {
                            units[x][y+1] = units[x][y];
                            units[x][y] = new Unit();
                        }
                    } else {
                        if (y - 1 < 0 ) {
                            redHealth--;
                            units[x][y] = new Unit();
                        } else if (units[x][y - 1].GetUnitType() == UnitType.EMPTY) {
                            units[x][y - 1] = units[x][y];
                            units[x][y] = new Unit();
                        }
                    }
                }
            }
        }
        for (int x = 0; x < 16; x++) {
            for (int y = 0; y < 16; y++) {
                units[x][y].ResetMove();
            }
        }
        match.Render(units, damages, turnNumber);
    }

    public void log(string message, bool team) {
        if (team) {
            match.redLogs.Children.Add(new TextBlock() { FontSize = 12, Foreground = new SolidColorBrush(Color.FromRgb(255, 200, 200)), Text = message });
        } else {
            match.blueLogs.Children.Add(new TextBlock() { FontSize = 12, Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 255)), Text = message });
        }
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

    public void log(string message) {
        gameboard.log(message, team);
    }

    public void Initialize() {
        strategy.OnStart();
    }
    public void RunUpdate(int turnNumber) {
        strategy.Update(turnNumber);
    }

    public UnitDetails GetUnitDetails(int x, int y) {
        if (x < 0 || x > 15 || y < 0 || y > 15) {
            return new UnitDetails(UnitType.EMPTY, 0);
        } else {
            int newX = x;
            int newY = y;
            if (!team) {
                newX = 15 - x;
                newY = 15 - y;
            }
            return gameboard.GetUnitDetails(team, newX, newY);
        }
    }

    public float GetCores() {
        return gameboard.GetCores(team);
    }

    public bool DeployUnit(UnitType unitType, int x, int y) {
        if (x < 0 || x > 7 || y < 0 || y > 15) {
            return false; 
        } else {
            int newX = x;
            int newY = y;
            if (!team) {
                newX = 15 - x;
                newY = 15 - y;
            }
            return gameboard.DeployUnit(team, unitType, newX, newY) ? true : false;
        }
    }
}

