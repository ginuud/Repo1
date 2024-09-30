using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Game
    {
        public int Id { get; set; }
        public List<Person> Participants { get; set; } = new List<Person>();
    }
}