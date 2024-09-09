using JRPG.Abstract;
using JRPG.Models;
using JRPG.Models.Components;
using JRPG.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JRPG
{
    public static class Program
    {
        public static Engine Engine { get; private set; }
        static void Main()
        {
            var combatChannel = new CombatChannel();
            var questChannel = new QuestChannel();
            var inventoryChannel = new InventoryChannel();
            var random = new Random();
            const int ZoneWidth = 50;
            const int ZoneHeight = 30;
            const int ZoneDepth = 3;

            Console.BufferWidth = Console.WindowWidth = ZoneWidth;
            Console.BufferHeight = Console.WindowHeight = ZoneHeight;

            Console.CursorVisible = false;

            var playerModel = new Player(questChannel,inventoryChannel);
            playerModel.AddAbility(new Ability("Fireball", 10));
            playerModel.AddAbility(new Ability("Firestorm", 100));
            inventoryChannel.AddItem(new Consumable("Molotov",3, damage: 25));
            inventoryChannel.AddItem(new Consumable("Bow",10, damage: 10));

            var player = new Entity();
            player.AddComponent(new SpriteComponent { Sprite = 'u' });
            player.AddComponent(new PlayerComponent(playerModel));
            player.Position = new Vector3(5, 5, 1);


            var tallGrass = new Entity();
            tallGrass.AddComponent(new SpriteComponent { Sprite = '#' });
            tallGrass.AddComponent(new CombatComponent(() => new Combat(playerModel, new BasicMob(), combatChannel,inventoryChannel)));
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
                (e) => inventoryChannel.AddItem(new Item("Letter (quest item)")),
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
            npc2.AddComponent(new BarterComponent(() => new Barter(playerModel, trader, inventoryChannel)));
            npc2.AddComponent(new SpriteComponent { Sprite = '?' });


            //var npc3 = new Entity();
            //npc3.Position = new Vector3(8, 1, 0);
            //var questLine = new QuestLine("Welcome aboard");
            //var questReward = new Weapon("Defender", 20, 10);
            //var quest = new Quest("Bring help",
            //    "A man wandering in the woods is hurt and needs supplies.",
            //    10,requirement: e => e.GetComponent<PlayerComponent>().Player.Inventory.Contains(new Consumable("Potion of Healing")), questReward);
            //questLine.AddQuest(quest);
            //var questDialogScreen = new DialogScreen(quest.Title, (quest.Description + "\nPlease take this gold and speak with the Trader"),
            //    (e) => {
            //        var player = e.GetComponent<PlayerComponent>().Player;
            //        player.StartQuestLine(questLine);
            //        player.ReceiveGold(50);
            //        },isFinalScreen: true);
            //var questDialog = new Dialog(questDialogScreen);
            //npc3.AddComponent(new DialogComponent(questDialog));
            //npc3.AddComponent(new SpriteComponent { Sprite = '|' });

            //var npc3 = new Entity();

            //npc3.Position = new Vector3(8, 1, 0);

            //var questLine = new QuestLine("War is upon us.");

            //var questReward = new Weapon("Defender", 20, 10);

            //var quest = new DeliveryQuest("Inform the villagers!",
            //    "An army is marching towards us...",
            //    10, questReward,questChannel,inventoryChannel);
            //questLine.AddQuest(quest);

            //var acceptQuestDialogScreen = new DialogScreen($"Good luck {player.GetComponent<SpriteComponent>().Sprite}!",
            //    "Thank you for your assistance.",
            //    (e) =>
            //    {
            //        var player = e.GetComponent<PlayerComponent>().Player;
            //        player.StartQuestLine(questLine);
            //    }, isFinalScreen: true);

            //var denyQuestDialogScreen = new DialogScreen($"Good luck on your journey {player.GetComponent<SpriteComponent>().Sprite}!",
            //    "Come back when you're ready.",
            //    isFinalScreen: true);

            //var optionalDialogScreen = new Dictionary<string, IDialogScreen>
            //{
            //    { "Accept!", acceptQuestDialogScreen },
            //    { "Deny!", denyQuestDialogScreen }
            //};

            //var questDialogScreen = new DialogScreen(quest.Title, (quest.Description + "\nPlease take this information to everyone"),
            //    optionalScreens: optionalDialogScreen);

            //var questDialog = new Dialog(questDialogScreen);

            //npc3.AddComponent(new DialogComponent(questDialog));

            //npc3.AddComponent(new SpriteComponent { Sprite = '|' });


            //var npc4 = new Entity();
            //npc4.Position = new Vector3(10, 1, 0);
            //var questDialogScreen2 = new DialogScreen(quest.Title, ("Thank you so for bringing this to us!"), (e) =>
            //{
            //    quest.CompleteQuest(e);
            //    playerModel.CompleteQuest(questLine);
            //}, isFinalScreen: true);
            //var questDialog2 = new Dialog(questDialogScreen2);
            //npc4.AddComponent(new DialogComponent(questDialog2));
            //npc4.AddComponent(new SpriteComponent { Sprite = 'D' });

            //var npc5 = new Entity();
            //npc5.Position = new Vector3(4, 2, 0);
            

            //var acceptQuest2DialogScreen = new DialogScreen($"Good luck {player.GetComponent<SpriteComponent>().Sprite}!",
            //    "Thank you for your assistance.",
            //    (e) =>
            //    {
            //        playerModel.StartQuestLine(questLine2);
            //        questLine2.Start();
            //    }, isFinalScreen: true);

            //var optionalQuest2DialogScreens = new Dictionary<string, IDialogScreen>
            //{
            //    { "Accept!", acceptQuest2DialogScreen },
            //    { "Deny!", denyQuestDialogScreen }
            //};

            //var quest2DialogScreen = new DialogScreen(quest2.Title, (quest2.Description + "\nPlease help them!"),
            //    optionalScreens: optionalQuest2DialogScreens);
            //var quest2Dialog = new Dialog(quest2DialogScreen);
            //npc5.AddComponent(new DialogComponent(quest2Dialog));
            //npc5.AddComponent(new SpriteComponent { Sprite = 'Q' });

            var npc6 = new Entity();
            var npc7 = new Entity();

            npc6.Position = new Vector3(4, 6, 0);
            npc7.Position = new Vector3(10, 4, 0);

            var NPC1QuestLine1 = new QuestLine("This is only the beginning");
            var NPC1Quest1Reward = new Consumable("Potion of Healing", 5, health: 25);
            var NPC1Quest1 = new KillQuest("Help with the battle!", "Kill 3 enemies", new BasicMob(), 1, 50, NPC1Quest1Reward, combatChannel, questChannel, inventoryChannel, npc6, npc6);
            var NPC1Quest2 = new DeliveryQuest("Help the soldiers!", "Take these healing potions and bring them to the forest", 10, NPC1Quest1Reward, questChannel, inventoryChannel, npc6, npc7);
            NPC1QuestLine1.AddQuest(NPC1Quest1);
            NPC1QuestLine1.AddQuest(NPC1Quest2);

            var NPC1QuestLine2 = new QuestLine("The farmer's way.");
            var NPC1Quest3Reward = new Gear("Leggings",5,3);
            var NPC1Quest3 = new KillQuest("test quest", "An animal is munching on the farmer's produce",new BasicMob(),1,50,NPC1Quest1Reward,combatChannel,questChannel,inventoryChannel,npc6,npc6);
            NPC1QuestLine2.AddQuest(NPC1Quest3);

            var NPC1QuestLine3 = new QuestLine("Around the world.");
            var NPC1Quest4 = new DeliveryQuest("Bring this message to my family!", "They live out in the woods, please inform them I have to leave in 2 weeks.", 50, null, questChannel, inventoryChannel,npc6, npc7);
            NPC1QuestLine3.AddQuest(NPC1Quest4);

            var NPC1QuestLineList = new List<IQuestLine> { NPC1QuestLine1,NPC1QuestLine2,NPC1QuestLine3 };
            var NPC1QuestLineDialog = new QuestDialog(NPC1QuestLineList);

            var NPC2QuestLineList = new List<IQuestLine> { NPC1QuestLine1, NPC1QuestLine3 };
            var NPC2QuestLineDialog = new QuestDialog(NPC2QuestLineList);


            npc6.AddComponent(new QuestComponent(NPC1QuestLineDialog,questChannel));
            npc6.AddComponent(new SpriteComponent { Sprite = 'A' });

            npc7.AddComponent(new QuestComponent(NPC2QuestLineDialog, questChannel));
            npc7.AddComponent(new SpriteComponent { Sprite = 'B' });

            var zone1 = new Zone("Zone 1", new Vector3(ZoneWidth, ZoneHeight, ZoneDepth));
            zone1.AddEntity(player);
            zone1.AddEntity(tallGrass);
            zone1.AddEntity(ceiling);
            zone1.AddEntity(wall);
            zone1.AddEntity(npc1);
            zone1.AddEntity(npc2);
            //zone1.AddEntity(npc3);
            //zone1.AddEntity(npc5);
            zone1.AddEntity(npc6);
            zone1.AddEntity(npc7);
            //zone1.AddEntity(npc4);
            
            Engine = new Engine();
            Engine.PushState(new ZoneState(player,zone1,inventoryChannel));
            
            while (Engine.IsRunning)
            {
                Engine.ProcessInput(Console.ReadKey(true));
            }
        }
    }
}
