using JRPG.Abstract;
using JRPG.Models;
using JRPG.Models.Components;
using JRPG.States;
using System;
using System.Collections.Generic;
using System.Linq;

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
            //npc1.AddComponent(new DialogComponent(new Dialog
            //    (new DialogScreen(
            //        "Have this item!",
            //        e => e.GetComponent<PlayerComponent>().Player.AddItem(
            //            new Item("Quest Item"))))));
            var dialogScreens = new List<IDialogScreen>();

            var optionalScreens = new Dictionary<string, IDialogScreen>
            {
                {
                    "Great!",
                    new DialogScreen("I need your assistance",
                "Bring this letter to Ivan for a Reward",
                e => e.GetComponent<PlayerComponent>().Player.AddItem(new Item("Letter (quest item)")),
                isFinalScreen: true)
                },
                {
                    "Okay!",
                    new DialogScreen("Are you sure?",
                "Tell me what is going on...",
                isFinalScreen: true)
                }
            };

            dialogScreens.Add(new DialogScreen(
                $"Greetings {player.GetComponent<SpriteComponent>().Sprite}",
                "Nice to meet you! How are you doing?",optionalScreens: optionalScreens));

            var dialog = new Dialog(screens: dialogScreens);

            npc1.AddComponent(new DialogComponent(dialog));
            npc1.AddComponent(new SpriteComponent { Sprite = '!' });
            npc1.Position = new Vector3(2, 1, 0);

            var npc2 = new Entity();
            npc2.Position = new Vector3 (4, 1, 0);
            var trader = new Trader();
            trader.AddItem(new Gear("Armor",8));
            trader.AddItem(new Gear("Armor",5,weight:5));
            trader.AddItem(new Gear("Armor",10,weight:5));
            trader.AddItem(new Weapon("Sword",attack: 10,weight: 5));
            trader.AddItem(new Weapon("Sword",attack: 15,weight: 5));
            trader.AddItem(new Consumable("Potion of Healing",3,health: 25));
            trader.AddItem(new Consumable("Potion of Protection", 1, armor: 10));
            trader.AddItem(new Consumable("Potion of Rage", 5,damageBuff: 50));
            npc2.AddComponent(new BarterComponent(() => new Barter(playerModel, trader)));
            npc2.AddComponent(new SpriteComponent { Sprite = '?' });


            var npc3 = new Entity();
            npc3.Position = new Vector3(8, 1, 0);
            var questLine = new QuestLine("Welcome aboard");
            var questReward = new Weapon("Defender", 20, 10);
            var quest = new Quest("Bring help",
                "A man wandering in the woods is hurt and needs supplies.",
                10,requirement: e => e.GetComponent<PlayerComponent>().Player.Inventory.Contains(new Consumable("Potion of Healing")), questReward);
            questLine.AddQuest(quest);
            var questDialogScreen = new DialogScreen(quest.Title, (quest.Description + "\nPlease take this gold and speak with the Trader"),
                (e) => {
                    var player = e.GetComponent<PlayerComponent>().Player;
                    player.StartQuestLine(questLine);
                    player.ReceiveGold(50);
                    }, true);
            var questDialog = new Dialog(questDialogScreen);
            npc3.AddComponent(new DialogComponent(questDialog));
            npc3.AddComponent(new SpriteComponent { Sprite = '|' });

            var zone1 = new Zone("Zone 1", new Vector3(ZoneWidth, ZoneHeight, ZoneDepth));
            zone1.AddEntity(player);
            zone1.AddEntity(tallGrass);
            zone1.AddEntity(ceiling);
            zone1.AddEntity(wall);
            zone1.AddEntity(npc1);
            zone1.AddEntity(npc2);
            zone1.AddEntity(npc3);
            
            Engine = new Engine();
            Engine.PushState(new ZoneState(player,zone1));
            
            while (Engine.IsRunning)
            {
                Engine.ProcessInput(Console.ReadKey(true));
            }
        }
    }
}
