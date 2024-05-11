using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    public class CaseData
    {
        public AnimalData CurrentAnimal => currentAnimal;
        private AnimalData currentAnimal;

        public CrimeData CurrentCrime => currentCrime;
        private CrimeData currentCrime;

        public CaseData(int dayIndex, AnimalData inAnimal, CrimeData inCrime)
        {
            //Initialize the details of the case

            //What animal is it?
            //Randomly choose an animal
            currentAnimal = inAnimal;

            //What crime is it?
            currentCrime = inCrime;
        }
    }
}
