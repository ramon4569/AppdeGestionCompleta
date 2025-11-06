using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppdeGestionCompleta
{
    #region Ejercicio 4 y 7: Gestor Central y LINQ (GestorMatriculas)

    public partial class GestorMatriculas
    {
        private List<Matricula> _listaGeneralMatriculas = new List<Matricula>();
        private readonly Repositorio<Estudiante> _repoEstudiantes;
        private readonly Repositorio<Profesor> _repoProfesores;
        private readonly Repositorio<Curso> _repoCursos;

        public GestorMatriculas(Repositorio<Estudiante> rE, Repositorio<Profesor> rP, Repositorio<Curso> rC)
        {
            _repoEstudiantes = rE; _repoProfesores = rP; _repoCursos = rC;
        }

        public void MatricularEstudiante(Estudiante est, Curso curso)
        {
            bool yaExiste = _listaGeneralMatriculas.Any(m => m.Estudiante.Identificacion == est.Identificacion && m.Curso.Codigo == curso.Codigo);
            if (yaExiste) throw new InvalidOperationException("Estudiante ya matriculado.");
            _listaGeneralMatriculas.Add(new Matricula(est, curso));
            Logger.Log($"Estudiante {est.Identificacion} matriculado en {curso.Codigo}."); // <-- BONUS LOG
        }

        public void MatricularEstudiante(string idEstudiante, string codigoCurso)
        {
            var est = _repoEstudiantes.BuscarPorId(idEstudiante);
            if (est == null) throw new KeyNotFoundException("Estudiante no encontrado.");
            var curso = _repoCursos.BuscarPorId(codigoCurso);
            if (curso == null) throw new KeyNotFoundException("Curso no encontrado.");
            MatricularEstudiante(est, curso);
        }

        public void AgregarCalificacion(string idEstudiante, string codigoCurso, decimal calificacion)
        {
            var matricula = _listaGeneralMatriculas.FirstOrDefault(m => m.Estudiante.Identificacion == idEstudiante && m.Curso.Codigo == codigoCurso);
            if (matricula == null) throw new KeyNotFoundException("Matrícula no encontrada.");
            matricula.AgregarCalificacion(calificacion);
            Logger.Log($"Calificación {calificacion} agregada a {idEstudiante} en {codigoCurso}."); // <-- BONUS LOG
        }

        public string GenerarReporteEstudiante(string idEstudiante)
        {
            var matriculas = _listaGeneralMatriculas.Where(m => m.Estudiante.Identificacion == idEstudiante);
            if (!matriculas.Any()) throw new KeyNotFoundException("Estudiante no encontrado o sin matrículas.");

            var est = matriculas.First().Estudiante;
            var reporte = new StringBuilder();
            reporte.AppendLine($"--- REPORTE ACADÉMICO: {est.Nombre} {est.Apellido} ---");
            foreach (var m in matriculas)
            {
                reporte.AppendLine($"  > Curso: {m.Curso.Nombre} | Promedio: {m.ObtenerPromedio():F2} | Estado: {m.ObtenerEstado()}");
            }
            return reporte.ToString();
        }
    }

    public partial class GestorMatriculas
    {
        public IEnumerable<dynamic> ObtenerTop10Estudiantes()
        {
            return _listaGeneralMatriculas.GroupBy(m => m.Estudiante)
                .Select(g => new { Estudiante = g.Key, PromedioGeneral = g.Average(m => m.ObtenerPromedio()) })
                .OrderByDescending(x => x.PromedioGeneral).Take(10);
        }

        public IEnumerable<Estudiante> ObtenerEstudiantesEnRiesgo()
        {
            return _listaGeneralMatriculas.GroupBy(m => m.Estudiante)
                .Where(g => g.Average(m => m.ObtenerPromedio()) < 7.0m)
                .Select(g => g.Key);
        }

        public IEnumerable<dynamic> ObtenerCursosMasPopulares()
        {
            return _listaGeneralMatriculas.GroupBy(m => m.Curso.Nombre)
                .Select(g => new { Curso = g.Key, CantidadEstudiantes = g.Count() })
                .OrderByDescending(x => x.CantidadEstudiantes);
        }

        public decimal ObtenerPromedioGeneral()
        {
            var promedios = _listaGeneralMatriculas.GroupBy(m => m.Estudiante)
                               .Select(g => g.Average(m => m.ObtenerPromedio()));
            return promedios.Any() ? promedios.Average() : 0;
        }

        public string ObtenerEstadisticasPorCarrera()
        {
            var reporte = new StringBuilder();
            reporte.AppendLine("--- Estadísticas por Carrera ---");
            var grupos = _listaGeneralMatriculas.GroupBy(m => m.Estudiante.Carrera);
            foreach (var g in grupos)
            {
                var estUnicos = g.Select(m => m.Estudiante).Distinct().Count();
                var promCarrera = g.GroupBy(m => m.Estudiante).Select(gEst => gEst.Average(m => m.ObtenerPromedio())).Average();
                reporte.AppendLine($"   > {g.Key}: {estUnicos} Estudiantes | Promedio General: {promCarrera:F2}");
            }
            return reporte.ToString();
        }

        public IEnumerable<Estudiante> BuscarEstudiantes(Func<Estudiante, bool> criterio)
        {
            return _repoEstudiantes.Buscar(criterio);
        }
    }
    #endregion
}
