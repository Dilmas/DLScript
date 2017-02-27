﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using SharpDX;
using Color = System.Drawing.Color;
using EloBuddy.SDK;
using EloBuddy;

namespace Evader
{
    public static class Geometry
    {
        private const int CircleLineSegment = 22;

        public static List<Polygon> ToPolygons(this List<List<IntPoint>> polygonList)
        {
            return polygonList.Select(path => path.ToPolygon()).ToList();
        }

        public static Vector3 SwitchYZ(this Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        public static Vector2 PositionAfter(this List<Vector2> self, int t, int speed, int delay = 0)
        {
            var distance = Math.Max(0, t - delay) * speed / 1000;
            for (var i = 0; i <= self.Count - 2; i++)
            {
                var segmentStart = self[i];
                var segmentEnd = self[i + 1];
                var distance1 = (int) segmentEnd.Distance(segmentStart);
                if (distance1 > distance)
                {
                    return segmentStart + distance * (segmentEnd - segmentStart).Normalized();
                }
                distance -= distance1;
            }
            return self[self.Count - 1];
        }

        public static Polygon ToPolygon(this List<IntPoint> v)
        {
            var polygon = new Polygon();
            foreach (var point in v)
            {
                polygon.Add(new Vector2(point.X, point.Y));
            }
            return polygon;
        }


        public static List<List<IntPoint>> ClipPolygons(List<Polygon> polygons)
        {
            var subj = new List<List<IntPoint>>(polygons.Count);
            var clip = new List<List<IntPoint>>(polygons.Count);

            foreach (var polygon in polygons)
            {
                subj.Add(polygon.ToClipperPath());
                clip.Add(polygon.ToClipperPath());
            }

            var solution = new List<List<IntPoint>>();
            var c = new Clipper();
            c.AddPaths(subj, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftEvenOdd);

            return solution;
        }


        public class Circle
        {
            public Vector2 Center;
            public float Radius;

            public Circle(Vector2 center, float radius)
            {
                Center = center;
                Radius = radius;
            }

            public Polygon ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon();
                var outRadius = (overrideWidth > 0
                    ? overrideWidth
                    : (offset + Radius) / (float) Math.Cos(2 * Math.PI / CircleLineSegment));

                for (var i = 1; i <= CircleLineSegment; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegment;
                    var point = new Vector2(
                        Center.X + outRadius * (float) Math.Cos(angle), Center.Y + outRadius * (float) Math.Sin(angle));
                    result.Add(point);
                }

                return result;
            }
        }

        public class Polygon
        {
            public List<Vector2> Points = new List<Vector2>();

            public void Add(Vector2 point)
            {
                Points.Add(point);
            }

            public List<IntPoint> ToClipperPath()
            {
                var result = new List<IntPoint>(Points.Count);
                result.AddRange(Points.Select(point => new IntPoint(point.X, point.Y)));
                return result;
            }

            public bool IsOutside(Vector2 pointVector2)
            {
                var point = new IntPoint(pointVector2.X, pointVector2.Y);
                return Clipper.PointInPolygon(point, ToClipperPath()) != 1;
            }

            public void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= Points.Count - 1; i++)
                {
                    var nextIndex = (Points.Count - 1 == i) ? 0 : (i + 1);
                    DrawLineInWorld(Points[i].To3D(), Points[nextIndex].To3D(), width, color);
                }
            }

            private static void DrawLineInWorld(Vector3 start, Vector3 end, int width, Color color)
            {
                var segmentStart = Drawing.WorldToScreen(start);
                var segmentEnd = Drawing.WorldToScreen(end);
                Drawing.DrawLine(segmentStart[0], segmentStart[1], segmentEnd[0], segmentEnd[1], width, color);
            }
        }

        public class Rectangle
        {
            public Vector2 Direction;
            public Vector2 Perpendicular;
            public Vector2 REnd;
            public Vector2 RStart;
            public float Width;

            public Rectangle(Vector2 start, Vector2 end, float width)
            {
                RStart = start;
                REnd = end;
                Width = width;
                Direction = (end - start).Normalized();
                Perpendicular = Direction.Perpendicular();
            }

            public Polygon ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon();

                result.Add(
                    RStart + (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular - offset * Direction);
                result.Add(
                    RStart - (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular - offset * Direction);
                result.Add(
                    REnd - (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular + offset * Direction);
                result.Add(
                    REnd + (overrideWidth > 0 ? overrideWidth : Width + offset) * Perpendicular + offset * Direction);

                return result;
            }
        }


        public class Ring
        {
            public Vector2 Center;
            public float Radius;
            public float RingRadius;

            public Ring(Vector2 center, float radius, float ringRadius)
            {
                Center = center;
                Radius = radius;
                RingRadius = ringRadius;
            }

            public Polygon ToPolygon(int offset = 0)
            {
                var result = new Polygon();

                var outRadius = (offset + Radius + RingRadius) / (float) Math.Cos(2 * Math.PI / CircleLineSegment);
                var innerRadius = Radius - RingRadius - offset;

                for (var i = 0; i <= CircleLineSegment; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegment;
                    var point = new Vector2(
                        Center.X - outRadius * (float) Math.Cos(angle), Center.Y - outRadius * (float) Math.Sin(angle));
                    result.Add(point);
                }

                for (var i = 0; i <= CircleLineSegment; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegment;
                    var point = new Vector2(
                        Center.X + innerRadius * (float) Math.Cos(angle),
                        Center.Y - innerRadius * (float) Math.Sin(angle));
                    result.Add(point);
                }


                return result;
            }
        }

        public class Sector
        {
            public float Angle;
            public Vector2 Center;
            public Vector2 Direction;
            public float Radius;

            public Sector(Vector2 center, Vector2 direction, float angle, float radius)
            {
                Center = center;
                Direction = direction;
                Angle = angle;
                Radius = radius;
            }

            public Polygon ToPolygon(int offset = 0)
            {
                var result = new Polygon();
                var outRadius = (Radius + offset) / (float) Math.Cos(2 * Math.PI / CircleLineSegment);

                result.Add(Center);
                var side1 = Direction.Rotated(-Angle * 0.5f);

                for (var i = 0; i <= CircleLineSegment; i++)
                {
                    var cDirection = side1.Rotated(i * Angle / CircleLineSegment).Normalized();
                    result.Add(new Vector2(Center.X + outRadius * cDirection.X, Center.Y + outRadius * cDirection.Y));
                }

                return result;
            }
        }

        public struct ProjectionInfo
        {
            public bool IsOnSegment;

            public Vector2 LinePoint;

            public Vector2 SegmentPoint;
            public ProjectionInfo(bool isOnSegment, Vector2 segmentPoint, Vector2 linePoint)
            {
                IsOnSegment = isOnSegment;
                SegmentPoint = segmentPoint;
                LinePoint = linePoint;
            }
        }
    }
}