using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pokemon.Models
{
    public class Entrenador
    {
        [Key]
        public int Id { get; set; }

        public int Id_Pokemon { get; set; }

        public int Id_Enemy { get; set; }

        [ForeignKey("Id_Pokemon")]
        public virtual Pokemones Pokemon { get; set; }

        [ForeignKey("Id_Enemy")]
        public virtual Pokemones Enemy { get; set; }
    }
}