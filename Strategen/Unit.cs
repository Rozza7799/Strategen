using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategen
{
    public enum UnitType {
        EMPTY,
        ATTACKER,
        TANK,
        WALL,
        MINE
    }
    public class Unit {
        private int tick = 0;
        private bool team;
        private UnitType type;
        private double health;
        public Unit() {
            type = UnitType.EMPTY;
            health = 0;
        }
        public int GetTick() {
            return tick;
        }

        public void SetType(UnitType type) {
            this.type = type;
            
        }

        public double GetHealth() {
            return health;
        }
        public UnitType GetUnitType() {
            return type;
        }

        public bool getTeam() {
            return team;
        }
        public void SetTeam(bool team) {
            this.team = team;
        }

        public UnitDetails GenerateUnitDetails() {
            return new UnitDetails(type, health);
        }
    }

    public class UnitDetails {
        public UnitType type;
        public double health;

        public UnitDetails(UnitType type, double health){
            this.type = type;
            this.health = health;
        }
        public double GetHealth() { return health; }
        public UnitType GetUnitType() { return type; }

    }
}
