using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacegame
{
    // item metadata
    public struct ItemData
    {
        // name of the item in the ui
        public string name;

        // whether the item should be shown as food in the ui
        public bool food;
        // whether item should be shown as armour in the ui
        public bool armour;
        // whether the item should be shown as a weapon in the ui
        public bool weapon;

        // amount healed when the item is used
        public int healAmount;
        // defense given when the item is equipped
        public int defense;
        // attack damage when the item is used as a weapon
        public int attackDamage;

        // text displayed in battle
        public string[] battleText;
        // script that's ran if the item is used 
        public string script;
        // script args
        public string[] scriptArgs;

        // constructor to initialize all of the fields manually, if needed
        public ItemData(
            string name,
            bool food, 
            bool armour, 
            bool weapon, 
            int healAmount, 
            int defense, 
            int attackDamage, 
            string[] battleText,
            string script,
            string[] scriptArgs)
        {
            this.name = name;
            this.food = food;
            this.armour = armour;
            this.weapon = weapon;
            this.healAmount = healAmount;
            this.defense = defense;
            this.attackDamage = attackDamage;
            this.battleText = battleText;
            this.script = script;
            this.scriptArgs = scriptArgs;
        }
    }
}
