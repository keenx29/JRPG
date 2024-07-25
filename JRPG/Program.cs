using JRPG.Abstract;
using JRPG.Models;
using JRPG.Models.Components;
using JRPG.States;
using System;
using System.Collections.Generic;

namespace JRPG
{
    public static class Program
    {
        public static Engine Engine { get; private set; }
        static void Main()
        {
            var random = new Random();
            const int ZoneWidth = 50;
            const int ZoneHeight = 30;
            const int ZoneDepth = 3;

            Console.BufferWidth = Console.WindowWidth = ZoneWidth;
            Console.BufferHeight = Console.WindowHeight = ZoneHeight;

            Console.CursorVisible = false;

            var playerModel = new Player();
            playerModel.AddAbility(new Ability("Fireball", 10));
            playerModel.AddAbility(new Ability("Firestorm", 100));
            playerModel.AddItem(new Consumable("Molotov",3, damage: 25));
            playerModel.AddItem(new Consumable("Bow",10, damage: 10));

            var player = new Entity();
            player.AddComponent(new SpriteComponent { Sprite = 'u' });
            player.AddComponent(new PlayerComponent(playerModel));
            player.Position = new Vector3(5, 5, 1);


            var tallGrass = new Entity();
            tallGrass.AddComponent(new SpriteComponent { Sprite = '#' });
            tallGrass.AddComponent(new CombatComponent(() => new Combat(playerModel, new BasicMob())));
            tallGrass.Position = new Vector3(6, 1, 0);
            //for (int i = 0; i < ZoneWidth; i++)
            //{
            //    var walls = new Entity();
            //    walls.AddComponent(new ConstantEntranceComponent(false));
            //    walls.AddComponent(new SpriteComponent { Sprite = '*' });
            //    walls.Position = new Vector3(i, 0, 0);
            //    zone1.AddEntity(walls);
            //}
            //for (int i = 0; i < ZoneHeight - 1; i++)
            //{
            //    var walls = new Entity();
            //    walls.AddComponent(new ConstantEntranceComponent(false));
            //    walls.AddComponent(new SpriteComponent { Sprite = '*' });
            //    walls.Position = new Vector3(0, i, 0);
            //    zone1.AddEntity(walls);
            //}

            var ceiling = new Entity();
            ceiling.AddComponent(new SpriteComponent { Sprite = '^' });
            ceiling.Position = new Vector3(1, 0, 0);

            var wall = new Entity();
            wall.AddComponent(new ConstantEntranceComponent(false));
            wall.AddComponent(new SpriteComponent { Sprite = '*' });
            wall.Position = new Vector3(0, 1, 0);

            var npc1 = new Entity();
            npc1.AddComponent(new DialogComponent(new Dialog
                (new DialogScreen(
                    "Have this item!",
                    e => e.GetComponent<PlayerComponent>().Player.AddItem(
                        new Item("Quest Item"))))));
            npc1.AddComponent(new SpriteComponent { Sprite = '!' });
            npc1.Position = new Vector3(2, 1, 0);

            var npc2 = new Entity();
            npc2.Position = new Vector3 (4, 1, 0);
            var trader = new Trader();
            trader.AddItem(new Gear("Armor",8));
            trader.AddItem(new Gear("Armor",5,weight:5));
            trader.AddItem(new Gear("Armor",10,weight:5));
            trader.AddItem(new Consumable("Potion of Healing",3,health: 25));
            trader.AddItem(new Consumable("Potion of Protection", 1, armor: 10));
            trader.AddItem(new Consumable("Potion of Rage", 5,damageBuff: 50));
            npc2.AddComponent(new BarterComponent(() => new Barter(playerModel, trader)));
            npc2.AddComponent(new SpriteComponent { Sprite = '?' });

            var zone1 = new Zone("Zone 1", new Vector3(ZoneWidth, ZoneHeight, ZoneDepth));
            zone1.AddEntity(player);
            zone1.AddEntity(tallGrass);
            zone1.AddEntity(ceiling);
            zone1.AddEntity(wall);
            zone1.AddEntity(npc1);
            zone1.AddEntity(npc2);
            
            Engine = new Engine();
            Engine.PushState(new ZoneState(player,zone1));
            
            while (Engine.IsRunning)
            {
                Engine.ProcessInput(Console.ReadKey(true));
            }
        }
    }
}
