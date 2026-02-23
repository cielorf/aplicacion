using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace Aplicacion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private List<Cliente> clientes = new List<Cliente>
            {
                new Cliente {
                    Id = 1,
                    Nombre = "Cielo",
                    ApPaterno = "Ramirez",
                    ApMaterno = "Farias",
                    Correo = "cielo@example.com",
                    Telefono = "4434486565",
                    FechaNacimiento = DateOnly.Parse("1999-02-15"),
                    Activo = true
                },
                new Cliente {
                    Id = 2,
                    Nombre = "Ronaldo",
                    ApPaterno = "Nuñez",
                    ApMaterno = "Castro",
                    Correo = "ronaldo@example.com",
                    Telefono = "4434486565",
                    FechaNacimiento = DateOnly.Parse("2000-12-16"),
                                        Activo = true
                },
                new Cliente {
                    Id = 3,
                    Nombre = "Gibran",
                    ApPaterno = "Gonzalez",
                    ApMaterno = "Perez",
                    Correo = "gibran@example.com",
                    Telefono = "4434486565",
                    FechaNacimiento = DateOnly.Parse("1999-08-25"),
                    Activo = false,
                    Compras = new List<Compras>()
                    {
                        new Compras{ Ticket = 1, FechaCompra = DateTime.Now, NumeroArticulos = 10, Total = 500.25M},
                        new Compras{ Ticket = 2, FechaCompra = DateTime.Now, NumeroArticulos = 10, Total = 500.25M},
                        new Compras{ Ticket = 3, FechaCompra = DateTime.Now, NumeroArticulos = 10, Total = 500.25M}
                    }
                }
            };

        [HttpGet("VerTodos")]
        public IActionResult Get()
        {
            return Ok(clientes);
        }

        //[HttpGet("VerActivos/{Activo}")]
        [HttpGet("VerActivos")]
        public IActionResult Get(bool Activo)
        {
            // linq

            //Select * From Clientes where Activo is true
            var lst = clientes
                .Where(c => c.Activo == Activo)
                .Select(c => new
                {
                    c.NombreCompleto,
                    c.FechaNacimiento
                })
                .ToList();

            return Ok(lst);
        }

        [HttpPost("Agregar")]
        public IActionResult Add(Cliente cliente)
        {
            clientes.Add(cliente);

            return Ok(clientes);
        }

        [HttpDelete("Borrar")]
        public IActionResult Delete(int id)
        {

            var ClienteAborrar = clientes.Where(c => c.Id == id).FirstOrDefault();

            if (ClienteAborrar == null)
            {
                return NotFound();
            }

            clientes.Remove(ClienteAborrar);

            ClienteAborrar.Activo = false;

            clientes.Add(ClienteAborrar);

            return Ok(clientes);
        }


        //Actualizar
        [HttpPut("Actualizar")]
        public IActionResult Update(int id, Cliente clienteActualizado)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return NotFound();

            cliente.Nombre = clienteActualizado.Nombre;
            cliente.ApPaterno = clienteActualizado.ApPaterno;
            cliente.ApMaterno = clienteActualizado.ApMaterno;
            cliente.Correo = clienteActualizado.Correo;
            cliente.Telefono = clienteActualizado.Telefono;
            cliente.FechaNacimiento = clienteActualizado.FechaNacimiento;
            cliente.Activo = clienteActualizado.Activo;
            cliente.Compras = clienteActualizado.Compras;

            return Ok(cliente);
        }
    }

    public class Cliente
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string ApMaterno { get; set; }
        public required string ApPaterno { get; set; }
        //propiedad calculada
        public string NombreCompleto { get => $"{Nombre} {ApPaterno} {ApMaterno}"; }
        public required string Correo { get; set; }
        public required DateOnly FechaNacimiento { get; set; }
        //propiedad calculada
        public int Edad { get => DateTime.Now.Year - FechaNacimiento.Year; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }

        public List<Compras> Compras { get; set; } = new List<Compras>();
    }

    public class Compras
    {
        public int Ticket { get; set; }
        public DateTime FechaCompra { get; set; }
        public int NumeroArticulos { get; set; }
        public decimal Total { get; set; }
    }
}