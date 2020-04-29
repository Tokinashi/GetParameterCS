using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetParameterCS
{
    class ClsLive2DMotion
    {
        Live2DMotionClass motion;
        float fps;
        List<string> ids;

        public ClsLive2DMotion(DataTable edittable,float fps)
        {
            this.fps = fps;
            motion = new Live2DMotionClass();
            SetIds(edittable);
        }

        private void SetIds(DataTable dataTable)
        {
            ids = new List<string>();

            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                ids.Add(dataTable.Rows[j][0].ToString());
            }
        }

        public Motion3Json GetMotions(List<List<float>> frames)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                for (int j = 0; j < frames[i].Count; j++)
                {
                    if (ids[j] != "")
                    {
                        motion.AddSegment(ids[j], i / fps, frames[i][j]);
                    }
                }
            }
            return motion.OutputMotion3Json();
        }

    }

    class Live2DMotionClass
    {
        public int Version = 3;
        public Live2dMeta Meta { get; set; }
        public List<LCurvePoint> Curves { get; set; }


        public Live2DMotionClass()
        {
            Meta = new Live2dMeta();
            Curves = new List<LCurvePoint>();
        }
        public void AddSegment(string curvename, float time, float value)
        {
            int segind = 0;
            List<TVpoint> points = new List<TVpoint> { new TVpoint(time, value) };
            AddSegment(curvename, segind, points);
        }
        public void AddSegment(string curvename, float time1, float value1, float time2, float value2, float time3, float value3)
        {
            int segind = 1;
            TVpoint p1 = new TVpoint(time1, value1);
            TVpoint p2 = new TVpoint(time2, value2);
            TVpoint p3 = new TVpoint(time3, value3);
            List<TVpoint> points = new List<TVpoint> { p1, p2, p3 };
            AddSegment(curvename, segind, points);
        }

        public void AddSegment(string curvename, int segind, List<TVpoint> points)
        {
            UpdateInfo(points);
            if (Curves.Exists(m => m.Id == curvename))
            {
                LCurvePoint tgCurve = Curves.Find(m => m.Id == curvename);
                tgCurve.AddSegment(new Segment(segind, points));
            }
            else
            {
                Meta.CurveCount++;
                Curves.Add(new LCurvePoint(curvename, new Segment(segind, points)));
            }
        }
        private void UpdateInfo(List<TVpoint> points)
        {
            Meta.TotalSegmentCount++;
            foreach (TVpoint point in points)
            {
                Meta.Duration = Math.Max(Meta.Duration, point.Time);
                Meta.TotalPointCount++;
            }
        }
        public Motion3Json OutputMotion3Json()
        {
            List<Curve> jsoncurves = new List<Curve>();
            foreach (LCurvePoint curvePoint in Curves)
            {
                List<object> alldata = new List<object>();
                foreach (Segment seg in curvePoint.Segments)
                {
                    alldata.Add(seg.points[0].Time);
                    alldata.Add(seg.points[0].Value);
                    alldata.Add(seg.segind);
                    for (int i = 1; i < seg.points.Count; i++)
                    {
                        alldata.Add(seg.points[i].Time);
                        alldata.Add(seg.points[i].Value);
                    }
                }
                alldata.RemoveAt(alldata.Count - 1);
                jsoncurves.Add(new Curve(curvePoint.Id, alldata));
            }
            return new Motion3Json(Meta, jsoncurves);
        }
    }

    struct TVpoint
    {
        public float Time;
        public float Value;

        public TVpoint(float time, float value)
        {
            Value = value;
            Time = time;
        }
    }
    struct Segment
    {
        public int segind;
        public List<TVpoint> points;

        public Segment(int segment, List<TVpoint> points)
        {
            segind = segment;
            this.points = points;
        }
    }

    class Motion3Json
    {
        public int Version = 3;
        public Live2dMeta Meta { get; set; }
        public List<Curve> Curves { get; set; }

        public Motion3Json(Live2dMeta meta, List<Curve> curves)
        {
            Meta = meta;
            Curves = curves;
        }
    }
    class Live2dMeta
    {
        // 全所要時間
        public float Duration = 0F;
        // Fps(ほぼ30固定)
        public float Fps = 30.0F;
        // モーションを繰り返すか (true固定)
        public Boolean Loop = true;
        public Boolean AreBeziersRestricted = true;
        // パラメータ種カウント
        public int CurveCount = 0;
        // 全セグメント（点と点のつなぎ方）カウント
        public int TotalSegmentCount = 0;
        // ポイントカウント
        public int TotalPointCount = 0;
        public int UserDataCount = 0;
        public int TotalUserDataSize = 0;
    }

    class Curve
    {
        public string Target = "Parameter";
        public string Id;
        public List<Object> Segments;
        public Curve(string id, List<object> seg)
        {
            Id = id;
            Segments = seg;
        }
    }

    class LCurvePoint
    {
        public string Target = "Parameter";
        public string Id;
        public List<Segment> Segments;

        public LCurvePoint(string id)
        {
            Id = id;
            Segments = new List<Segment>();
        }
        public LCurvePoint(string id, Segment seg)
        {
            Id = id;
            Segments = new List<Segment> { seg };
        }
        public void AddSegment(Segment seg)
        {
            Segments.Add(seg);
        }
    }
}
