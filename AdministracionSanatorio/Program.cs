using System;
using System.Collections.Generic;

namespace AdministracionSanatorio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Inicializar el sanatorio con 15% de adicional por alta complejidad
            Hospital sanatorio = new Hospital(15);
            sanatorio.CargarDatosPrueba();

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA DE GESTIÓN DEL SANATORIO ===");
                Console.WriteLine("1. Dar de alta un nuevo paciente");
                Console.WriteLine("2. Listar todos los pacientes");
                Console.WriteLine("3. Asignar intervención a paciente");
                Console.WriteLine("4. Calcular costo de intervenciones por DNI");
                Console.WriteLine("5. Reporte de liquidaciones pendientes");
                Console.WriteLine("6. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": // Alta de nuevo paciente
                        AltaNuevoPaciente(sanatorio);
                        break;

                    case "2": // Listar pacientes
                        ListarPacientes(sanatorio);
                        break;

                    case "3": // Asignar intervención
                        AsignarIntervencion(sanatorio);
                        break;

                    case "4": // Calcular costos por DNI
                        CalcularCostosPaciente(sanatorio);
                        break;

                    case "5": // Reporte de liquidaciones
                        GenerarReporteLiquidaciones(sanatorio);
                        break;

                    case "6": // Salir
                        salir = true;
                        Console.WriteLine("Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        static void AltaNuevoPaciente(Hospital sanatorio)
        {
            Console.Clear();
            Console.WriteLine("=== ALTA DE NUEVO PACIENTE ===");

            Console.Write("DNI: ");
            string dni = Console.ReadLine();

            Console.Write("Nombre completo: ");
            string nombre = Console.ReadLine();

            Console.Write("Teléfono: ");
            string telefono = Console.ReadLine();

            Console.Write("¿Tiene obra social? (S/N): ");
            bool tieneOS = Console.ReadLine().ToUpper() == "S";

            if (tieneOS)
            {
                Console.Write("Nombre obra social: ");
                string obraSocial = Console.ReadLine();

                Console.Write("Porcentaje de cobertura (0-100): ");
                double cobertura;
                while (!double.TryParse(Console.ReadLine(), out cobertura) || cobertura < 0 || cobertura > 100)
                {
                    Console.Write("Porcentaje inválido. Ingrese un valor entre 0 y 100: ");
                }

                sanatorio.AgregarPaciente(new Paciente(dni, nombre, telefono, obraSocial, cobertura));
            }
            else
            {
                sanatorio.AgregarPaciente(new Paciente(dni, nombre, telefono));
            }

            Console.WriteLine("\nPaciente registrado exitosamente!");
        }

        static void ListarPacientes(Hospital sanatorio)
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE PACIENTES ===");

            var pacientes = sanatorio.ListarPacientes();
            if (pacientes.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                return;
            }

            foreach (var paciente in pacientes)
            {
                Console.WriteLine(paciente.ToString());
                Console.WriteLine("-------------------------");
            }
        }

        static void AsignarIntervencion(Hospital sanatorio)
        {
            Console.Clear();
            Console.WriteLine("=== ASIGNAR INTERVENCIÓN ===");

            Console.Write("DNI del paciente: ");
            string dni = Console.ReadLine();

            var paciente = sanatorio.BuscarPacientePorDNI(dni);
            if (paciente == null)
            {
                Console.WriteLine("Paciente no encontrado. Debe darlo de alta primero.");
                return;
            }

            Console.WriteLine("\nIntervenciones disponibles:");
            foreach (var intervencion in sanatorio.Intervenciones)
            {
                Console.WriteLine(intervencion);
            }

            Console.Write("\nCódigo de intervención a asignar: ");
            string codigo = Console.ReadLine();

            Console.Write("Fecha de intervención (dd/mm/aaaa): ");
            DateTime fecha;
            while (!DateTime.TryParse(Console.ReadLine(), out fecha))
            {
                Console.Write("Formato inválido. Ingrese la fecha (dd/mm/aaaa): ");
            }

            

            Console.WriteLine("\nMédicos disponibles para esta intervención:");
            var intervencionSeleccionada = sanatorio.BuscarIntervencionPorCodigo(codigo);
            if (intervencionSeleccionada == null)
            {
                Console.WriteLine("Intervención no válida.");
                return;
            }

            var medicosDisponibles = sanatorio.ListarDoctoresDisponiblesPorEspecialidad(intervencionSeleccionada.Especialidad);
            if (medicosDisponibles.Count == 0)
            {
                Console.WriteLine("No hay médicos disponibles para esta especialidad.");
                return;
            }

            foreach (var medico in medicosDisponibles)
            {
                Console.WriteLine(medico.ToString());
            }

            Console.Write("\nMatrícula del médico asignado: ");
            string matricula = Console.ReadLine();

            bool asignado = sanatorio.ProgramarIntervencion(dni, codigo, matricula, fecha);
            if (asignado)
            {
                Console.WriteLine("\nIntervención asignada exitosamente!");
            }
            else
            {
                Console.WriteLine("\nError al asignar la intervención. Verifique los datos.");
            }
        }

        static void CalcularCostosPaciente(Hospital sanatorio)
        {
            Console.Clear();
            Console.WriteLine("=== CALCULAR COSTOS POR DNI ===");

            Console.Write("Ingrese DNI del paciente: ");
            string dni = Console.ReadLine();

            double total = sanatorio.CalcularCostoPaciente(dni);
            var paciente = sanatorio.BuscarPacientePorDNI(dni);

            if (paciente == null)
            {
                Console.WriteLine("Paciente no encontrado.");
                return;
            }

            Console.WriteLine($"\nPaciente: {paciente.NombreCompleto}");
            Console.WriteLine($"Total adeudado: ${total:0.00}");

            var intervencionesPendientes = paciente.ObtenerIntervencionesPendientes();
            if (intervencionesPendientes.Count > 0)
            {
                Console.WriteLine("\nIntervenciones pendientes de pago:");
                foreach (var intervencion in intervencionesPendientes)
                {
                    Console.WriteLine($"- {intervencion.Intervencion.Descripcion} ({intervencion.Fecha.ToShortDateString()}): " +
                                    $"${sanatorio.CalcularCostoIntervencion(intervencion):0.00}");
                }
            }
            else
            {
                Console.WriteLine("El paciente no tiene intervenciones pendientes de pago.");
            }
        }

        static void GenerarReporteLiquidaciones(Hospital sanatorio)
        {
            Console.Clear();
            Console.WriteLine("=== REPORTE DE LIQUIDACIONES PENDIENTES ===");

            var reporte = sanatorio.GenerarReporteLiquidacionesPendientes();
            if (reporte.Count == 0)
            {
                Console.WriteLine("No hay liquidaciones pendientes.");
                return;
            }

            foreach (var linea in reporte)
            {
                Console.WriteLine(linea);
                Console.WriteLine(new string('-', 100));
            }

            Console.WriteLine($"\nTotal de liquidaciones pendientes: {reporte.Count}");
        }
    }
}