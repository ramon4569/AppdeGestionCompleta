using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppdeGestionCompleta
{
    #region Ejercicio 2: Interfaces y Clases de Cursos (IEvaluable, Curso, Matricula)

    public interface IEvaluable
    {
        void AgregarCalificacion(decimal calificacion);
        decimal ObtenerPromedio();
        bool HaAprobado();
    }

    public class Curso : IIdentificable
    {
        [Requerido][Formato(@"^[A-Z]{2,3}-\d{3}$")] public string Codigo { get; set; }
        [Requerido] public string Nombre { get; set; }
        [ValidacionRango(1, 6)] public int Creditos { get; set; }
        public Profesor ProfesorAsignado { get; set; }
        public string Identificacion => Codigo;
    }

    public class Matricula : IEvaluable
    {
        public Estudiante Estudiante { get; set; }
        public Curso Curso { get; set; }
        private List<decimal> Calificaciones = new List<decimal>();
        private const decimal NotaMinimaAprobatoria = 7.0m;
        public Matricula(Estudiante est, Curso cur) { Estudiante = est; Curso = cur; }
        public void AgregarCalificacion(decimal calificacion)
        {
            if (calificacion < 0 || calificacion > 10) throw new ArgumentException("Calificación debe estar entre 0 y 10.");
            Calificaciones.Add(calificacion);
        }
        public decimal ObtenerPromedio() => Calificaciones.Count == 0 ? 0 : Calificaciones.Average();
        public bool HaAprobado() => ObtenerPromedio() >= NotaMinimaAprobatoria;
        public string ObtenerEstado() => Calificaciones.Count == 0 ? "En Curso" : (HaAprobado() ? "Aprobado" : "Reprobado");
    }
    #endregion
}
