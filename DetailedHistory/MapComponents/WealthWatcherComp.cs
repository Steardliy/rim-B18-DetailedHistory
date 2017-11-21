using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ExtendedHistoryTab
{
    class WealthWatcherComp : MapComponent
    {
        private float wealthWeapons;
        private float wealthApparels;
        private float lastCountTick = -99999f;

        private const int MinCountInterval = 5000;

        public WealthWatcherComp(Map map) : base(map) { }

        public float WealthWeapons
        {
            get
            {
                this.RecountIfNeeded();
                return this.wealthWeapons;
            }
        }

        public float WealthApparels
        {
            get
            {
                this.RecountIfNeeded();
                return this.wealthApparels;
            }
        }

        private void RecountIfNeeded()
        {
            if ((float)Find.TickManager.TicksGame - this.lastCountTick > MinCountInterval)
            {
                this.ForceRecount(false);
            }
        }

        public void ForceRecount(bool allowDuringInit = false)
        {
            if (!allowDuringInit && Current.ProgramState != ProgramState.Playing)
            {
                Log.Error("WealthWatcherComp recount in game mode " + Current.ProgramState);
                return;
            }
            this.wealthWeapons = 0f;
            this.wealthApparels = 0f;
            List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.Weapon);
            for (int i = 0; i < list.Count; i++)
            {
                Thing thing = list[i];
                if (!thing.Position.Fogged(thing.Map))
                {
                    this.wealthWeapons += (float)thing.stackCount * thing.MarketValue;
                }
            }
            List<Thing> list2 = this.map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel);
            for (int i = 0; i < list2.Count; i++)
            {
                Thing thing = list2[i];
                if (!thing.Position.Fogged(thing.Map))
                {
                    this.wealthApparels += (float)thing.stackCount * thing.MarketValue;
                }
            }

            foreach (Pawn current in this.map.mapPawns.FreeColonists)
            {
                this.wealthWeapons += this.GetEquipmentWeaponWealth(current);
                this.wealthApparels += this.GetEquipmentApparelWealth(current);
            }

            this.lastCountTick = (float)Find.TickManager.TicksGame;
        }

        private float GetEquipmentWeaponWealth(Pawn pawn)
        {
            return 0;
        }
        private float GetEquipmentApparelWealth(Pawn pawn)
        {
            return 0;
        }
    }
}
