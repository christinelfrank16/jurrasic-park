using System;
using System.Threading;
using System.Collections.Generic;
using Interactions;

namespace Park 
{

    class Program
    {
        static void Main()
        {
            Map map = new Map();
            Party party = new Party();
            List<Character> characters = Character.CreateCharacters();
            Place visitorLocation = map.Locations.Find(location => location.Name == "Visitor Center");
            bool gameOver = false;
            Character visitor = Welcome();
            party.AddCharacter(visitor);
            Intro();
            while(!gameOver){
                Place.VisitorCenterEvents(visitorLocation, party, characters);
                // Other Event "Listeners"
                visitorLocation = UserDirection(party, map, visitorLocation);
                gameOver = CheckForEnd(visitor);
            }
            EndGame();

        }

        public static Character Welcome(){
            Console.WriteLine("Please register with our attendant.");
            Console.WriteLine("What is your name?");
            string name = Console.ReadLine();
            Console.WriteLine("Hi " + name + "! Welcome to Jurrasic Park!");
            Console.WriteLine("You are currently in our lovely Visitor Center. Feel free to");
            Console.WriteLine("wander around and explore the park! Our employees are very ");
            Console.WriteLine("friendly and helpful if you need anything.");

            Character visitor = new Character(name, "Visitor", "luck");
            visitor.Health = 5;

            return visitor;
        }

        public static void Intro()
        {
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("If you want to see a map of the park, type \" Map \".");
            Console.WriteLine("It will tell you the locations you can move to.");
            Console.WriteLine("You can move around the park by typing W, S, E, or N");
            Console.WriteLine("");

            Thread.Sleep(1000);

            Console.WriteLine("You can search your current location by typing \" Search \"");
            Console.WriteLine("Don't search too often, you may find something unexpected.");
            Console.WriteLine("");

            Thread.Sleep(1000);

            Console.WriteLine("Accidents happen around here. You have a health of 5;");
            Console.WriteLine("don't get to 0, or we can't save you.");
            Console.WriteLine("You can see your current health level by typing \" Health \".");
            Console.WriteLine("");

            Thread.Sleep(1000);

            Console.WriteLine("We have a doctor somewhere on the island. If you have");
            Console.WriteLine("them in your party, they can heal you.");
            Console.WriteLine("Have the doctor heal you by typing \" Heal \".");
            Console.WriteLine("Or have the doctor heal others by typing \" Heal Party \".");
            Console.WriteLine("");
        }

        public static Place UserDirection(Party party, Map map, Place currentLocation)
        {
            bool directionSelected = false;
            while (!directionSelected)
            {
                Console.WriteLine("What do you want to do? [Heal, Heal Party, Map, Move, Search]");
                string action = Console.ReadLine().ToLower();
                if(action == "move")
                {
                    directionSelected = true;
                }
                else if (action == "heal")
                {
                    Character doc = party.GroupMembers.Find(person => person.Specialty == "Doctor");
                    if(doc != null)
                    {
                        doc.Heal(party.GroupMembers[0]);
                    }
                    else {
                        Console.WriteLine("You're in bad shape, you might want to find a doctor.");
                    }
                } 
                else if (action == "heal party")
                {
                    Character doc = party.GroupMembers.Find(person => person.Specialty == "Doctor");
                    if (doc != null)
                    {
                        for(int i = 1; i < party.GroupMembers.Count; i++){
                            doc.Heal(party.GroupMembers[i]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("You might want to find a doctor.");
                    }
                }
                else if (action == "map")
                {
                    ShowMap(currentLocation, map);
                }
                else if (action == "search")
                {
                    Console.WriteLine("You search the area...");
                    if(currentLocation.Item != ""){
                        party.GroupMembers[0].AddItem(currentLocation.Item);
                        currentLocation.Item = "";
                    }
                    else 
                    {
                        Random rnd = new Random();
                        int diceRoll = rnd.Next(0,2);
                        if(diceRoll != 0){
                            Place.RandomDinoAttack(party);
                        } else {
                            Console.WriteLine("You find nothing of value.");
                        }
                    }
                }

            }
            bool validDirection = false;
            Console.WriteLine("What direction do you want to go? [N, S, E, W]");

            while(!validDirection)
            {
                string direction = Console.ReadLine().ToLower();
                List<Place> options = currentLocation.MoveOptions(map);
                Place next = Move(direction, options, currentLocation);
                if(next != null)
                {
                    currentLocation = next;
                    validDirection = true;
                }
                else
                {
                    Console.WriteLine("Not a valid location direction. Please retry.");
                }
            }
            return currentLocation;
        }

        public static Place Move(string letter, List<Place> moveOptions, Place currentLocation)
        {   
            int currX = currentLocation.Coordinates[0];
            int currY = currentLocation.Coordinates[1];
            int nextX = -1;
            int nextY = -1;
            if(letter == "s")
            {
                nextY = currY - 1;
            } 
            else if (letter == "n")
            {
                nextY = currY + 1;
            } 
            else if (letter == "w")
            {
                nextX = currX - 1;
            }
            else if (letter == "e")
            {
                nextX = currX + 1;
            }

            nextX = nextX >= 0 ? nextX : currX;
            nextY = nextY >= 0 ? nextY : currY;

            Place next = moveOptions.Find(place => place.Coordinates[0] == nextX && place.Coordinates[1] == nextY);
            return next; 
        }

        public static void ShowMap(Place currentLocation, Map map)
        {
            var arr = new[]
                            {
                            @"                                                                         ",
                            @"                             ---------------                             ",
                            @"                            |               |                            ",
                            @"                            |     RADIO     |                            ",
                            @"                            |     TOWER     |                            ",
                            @"               --------------------------------------------              ",
                            @"              |             |               |              |             ",
                            @"              |             |               |              |             ",
                            @"              |   UTILITY   |   HERBIVORE   |   T-REX      |             ",
                            @"              |   BUILDING  |   PLAYLAND    |   COMPOUND   |             ",
                            @"              |             |               |              |             ",
                            @"              |             |               |              |             ",
                            @"     ---------|-----------------------------------------------------     ",
                            @"    |         |             |               |              |        |    ",
                            @"    | H-PAD   |             |               |              | B-DOCK |    ",
                            @"    |         |             |               |              |        |    ",
                            @"     ---------------------------------------------------------------     ",
                            @"              |             |               |              |             ",
                            @"              |             |               |              |             ",
                            @"              |    RAPTOR   |               |   PARK OPS   |             ",
                            @"              |     PEN     |   CAFETERIA   |    CENTER    |             ",
                            @"              |             |               |              |             ",
                            @"              |             |               |              |             ",
                            @"               --------------------------------------------              ",
                            @"                            |               |                            ",
                            @"                            |    VISITOR    |                  N         ",
                            @"                            |    CENTER     |                W   E       ",
                            @"                             ---------------                   S         ",
                            @"                                                                         ",
                            };

            Console.WriteLine("\n\n");
            foreach (string line in arr)
            Console.WriteLine(line);
            List<Place> options = currentLocation.MoveOptions(map);
            string optionString = "";
            Console.WriteLine("You are currently at: " + currentLocation.Name);
            foreach (Place option in options)
            {
                optionString += option.Name + " ";
            }
            Console.WriteLine("You can move to: " + optionString);
        }

        public static bool CheckForEnd(Character visitor){
            bool gameOver = false;
            if(visitor.Health < 0){
                gameOver = true;
            }
            return gameOver;
        }
        public static void EndGame()
        {
            Console.WriteLine("You valiantly tried to escape the park, but dinosaurs proved to be cunning adversaries. Hopefully there were some survivors to tell your tale.");
            Console.WriteLine("Do you want to play again?  [Y/N]");
            string playAgain = Console.ReadLine();
            if (playAgain == "Y")
            {
                Main();
            }
            else
            {
                Console.WriteLine("Thanks for playing!");
            }
        }
    }
}