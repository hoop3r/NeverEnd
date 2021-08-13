using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class World
    {
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();
        public static readonly List<Character> Сharacters = new List<Character>();

        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;
        public const int ITEM_ID_TOAD_FOOT = 11;
        public const int ITEM_ID_TOAD_TONGUE = 12;
        public const int ITEM_ID_GOLD_RING = 13;
        public const int ITEM_ID_BOW = 14;

        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;
        public const int MONSTER_ID_GIANT_TOAD = 4;

        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;
        public const int QUEST_ID_TOWN_SQUARE_GOLDEN_TICKET = 3;
        public const int QUEST_ID_FIND_WIFE = 4;

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;
        public const int LOCATION_ID_YARD = 10;
        public const int LOCATION_ID_SHOP = 11;

        public const int CHARACTER_ID_WIFE = 1;
        public const int CHARACTER_ID_BOOZER = 2;
        public const int CHARACTER_ID_DOG = 3;

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateCharacters();
            PopulateLocations();
        }

        private static void PopulateItems()
        {
            // Создаем вещи и указываем их хар-ки
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Ржавый меч", "Ржавые мечи", 0, 5));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Крысиный хвост", "Крысиные хвосты"));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Кусок меха", "Кусочки меха"));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Змеиный клык", "Змеиные клыки"));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Змеиная кожа", "Змеиные кожи"));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Дубина", "Дибины", 3, 10));
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Лечебное зелье", "Лечебные зелья", 5));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Паучий клык", "Паучие клыки"));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Паучий шелк", "Паучье шелка"));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Пропуск", "Пропуска"));
            Items.Add(new Item(ITEM_ID_TOAD_FOOT, "Жабья лапка", "Жабьи лапки"));
            Items.Add(new Item(ITEM_ID_TOAD_TONGUE, "Жабий язык", "Жабьи языки"));
            Items.Add(new Item(ITEM_ID_TOAD_TONGUE, "Жабий язык", "Жабьи языки"));
            Items.Add(new Weapon(ITEM_ID_BOW, "Лук", "Луки",0,15));
        }
        public static void PopulateCharacters()
        {
            Character wife = new Character(CHARACTER_ID_WIFE, "Анна", "Жена", "Ваша любимая жена, которая всегда вас встречала с работы со вкусным ужином.", 5, 5);
            Character boozer = new Character(CHARACTER_ID_BOOZER, "Пьяница", "Житель города", "Вы часто его видите по вечерам около местной таверны, выглядит не опрятно.", 100, 100);
            Character dog = new Character(CHARACTER_ID_DOG, "Арбуз", "Животное", "Ваша любимая собака. У нее морда чем-то похожа на арбуз.", 3, 3);

            Сharacters.Add(wife);
            Сharacters.Add(boozer);
            Сharacters.Add(dog);
        }

        private static void PopulateMonsters()
        {
            //Создаем монстров
            Monster rat = new Monster(MONSTER_ID_RAT, "Крыса", 5, 3, 10, 3, 3);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Змея", 5, 3, 10, 3, 3);
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));

            Monster giantFrog = new Monster(MONSTER_ID_GIANT_TOAD, "Огромная жаба", 10, 5, 15, 15, 15);
            giantFrog.LootTable.Add(new LootItem(ItemByID(ITEM_ID_TOAD_FOOT), 80, false));
            giantFrog.LootTable.Add(new LootItem(ItemByID(ITEM_ID_TOAD_TONGUE), 20, false));


            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Огромный паук", 20, 5, 40, 10, 10);
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));

            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(giantSpider);
            Monsters.Add(giantFrog);
        }

        private static void PopulateQuests()
        {
            // Создаем квесты и их награды
            Quest clearAlchemistGarden =
                new Quest(
                    QUEST_ID_CLEAR_ALCHEMIST_GARDEN,
                    "Очистить сад алхимика",
                    "Убейте крыс в саду алхимика и принесите 3 крысиных хвоста. Вы получите лечебное зелье и 10 золотых монет.", 20, 10);

            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));

            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_CLUB);

            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "Очистить поле фермера",
                    "Убейте змей на поле фермера и принесите 3 змеиных клыка. Вы получите дубину, пропуск и 20 золотых монет.", 20, 20);

            clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));

            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            Quest getGold =
                new Quest(
                    QUEST_ID_TOWN_SQUARE_GOLDEN_TICKET,
                    "Золотой билет",
                    "Получите пропуск. Вы получите дубину и 20 золотых монет.", 20, 20);

            getGold.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_ADVENTURER_PASS), 1));

            getGold.RewardItem = ItemByID(ITEM_ID_CLUB);

            Quest findWIfe =
                new Quest(
                    QUEST_ID_FIND_WIFE,
                    "Найдите вашу жену.",
                    "Ваша жена пропала сегодня ночью. Около дома видны большие следы, которые вы раньше не видели.", 50, 50);

            findWIfe.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_ADVENTURER_PASS), 1));

            findWIfe.RewardItem = ItemByID(ITEM_ID_GOLD_RING);

            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
            Quests.Add(getGold);
            Quests.Add(findWIfe);
        }

        private static void PopulateLocations()
        {
            // Создаем локации
            Location home = new Location(LOCATION_ID_HOME, "Дом", "Ваш дом.");

            Location yard = new Location(LOCATION_ID_YARD, "Двор", "Двор перед вашим домом. У вас тут растет много овощей и фруктов. Тут есть какие-то большие следы.");
            yard.QuestAvailableHere = QuestByID(QUEST_ID_FIND_WIFE);

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Центр города", "Вы видите фонтан.");
            townSquare.QuestAvailableHere = QuestByID(QUEST_ID_TOWN_SQUARE_GOLDEN_TICKET);

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Хижина алхимика", "На полках много странных растений.");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "Сад алхимика", "Здесь растет много растений.");
            alchemistsGarden.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Ферма", "Там маленькая ферма с фермером.");
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Поле фермера", "Вы видите, что здесь растут ряды овощей.");
            farmersField.MonsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Пост охраны", "Здесь большой, крепкий на вид охранник.", ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Мост", "Каменный мост через широкую реку.");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Лес", "Вы видите паутину, покрывающую деревья в этом лесу.");
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);

            Location shop = new Location(LOCATION_ID_SHOP, "Магазин", "Местная лавака, где можно купить различные товары.");


            // Связываем локации
            home.LocationToNorth = yard;

            yard.LocationToNorth = townSquare;
            yard.LocationToSouth = home;
            yard.LocationToEast = shop;

            shop.LocationToWest = yard;

            townSquare.LocationToNorth = alchemistHut;
            townSquare.LocationToSouth = yard;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToWest = farmhouse;

            farmhouse.LocationToEast = townSquare;
            farmhouse.LocationToWest = farmersField;

            farmersField.LocationToEast = farmhouse;

            alchemistHut.LocationToSouth = townSquare;
            alchemistHut.LocationToNorth = alchemistsGarden;

            alchemistsGarden.LocationToSouth = alchemistHut;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = townSquare;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = spiderField;

            spiderField.LocationToWest = bridge;

            // Добавляем локации в статический список
            Locations.Add(home);
            Locations.Add(yard);
            Locations.Add(shop);
            Locations.Add(townSquare);
            Locations.Add(guardPost);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistsGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(bridge);
            Locations.Add(spiderField);
        }

        public static Item ItemByID(int id)
        {
            foreach (Item item in Items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }

            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach (Quest quest in Quests)
            {
                if (quest.ID == id)
                {
                    return quest;
                }
            }

            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach (Location location in Locations)
            {
                if (location.ID == id)
                {
                    return location;
                }
            }

            return null;
        }
    }
}