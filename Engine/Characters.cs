using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Character : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public Character(int id, string name, string role, string description, int currentHitPoints, int maximumHitPoints) : base(currentHitPoints, maximumHitPoints)
        {
            ID = id;
            Name = name;
            Role = role;
            Description = description;
        }
    }
}
