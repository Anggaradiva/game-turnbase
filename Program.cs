using System;
using System.Collections.Generic;

namespace DungeonAdventure
{
    public class Character
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
    }

    public class Player : Character
    {
        public List<string> Inventory { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public Player()
        {
            Inventory = new List<string>();
        }
    }

    public class Enemy : Character
    {
        public string Name { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public string Race { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttackBonus { get; set; }
        public int DefenseBonus { get; set; }
    }

    public class Dungeon
    {
        private Random random = new Random();
        private List<Enemy> enemies;
        private List<Item> availableItems;
        private Player player;

        public Dungeon()
        {
            enemies = new List<Enemy>
            {
                new Enemy { Name = "Goblin", Health = 20, Attack = 10, Defense = 5, Experience = 10, Gold = 5, Race = "Setan" },
                new Enemy { Name = "Skeleton", Health = 30, Attack = 15, Defense = 3, Experience = 15, Gold = 8, Race = "Tengkorak" },
                new Enemy { Name = "Orc", Health = 40, Attack = 20, Defense = 8, Experience = 20, Gold = 10, Race = "Orc" }
            };

            availableItems = new List<Item>
            {
                new Item { Name = "Sword", Description = "Pedang biasa", AttackBonus = 5, DefenseBonus = 0 },
                new Item { Name = "Shield", Description = "Perisai kokoh", AttackBonus = 0, DefenseBonus = 5 },
                new Item { Name = "Potion", Description = "Menyembuhkan 20 poin kesehatan", AttackBonus = 0, DefenseBonus = 0 }
            };
        }

        public void Start()
        {
            Console.WriteLine("Selamat datang di Game Turn Based Dungeon!");
            Console.WriteLine("=======================================");

            player = CreatePlayer();
            Item chosenItem = ChooseItem();

            player.Attack += chosenItem.AttackBonus;
            player.Defense += chosenItem.DefenseBonus;

            Console.WriteLine($"Anda memilih {chosenItem.Name}. Saatnya Memulai Pertualangan!");

            while (player.Health > 0)
            {
                ExploreRoom();
            }

            Console.WriteLine("Game over! Anda telah kalah.");
        }

        private Player CreatePlayer()
        {
            Console.WriteLine("Silahkan Buat Karakter Anda:");
            Console.Write("Silahkan Masukan Nama Pemain: ");
            string playerName = Console.ReadLine();

            Player player = new Player
            {
                Health = 100,
                Attack = 10,
                Defense = 5,
                PositionX = 0,
                PositionY = 0
            };

            Console.WriteLine($"Selamat datang, {playerName}! Status Anda: Hp: {player.Health}, Serangan: {player.Attack}, Pertahanan: {player.Defense}");
            Console.WriteLine("Anda berada di ruangan awal.");

            return player;
        }

        private Item ChooseItem()
        {
            Console.WriteLine("\nSilahkan Memilih Item Sebelum Berpetualang:");
            for (int i = 0; i < availableItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableItems[i].Name}: {availableItems[i].Description}");
            }

            Console.Write("Masukkan nomor item: ");
            int choice = int.Parse(Console.ReadLine()) - 1;

            return availableItems[choice];
        }

        private void ExploreRoom()
        {
            Console.WriteLine("Anda Sedang Menjelajahi Ruangan...");
            Console.WriteLine("Anda berada di ruangan ini.");

            Console.WriteLine("Pilih arah untuk menjelajahi ruangan:");
            Console.WriteLine("1. Utara");
            Console.WriteLine("2. Selatan");
            Console.WriteLine("3. Timur");
            Console.WriteLine("4. Barat");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    player.PositionY++;
                    break;
                case "2":
                    player.PositionY--;
                    break;
                case "3":
                    player.PositionX++;
                    break;
                case "4":
                    player.PositionX--;
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid.");
                    break;
            }

            int encounterChance = random.Next(1, 101);
            if (encounterChance <= 60)
            {
                Battle();
            }
            else
            {
                Console.WriteLine("Tidak ada musuh di ruangan ini.");
            }
        }

        private void Battle()
        {
            Enemy enemy = enemies[random.Next(enemies.Count)];
            int enemyHealth = random.Next(20, 51);

            Console.WriteLine($"Anda bertemu seorang {enemy.Name}!");

            // Dialog dengan monster
            Console.WriteLine($"[{enemy.Name}]: Hai! Saya seorang {enemy.Race}. Apakah Anda ingin bertarung dengan saya? (ya/tidak)");
            string response = Console.ReadLine().ToLower();

            if (response == "tidak")
            {
                Console.WriteLine($"[{enemy.Name}]: Baiklah, selamat jalan!");
                return;
            }

            Console.WriteLine($"[{enemy.Name}]: Bagus! Mari kita mulai pertempuran.");

            while (enemyHealth > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Kesehatan Anda: {player.Health}");
                Console.WriteLine($"Kesehatan {enemy.Name}: {enemyHealth}");
                Console.WriteLine("Apa yang akan Anda lakukan?");
                Console.WriteLine("1. Serang");
                Console.WriteLine("2. Lari");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        int playerDamage = random.Next(player.Attack - 5, player.Attack + 6);
                        int enemyDamage = random.Next(enemy.Attack - 3, enemy.Attack + 4);

                        enemyHealth -= playerDamage;
                        Console.WriteLine($"Anda menyerang {enemy.Name} dan menyebabkan {playerDamage} kerusakan.");

                        if (enemyHealth <= 0)
                        {
                            Console.WriteLine($"Anda berhasil mengalahkan {enemy.Name}!");
                            return;
                        }

                        player.Health -= enemyDamage;
                        Console.WriteLine($"{enemy.Name} menyerang Anda dan menyebabkan {enemyDamage} kerusakan.");

                        if (player.Health <= 0)
                        {
                            Console.WriteLine("Kesehatan Anda habis. Permainan berakhir.");
                            return;
                        }
                        break;
                    case "2":
                        Console.WriteLine("Anda melarikan diri dari pertempuran.");
                        return;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }

                Console.WriteLine("Tekan tombol apa saja untuk melanjutkan...");
                Console.ReadKey(true);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dungeon dungeon = new Dungeon();
            dungeon.Start();
        }
    }
}
