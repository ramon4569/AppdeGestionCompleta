using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppdeGestionCompleta
{
    #region Ejercicio 1: Jerarquía de Clases (Persona, Estudiante, Profesor)

    public abstract class Persona : IIdentificable
    {
        [Requerido] public string Identificacion { get; set; }
        [Requerido] public string Nombre { get; set; }
        [Requerido] public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get { return DateTime.Today.Year - FechaNacimiento.Year; } }
        public Persona(string id, string nom, string ape, DateTime fechaNac)
        { Identificacion = id; Nombre = nom; Apellido = ape; FechaNacimiento = fechaNac; }
        public abstract string ObtenerRol();
        public override string ToString() => $"[{Identificacion}] {Nombre} {Apellido} (Rol: {ObtenerRol()})";
    }

    public class Profesor : Persona
    {
        public string Departamento { get; set; }
        [ValidacionRango(30000, 150000)] public decimal Salario { get; set; }
        public Profesor(string id, string nom, string ape, DateTime fechaNac, string depto)
            : base(id, nom, ape, fechaNac)
        {
            if (Edad < 25) throw new ArgumentException("Profesor debe tener al menos 25 años.");
            Departamento = depto;
        }
        public override string ObtenerRol() => "Profesor";
    }

    public class Estudiante : Persona
    {
        [Requerido] public string Carrera { get; set; }
        [Formato(@"^\d{4}-\d{4}$")] public string NumeroMatricula { get; set; }
        public Estudiante(string id, string nom, string ape, DateTime fechaNac, string carrera, string matricula)
            : base(id, nom, ape, fechaNac)
        {
            if (Edad < 15) throw new ArgumentException("Estudiante debe tener al menos 15 años.");
            Carrera = carrera; NumeroMatricula = matricula;
        }
        public override string ObtenerRol() => "Estudiante";
    }
    #endregion
}
