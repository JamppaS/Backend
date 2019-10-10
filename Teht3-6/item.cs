using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gamewebapi
{
    public class Item : IValidatableObject
    {
        
        public Guid Id { get; set; }

        [Range(1,99)]
        public int Level {get; set;}
        public int Price { get; set; }
        
        public ItemType itemType { get; set; }
        
        
        public DateTime CreationTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(CreationTime>=DateTime.Now){
                yield return new ValidationResult(
                    "CreationDate is in the past"
                );
            }
             if(!(itemType.Equals(ItemType.HealthKit)|| itemType.Equals(ItemType.Relic) || itemType.Equals(ItemType.Weapon) || itemType.Equals(ItemType.Sword))){
                yield return new ValidationResult(
                    "Item type does not match parameters"
                );
            }
        }
    }
}