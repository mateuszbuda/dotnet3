using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.DatabaseAccess
{
    /// <summary>
    /// Kontekst transakcji. Definiuje dostęp do encji i możliwość wycofania transakcji.
    /// </summary>
    public class TransactionContext : IDisposable
    {
        public SystemEntities Entities { get; private set; }
        public bool Rollback { get; set; }

        public TransactionContext(SystemEntities entities)
        {
            Rollback = false;
            Entities = entities;
        }

        public void Dispose()
        {
        }
    }
}
