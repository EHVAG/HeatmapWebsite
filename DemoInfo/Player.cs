﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoInfo
{
    public class Player
    {
        public string Name { get; set; }
        public int SteamID { get; set; }
        public Vector Position { get; set; }
        public int EntityID { get; set; }
		public int HP { get; set; }

		public Vector LastAlivePosition { get; set; }

		public float ViewDirectionX { get; set; }
		public float ViewDirectionY { get; set; }

		public bool IsAlive 
		{
			get { return HP > 0; }
		}

        public Team Team { get; set; }

    }
    public enum Team {
		Spectate = 1,
		Terrorist = 2,
		CounterTerrorist = 3,
    }
}
