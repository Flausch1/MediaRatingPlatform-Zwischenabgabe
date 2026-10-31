using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServerDemo.Data
{
    internal class AnimalService
    {
        private IAnimalRepository animalRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            this.animalRepository = animalRepository;
        }

        public bool AddAnimal(Animal animal)
        {
            bool success = animalRepository.Create(animal);
            return success;
        }
    }
}
