using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace WMS.DatabaseAccess
{
    /// <summary>
    /// Kontekst
    /// </summary>
    public class SystemContext : IDisposable
    {
        private SystemEntities entities;
        private object syncObj;

        public SystemContext()
        {
            entities = new SystemEntities();
            syncObj = new object();
        }

        /// <summary>
        /// Metoda wykonująca akcje w transakcji.
        /// </summary>
        /// <param name="action">Akcja do wykonania</param>
        public void TransactionSync(Action<TransactionContext> action)
        {
            TransactionSync<bool>(tc => { action(tc); return false; });
        }

        /// <summary>
        /// Metoda wykonująca akcje w transakcji.
        /// </summary>
        /// <typeparam name="T">Typ zwracanej wartości</typeparam>
        /// <param name="action">Akcja do wykonania</param>
        /// <returns></returns>
        public T TransactionSync<T>(Func<TransactionContext, T> action)
        {
            lock (syncObj)
            {
                using (var transaction = new TransactionScope())
                {
                    using (var tc = new TransactionContext(entities))
                    {
                        T r = action(tc);

                        if (!tc.Rollback)
                        {
                            entities.SaveChanges();
                            transaction.Complete();
                        }

                        return r;
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
