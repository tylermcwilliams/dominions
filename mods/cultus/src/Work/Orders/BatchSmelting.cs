using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.ServerMods.NoObf;

namespace cultus
{
    internal class BatchSmelting
    {
        public Item product;
        public int qty;

        public BlockToolMold mold;

        public int units
        {
            get => qty;
        }

        public BatchSmelting(BlockToolMold mold, int qty = 1)
        {
            this.product = product;
            this.qty = qty;

            this.mold = mold;
        }
    }
}