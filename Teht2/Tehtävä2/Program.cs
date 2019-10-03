using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtävä2
{
    class Program
    {
        

        static void Main(string[] args)
        {
            List<Player> playerList = CreatePlayers();
            List<PlayerForAnotherGame> anotherPlayerList = CreateAnotherPlayers();

            Game<Player> originalGame = new Game<Player>(playerList);
            Game<PlayerForAnotherGame> ripOffGame = new Game<PlayerForAnotherGame>(anotherPlayerList);

            Item bestItem = playerList[5].GetHighestLevelItem();

            Console.WriteLine("Best item of Player " + playerList[5].Id +": "  + bestItem.Id + " level: " + bestItem.Level);

            Item[] playerItemList = GetItems(playerList[10]);
            Item[] playerItemList2 = GetItemsWithLinq(playerList[25]);

            Console.WriteLine("Player " + playerList[10].Id  + " items:");


            foreach (Item item in playerItemList)
            {
                Console.WriteLine("Item name: " + item.Id + " Level: " + item.Level);
            }

            Console.WriteLine("Player " + playerList[25].Id + " items:");

            foreach (Item item in playerItemList2)
            {
                Console.WriteLine("Item name: " + item.Id + " Level: " + item.Level);
            }

            ActionTest.ProcessEachItem(playerList[86], ActionTest.PrintItem);

            ActionTest.ProcessEachItem(playerList[86], x => Console.WriteLine("Lambda: Item ID: {0} and Item Level: {1}", x.Id, x.Level));
            Player[] originalTop10 = originalGame.GetTop10Players();
            Console.WriteLine("\n\n\nOriginal Game Leaderboards\n\n");
            for (int i = 0; i < originalTop10.Length; i++)
            {
                Console.WriteLine((i+1) + ". " + originalTop10[i].Id + "   Score: " + originalTop10[i].Score);
            }
            PlayerForAnotherGame[] ripoffTop10 =  ripOffGame.GetTop10Players();
            Console.WriteLine("\n\n\nRip-Off Game Leaderboards\n\n");
            for (int i = 0; i < ripoffTop10.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + ripoffTop10[i].Id + "   Score: " + ripoffTop10[i].Score);
            }
            Console.ReadKey();


            List<Player> CreatePlayers()
            {
                List<Player> players = new List<Player>();

                int playerAmount = 1000000;
                Random rand = new Random();

                for (int i = 0; i < playerAmount; i++)
                {
                    Player player = new Player
                    {
                        Id = Guid.NewGuid(),
                        Score = rand.Next(0,50000),
                        Items = new List<Item>() { new Item() { Id = Guid.NewGuid(), Level = rand.Next(0, 60) }, new Item() { Id = Guid.NewGuid(), Level = rand.Next(0, 60) } }
                    };

                    players.Add(player);
                }

                int uniquePlayers = players.Distinct().Count();

                if (uniquePlayers != playerAmount)
                {
                    players = CreatePlayers();
                    Console.WriteLine("Duplicates found");
                }
                else
                {
                    Console.WriteLine("No duplicates found");
                }

                return players;
            }

            List<PlayerForAnotherGame> CreateAnotherPlayers()
            {
                List<PlayerForAnotherGame> players = new List<PlayerForAnotherGame>();

                int playerAmount = 1000;
                Random rand = new Random();

                for (int i = 0; i < playerAmount; i++)
                {
                    PlayerForAnotherGame player = new PlayerForAnotherGame
                    {
                        Id = Guid.NewGuid(),
                        Score = rand.Next(0, 50000),
                        Items = new List<Item>() { new Item() { Id = Guid.NewGuid(), Level = rand.Next(0, 60) }, new Item() { Id = Guid.NewGuid(), Level = rand.Next(0, 60) } }
                    };

                    players.Add(player);
                }

                int uniquePlayers = players.Distinct().Count();

                if (uniquePlayers != playerAmount)
                {
                    players = CreateAnotherPlayers();
                    Console.WriteLine("Duplicates found");
                }
                else
                {
                    Console.WriteLine("No duplicates found");
                }

                return players;
            }

            Item[] GetItems(Player player)
            {
                Item[] items = new Item[player.Items.Count()];

                Console.WriteLine(" " + items.Length);
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = player.Items[i];
                    Console.WriteLine("Level " + items[i].Level);
                }

                return items;
            }

            Item[] GetItemsWithLinq(Player player)
            {
                return player.Items.ToArray();
            }
        }

    }

    public class ActionTest
    {
        public static void ProcessEachItem(Player player, Action<Item> process)
        {
            foreach (var item in player.Items)
            {
                process(item);
            }
        }

        public static void PrintItem(Item item)
        {
            Console.WriteLine("Item ID: {0} and Item Level: {1}", item.Id, item.Level);
        }
    }

    public class Game<T> where T : IPlayer
    {
        private List<T> _players;

        public Game(List<T> players)
        {
            _players = players;
        }

        public T[] GetTop10Players()
        {
            return _players.OrderByDescending(o => o.Score).Take(10).ToArray();
        }
    }

    public interface IPlayer
    {
        int Score { get; set; }
    }

    public static class HighestLevelItem
    {
        public static Item GetHighestLevelItem(this Player p)
        {
            List<Item> items = p.Items;

            Item item = items.OrderByDescending(o => o.Level).First();

            return item;
        }
    }

    public class Player : IPlayer
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public List<Item> Items { get; set; }
    }

    public class PlayerForAnotherGame : IPlayer
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public Guid Id { get; set; }
        public int Level { get; set; }
    }

}
