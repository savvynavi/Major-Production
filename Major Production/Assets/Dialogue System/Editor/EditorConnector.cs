using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Dialogue
{
    public class EditorConnector
    {
        public EditorNode Parent;
        public EditorNode Target;

        Vector2 start;
        Vector2 end;

        public void FollowMouse(Event e)
        {
            //HACK might need to test this is a mouse event
            end = e.mousePosition;
        }

        public void Draw()
        {
            // use handles to draw line from start to end, with triangle in middle
            //TODO if target has connector to parent, offset start and end (relative to direction, so other line offset opposite way)
            // Selected connector is different colour

            start = Parent.rect.center;
            if(Target != null)
            {
                end = Target.rect.center;
            }

            //Handles.DrawLine(start, end);
            //HACK figure out how to draw a triangle texture or something
            Handles.DrawAAPolyLine(3, start, end);
            Vector2 midpoint = 0.5f * (start + end);
            Vector2 direction = (end - start).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            Vector2 arrowStart = midpoint + (perpendicular - direction) * 5.0f;
            Vector2 arrowEnd = midpoint + (-perpendicular - direction) * 5.0f;
            Handles.DrawAAPolyLine(3, arrowStart, midpoint, arrowEnd);

            // TODO transition to self? if allowed, draw like a loop?
        }

        public void ProcessEvents(Event e, DialogueEditorWindow window)
        {

        }

        private float DistanceToLine(Vector2 point)
        {
            Vector2 direction = (end - start).normalized;
            Vector2 mouseOffset = (point - start);
            float endPointDistance = Vector2.Dot(end - start, direction);
            float nearestPointDistance = Mathf.Clamp(Vector2.Dot(mouseOffset, direction), 0, endPointDistance);
            Vector2 nearestPoint = nearestPointDistance * direction;
            return (mouseOffset - nearestPoint).magnitude;
        }
    }
}
