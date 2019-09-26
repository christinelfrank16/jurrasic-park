using System;
using System.Threading;
using System.Collections.Generic;
using Interactions;

class Program
{
    static void Main()
    {
        Map map = new Map();
        Party party = new Party();
        List<Character> characters = Character.CreateCharacters();
        Place visitorLocation = map.Locations.Find(location => location.Name == "Visitor Center");

        Character visitor = Welcome();
        party.AddCharacter(visitor);

        foreach(Character c in characters){
            Console.WriteLine("Hi " + c.Name);
        }
        foreach(Place p in map.Locations){
            Console.WriteLine("Place " + p.Name);
        }

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

    public static Move(string letter, List<Place> moveOptions)
    {
        if(letter == "s"){

        }
    }
}