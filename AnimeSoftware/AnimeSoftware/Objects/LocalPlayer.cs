﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using hazedumper;

namespace AnimeSoftware
{
    class LocalPlayer : IDisposable
    {
        public void Dispose()
        {

        }
        public static int Ptr
        {
            get
            {
                return Memory.Read<int>(Memory.Client + signatures.dwLocalPlayer);
            }
        }

        public static void GetName()
        {
            int radarBasePtr = 0x78;
            int radarStructSize = 0x174;
            int radarStructPos = 0x18;

            Encoding enc = Encoding.UTF8;

            int radarBase = Memory.Read<int>(Memory.Client + signatures.dwRadarBase);

            int radarPtr = Memory.Read<int>(radarBase + radarBasePtr);

            int ind = Index + 1;

            var nameAddr = radarPtr + ind * radarStructSize + radarStructPos;
            Name = Memory.ReadString(nameAddr, 64, enc);
        }

        public static string Name { get; set; }


        public static void GetIndex()
        {
            Index = -1;
            while (Index == -1)
                foreach (Entity x in Entity.List())
                {
                    if (x.Health <= 0)
                        continue;
                    if (x.Ptr == Ptr)
                    {
                        Index = x.Index;
                        break;
                    }
                }
        }
        public static int Index { get; set; }
        

        public static Vector3 Position
        {
            get
            {
                Vector3 position = Memory.Read<Vector3>(Ptr + netvars.m_vecOrigin);
                return position;
            }
        }
        public static Vector3 Velocity
        {
            get
            {
                Vector3 velocity = Memory.Read<Vector3>(Ptr + netvars.m_vecVelocity);
                return velocity;
            }
        }
        public static void MoveRight()
        {
            ClientCMD.Exec("-moveleft");
            Thread.Sleep(1);
            ClientCMD.Exec("+moveright");
        }
        public static void MoveLeft()
        {
            ClientCMD.Exec("-moveright");
            Thread.Sleep(1);
            ClientCMD.Exec("+moveleft");
        }
        public static void MoveClearY()
        {
            ClientCMD.Exec("-moveright");
            Thread.Sleep(1);
            ClientCMD.Exec("-moveleft");
        }
        public static void MoveForward()
        {
            ClientCMD.Exec("+forward");
        }
        public static void MoveClearX()
        {
            ClientCMD.Exec("-forward");
        }

        public static void Jump()
        {
            Memory.Write<int>(Memory.Client + signatures.dwForceJump, 5);
            Thread.Sleep(20);
            Memory.Write<int>(Memory.Client + signatures.dwForceJump, 4);
        }
        public static int Flags
        {
            get
            {
                return Memory.Read<int>(Ptr + netvars.m_fFlags);
            }
        }
        public static float Speed
        {
            get
            {
                Vector3 velocity = Velocity;
                float result = (float)Math.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y + velocity.z * velocity.z);
                return result;
            }
        }

        public static bool InGame
        {
            get
            {
                int ClientState = Memory.Read<int>(Memory.Engine + signatures.dwClientState);
                return Memory.Read<int>(ClientState + signatures.dwClientState_State) == 6;
            }
        }
        public static int Health
        {
            get
            {
                return Memory.Read<int>(Ptr + netvars.m_iHealth);
            }
        }
        public static Vector3 ViewPosition
        {
            get
            {
                Vector3 position = Position;
                position.z += Memory.Read<float>(Ptr + netvars.m_vecViewOffset + 0x8);
                return position;
            }
        }

        public static Vector3 ViewAngle
        {
            get
            {
                int ClientState = Memory.Read<Int32>(Memory.Engine + signatures.dwClientState);

                Vector3 viewAngles = Memory.Read<Vector3>(ClientState + signatures.dwClientState_ViewAngles);
                return viewAngles;
            }
            set
            {
                int ClientState = Memory.Read<Int32>(Memory.Engine + signatures.dwClientState);

                Memory.Write<Vector3>(ClientState + signatures.dwClientState_ViewAngles, value);
            }
        }
        public static float ViewAngleY
        {
            set
            {
                int ClientState = Memory.Read<Int32>(Memory.Engine + signatures.dwClientState);

                Memory.Write<float>(ClientState + signatures.dwClientState_ViewAngles + 0x4, value);
            }
        }
    }
}
