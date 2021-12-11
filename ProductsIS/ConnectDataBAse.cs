using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsIS
{
    class ConnectDataBAse
    {
        private static dataBaseEntities _context;
        public static dataBaseEntities GetContext()
        {
            if (_context == null) _context = new dataBaseEntities();
            return _context;
        }
        public static void ApplyDataBaseChange() => _context?.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
    }
}
