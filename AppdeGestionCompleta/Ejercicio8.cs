using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppdeGestionCompleta
{

    #region Ejercicio 8: Atributos Personalizados y Validador

    [AttributeUsage(AttributeTargets.Property)]
    public class RequeridoAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidacionRangoAttribute : Attribute
    {
        public double Min { get; }
        public double Max { get; }
        public ValidacionRangoAttribute(double min, double max) { Min = min; Max = max; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FormatoAttribute : Attribute
    {
        public string Pattern { get; }
        public FormatoAttribute(string pattern) { Pattern = pattern; }
    }

    public class Validador
    {
        public List<string> Validar(object instancia)
        {
            var errores = new List<string>();
            Type tipo = instancia.GetType();
            foreach (var prop in tipo.GetProperties())
            {
                object valor = prop.GetValue(instancia);
                foreach (var attr in prop.GetCustomAttributes(true))
                {
                    if (attr is RequeridoAttribute)
                    {
                        if (valor == null || (valor is string s && string.IsNullOrWhiteSpace(s)))
                            errores.Add($"[Requerido] '{prop.Name}' no puede ser nulo o vacío.");
                    }
                    else if (attr is ValidacionRangoAttribute rango)
                    {
                        if (valor != null)
                        {
                            try
                            {
                                double valNum = Convert.ToDouble(valor);
                                if (valNum < rango.Min || valNum > rango.Max)
                                    errores.Add($"[Rango] '{prop.Name}' ({valNum}) fuera de rango ({rango.Min}-{rango.Max}).");
                            }
                            catch (Exception) { /* Ignorar si no es numérico */ }
                        }
                    }
                    else if (attr is FormatoAttribute formato)
                    {
                        if (valor is string s && !string.IsNullOrEmpty(s) && !Regex.IsMatch(s, formato.Pattern))
                            errores.Add($"[Formato] '{prop.Name}' ('{s}') no cumple el formato '{formato.Pattern}'.");
                    }
                }
            }
            return errores;
        }
    }
    #endregion
}
