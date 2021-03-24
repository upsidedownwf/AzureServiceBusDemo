using System;
using System.ComponentModel.DataAnnotations;

namespace AzureServiceBusModels
{
    public class Person
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
