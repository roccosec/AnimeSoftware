﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hazedumper;

namespace AnimeSoftware
{
    class Entity : IDisposable
    {
        public void Dispose()
        {

        }

        public int Index;
        public int Ptr
        {
            get
            {
                return Memory.Read<int>(Memory.Client + signatures.dwEntityList + (Index - 1) * 0x10);
            }
        }

        public float DistanceToPlayer
        {
            get
            {
                return VectorMath.Distance(Position, LocalPlayer.Position);
            }
        }
            
        public Vector3 Velocity
        {
            get
            {
                Vector3 velocity = Memory.Read<Vector3>(Ptr + netvars.m_vecVelocity);
                return velocity;
            }
        }

        public float Speed
        {
            get
            {
                Vector3 velocity = Velocity;
                float result = (float)Math.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y + velocity.z * velocity.z);
                return result;
            }
        }
        public Vector3 Position
        {
            get
            {
                Vector3 position = Memory.Read<Vector3>(Ptr + netvars.m_vecOrigin);
                return position;
            }
        }
        public int Health
        {
            get
            {
                return Memory.Read<int>(Ptr + netvars.m_iHealth);
            }
        }

        public bool Dormant
        {
            get
            {
                return Memory.Read<bool>(Ptr + signatures.m_bDormant);
            }
        }

        public static Entity[] List()
        {

            List<Entity> entityList = new List<Entity>();
            for (int i = 0; i < 64; i++)
            {
                Entity entity = new Entity(i);

                if (entity.Ptr == 0)
                    continue;

                if (entity.Ptr == LocalPlayer.Ptr)
                    continue;

                entityList.Add(entity);
            }
            return entityList.ToArray();
        }

        public Vector3 BonePosition(int BoneID)
        {
            int BoneMatrix = Memory.Read<Int32>(Ptr + netvars.m_dwBoneMatrix);
            Vector3 position = new Vector3
            {
                x = Memory.Read<float>(BoneMatrix + 0x30 * BoneID + 0x0C),
                y = Memory.Read<float>(BoneMatrix + 0x30 * BoneID + 0x1C),
                z = Memory.Read<float>(BoneMatrix + 0x30 * BoneID + 0x2C)
            };
            return position;
        }

        public Entity(int index)
        {
            Index = index;
        }
    }
}