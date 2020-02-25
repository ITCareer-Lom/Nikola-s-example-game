using System;
using System.Linq;
using System.Collections.Generic;

namespace Game
{ 
    class Program
    {
        static void Main(string[] args)
        {
            // TODO
            /*
            * Defensife stats - block some damage
            */

            List<Enemy> enemies = new List<Enemy>();

            // Adding enemies
            #region
            enemies.Add(new Enemy { name = "Spider", health = 20, attack = 4 });

            enemies.Add(new Enemy { name = "Wolf", health = 10, attack = 12 });

            enemies.Add(new Enemy { name = "Shark", health = 40, attack = 6 });

            enemies.Add(new Enemy { name = "Granny", health = 100, attack = 1 });

            enemies.Add(new Enemy { name = "TRex", health = 130, attack = 25 });
            #endregion

            List<Card> cards = new List<Card>();
            List<string> availableCards = new List<string>();

            // Adding cards from
            #region
            cards.Add(new Card
            {
                name = "Troll",
                elixirCost = 7
                ,
                health = 40,
                attack = 5
            }); availableCards.Add("Troll");

            cards.Add(new Card
            {
                name = "Swordsman",
                elixirCost = 3
                ,
                health = 20,
                attack = 12
            }); availableCards.Add("Swordsman");

            cards.Add(new Card
            {
                name = "Archer",
                elixirCost = 2
                ,
                health = 10,
                attack = 10
            }); availableCards.Add("Archer");

            cards.Add(new Card
            {
                name = "Giant",
                elixirCost = 7
                ,
                health = 60,
                attack = 7
            }); availableCards.Add("Giant");
            #endregion

            int elixir = 5;
            int turns = 1, countOfCardsInOutputting = 1, countOfEnemiesInOutputting = 1;
            Enemy currentEnemy = new Enemy();
            List<Card> currentCards = new List<Card>();

            Console.WriteLine("The availbable actions for a card are - heal, attack");
            Console.WriteLine();

            // Code for game
            while (true)
            {
                Console.WriteLine($"You have these cards right now and {elixir} elixir");
                Console.WriteLine();

                // Showing available cards
                #region
                foreach (Card carrd in cards)
                {
                    Console.WriteLine(carrd.name + $" with {carrd.health} HP and " +
                        $"{carrd.attack} attack, costs {carrd.elixirCost} elixir");
                    currentCards.Add(carrd);
                    if (countOfCardsInOutputting == 2)
                    {
                        countOfCardsInOutputting = 1;
                        break;
                    }
                    countOfCardsInOutputting++;
                }
                Console.WriteLine();
                #endregion

                // Showing new enemy
                #region
                if (enemies.Count == 0)
                {
                    Console.WriteLine("Congratz! You killed all enemies!");
                    return;
                }
                foreach (Enemy enemy in enemies)
                {
                    Console.WriteLine($"The enemy is a {enemy.name}, it has {enemy.health} HP and" +
                        $" {enemy.attack} attack");
                    currentEnemy = enemy;
                    countOfEnemiesInOutputting++;
                    if (countOfEnemiesInOutputting == 2)
                    {
                        countOfEnemiesInOutputting = 1;
                        break;
                    }
                }
                Console.WriteLine();
                #endregion


                // Getting the information from the user
                Console.WriteLine("What do you do?(type {card},{action})");
                string[] input = Console.ReadLine().Split(',');
                string card = input[0];
                string action = input[1];

                // Checking if card exists
                while (availableCards.Contains(card) == false || availableCards.Contains(""))
                {
                    Console.WriteLine("You entered wrong card name. What do you do?(type {card},{action})");
                    input = Console.ReadLine().Split(',');
                    card = input[0];
                    action = input[1];
                }

                // Checking if action exists
                while (action != "attack" && action != "heal" && action != "nothing")
                {
                    Console.WriteLine("You entered wrong action. What do you do?(type {card},{action})");
                    input = Console.ReadLine().Split(',');
                    card = input[0];
                    action = input[1];
                }

                if (action == "nothing")
                {

                }
                else
                {
                    elixir = PayElixir(elixir, currentCards, card);

                    // Checking if elixir is enough
                    while (elixir < 0)
                    {
                        Console.WriteLine("You don't have enough elixir. What do you do?(type {card},{action})");
                        input = Console.ReadLine().Split(',');
                        card = input[0];
                        action = input[1];
                        elixir = PayElixir(elixir, currentCards, card);
                    }

                }

                // Doing the action
                if (action == "attack")
                {
                    Console.WriteLine($"{card} attacked {currentEnemy.name}!");
                    Attack(cards, enemies, card, currentEnemy, currentCards);

                }
                else if (action == "nothing")
                {
                    EnemyHeal(currentEnemy);
                    elixir += 5;
                    if (elixir > 10)
                    {
                        elixir = 10;
                    }
                    turns++;
                    continue;
                }
                else
                {
                    EnemyHeal(currentEnemy);
                    Console.WriteLine($"{card} healed for 10 HP");
                    Heal(card, currentCards);
                }

                // Giving elixir and ending the turn
                elixir += 5;
                if (elixir > 10)
                {
                    elixir = 10;
                }
                turns++;
            }

        }
        private static void EnemyHeal(Enemy enemy)
        {
            enemy.health += 2;
        }
        private static int PayElixir(int elixir, List<Card> currentCards, string cardName)
        {
            int elixirCost = 0;
            foreach (Card card in currentCards)
            {
                if (card.name == cardName)
                {
                    elixirCost = card.elixirCost;
                }
            }
            elixir -= elixirCost;
            return elixir;
        }
        private static void Heal(string cardName, List<Card> currentCards)
        {
            foreach (Card card in currentCards)
            {
                if (card.name == cardName)
                {
                    card.health = card.health + 5;
                    break;
                }
            }
        }
        private static void Attack(List<Card> cards, List<Enemy> enemies, string cardName,
            Enemy enemy, List<Card> currentCards)
        {
            Card attackingCard = new Card();
            foreach (Card card in currentCards)
            {

                if (card.name == cardName)
                {
                    attackingCard = card;
                }
            }

            Console.WriteLine();
            enemy.health -= attackingCard.attack;
            if (enemy.health < 0)
            {
                enemy.health = 0;
            }

            Console.WriteLine($"You dealt {attackingCard.attack} damage to your opponent" +
                $"and now it has only {enemy.health} HP!");

            attackingCard.health -= enemy.attack;
            if (attackingCard.health <= 0)
            {
                attackingCard.health = 0;
            }

            Console.WriteLine($"Your enemy struck you with {enemy.attack} damage and now you" +
                $" have {attackingCard.health} HP!");
            Console.WriteLine();

            if (enemy.health <= 0)
            {
                EnemyDead(enemy, enemies);
                Console.WriteLine($"You killed the {enemy.name}!");
                Console.WriteLine();
            }
            if (attackingCard.health <= 0)
            {
                CardDead(cards, attackingCard);
                Console.WriteLine($"Unfortunately your {attackingCard.name} got killed!");
                Console.WriteLine();
            }
        }

        private static void CardDead(List<Card> cards, Card card)
        {
            if (cards.Contains(card))
            {
                cards.Remove(card);
            }
        }

        public static void EnemyDead(Enemy enemy, List<Enemy> enemies)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
        }
    }
}
