using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    internal static class ServiceLocator // on pourra appeler les méthodes directement
    {
        private static Dictionary<Type, object> listServices = new Dictionary<Type, object>(); // permet d'avoir une liste avec 2 colonnes : Keys (type) / Values (objet)
        public static void RegisterService<T>(T service)
        {
            listServices[typeof(T)] = service; // on rajoute notre service à notre liste de service

        }
        public static T GetService<T>()
        {
            return (T)listServices[typeof(T)];   // on récupère le service dont on a besoin
        }
    }
}
