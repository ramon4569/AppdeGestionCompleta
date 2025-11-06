
using System;
using System.Collections.Generic;
using System.Globalization; // Para formatos
using System.IO; // BONUS: Para Archivos (Logs y JSON)
using System.Linq; // Para LINQ
using System.Reflection; // Para Reflection
using System.Text; // Para StringBuilder
using System.Text.Json; // BONUS: Para Serialización
using System.Text.RegularExpressions; // Para Atributo [Formato]
using System.Threading; // BONUS: Para Pausas 

// El namespace principal que envuelve toda la aplicación
namespace AppdeGestionCompleta
{
   
   

    // =====================================================================
    // =====================================================================
    #region EJERCICIO 9: MENÚ INTERACTIVO (FRONTEND 1)
    // =====================================================================
    // =====================================================================

    public static class MenuInteractivo
    {
        private static Repositorio<Estudiante> _repoEstudiantes;
        private static Repositorio<Profesor> _repoProfesores;
        private static Repositorio<Curso> _repoCursos;
        private static GestorMatriculas _gestor;
        private static AnalizadorReflection _analizador;

        public static void Ejecutar(
            Repositorio<Estudiante> repoE,
            Repositorio<Profesor> repoP,
            Repositorio<Curso> repoC,
            GestorMatriculas gestor,
            AnalizadorReflection analizador)
        {
            _repoEstudiantes = repoE;
            _repoProfesores = repoP;
            _repoCursos = repoC;
            _gestor = gestor;
            _analizador = analizador;

            bool continuar = true;
            while (continuar)
            {
                MostrarMenuPrincipal();
                // BONUS: Se agregó la opción 9
                int opcion = LeerOpcion(1, 9);

                Console.Clear();
                switch (opcion)
                {
                    case 1: MostrarMenuGestion("Estudiantes"); break;
                    case 2: MostrarMenuGestion("Profesores"); break;
                    case 3: MostrarMenuGestion("Cursos"); break;
                    case 4: MatricularEstudiante(); break;
                    case 5: RegistrarCalificaciones(); break;
                    case 6: MostrarMenuReportes(); break;
                    case 7: MostrarMenuReflection(); break;
                    case 8:
                        Ejercicio9y10yBonusRestantes.GuardarDatos(); // <-- BONUS
                        continuar = false;
                        break;
                    case 9: continuar = false; break; // Salir sin guardar
                }
            }
            MostrarMensaje("Saliendo del Menú Interactivo...", ConsoleColor.Cyan);
        }

        #region Menús (Ej. 9)

        private static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=============================================");
            Console.WriteLine("    SISTEMA DE GESTIÓN ACADÉMICA (MENÚ MANUAL)");
            Console.WriteLine("=============================================");
            Console.ResetColor();
            Console.WriteLine(" 1. Gestionar Estudiantes");
            Console.WriteLine(" 2. Gestionar Profesores");
            Console.WriteLine(" 3. Gestionar Cursos");
            Console.WriteLine(" 4. Matricular Estudiante en Curso");
            Console.WriteLine(" 5. Registrar Calificaciones");
            Console.WriteLine(" 6. Ver Reportes");
            Console.WriteLine(" 7. Análisis con Reflection");
            Console.WriteLine(" 8. Guardar y Salir"); // <-- BONUS
            Console.WriteLine(" 9. Salir (Sin Guardar)"); // <-- BONUS
            Console.WriteLine("---------------------------------------------");
        }

        private static void MostrarMenuGestion(string entidad)
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"--- GESTIÓN DE {entidad.ToUpper()} ---");
                Console.ResetColor();
                Console.WriteLine(" 1. Agregar");
                Console.WriteLine(" 2. Listar Todos");
                Console.WriteLine(" 3. Buscar por ID");
                if (entidad == "Cursos")
                    Console.WriteLine(" 4. Asignar Profesor a Curso");
                else
                    Console.WriteLine(" 4. Modificar (No implementado)");

                Console.WriteLine(" 5. Eliminar");
                Console.WriteLine(" 6. Volver al Menú Principal");
                Console.WriteLine("---------------------------------");

                int opcion = LeerOpcion(1, 6);
                try
                {
                    switch (entidad)
                    {
                        case "Estudiantes": ManejarOpcionEstudiante(opcion, ref volver); break;
                        case "Profesores": ManejarOpcionProfesor(opcion, ref volver); break;
                        case "Cursos": ManejarOpcionCurso(opcion, ref volver); break;
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error Inesperado: {ex.Message}");
                    Logger.Log($"Error en MenuGestion: {ex.Message}", "ERROR"); // <-- BONUS LOG
                }
                if (!volver) Pausar();
            }
        }

        private static void MostrarMenuReportes()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--- MENÚ DE REPORTES ---");
                Console.ResetColor();
                Console.WriteLine(" 1. Top 10 Estudiantes (Mejor Promedio)");
                Console.WriteLine(" 2. Estudiantes en Riesgo (Promedio < 7.0)");
                Console.WriteLine(" 3. Cursos Más Populares (Inscritos)");
                Console.WriteLine(" 4. Promedio General de la Institución");
                Console.WriteLine(" 5. Estadísticas por Carrera");
                Console.WriteLine(" 6. Generar Reporte de Estudiante (por ID)");
                Console.WriteLine(" 7. Volver al Menú Principal");
                Console.WriteLine("---------------------------------");

                int opcion = LeerOpcion(1, 7);
                Console.Clear();
                try
                {
                    switch (opcion)
                    {
                        case 1: ReporteTop10(); break;
                        case 2: ReporteEstudiantesEnRiesgo(); break;
                        case 3: ReporteCursosPopulares(); break;
                        case 4: ReportePromedioGeneral(); break;
                        case 5: ReporteEstadisticasCarrera(); break;
                        case 6: ReporteIndividualEstudiante(); break;
                        case 7: volver = true; break;
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al generar reporte: {ex.Message}");
                    Logger.Log($"Error en Reportes: {ex.Message}", "ERROR"); // <-- BONUS LOG
                }
                if (!volver) Pausar();
            }
        }

        private static void MostrarMenuReflection()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--- ANÁLISIS CON REFLECTION ---");
                Console.ResetColor();
                Console.WriteLine(" 1. Analizar Clase Estudiante");
                Console.WriteLine(" 2. Analizar Clase Profesor");
                Console.WriteLine(" 3. Analizar Clase Curso");
                Console.WriteLine(" 4. Volver al Menú Principal");
                Console.WriteLine("---------------------------------");

                int opcion = LeerOpcion(1, 4);
                Console.Clear();
                switch (opcion)
                {
                    case 1:
                        _analizador.MostrarPropiedades(typeof(Estudiante));
                        _analizador.MostrarMetodos(typeof(Estudiante));
                        break;
                    case 2:
                        _analizador.MostrarPropiedades(typeof(Profesor));
                        _analizador.MostrarMetodos(typeof(Profesor));
                        break;
                    case 3:
                        _analizador.MostrarPropiedades(typeof(Curso));
                        _analizador.MostrarMetodos(typeof(Curso));
                        break;
                    case 4: volver = true; break;
                }
                if (!volver) Pausar();
            }
        }

        #endregion

        #region Lógica de Opciones (CRUD y Gestor - Ej. 9)

        // --- MANEJO DE ESTUDIANTES ---
        private static void ManejarOpcionEstudiante(int opcion, ref bool volver)
        { /* ... (código idéntico al de la respuesta anterior) ... */
            switch (opcion)
            {
                case 1: AgregarEstudiante(); break;
                case 2: ListarEstudiantes(); break; // <-- MODIFICADO
                case 3: BuscarEstudiante(); break;
                case 4: MostrarError("Función 'Modificar Estudiante' no implementada."); break;
                case 5: EliminarEstudiante(); break;
                case 6: volver = true; break;
            }
        }
        private static void AgregarEstudiante()
        {
            try
            {
                MostrarTitulo("Agregar Nuevo Estudiante");
                string id = LeerTextoRequerido("Identificación (ID): ");
                string nombre = LeerTextoRequerido("Nombre: ");
                string apellido = LeerTextoRequerido("Apellido: ");
                string carrera = LeerTextoRequerido("Carrera: ");
                string matricula = LeerTextoRequerido("Matrícula (Ej: 2024-0001): ");
                DateTime fechaNac = LeerFecha("Fecha de Nacimiento (yyyy-MM-dd): ");

                Estudiante est = new Estudiante(id, nombre, apellido, fechaNac, carrera, matricula);
                _repoEstudiantes.Agregar(est);
                MostrarExito("¡Estudiante agregado con éxito!");
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
                Logger.Log($"Error al agregar estudiante: {ex.Message}", "ERROR");
            }
        }
        private static void ListarEstudiantes()
        {
            MostrarTitulo("Listado de Estudiantes");
            var todos = _repoEstudiantes.ObtenerTodos();

            // --- BONUS: Llamada al Generador de Reportes ---
            string reporte = GeneradorReportes.GenerarReporteEstudiantes(todos);
            Console.WriteLine(reporte);
            // --- Fin del Bonus ---
        }
        private static void BuscarEstudiante()
        { /* ... (código idéntico al de la respuesta anterior) ... */
            MostrarTitulo("Buscar Estudiante por ID");
            string id = LeerTextoRequerido("Ingrese el ID del estudiante: ");
            var est = _repoEstudiantes.BuscarPorId(id);
            if (est == null) MostrarError("Estudiante no encontrado.");
            else { MostrarExito("Estudiante Encontrado:"); Console.WriteLine(est.ToString()); }
        }
        private static void EliminarEstudiante()
        { /* ... (código idéntico al de la respuesta anterior) ... */
            MostrarTitulo("Eliminar Estudiante");
            string id = LeerTextoRequerido("Ingrese el ID del estudiante a eliminar: ");
            if (_repoEstudiantes.Eliminar(id)) MostrarExito("Estudiante eliminado con éxito.");
            else MostrarError("No se encontró un estudiante con ese ID.");
        }

        // --- MANEJO DE PROFESORES ---
        private static void ManejarOpcionProfesor(int opcion, ref bool volver)
        { /* ... (código idéntico al de la respuesta anterior) ... */
            MostrarError("Funcionalidad de Profesores no implementada en este ejemplo.");
            Pausar();
            volver = true;
        }

        // --- MANEJO DE CURSOS ---
        private static void ManejarOpcionCurso(int opcion, ref bool volver)
        { /* ... (código idéntico al de la respuesta anterior) ... */
            switch (opcion)
            {
                case 1: AgregarCurso(); break;
                case 2: ListarCursos(); break; // <-- MODIFICADO
                case 3: BuscarCurso(); break;
                case 4: AsignarProfesorACurso(); break;
                case 5: EliminarCurso(); break;
                case 6: volver = true; break;
            }
        }
        private static void AgregarCurso()
        { /* ... (código idéntico al de la respuesta anterior) ... */
            try
            {
                MostrarTitulo("Agregar Nuevo Curso");
                string codigo = LeerTextoRequerido("Código (Ej: CS-101): ");
                string nombre = LeerTextoRequerido("Nombre del Curso: ");
                int creditos = (int)LeerDecimal("Créditos: ", 1, 10);
                Curso curso = new Curso { Codigo = codigo, Nombre = nombre, Creditos = creditos };
                _repoCursos.Agregar(curso);
                MostrarExito("¡Curso agregado con éxito!");
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
                Logger.Log($"Error al agregar curso: {ex.Message}", "ERROR");
            }
        }
        private static void ListarCursos()
        {
            MostrarTitulo("Listado de Cursos");
            var todos = _repoCursos.ObtenerTodos();

            // --- BONUS: Llamada al Generador de Reportes ---
            string reporte = GeneradorReportes.GenerarReporteCursos(todos);
            Console.WriteLine(reporte);
            // --- Fin del Bonus ---
        }
        private static void BuscarCurso()
        { /* ... (código idéntico al de la respuesta anterior) ... */
            MostrarTitulo("Buscar Curso por Código");
            string codigo = LeerTextoRequerido("Ingrese el Código del curso: ");
            var curso = _repoCursos.BuscarPorId(codigo);
            if (curso == null) MostrarError("Curso no encontrado.");
            else
            {
                MostrarExito("Curso Encontrado:");
                string prof = curso.ProfesorAsignado != null ? curso.ProfesorAsignado.Nombre : "Sin Asignar";
                Console.WriteLine($"[{curso.Codigo}] {curso.Nombre} ({curso.Creditos} créd.) - Prof: {prof}");
            }
        }
        private static void AsignarProfesorACurso()
        { /* ... (código idéntico al de la respuesta anterior) ... */
            MostrarTitulo("Asignar Profesor a Curso");
            string codigoCurso = LeerTextoRequerido("Código del Curso: ");
            var curso = _repoCursos.BuscarPorId(codigoCurso);
            if (curso == null) { MostrarError("Curso no encontrado."); return; }
            string idProfesor = LeerTextoRequerido("ID del Profesor: ");
            var prof = _repoProfesores.BuscarPorId(idProfesor);
            if (prof == null) { MostrarError("Profesor no encontrado."); return; }
            curso.ProfesorAsignado = prof;
            MostrarExito($"Profesor {prof.Nombre} asignado a {curso.Nombre}.");
            Logger.Log($"Profesor {prof.Identificacion} asignado a curso {curso.Codigo}.");
        }
        private static void EliminarCurso()
        { /* ... (código idéntico al de la respuesta anterior) ... */
            MostrarTitulo("Eliminar Curso");
            string codigo = LeerTextoRequerido("Ingrese el Código del curso a eliminar: ");
            if (_repoCursos.Eliminar(codigo)) MostrarExito("Curso eliminado con éxito.");
            else MostrarError("No se encontró un curso con ese código.");
        }

        // --- MANEJO DE MATRÍCULA Y CALIFICACIONES ---
        private static void MatricularEstudiante()
        { /* ... (código idéntico al de la respuesta anterior) ... */
            MostrarTitulo("Matricular Estudiante en Curso");
            try
            {
                string idEstudiante = LeerTextoRequerido("ID del Estudiante: ");
                string codigoCurso = LeerTextoRequerido("Código del Curso: ");
                _gestor.MatricularEstudiante(idEstudiante, codigoCurso);
                MostrarExito("Matrícula procesada.");
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
                Logger.Log($"Error en Matricular: {ex.Message}", "ERROR");
            }
        }
        private static void RegistrarCalificaciones()
        { /* ... (código idéntico al de la respuesta anterior) ... */
            MostrarTitulo("Registrar Calificación");
            try
            {
                string idEstudiante = LeerTextoRequerido("ID del Estudiante: ");
                string codigoCurso = LeerTextoRequerido("Código del Curso: ");
                decimal calificacion = LeerDecimal("Calificación (0-10): ", 0, 10);
                _gestor.AgregarCalificacion(idEstudiante, codigoCurso, calificacion);
                MostrarExito("Calificación registrada con éxito.");
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
                Logger.Log($"Error en Calificar: {ex.Message}", "ERROR");
            }
        }

        #endregion

        #region Lógica de Reportes (LINQ - Ej. 9)
        // (Esta sección no necesita cambios, solo se copian los métodos)

        private static void ReporteTop10()
        {
            MostrarTitulo("Reporte: Top 10 Estudiantes");
            var top10 = _gestor.ObtenerTop10Estudiantes();
            if (!top10.Any()) { MostrarMensaje("No hay datos para mostrar.", ConsoleColor.Yellow); return; }
            int i = 1;
            foreach (var item in top10)
            {
                Console.WriteLine($" #{i++}. {item.Estudiante.Nombre} {item.Estudiante.Apellido} - Prom: {item.PromedioGeneral:F2}");
            }
        }
        private static void ReporteEstudiantesEnRiesgo()
        {
            MostrarTitulo("Reporte: Estudiantes en Riesgo (Promedio < 7.0)");
            var enRiesgo = _gestor.ObtenerEstudiantesEnRiesgo();
            if (!enRiesgo.Any()) { MostrarMensaje("¡Buenas noticias! No hay estudiantes en riesgo.", ConsoleColor.Green); return; }
            foreach (var est in enRiesgo)
            {
                Console.WriteLine($" - {est.Nombre} {est.Apellido} (ID: {est.Identificacion})");
            }
        }
        private static void ReporteCursosPopulares()
        {
            MostrarTitulo("Reporte: Cursos Más Populares (por Inscritos)");
            var populares = _gestor.ObtenerCursosMasPopulares();
            if (!populares.Any()) { MostrarMensaje("No hay cursos con inscripciones.", ConsoleColor.Yellow); return; }
            foreach (var item in populares)
            {
                Console.WriteLine($" - {item.Curso}: {item.CantidadEstudiantes} Estudiante(s)");
            }
        }
        private static void ReportePromedioGeneral()
        {
            MostrarTitulo("Reporte: Promedio General de la Institución");
            decimal promGeneral = _gestor.ObtenerPromedioGeneral();
            MostrarExito($"El promedio general de todos los estudiantes es: {promGeneral:F2}");
        }
        private static void ReporteEstadisticasCarrera()
        {
            MostrarTitulo("Reporte: Estadísticas por Carrera");
            string reporte = _gestor.ObtenerEstadisticasPorCarrera();
            Console.WriteLine(reporte);
        }
        private static void ReporteIndividualEstudiante()
        {
            MostrarTitulo("Reporte Individual de Estudiante");
            try
            {
                string idEstudiante = LeerTextoRequerido("ID del Estudiante: ");
                string reporte = _gestor.GenerarReporteEstudiante(idEstudiante);
                Console.WriteLine(reporte);
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
                Logger.Log($"Error en Reporte Individual: {ex.Message}", "ERROR");
            }
        }

        #endregion

        #region Helpers de UI (Entrada y Colores - Ej. 9)

        private static int LeerOpcion(int min, int max)
        {
            int opcion;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Seleccione una opción: ");
                Console.ResetColor();
                string input = Console.ReadLine();
                if (int.TryParse(input, out opcion) && opcion >= min && opcion <= max)
                {
                    return opcion;
                }
                else
                {
                    MostrarError($"Opción inválida. Debe ingresar un número entre {min} y {max}.");
                }
            }
        }
        private static string LeerTextoRequerido(string mensaje)
        {
            string input;
            while (true)
            {
                Console.Write($"> {mensaje} ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                else
                {
                    MostrarError("Este campo es requerido. No puede estar vacío.");
                }
            }
        }
        private static DateTime LeerFecha(string mensaje)
        {
            DateTime fecha;
            while (true)
            {
                string input = LeerTextoRequerido(mensaje);
                if (DateTime.TryParse(input, out fecha))
                {
                    return fecha;
                }
                else
                {
                    MostrarError("Formato de fecha inválido. Use yyyy-MM-dd.");
                }
            }
        }
        private static decimal LeerDecimal(string mensaje, decimal min, decimal max)
        {
            decimal valor;
            while (true)
            {
                string input = LeerTextoRequerido(mensaje);
                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out valor)
                    && valor >= min && valor <= max)
                {
                    return valor;
                }
                else
                {
                    MostrarError($"Valor inválido. Debe ser un número entre {min} y {max}.");
                }
            }
        }
        private static void Pausar()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPresione Enter para continuar...");
            Console.ResetColor();
            Console.ReadLine();
        }
        private static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERROR] {mensaje}\n");
            Console.ResetColor();
        }
        private static void MostrarExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[ÉXITO] {mensaje}\n");
            Console.ResetColor();
        }
        private static void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n{mensaje}\n");
            Console.ResetColor();
        }
        private static void MostrarTitulo(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- {titulo.ToUpper()} ---");
            Console.ResetColor();
        }

        #endregion
    }

    #endregion

    // =====================================================================
    // =====================================================================
    #region EJERCICIO 10: DEMO AUTOMATIZADA (FRONTEND 2)
    // =====================================================================
    // =====================================================================

    public static class DemoAutomatizada
    {
        // CORREGIDO: Quitada la palabra 'readonly'
        private static Repositorio<Estudiante> _repoEstudiantes;
        private static Repositorio<Profesor> _repoProfesores;
        private static Repositorio<Curso> _repoCursos;
        private static GestorMatriculas _gestor;
        private static Validador _validador;
        private static AnalizadorReflection _analizador;
        private static Random _rand;

        public static void Ejecutar(
            Repositorio<Estudiante> repoE,
            Repositorio<Profesor> repoP,
            Repositorio<Curso> repoC,
            GestorMatriculas gestor,
            Validador validador,
            AnalizadorReflection analizador,
            Random rand)
        {
            _repoEstudiantes = repoE;
            _repoProfesores = repoP;
            _repoCursos = repoC;
            _gestor = gestor;
            _validador = validador;
            _analizador = analizador;
            _rand = rand;

            // 1. Poblar el sistema (solo si está vacío)
            if (!_repoEstudiantes.ObtenerTodos().Any())
            {
                GenerarDatosPrueba();
            }
            else
            {
                Titulo("Datos ya cargados. Omitiendo generación...", ConsoleColor.Yellow);
                Pausar();
            }

            // 2. Ejecutar la demostración
            DemostrarFuncionalidades();
        }

        #region Generador de Datos (Ej. 10)

        public static void GenerarDatosPrueba()
        {
            Titulo("Generando Datos de Prueba...", ConsoleColor.Yellow);
            try
            {
                var p1 = new Profesor("P-101", "Carlos", "Perez", new DateTime(1980, 1, 1), "Ingeniería");
                var p2 = new Profesor("P-102", "Maria", "Lopez", new DateTime(1985, 2, 2), "Derecho");
                var p3 = new Profesor("P-103", "Juan", "Castro", new DateTime(1975, 3, 3), "Medicina");
                var p4 = new Profesor("P-104", "Lucia", "Gomez", new DateTime(1988, 4, 4), "Arquitectura");
                var p5 = new Profesor("P-105", "Pedro", "Martinez", new DateTime(1990, 5, 5), "Humanidades");
                _repoProfesores.Agregar(p1); _repoProfesores.Agregar(p2); _repoProfesores.Agregar(p3); _repoProfesores.Agregar(p4); _repoProfesores.Agregar(p5);
                Console.WriteLine(" > 5 Profesores agregados.");

                var c1 = new Curso { Codigo = "CS-101", Nombre = "Programación I", Creditos = 4, ProfesorAsignado = p1 };
                var c2 = new Curso { Codigo = "MAT-101", Nombre = "Cálculo I", Creditos = 5, ProfesorAsignado = p1 };
                var c3 = new Curso { Codigo = "LAW-101", Nombre = "Derecho Romano", Creditos = 3, ProfesorAsignado = p2 };
                var c4 = new Curso { Codigo = "LAW-102", Nombre = "Derecho Penal", Creditos = 4, ProfesorAsignado = p2 };
                var c5 = new Curso { Codigo = "MED-101", Nombre = "Anatomía I", Creditos = 6, ProfesorAsignado = p3 };
                var c6 = new Curso { Codigo = "MED-102", Nombre = "Fisiología", Creditos = 5, ProfesorAsignado = p3 };
                var c7 = new Curso { Codigo = "ARQ-101", Nombre = "Dibujo Técnico", Creditos = 4, ProfesorAsignado = p4 };
                var c8 = new Curso { Codigo = "ARQ-102", Nombre = "Historia del Arte", Creditos = 3, ProfesorAsignado = p4 };
                var c9 = new Curso { Codigo = "HUM-101", Nombre = "Filosofía", Creditos = 3, ProfesorAsignado = p5 };
                var c10 = new Curso { Codigo = "HUM-102", Nombre = "Literatura", Creditos = 3, ProfesorAsignado = p5 };
                _repoCursos.Agregar(c1); _repoCursos.Agregar(c2); _repoCursos.Agregar(c3); _repoCursos.Agregar(c4); _repoCursos.Agregar(c5);
                _repoCursos.Agregar(c6); _repoCursos.Agregar(c7); _repoCursos.Agregar(c8); _repoCursos.Agregar(c9); _repoCursos.Agregar(c10);
                var listaCursos = _repoCursos.ObtenerTodos().ToList();
                Console.WriteLine(" > 10 Cursos agregados.");

                var estudiantes = new List<Estudiante>
                {
                    new Estudiante("E-001", "Ana", "Gomez", new DateTime(2005, 1, 1), "Ing. de Software", "2024-0001"),
                    new Estudiante("E-002", "Luis", "Rosario", new DateTime(2004, 2, 2), "Derecho", "2023-0002"),
                    new Estudiante("E-003", "Sara", "Peralta", new DateTime(2005, 3, 3), "Medicina", "2024-0003"),
                    new Estudiante("E-004", "Juan", "Mota", new DateTime(2003, 4, 4), "Ing. de Software", "2022-0004"),
                    new Estudiante("E-005", "Carla", "Diaz", new DateTime(2004, 5, 5), "Derecho", "2023-0005"),
                    new Estudiante("E-006", "Pedro", "Velez", new DateTime(2005, 6, 6), "Medicina", "2024-0006"),
                    new Estudiante("E-007", "Laura", "Nuñez", new DateTime(2004, 7, 7), "Ing. de Software", "2023-0007"),
                    new Estudiante("E-008", "Miguel", "Reyes", new DateTime(2003, 8, 8), "Derecho", "2022-0008"),
                    new Estudiante("E-009", "Sofia", "Guzman", new DateTime(2005, 9, 9), "Medicina", "2024-0009"),
                    new Estudiante("E-010", "David", "Gil", new DateTime(2004, 10, 10), "Ing. de Software", "2023-0010"),
                    new Estudiante("E-011", "Elena", "Cruz", new DateTime(2003, 11, 11), "Derecho", "2022-0011"),
                    new Estudiante("E-012", "Mateo", "Solano", new DateTime(2005, 12, 12), "Medicina", "2024-0012"),
                    new Estudiante("E-013", "Valeria", "Pena", new DateTime(2004, 1, 13), "Arquitectura", "2023-0013"),
                    new Estudiante("E-014", "Diego", "Luna", new DateTime(2003, 2, 14), "Arquitectura", "2022-0014"),
                    new Estudiante("E-015", "Camila", "Lara", new DateTime(2005, 3, 15), "Humanidades", "2024-0015")
                };
                foreach (var est in estudiantes) { _repoEstudiantes.Agregar(est); }
                Console.WriteLine(" > 15 Estudiantes agregados.");

                int countMatriculas = 0;
                foreach (var est in estudiantes)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var curso = listaCursos[_rand.Next(listaCursos.Count)];
                        try
                        {
                            _gestor.MatricularEstudiante(est.Identificacion, curso.Codigo);
                            countMatriculas++;
                            int numCalificaciones = _rand.Next(3, 5);
                            for (int j = 0; j < numCalificaciones; j++)
                            {
                                decimal nota = (decimal)(_rand.NextDouble() * 5.0 + 5.0);
                                _gestor.AgregarCalificacion(est.Identificacion, curso.Codigo, Math.Round(nota, 1));
                            }
                        }
                        catch (InvalidOperationException) { /* Ignorar matricula duplicada */ }
                    }
                }
                Console.WriteLine($" > {countMatriculas} Matrículas realizadas.");
                Console.WriteLine($" > Calificaciones agregadas a todas las matrículas.");
                Exito("\n¡DATOS DE PRUEBA GENERADOS EXITOSAMENTE!");
            }
            catch (Exception ex)
            {
                Error($"Error fatal al generar datos: {ex.Message}");
                Logger.Log($"Error al generar datos: {ex.Message}", "FATAL");
            }
            Pausar();
        }
        #endregion

        #region Script de Demostración (Ej. 10)

        public static void DemostrarFuncionalidades()
        {
            // --- 1. DEMOSTRACIÓN DE CONSULTAS LINQ (Ejercicio 7) ---
            Titulo("INICIO DEMO: 1. CONSULTAS CON LINQ", ConsoleColor.Cyan);

            Subtitulo("1.1. Top 10 Estudiantes (Mejor Promedio)");
            var top10 = _gestor.ObtenerTop10Estudiantes();
            foreach (var item in top10) { Console.WriteLine($"   > {item.Estudiante.Nombre} {item.Estudiante.Apellido} - Prom: {item.PromedioGeneral:F2}"); }

            Subtitulo("1.2. Estudiantes en Riesgo (Promedio < 7.0)");
            var enRiesgo = _gestor.ObtenerEstudiantesEnRiesgo();
            if (!enRiesgo.Any()) { Console.WriteLine("   > ¡No hay estudiantes en riesgo!"); }
            foreach (var est in enRiesgo) { Console.WriteLine($"   > {est.Nombre} {est.Apellido}"); }

            Subtitulo("1.3. Cursos Más Populares (Inscritos)");
            var populares = _gestor.ObtenerCursosMasPopulares();
            foreach (var item in populares) { Console.WriteLine($"   > {item.Curso}: {item.CantidadEstudiantes} Estudiante(s)"); }

            Subtitulo("1.4. Promedio General de la Institución");
            Console.WriteLine($"   > Promedio general: {_gestor.ObtenerPromedioGeneral():F2}");

            Subtitulo("1.5. Estadísticas por Carrera");
            Console.WriteLine(_gestor.ObtenerEstadisticasPorCarrera());

            Subtitulo("1.6. Búsqueda Flexible (Lambda: Estudiantes de 'Derecho')");
            var estudiantesDerecho = _gestor.BuscarEstudiantes(e => e.Carrera == "Derecho");
            foreach (var est in estudiantesDerecho) { Console.WriteLine($"   > {est.Nombre}"); }

            Pausar();

            // --- 2. DEMOSTRACIÓN DE ANÁLISIS CON REFLECTION (Ejercicio 6) ---
            Titulo("INICIO DEMO: 2. ANÁLISIS CON REFLECTION", ConsoleColor.Cyan);
            _analizador.MostrarPropiedades(typeof(Estudiante));
            _analizador.MostrarMetodos(typeof(Estudiante));
            _analizador.MostrarPropiedades(typeof(Curso));
            _analizador.MostrarMetodos(typeof(Curso));

            Pausar();

            // --- 3. DEMOSTRACIÓN DE VALIDACIÓN CON ATRIBUTOS (Ejercicio 8) ---
            Titulo("INICIO DEMO: 3. VALIDACIÓN CON ATRIBUTOS", ConsoleColor.Cyan);
            Subtitulo("Creando un 'Estudiante' inválido (Nombre null, Matrícula con formato erróneo)");
            Estudiante estudianteInvalido = new Estudiante(
                "E-INV", null, "Invalido", DateTime.Now.AddYears(-20),
                "Ing. de Software", "MATRICULA-MALA");

            var errores = _validador.Validar(estudianteInvalido);
            if (errores.Any())
            {
                Error("Se encontraron los siguientes errores de validación (¡éxito!):");
                foreach (var error in errores)
                {
                    Console.WriteLine($"   > {error}");
                }
            }
            else
            {
                Error("FALLO: El validador no encontró errores.");
            }

            Pausar();

            // --- 4. DEMOSTRACIÓN DE TIPOS ESPECIALES (Ejercicio 5) ---
            Titulo("INICIO DEMO: 4. TIPOS ESPECIALES (BOXING, CONVERSIONES)", ConsoleColor.Cyan);

            Subtitulo("4.1. ConvertirDatos (Pattern Matching)");
            Console.WriteLine($"   > {ConvertirDatos(101)}");
            Console.WriteLine($"   > {ConvertirDatos(123.456)}");
            Console.WriteLine($"   > {ConvertirDatos("Hola Mundo")}");
            Console.WriteLine($"   > {ConvertirDatos(DateTime.Now)}");

            Subtitulo("4.2. ParsearCalificacion (TryParse Seguro)");
            ParsearCalificacion("8.5");    // Éxito
            ParsearCalificacion("Aprobado"); // Fallo

            Subtitulo("4.3. Demostración Boxing y Unboxing");
            DemostrarBoxingUnboxing();

            Pausar();
        }
        #endregion

        #region Métodos de Demostración (Ejercicio 5)

        public static string ConvertirDatos(object dato)
        {
            switch (dato)
            {
                case int i: return $"[Entero] Valor: {i}";
                case double d: return $"[Doble] Valor: {d:F2}";
                case string s: return $"[Texto] En mayúsculas: {s.ToUpper()}";
                case DateTime dt: return $"[Fecha] Formato largo: {dt.ToString("D", new CultureInfo("es-ES"))}";
                case null: return "[Nulo] El objeto no tiene valor.";
                default: return $"[Desconocido] Tipo: {dato.GetType().Name}";
            }
        }

        public static void ParsearCalificacion(string entrada)
        {
            Console.Write($"   > Intentando parsear '{entrada}': ");
            if (decimal.TryParse(entrada, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal calificacion))
            {
                Exito($"ÉXITO. Calificación: {calificacion:F2}");
            }
            else
            {
                Error($"FALLO. '{entrada}' no es un número válido.");
            }
        }

        public static void DemostrarBoxingUnboxing()
        {
            decimal calificacionOriginal = 9.5m;
            Console.WriteLine($"   1. Valor original (decimal): {calificacionOriginal}");
            object objetoEnCaja = calificacionOriginal; // <-- BOXING
            Console.WriteLine($"   2. Boxing (object): {objetoEnCaja}");
            try
            {
                decimal calificacionExtraida = (decimal)objetoEnCaja; // <-- UNBOXING
                Console.WriteLine($"   3. Unboxing (decimal): {calificacionExtraida}");
                Console.WriteLine("   4. Intentando unboxing erróneo (a 'int')...");
                int unboxingFallido = (int)objetoEnCaja;
            }
            catch (InvalidCastException ex)
            {
                Error($"   ¡FALLO CONTROLADO! {ex.Message}");
            }
        }
        #endregion

        #region Helpers de UI (Formato de Consola)

        private static void Titulo(string texto, ConsoleColor color)
        {
            Console.WriteLine();
            Console.ForegroundColor = color;
            Console.WriteLine("====================================================================");
            Console.WriteLine($"== {texto.ToUpper()}");
            Console.WriteLine("====================================================================");
            Console.ResetColor();
        }
        private static void Subtitulo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n--- {texto} ---");
            Console.ResetColor();
        }
        private static void Pausar()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n(Presione Enter para continuar con la demostración...)");
            Console.ResetColor();
            Console.ReadLine();
            Console.Clear();
        }
        private static void Error(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }
        private static void Exito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        #endregion
    }
    #endregion

    // =====================================================================
    // =====================================================================
    #region EJERCICIO 9 Y 10: LANZADOR PRINCIPAL (MAIN)
    // =====================================================================
    // =====================================================================

    /// <summary>
    /// Esta es la clase principal que contiene el ÚNICO punto de entrada (Main).
    /// Actúa como un "enrutador" para iniciar el Ej. 9 o el Ej. 10.
    /// </summary>
    public class Ejercicio9y10yBonusRestantes
    {
        // --- Almacenes de datos "Backend" GLOBALES (estáticos) ---
        // CORREGIDO: Todos los campos estáticos AHORA SÍ ESTÁN INICIALIZADOS.
        private static readonly Repositorio<Estudiante> _repoEstudiantes = new Repositorio<Estudiante>();
        private static readonly Repositorio<Profesor> _repoProfesores = new Repositorio<Profesor>();
        private static readonly Repositorio<Curso> _repoCursos = new Repositorio<Curso>();
        private static readonly GestorMatriculas _gestor = new GestorMatriculas(_repoEstudiantes, _repoProfesores, _repoCursos);
        private static readonly Validador _validador = new Validador();
        private static readonly AnalizadorReflection _analizador = new AnalizadorReflection();
        private static readonly Random _rand = new Random(123); // Random con semilla para datos consistentes

        /// <summary>
        /// PUNTO DE ENTRADA PRINCIPAL DE TODA LA APLICACIÓN.
        /// </summary>
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Sistema de Gestión Académica - COMPLETO";
            Logger.Log("Inicio de la aplicación."); // <-- BONUS LOG

            // --- BONUS: Autenticación ---
            if (!Autenticar())
            {
                Thread.Sleep(1500);
                Logger.Log("Autenticación fallida. Saliendo.", "WARN");
                return; // Salir de la aplicación si falla el login
            }

            // --- BONUS: Cargar Datos ---
            CargarDatos(); // Cargar datos de JSON si existen

            Console.WriteLine("Bienvenido al Sistema de Gestión Académica.");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===========================================");
            Console.ResetColor();
            Console.WriteLine("¿Qué desea ejecutar?");
            Console.WriteLine(" 1. El Menú Interactivo (Ejercicio 9)");
            Console.WriteLine(" 2. El Script de Demostración Automatizado (Ejercicio 10)");
            Console.WriteLine("-------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Seleccione una opción (1 o 2): ");
            Console.ResetColor();

            string opcion = Console.ReadLine();

            if (opcion == "1")
            {
                // --- Ejecutar Ejercicio 9 ---
                Console.Clear();
                Logger.Log("Iniciando Menú Interactivo (Ej. 9).");
                // Llamamos al método principal del Ej. 9
                MenuInteractivo.Ejecutar(_repoEstudiantes, _repoProfesores, _repoCursos, _gestor, _analizador);
            }
            else if (opcion == "2")
            {
                // --- Ejecutar Ejercicio 10 ---
                Console.Clear();
                Logger.Log("Iniciando Demo Automatizada (Ej. 10).");
                // Llamamos al método principal del Ej. 10
                DemoAutomatizada.Ejecutar(_repoEstudiantes, _repoProfesores, _repoCursos, _gestor, _validador, _analizador, _rand);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opción no válida. Saliendo.");
                Console.ResetColor();
            }

            Logger.Log("Cierre de la aplicación.");
            Console.WriteLine("\nPrograma finalizado. Presione Enter para salir.");
            Console.ReadLine();
        }

        /// <summary>
        /// BONUS: Valida un usuario y contraseña (básico).
        /// </summary>
        private static bool Autenticar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=============================================");
            Console.WriteLine("    SISTEMA DE GESTIÓN ACADÉMICA - LOGIN");
            Console.WriteLine("=============================================");
            Console.ResetColor();

            // Usuarios hardcodeados (user/pass)
            var usuarios = new Dictionary<string, string>
            {
                { "admin", "1234" },
                { "user", "pass" }
            };

            for (int i = 0; i < 3; i++) // 3 intentos
            {
                Console.Write("Usuario: ");
                string user = Console.ReadLine();
                Console.Write("Contraseña: ");
                string pass = Console.ReadLine(); // (En una app real, esto se enmascararía)

                if (usuarios.ContainsKey(user) && usuarios[user] == pass)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n¡Acceso concedido! Bienvenido.");
                    Console.ResetColor();
                    Logger.Log($"Inicio de sesión exitoso para el usuario: {user}");
                    Thread.Sleep(1000); // Pequeña pausa
                    return true;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Credenciales incorrectas. Intentos restantes: {2 - i}");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Demasiados intentos fallidos. Saliendo del sistema.");
            Console.ResetColor();
            Logger.Log("3 intentos de inicio de sesión fallidos.", "WARN");
            return false;
        }

        /// <summary>
        /// BONUS: Guarda el estado actual de los repositorios en archivos JSON.
        /// </summary>
        public static void GuardarDatos()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    // Maneja referencias circulares (ej: Profesor en Curso y Curso en Profesor)
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };

                File.WriteAllText("profesores.json", JsonSerializer.Serialize(_repoProfesores.ObtenerTodos(), options));
                File.WriteAllText("estudiantes.json", JsonSerializer.Serialize(_repoEstudiantes.ObtenerTodos(), options));
                File.WriteAllText("cursos.json", JsonSerializer.Serialize(_repoCursos.ObtenerTodos(), options));

                // Nota: Las matrículas (la lógica del gestor) no se guardan,
                // solo las entidades maestras.

                Logger.Log("Datos guardados en JSON.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Error al guardar JSON: {ex.Message}", "ERROR");
            }
        }

        /// <summary>
        /// BONUS: Carga los datos desde los archivos JSON al iniciar.
        /// </summary>
        private static void CargarDatos()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };

                // Es importante cargar Profesores primero
                if (File.Exists("profesores.json"))
                {
                    var data = JsonSerializer.Deserialize<List<Profesor>>(File.ReadAllText("profesores.json"), options);
                    foreach (var item in data) if (_repoProfesores.BuscarPorId(item.Identificacion) == null) _repoProfesores.Agregar(item);
                }
                if (File.Exists("estudiantes.json"))
                {
                    var data = JsonSerializer.Deserialize<List<Estudiante>>(File.ReadAllText("estudiantes.json"), options);
                    foreach (var item in data) if (_repoEstudiantes.BuscarPorId(item.Identificacion) == null) _repoEstudiantes.Agregar(item);
                }
                if (File.Exists("cursos.json"))
                {
                    var data = JsonSerializer.Deserialize<List<Curso>>(File.ReadAllText("cursos.json"), options);
                    foreach (var item in data) if (_repoCursos.BuscarPorId(item.Identificacion) == null) _repoCursos.Agregar(item);
                }

                Logger.Log("Datos cargados desde JSON.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Error al cargar JSON: {ex.Message}", "ERROR");
            }
        }
    }
    #endregion
}