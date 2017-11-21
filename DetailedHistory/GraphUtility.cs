using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace DetailedHistory
{
    class GraphUtility
    {
        private static float cachedGraphTick = -1;
        private static List<SimpleCurveDrawInfo> curves = new List<SimpleCurveDrawInfo>();
        public static void DrawGraph(Rect graphRect, Rect legendRect, FloatRange section, List<CurveMark> marks, HistoryAutoRecorderGroup recorderGroup)
        {
            int ticksGame = Find.TickManager.TicksGame;
            if (ticksGame != GraphUtility.cachedGraphTick)
            {
                GraphUtility.cachedGraphTick = ticksGame;
                GraphUtility.curves.Clear();
                for (int i = 0; i < recorderGroup.recorders.Count; i++)
                {
                    HistoryAutoRecorder historyAutoRecorder = recorderGroup.recorders[i];
                    SimpleCurveDrawInfo simpleCurveDrawInfo = new SimpleCurveDrawInfo();
                    simpleCurveDrawInfo.color = historyAutoRecorder.def.graphColor;
                    simpleCurveDrawInfo.label = historyAutoRecorder.def.LabelCap;
                    simpleCurveDrawInfo.labelY = historyAutoRecorder.def.GraphLabelY;
                    simpleCurveDrawInfo.curve = new SimpleCurve();
                    for (int j = 0; j < historyAutoRecorder.records.Count; j++)
                    {
                        simpleCurveDrawInfo.curve.Add(new CurvePoint((float)j * (float)historyAutoRecorder.def.recordTicksFrequency / 60000f, historyAutoRecorder.records[j]), false);
                    }
                    simpleCurveDrawInfo.curve.SortPoints();
                    if (historyAutoRecorder.records.Count == 1)
                    {
                        simpleCurveDrawInfo.curve.Add(new CurvePoint(1.66666669E-05f, historyAutoRecorder.records[0]), true);
                    }
                    GraphUtility.curves.Add(simpleCurveDrawInfo);
                }
            }
            if (Mathf.Approximately(section.min, section.max))
            {
                section.max += 1.66666669E-05f;
            }
            SimpleCurveDrawerStyle curveDrawerStyle = Find.History.curveDrawerStyle;
            curveDrawerStyle.FixedSection = section;
            curveDrawerStyle.UseFixedScale = recorderGroup.def.useFixedScale;
            curveDrawerStyle.FixedScale = recorderGroup.def.fixedScale;
            curveDrawerStyle.YIntegersOnly = recorderGroup.def.integersOnly;
            SimpleCurveDrawer.DrawCurves(graphRect, GraphUtility.curves, curveDrawerStyle, marks, legendRect);
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}
