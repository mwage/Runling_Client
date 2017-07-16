﻿using Characters.Types;
using Players;
using UnityEngine;

namespace Launcher
{
    public class PlayerState
    {
        // Player
        public GameObject Player;
        public CharacterDto CharacterDto;
        public PlayerTrigger PlayerTrigger = GameControl.PlayerState.Player.transform.Find("Trigger").gameObject.GetComponent<PlayerTrigger>();
        public float MoveSpeed = 0;
        public bool IsDead = true;
        public bool IsInvulnerable = false;
        public bool IsSafe = false;
        public bool IsImmobile = false;
        public int Lives = 0;
    }
}
