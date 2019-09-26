using System.Collections.Generic;
using System;

namespace Interactions
{
    public class Place
    {
        public string Name { get; private set; }
        public int[] Coordinates { get; private set; }
        public string Item { get; private set; }
        public bool IsEndLocation { get; private set; }
        public List<Scenario> Scenarios { get; set; }
        public int VisitCount { get; set; }

        public Place(string name, int xCoord, int yCoord, string item, bool isEnd, List<Scenario> scenarios)
        {
            Name = name;
            Coordinates = new int[]{xCoord, yCoord};
            Item = item;
            IsEndLocation = isEnd;
            Scenarios = scenarios;
            VisitCount = 0;
        }

        public bool CanLeave(List<string> items, List<Character> partyGroup)
        {
            bool canLeave = false;
            if(IsEndLocation){
                Character pilot = partyGroup.Find(character => character.Specialty == "Pilot");
                Character boatOp = partyGroup.Find(character => character.Specialty == "Boat Operator");
                Character hamRadioOp = partyGroup.Find(character => character.Specialty == "Ham Radio Operator");

                if(Name == "Heli Pad" && (pilot != null && items.Contains("helicopter keys")))
                {
                    canLeave = true;
                } 
                else if(Name == "Boat Dock" && (boatOp != null && items.Contains("boat keys")))
                {
                    canLeave = true;
                }
                else if (Name == "HAM Radio Station" && (hamRadioOp != null && items.Contains("park keys")))
                {
                    canLeave = true;
                }
            }
            return canLeave;
        }

        public List<Place> MoveOptions(Map map)
        {
            int curXCoord = Coordinates[0];
            int curYCoord = Coordinates[1];
            List<Place> canMoveHere = new List<Place>();
            foreach(Place location in map.Locations)
            {
                int xCoord = location.Coordinates[0];
                int yCoord = location.Coordinates[1];
                if((Math.Abs(curXCoord - xCoord) < 2 && curYCoord == yCoord) || (curXCoord == xCoord && Math.Abs(curYCoord - yCoord) < 2))
                {
                    if(xCoord != curXCoord && yCoord != curYCoord)
                    {
                        canMoveHere.Add(location);
                    }
                }
            }
            return canMoveHere;
        }

        public static void VisitorCenterEvents(Place currentLocation, Party party, List<Character> characters)
        {
            if(currentLocation.Name == "Visitor Center")
            {
                currentLocation.VisitCount++;

                if(currentLocation.VisitCount == 1)
                {
                    Console.WriteLine("You see Andy watching TV. Want to see what he's watching? (Y/N)");
                    string watch = Console.ReadLine().ToLower();
                    if(watch == "y")
                    {
                        Console.WriteLine("Oh no! It showed a dinosaur escaping from its pen and startled Andy. Now they're choking!");
                        Console.WriteLine("Do you help Andy? (Y/N)");
                        string help = Console.ReadLine().ToLower();
                        if(help == "y")
                        {
                            Console.WriteLine("You save Andy! They are grateful to you and join your group.");
                            party.GroupMembers.Add(characters.Find(person => person.Name == "Andy"));
                        }
                        else
                        {
                            Console.WriteLine("Andy survived without your help. But they don't like you and will not join your group. You're on your own.");
                        }
                    }
                    else if (watch == "n")
                    {
                        Console.WriteLine("You move on...");
                    }
                } 
                else 
                {
                    RandomDinoAttack(party);
                }

            }

        }

        public static void RandomDinoAttack(Party party)
        {
            string[] dinoNames = {"T-Rex", "pack of Compys", "velociraptor", "carnotaurus", "utahraptor", "pterodactyl"};
            Random rnd = new Random();
            int index = rnd.Next(0, dinoNames.Length);
            Console.WriteLine("You are visciously attacked by a " + dinoNames[index] + "! Do you attack or try to run?! (A/R)");
            string response = Console.ReadLine().ToLower();
            if(response == "a")
            {
                Console.WriteLine("You fool! It's a dinosaur, you had no chance!");
                int partyMemberIndex = rnd.Next(0, party.GroupMembers.Count);
                Console.WriteLine(party.GroupMembers[partyMemberIndex].Name + " has been bitten!");
                party.GroupMembers[partyMemberIndex].Health--;

                if(party.GroupMembers[partyMemberIndex].Health <= 0){
                    Console.WriteLine("After suffering in pain, " + party.GroupMembers[partyMemberIndex].Name + ", succummed to their wounds and died.");
                    party.GroupMembers.Remove(party.GroupMembers[partyMemberIndex]);
                }
            }
            else 
            {   
                if(party.GroupMembers.Count > 1){
                    int partyMemberIndex = rnd.Next(0, party.GroupMembers.Count);
                    if(partyMemberIndex != 0){
                        Console.WriteLine("You somehow convinced " + party.GroupMembers[partyMemberIndex].Name + " to run in a different direction.");
                        Console.WriteLine("You escape but wonder if you will ever see them again.");
                        party.GroupMembers.Remove(party.GroupMembers[partyMemberIndex]);
                    }
                    else 
                    {
                        Console.WriteLine("The party stuck together while you ran. The dinosaurs chased after you!");
                        party.GroupMembers[0].Health--;
                        Console.WriteLine("You were bitten, but somehow managed to escape and find the  group.");
                    }
                }
            }
        }




        public static List<Place> CreateLocations(){
            Scenario introduction = new Scenario("Move around the park and have fun. Keep an eye out for special events!", "", "", "", "");
            Scenario saveAndy = new Scenario("Let's see what Andy is watching on TV.. Oh no! It showed a dinosaur escaping from its pen and startled Andy. Now they're choking!", "", "", "", "");
            Scenario parkOpIntro = new Scenario("A big stegasaurus is being cared for by Sam; it fought with another stegasaurus and lost and needed help. It's nice, you can pet it if you want to.", "", "", "", "");
            Scenario stegasaurus = new Scenario("You moved too quickly and startled it! It swings its spiked tail at you!", "", "Visitor", "", "");
            Scenario raptorPenIntroNoPaleo = new Scenario("This is the raptor pen.. You hear snarles from inside. Are you sure you want to go in?", "", "", "", "");
            Scenario raptorPenIntroWPaleo = new Scenario("You enter the raptor pen but see no raptors, or even indications they are there.", "", "", "", "");
            Scenario raptor = new Scenario("The velociraptors have surrounded you!", "", "Paleontologist", "Devan becomes the Alpha Raptor and sneaks you out of the surrounding raptor pack. You find the Pilot and add them to your party.", "You had found the Pilot, but they sacrificed themselves by running first.");
            Scenario tRexIntro = new Scenario("T-Rex are known as the King of Dinosaurs. Did you know they can't see you if you don't move?", "", "", "", "");
            Scenario tRex = new Scenario("You must have eaten recently, the T-Rex smelled you and is coming this way!", "Red Shirt", "", "Devan told you to throw your Red Shirt to distract the T-Rex. It worked! The Paleontologist joins your party.", "You stood still thinking the T-Rex wouldn't see you. Unfortunately your scent betrayed your location, you were bitten but Devan saved you. The Paleontologist joins your party.");
            Scenario herbPenIntro = new Scenario("This is the herbivore pen. It should be safer.", "", "", "", "");
            Scenario plantAttack = new Scenario("You brushed against a poisonous plant and have a painful rash.", "Biologist", "");
            Scenario utilityIntro = new Scenario("There was a power surge! The utility bunker is super dark, but you hear a rustle in the bushes behind you.", "", "", "", "");
            Scenario compys = new Scenario("A large flock of Compsognathus has surged into the room behind you! You struggle to find your way in the dark.", "flashlight", "");
            Scenario heliIntro = new Scenario("A waiting helicoptor... Looks like it's ready to go, but you don't see any keys.", "", "", "", "");
            Scenario boatIntro = new Scenario("The boat dock has an excellent view. You see a boat staged and waiting, but no one is around and you don't find any keys.", "", "", "", "");
            Scenario hamIntro = new Scenario("A special room to contact the outside world, but it's locked. You need park keys to get in.", "", "", "", "");

            Place visitorCenter = new Place("Visitor Center", 1, 0, "", false, new List<Scenario>(){});
            Place heliPad = new Place("Heli Pad", 0, 2, "", true, new List<Scenario>() {heliIntro});
            Place boatDock = new Place("Boat Dock", 4,2, "", true, new List<Scenario>() {boatIntro});
            Place hamRadioStation = new Place("HAM Radio Station", 2, 4, "", true, new List<Scenario>() {hamIntro});
            Place foodPlaza = new Place("Food Plaza", 2, 1, "", false, new List<Scenario>(){introduction, saveAndy});
            Place parkOp = new Place("Park Operations", 3, 1, "helicopter keys", false, new List<Scenario>() {parkOpIntro, stegasaurus});
            Place raptorPen = new Place("Velociraptor Pen", 1, 1, "", false, new List<Scenario>() {raptorPenIntroNoPaleo, raptorPenIntroWPaleo, raptor});
            Place tRexPen = new Place("T-Rex Pen", 3, 3, "", false, new List<Scenario>() {tRexIntro, tRex});
            Place herbivorePen = new Place("Herbivore Pen", 2, 3, "", false, new List<Scenario>() {herbPenIntro, plantAttack});
            Place utilities = new Place("Utilities Bunker", 1, 3, "boat keys", false, new List<Scenario>() {utilityIntro, compys});
            Place pathWest = new Place("Path West", 1, 2,"", false, new List<Scenario>(){});
            Place pathCenter = new Place("Path Center", 2, 2,"", false, new List<Scenario>(){});
            Place pathEast = new Place("Path East", 3, 2, "", false, new List<Scenario>() { });

            List<Place> locations = new List<Place>(){visitorCenter, heliPad, boatDock, hamRadioStation, foodPlaza, parkOp, raptorPen, tRexPen, herbivorePen, utilities};

            return locations;
        }
        
    }

    public class Scenario
    {
        public string EventDesc { get; private set; }
        public string ItemNeeded { get; private set; }
        public string SpecialistNeeded { get; private set; }

        public string Success { get; private set; }
        public string Failure { get; private set; }
        public Scenario (string eventDescription, string itemNeeded, string specialistNeeded, string success, string failure)
        {
            EventDesc = eventDescription;
            ItemNeeded = itemNeeded;
            SpecialistNeeded = specialistNeeded;
            Success = success;
            Failure = failure;
        }

        public bool CheckOutcome(List<string> items, List<Character> party)
        {
            bool outcome = false;
            Character neededPerson = party.Find(character => character.Specialty == SpecialistNeeded);
            if(SpecialistNeeded != "" && neededPerson != null)
            {
                outcome = true;
            }
            else if(ItemNeeded != "" && items.Contains(ItemNeeded))
            {
                outcome = true;
            }
            else if(ItemNeeded == "" && SpecialistNeeded == ""){
                outcome = true;
            }
            return outcome;
        }
    }

    public class Map
    {
        public List<Place> Locations { get; private set; }
        public Map(){
            Locations = Place.CreateLocations();
        }
    }
}