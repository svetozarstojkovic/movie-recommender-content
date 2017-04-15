using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi
{
    class DataModel
    {
        public List<int> id { get; set; }
        public List<string> name { get; set; }
        public List<int> year { get; set; }
        public List<string> genres { get; set; }
        public List<string> tags { get; set; }
        public static List<string> odgledani = new List<string>();

        public DataModel()
        {
            this.id = new List<int>();
            this.name = new List<string>();
            this.year = new List<int>();
            this.genres = new List<string>();
            this.tags = new List<string>();
        }
    }
}
