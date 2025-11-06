using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppdeGestionCompleta
{
    // =====================================================================
    // =====================================================================
    #region BONUS: CLASES DE SOPORTE (Logger, Reportes)
    // =====================================================================
    // =====================================================================

    /// <summary>
    /// BONUS: Clase estática simple para registrar eventos del sistema.
    /// </summary>
    public static class Logger
    {
        private static readonly string _logFile = "sistema_academico.log";

        public static void Log(string mensaje, string nivel = "INFO")
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nivel.ToUpper()}] - {mensaje}{Environment.NewLine}";
                File.AppendAllText(_logFile, logEntry);
            }
            catch (Exception)
            {
                // No detener la aplicación si falla el log
            }
        }
    }

    /// <summary>
    /// BONUS: Clase estática para generar reportes en formato de tabla de texto.
    /// </summary>
    public static class GeneradorReportes
    {
        public static string GenerarReporteEstudiantes(IEnumerable<Estudiante> estudiantes)
        {
            if (!estudiantes.Any()) return "No hay estudiantes para mostrar.";

            var sb = new StringBuilder();
            int idWidth = 12;
            int nameWidth = 25;
            int carreraWidth = 20;
            int matriculaWidth = 15;
            string separator = new string('-', idWidth + nameWidth + carreraWidth + matriculaWidth + 10);

            // Encabezado
            sb.AppendLine(separator);
            sb.AppendLine($"| {"ID".PadRight(idWidth)} | {"Nombre Completo".PadRight(nameWidth)} | {"Carrera".PadRight(carreraWidth)} | {"Matrícula".PadRight(matriculaWidth)} |");
            sb.AppendLine(separator);

            // Filas
            foreach (var est in estudiantes)
            {
                string nombreCompleto = $"{est.Nombre} {est.Apellido}";
                sb.AppendLine($"| {est.Identificacion.PadRight(idWidth)} | {nombreCompleto.PadRight(nameWidth)} | {est.Carrera.PadRight(carreraWidth)} | {est.NumeroMatricula.PadRight(matriculaWidth)} |");
            }
            sb.AppendLine(separator);
            return sb.ToString();
        }

        public static string GenerarReporteCursos(IEnumerable<Curso> cursos)
        {
            if (!cursos.Any()) return "No hay cursos para mostrar.";

            var sb = new StringBuilder();
            int colCod = 10;
            int colNom = 25;
            int colCred = 10;
            int colProf = 20;
            string separator = new string('-', colCod + colNom + colCred + colProf + 10);

            sb.AppendLine(separator);
            sb.AppendLine($"| {"Código".PadRight(colCod)} | {"Nombre".PadRight(colNom)} | {"Créditos".PadRight(colCred)} | {"Profesor".PadRight(colProf)} |");
            sb.AppendLine(separator);

            foreach (var curso in cursos)
            {
                string prof = curso.ProfesorAsignado?.Nombre ?? "N/A";
                sb.AppendLine($"| {curso.Codigo.PadRight(colCod)} | {curso.Nombre.PadRight(colNom)} | {curso.Creditos.ToString().PadRight(colCred)} | {prof.PadRight(colProf)} |");
            }
            sb.AppendLine(separator);
            return sb.ToString();
        }

        public static string GenerarReporteProfesores(IEnumerable<Profesor> profesores)
        {
            if (!profesores.Any()) return "No hay profesores para mostrar.";

            var sb = new StringBuilder();
            int idWidth = 12;
            int nameWidth = 25;
            int deptoWidth = 20;
            int salarioWidth = 15;
            string separator = new string('-', idWidth + nameWidth + deptoWidth + salarioWidth + 10);

            sb.AppendLine(separator);
            sb.AppendLine($"| {"ID".PadRight(idWidth)} | {"Nombre Completo".PadRight(nameWidth)} | {"Departamento".PadRight(deptoWidth)} | {"Salario".PadRight(salarioWidth)} |");
            sb.AppendLine(separator);

            foreach (var prof in profesores)
            {
                string nombreCompleto = $"{prof.Nombre} {prof.Apellido}";
                // Usamos "C0" para formato de Moneda (ej: $50,000)
                sb.AppendLine($"| {prof.Identificacion.PadRight(idWidth)} | {nombreCompleto.PadRight(nameWidth)} | {prof.Departamento.PadRight(deptoWidth)} | {prof.Salario.ToString("C0").PadRight(salarioWidth)} |");
            }
            sb.AppendLine(separator);
            return sb.ToString();
        }
    }

    #endregion


}
