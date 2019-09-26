using System;
using System.Collections.Generic;
using Park;

namespace Interactions
{
    public class Character
    {
        public string Name{ get; set; }
        public string Specialty { get; private set;}
        public List<string> Backpack { get; set;}
        public int Health { get; set; }

        public Character(string name, string specialty, string item)
        {
            Name = name;
            Specialty = specialty;
            Backpack = new List<string>();
            if(item != ""){
                Backpack.Add(item);
            }
            Health = 2;
        }

        public void Heal(Character characterToHeal)
        {
            if(Specialty == "Doctor" && characterToHeal.Health < 5)
            {
                characterToHeal.Health++;
            }
            else if( Specialty == "Doctor" &&  characterToHeal.Health >= 5 )
            {
                Console.WriteLine("You're looking just fine. You don't need my help.");
            } else {
                Console.WriteLine("I don't know how to do that...");
            }

        }

        public void Attacked(int pointsLost){
            Health = Health - pointsLost;
        }

        public void AddItem (string item){
            Backpack.Add(item);
        }

        public static List<Character> CreateCharacters()
        {
            Character pilot = new Character("Ryan", "Pilot", "");
            Character doctor = new Character("Sam", "Doctor", "");
            Character paleontologist = new Character("Devan", "Paleontologist", "Raptor Claw");
            Character parkOwner = new Character("Sydney", "Park Owner", "park keys");
            Character biologist = new Character("Addison", "Biologist", "");
            Character hamRadioOp = new Character("Andy", "Ham Radio Operator", "flashlight");
            Character boatOp = new Character("Taylor", "Boat Operator", "");
            Character generic = new Character("Bobby", "Visitor", "Red Shirt");

            List<Character> people = new List<Character>(){ pilot, doctor, paleontologist, parkOwner, biologist,hamRadioOp, boatOp, generic};

            return people;
        }
    }

    public class Party
    {
        public List<Character> GroupMembers { get; set; }
        public Party(){
            GroupMembers = new List<Character>();
        }

        public void AddCharacter(Character character)
        {
            GroupMembers.Add(character);
        }

        public void RemoveCharacter(Character character)
        {
            GroupMembers.Remove(character);
        }
    }
}
