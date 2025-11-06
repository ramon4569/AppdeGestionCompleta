using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppdeGestionCompleta
{
    #region Ejercicio 6: Reflection (AnalizadorReflection)

    public class AnalizadorReflection
    {
        public void MostrarPropiedades(Type tipo)
        {
            Console.WriteLine($"\n--- Propiedades Públicas de [{tipo.Name}] ---");
            PropertyInfo[] propiedades = tipo.GetProperties();
            foreach (var prop in propiedades) { Console.WriteLine($"   > {prop.Name} (Tipo: {prop.PropertyType.Name})"); }
        }

        public void MostrarMetodos(Type tipo)
        {
            Console.WriteLine($"\n--- Métodos Públicos de [{tipo.Name}] ---");
            MethodInfo[] metodos = tipo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (metodos.Length == 0) { Console.WriteLine("   > (No se encontraron métodos públicos declarados)"); return; }
            foreach (var met in metodos) { Console.WriteLine($"   > {met.Name}()"); }
        }
    }
    #endregion
}
