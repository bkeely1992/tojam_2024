using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    //Data class used for managing the data of a specific animal species
    [Serializable]
    public class AnimalData
    {
        public Sprite characterSprite;
        public Sprite caseFileSprite;
        public Sprite dietSprite;
        public Sprite classSprite;
        public string dialogueSound;

        public enum Animal_class
        {
            mammal, bird, reptile, invalid
        }
        public enum Animal_diet
        {
            carnivore, herbivore, omnivore, invalid
        }
        public enum Animal_species
        {
            dog, lion, goat, owl, penguin, chicken, lizard, turtle, snake, invalid
        }
        public Animal_class Class = Animal_class.invalid;
        public Animal_diet Diet = Animal_diet.invalid;
        public Animal_species Species = Animal_species.invalid;

        [SerializeField]
        private List<string> possibleNames = new List<string>();

        public string CurrentName => currentName;
        private string currentName = "";

        public static List<Animal_class> AllAnimalClasses = new List<Animal_class>() { Animal_class.bird, Animal_class.reptile, Animal_class.mammal };
        public static List<Animal_diet> AllAnimalDiets = new List<Animal_diet>() { Animal_diet.herbivore, Animal_diet.carnivore, Animal_diet.omnivore };
        public static List<Animal_species> AllAnimalSpecies = new List<Animal_species>() { Animal_species.dog, Animal_species.lion, Animal_species.goat, Animal_species.owl, Animal_species.penguin, Animal_species.chicken, Animal_species.lizard, Animal_species.turtle, Animal_species.snake };

        public List<DialogueData> PossibleGreetings => possibleGreetings;
        [SerializeField]
        private List<DialogueData> possibleGreetings = new List<DialogueData>();

        public List<DialogueData> PossibleCorrectGuiltyReactions => possibleCorrectGuiltyReactions;
        [SerializeField]
        private List<DialogueData> possibleCorrectGuiltyReactions = new List<DialogueData>();

        public List<DialogueData> PossibleIncorrectGuiltyReactions => possibleIncorrectGuiltyReactions;
        [SerializeField]
        private List<DialogueData> possibleIncorrectGuiltyReactions = new List<DialogueData>();

        public List<DialogueData> PossibleCorrectInnocentReactions => possibleCorrectInnocentReactions;
        [SerializeField]
        private List<DialogueData> possibleCorrectInnocentReactions = new List<DialogueData>();

        public List<DialogueData> PossibleIncorrectInnocentReactions => possibleIncorrectInnocentReactions;
        [SerializeField]
        private List<DialogueData> possibleIncorrectInnocentReactions = new List<DialogueData>();

        public void SetName()
        {
            currentName = possibleNames[UnityEngine.Random.Range(0, possibleNames.Count)];
        }

        public void AddToReactionPool(List<DialogueData> inGreetings, List<DialogueData> inCorrectGuilties, List<DialogueData> inIncorrectGuilties, List<DialogueData> inCorrectInnocents, List<DialogueData> inIncorrectInnocents)
        {
            possibleGreetings.AddRange(inGreetings);
            possibleCorrectGuiltyReactions.AddRange(inCorrectGuilties);
            possibleIncorrectGuiltyReactions.AddRange(inIncorrectGuilties);
            possibleCorrectInnocentReactions.AddRange(inCorrectInnocents);
            possibleIncorrectInnocentReactions.AddRange(inIncorrectInnocents);
        }
    }
}
