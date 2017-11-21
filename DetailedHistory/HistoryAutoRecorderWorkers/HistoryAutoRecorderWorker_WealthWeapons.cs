using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ExtendedHistoryTab
{
    class HistoryAutoRecorderWorker_WealthWeapons : HistoryAutoRecorderWorker
    {
        public override float PullRecord()
        {
            float num = 0f;
            List<Map> maps = Find.Maps;
            for (int i = 0; i < maps.Count; i++)
            {
                if (maps[i].IsPlayerHome)
                {
                    num += maps[i].GetComponent<WealthWatcherComp>().WealthWeapons;
                }
            }
            return num;
        }
    }
}
