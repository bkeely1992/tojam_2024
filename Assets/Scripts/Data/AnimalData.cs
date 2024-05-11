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
            dog, lion, goat, owl, penguin, hawk, lizard, turtle, snake, invalid
        }
        public Animal_class Class = Animal_class.invalid;
        public Animal_diet Diet = Animal_diet.invalid;
        public Animal_species Species = Animal_species.invalid;

        public static List<Animal_class> AllAnimalClasses = new List<Animal_class>() { Animal_class.bird, Animal_class.reptile, Animal_class.mammal };
        public static List<Animal_diet> AllAnimalDiets = new List<Animal_diet>() { Animal_diet.herbivore, Animal_diet.carnivore, Animal_diet.omnivore };
        public static List<Animal_species> AllAnimalSpecies = new List<Animal_species>() { Animal_species.dog, Animal_species.lion, Animal_species.goat, Animal_species.owl, Animal_species.penguin, Animal_species.hawk, Animal_species.lizard, Animal_species.turtle, Animal_species.snake };
    }
}
