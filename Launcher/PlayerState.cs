﻿using Characters.Types;
using UnityEngine;

namespace Launcher
{
    public class PlayerState
    {
        // Player
        public GameObject Player;
        public CharacterDto CharacterDto;
        public float MoveSpeed = 0;
        public bool IsDead = true;
        public bool IsInvulnerable = false;
        public bool IsSafe = false;
        public bool IsImmobile = false;
        public int Lives = 0;
    }
}
