using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    //Data class used for containing important information on the current case
    public class CaseData
    {
        public AnimalData CurrentAnimal => currentAnimal;
        private AnimalData currentAnimal;

        public CrimeData CurrentCrime => currentCrime;
        private CrimeData currentCrime;

        public CaseData(int dayIndex, AnimalData inAnimal, CrimeData inCrime)
        {
            currentAnimal = inAnimal;
            currentCrime = inCrime;
        }
    }
}
