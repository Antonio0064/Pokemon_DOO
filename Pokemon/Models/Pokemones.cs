using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pokemon.Models
{
    public class Pokemones
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public int Vida { get; set; }
        public int Ataque { get; set; }
        public int Defensa { get; set; }

        public virtual List<Entrenador> Entrenadores { get; set; }
    }
}