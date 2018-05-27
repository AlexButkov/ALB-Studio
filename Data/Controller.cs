﻿using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ALB
{
    static class Controller
    {
        static Stopwatch keyTimer = new Stopwatch();
        static ConsoleKey? keyFirst;
        static ConsoleKey? keySecond;
        static bool isFirst;
        static bool getKeyIsOn;
        static bool resetKeyIsOn;
        static int deltaTimer = 100;
        static int? stopTimer = 100;
        static int keyX;
        static int keyY;
        //======
        /*/// <summary> </summary>
        public static void MoveByKey(this ObjectGroup objectToMove, float speed, float? stopDistance = null)
        {
            objectToMove.MoveByKey(speed, stopDistance);
        }*/
        /// <summary> </summary>
        public static void MoveByKey(this ObjectSingle objectToMove, float speed, float? stopDistance = null )
        {
            if (!getKeyIsOn)
            {
                Model.StartThread(GetKey);
                getKeyIsOn = true;
            }
            if (stopDistance != null)
            {
                stopTimer = (int)(stopDistance / speed * 1000);
                if (!resetKeyIsOn)
                {
                    Model.StartThread(ResetKey);
                    resetKeyIsOn = true;
                }
            }
            else
            {
                stopTimer = null;
            }
            Move(objectToMove, speed, keyX, keyY);
        }

        /// <summary> </summary>
        public static void MoveAside(this ObjectSingle objectToMove, float speed, SideX typeX = SideX.Middle, SideY typeY = SideY.Middle)
        {
            int kX = 0;
            int kY = 0;
            switch (typeX)
            {
                case SideX.Left:
                    kX = -1; break;
                case SideX.Middle:
                    kX = 0; break;
                case SideX.Right:
                    kX = 1; break;
            }
            switch (typeY)
            {
                case SideY.Up:
                    kY = -1; break;
                case SideY.Middle:
                    kY = 0; break;
                case SideY.Down:
                    kY = 1; break;
            }
            Move(objectToMove, speed, kX, kY);
        }

        /// <summary> </summary>
        public static void MoveTowards(this ObjectSingle objectToMove, float speed, ObjectSingle targetObject)
        {
            int kX = (int)targetObject.Position.X - (int)objectToMove.Position.X;
            int kY = (int)targetObject.Position.Y - (int)objectToMove.Position.Y;
            Move(objectToMove, speed, kX, kY);       
        }

        /// <summary> </summary>
        public static void MoveTowards(this ObjectSingle objectToMove, float speed, Vector targetPosition)
        {
            int kX = (int)targetPosition.X - (int)objectToMove.Position.X;
            int kY = (int)targetPosition.Y - (int)objectToMove.Position.Y;
            Move(objectToMove, speed, kX, kY);
        }
        //---private---
        /// <summary> </summary>
        static void Move(ObjectSingle objectToMove, float speed, int targetX, int targetY )
        {
            if (targetX != 0 || targetY != 0)
            {
                double atang = Math.Atan2(targetY, targetX);
                objectToMove.Position.X += ((float)Math.Cos(atang)).GridToX() * speed * Model.DeltaTime;
                objectToMove.Position.Y += ((float)Math.Sin(atang)).GridToY() * speed * Model.DeltaTime;
            }
        }

        //------
        static void ResetKey()
        {
            while (true)
            {
                if (keyTimer.ElapsedMilliseconds > stopTimer)
                {
                    keyX = 0;
                    keyY = 0;
                }
                Thread.Sleep(Model.DeltaTimeMs);
            }
        }

        static void GetKey()
        {
            while (true)
            {

                switch (isFirst ? keyFirst = Console.ReadKey(true).Key : keySecond)
                {
                    case ConsoleKey.W:
                        goto case ConsoleKey.UpArrow;
                    case ConsoleKey.S:
                        goto case ConsoleKey.DownArrow;
                    case ConsoleKey.D:
                        goto case ConsoleKey.RightArrow;
                    case ConsoleKey.A:
                        goto case ConsoleKey.LeftArrow;
                    case ConsoleKey.UpArrow:
                        keyX = 0; keyY = -1; goto case null;
                    case ConsoleKey.DownArrow:
                        keyX = 0; keyY = 1; goto case null;
                    case ConsoleKey.RightArrow:
                        keyX = 1; keyY = 0; goto case null;
                    case ConsoleKey.LeftArrow:
                        keyX = -1; keyY = 0; goto case null;
                    case null:
                        keyTimer.Reset(); keyTimer.Start(); isFirst = true; break;
                }
                switch (keySecond = Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        goto case ConsoleKey.UpArrow;
                    case ConsoleKey.S:
                        goto case ConsoleKey.DownArrow;
                    case ConsoleKey.D:
                        goto case ConsoleKey.RightArrow;
                    case ConsoleKey.A:
                        goto case ConsoleKey.LeftArrow;
                    case ConsoleKey.UpArrow:
                        if (keyTimer.ElapsedMilliseconds < deltaTimer)
                        { keyY = -1; break; }
                        else
                        { goto case null; }
                    case ConsoleKey.DownArrow:
                        if (keyTimer.ElapsedMilliseconds < deltaTimer)
                        { keyY = 1; break; }
                        else
                        { goto case null; }
                    case ConsoleKey.RightArrow:
                        if (keyTimer.ElapsedMilliseconds < deltaTimer)
                        { keyX = 1; break; }
                        else
                        { goto case null; }
                    case ConsoleKey.LeftArrow:
                        if (keyTimer.ElapsedMilliseconds < deltaTimer)
                        { keyX = -1; break; }
                        else
                        { goto case null; }
                    case null:
                        isFirst = false; break;
                }
                //Thread.Sleep(DeltaTimeMs);
            }
        }
    }
}
