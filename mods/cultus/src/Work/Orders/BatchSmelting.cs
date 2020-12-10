using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.ServerMods.NoObf;

namespace cultus
{
    internal class BatchSmelting
    {
        public string ore;
        public int units;

        private Dictionary<string, int> book;

        public BatchSmelting()
        {
            ore = "copper";
            units = 0;
            book = new Dictionary<string, int>();
        }

        public void AddItems(string item, int amount)
        {
            if (book.ContainsKey(item))
            {
                book[item] += amount;
            }
            else
            {
                book.Add(item, amount);
            }

            units = amount * 100;
        }
    }
}