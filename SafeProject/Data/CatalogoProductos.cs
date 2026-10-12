using CalentonesAgua.Models;

namespace CalentonesAgua.Data;

public class CatalogoProductos
{
    private readonly IReadOnlyList<Producto> _productos;

    public CatalogoProductos()
    {
        _productos = new List<Producto>
        {
            new()
            {
                Id = 1,
                Nombre = "Paso 13L Natural",
                Slug = "paso-13l-natural",
                Tipo = TipoCalenton.GasPaso,
                Resumen = "Agua caliente continua para 1 o 2 baños, sin depósito.",
                Descripcion = "Calentón de paso a gas natural de 13 litros por minuto. Ideal para departamentos y casas compactas. Enciende al abrir la llave y deja de consumir gas al cerrarla. Cuerpo compacto para muro, con encendido electrónico y sensor de flama.",
                Precio = 4890,
                PrecioAnterior = 5590,
                CapacidadLitros = 13,
                Combustible = "Gas natural",
                PersonasRecomendadas = 3,
                EnOferta = true,
                Destacado = true,
                AcentoColor = "#c45c26",
                Caracteristicas = new[]
                {
                    "13 L/min de agua caliente continua",
                    "Encendido electrónico",
                    "Ahorro de gas al no mantener depósito",
                    "Instalación en muro interior o exterior techado"
                }
            },
            new()
            {
                Id = 2,
                Nombre = "Paso 16L Familia",
                Slug = "paso-16l-familia",
                Tipo = TipoCalenton.GasPaso,
                Resumen = "Dos duchas al mismo tiempo sin perder temperatura.",
                Descripcion = "Modelo de paso de 16 litros, pensado para familias que usan cocina y baño a la vez. Control de temperatura preciso y llama modulante para evitar cambios bruscos de calor.",
                Precio = 6250,
                CapacidadLitros = 16,
                Combustible = "Gas LP / natural",
                PersonasRecomendadas = 5,
                Destacado = true,
                AcentoColor = "#9a3f14",
                Caracteristicas = new[]
                {
                    "16 L/min para uso simultáneo",
                    "Llama modulante",
                    "Pantalla de temperatura",
                    "Protección contra sobrecalentamiento"
                }
            },
            new()
            {
                Id = 3,
                Nombre = "Mini Paso 6L Studio",
                Slug = "mini-paso-6l-studio",
                Tipo = TipoCalenton.GasPaso,
                Resumen = "Compacto para estudio, loft o casa de fin de semana.",
                Descripcion = "El más pequeño de la línea de paso. Cubre un lavabo y una ducha ligera. Fácil de instalar donde el espacio es limitado.",
                Precio = 2890,
                CapacidadLitros = 6,
                Combustible = "Gas LP",
                PersonasRecomendadas = 2,
                AcentoColor = "#d97706",
                Caracteristicas = new[]
                {
                    "6 L/min",
                    "Cuerpo ultra compacto",
                    "Bajo consumo de gas",
                    "Ideal para un baño"
                }
            },
            new()
            {
                Id = 4,
                Nombre = "Paso 10L Esencial",
                Slug = "paso-10l-esencial",
                Tipo = TipoCalenton.GasPaso,
                Resumen = "Equilibrio entre precio, tamaño y rendimiento.",
                Descripcion = "Calentón de paso de 10 litros, la opción más pedida para casas de un baño y cocina. Arranque rápido y mantenimiento sencillo.",
                Precio = 3990,
                PrecioAnterior = 4290,
                CapacidadLitros = 10,
                Combustible = "Gas LP / natural",
                PersonasRecomendadas = 3,
                EnOferta = true,
                AcentoColor = "#b45309",
                Caracteristicas = new[]
                {
                    "10 L/min",
                    "Encendido automático",
                    "Repuestos fáciles de conseguir",
                    "Garantía de 3 años"
                }
            },
            new()
            {
                Id = 5,
                Nombre = "Depósito 40L Clásico",
                Slug = "deposito-40l-clasico",
                Tipo = TipoCalenton.GasDeposito,
                Resumen = "Agua acumulada lista para ducha y lavado de trastes.",
                Descripcion = "Tanque de 40 litros a gas. Mantiene el agua a temperatura constante y es una solución confiable si prefieres depósito en lugar de paso.",
                Precio = 7490,
                CapacidadLitros = 40,
                Combustible = "Gas LP",
                PersonasRecomendadas = 3,
                Destacado = true,
                AcentoColor = "#1a6b73",
                Caracteristicas = new[]
                {
                    "Tanque esmaltado de 40 L",
                    "Ánodo de magnesio",
                    "Termostato ajustable",
                    "Aislamiento de alta densidad"
                }
            },
            new()
            {
                Id = 6,
                Nombre = "Depósito 60L Familiar",
                Slug = "deposito-60l-familiar",
                Tipo = TipoCalenton.GasDeposito,
                Resumen = "Capacidad extra para mañanas con varios baños.",
                Descripcion = "Depósito de 60 litros para familias de 4 a 5 personas. Recupera temperatura con rapidez y conserva el calor por más tiempo.",
                Precio = 9180,
                CapacidadLitros = 60,
                Combustible = "Gas LP / natural",
                PersonasRecomendadas = 5,
                AcentoColor = "#0f766e",
                Caracteristicas = new[]
                {
                    "60 litros de almacenamiento",
                    "Recuperación rápida",
                    "Válvula de alivio incluida",
                    "Pies niveladores"
                }
            },
            new()
            {
                Id = 7,
                Nombre = "Depósito 110L Plus",
                Slug = "deposito-110l-plus",
                Tipo = TipoCalenton.GasDeposito,
                Resumen = "Máxima reserva de agua caliente para casa grande.",
                Descripcion = "El tanque más grande de la línea de gas. Pensado para residencias con tres baños o uso intensivo de lavandería.",
                Precio = 11250,
                CapacidadLitros = 110,
                Combustible = "Gas natural",
                PersonasRecomendadas = 7,
                AcentoColor = "#115e59",
                Caracteristicas = new[]
                {
                    "110 litros",
                    "Doble aislamiento",
                    "Alta recuperación",
                    "Ideal para 3 baños"
                }
            },
            new()
            {
                Id = 8,
                Nombre = "Eléctrico 50L Urban",
                Slug = "electrico-50l-urban",
                Tipo = TipoCalenton.Electrico,
                Resumen = "Sin gas. Perfecto para departamentos y oficinas.",
                Descripcion = "Calentón eléctrico de 50 litros. No requiere instalación de gas ni ventilación de combustión. Termostato digital y protección contra sobrepresión.",
                Precio = 5670,
                CapacidadLitros = 50,
                Combustible = "Electricidad 220 V",
                PersonasRecomendadas = 3,
                Destacado = true,
                AcentoColor = "#2563eb",
                Caracteristicas = new[]
                {
                    "50 L, 220 V",
                    "Sin combustión ni chimenea",
                    "Termostato digital",
                    "Instalación vertical u horizontal"
                }
            },
            new()
            {
                Id = 9,
                Nombre = "Eléctrico 80L Comfort",
                Slug = "electrico-80l-comfort",
                Tipo = TipoCalenton.Electrico,
                Resumen = "Más litros para familia que no quiere gas en casa.",
                Descripcion = "Tanque eléctrico de 80 litros con resistencia de alto rendimiento y aislamiento reforzado para bajar el consumo en espera.",
                Precio = 7920,
                PrecioAnterior = 8490,
                CapacidadLitros = 80,
                Combustible = "Electricidad 220 V",
                PersonasRecomendadas = 5,
                EnOferta = true,
                AcentoColor = "#1d4ed8",
                Caracteristicas = new[]
                {
                    "80 litros",
                    "Baja pérdida de calor",
                    "Resistencia reemplazable",
                    "Modo eco"
                }
            },
            new()
            {
                Id = 10,
                Nombre = "Instantáneo 5.5 kW",
                Slug = "instantaneo-electrico-55kw",
                Tipo = TipoCalenton.Electrico,
                Resumen = "Agua caliente al instante, sin tanque ni espera.",
                Descripcion = "Calentón eléctrico instantáneo para un punto de uso: lavabo, tarja o ducha de bajo flujo. Se instala junto a la llave y ocupa casi nada de espacio.",
                Precio = 3450,
                CapacidadLitros = 0,
                Combustible = "Electricidad 220 V",
                PersonasRecomendadas = 1,
                AcentoColor = "#4f46e5",
                Caracteristicas = new[]
                {
                    "Sin tanque",
                    "5.5 kW",
                    "Arranque al abrir la llave",
                    "Ideal como refuerzo de un punto"
                }
            },
            new()
            {
                Id = 11,
                Nombre = "Solar 150L Terraza",
                Slug = "solar-150l-terraza",
                Tipo = TipoCalenton.Solar,
                Resumen = "Aprovecha el sol. Baja tu recibo casi todo el año.",
                Descripcion = "Sistema solar de 150 litros con tubos de vacío y tanque en azotea. Incluye respaldo opcional para días nublados. La opción con menor costo de operación a mediano plazo.",
                Precio = 12890,
                CapacidadLitros = 150,
                Combustible = "Energía solar",
                PersonasRecomendadas = 5,
                Destacado = true,
                GarantiaAnios = 5,
                AcentoColor = "#ca8a04",
                Caracteristicas = new[]
                {
                    "150 L de almacenamiento",
                    "Tubos de vacío",
                    "Bajo costo de operación",
                    "Garantía de 5 años en colector"
                }
            },
            new()
            {
                Id = 12,
                Nombre = "Híbrido Solar + Gas",
                Slug = "hibrido-solar-gas",
                Tipo = TipoCalenton.Hibrido,
                Resumen = "Sol de día, gas de respaldo. Agua caliente todo el año.",
                Descripcion = "Combina colector solar con calentón de paso a gas. El sistema prioriza el sol y enciende el gas solo cuando hace falta. La solución más completa para clima variable.",
                Precio = 15400,
                CapacidadLitros = 180,
                Combustible = "Solar + gas LP",
                PersonasRecomendadas = 6,
                Destacado = true,
                GarantiaAnios = 5,
                AcentoColor = "#c2410c",
                Caracteristicas = new[]
                {
                    "Prioridad solar automática",
                    "Respaldo de gas en días nublados",
                    "180 L equivalentes de confort",
                    "Máximo ahorro anual"
                }
            }
        };
    }

    public IReadOnlyList<Producto> Todos() => _productos;

    public IReadOnlyList<Producto> Destacados() =>
        _productos.Where(p => p.Destacado).ToList();

    public Producto? PorId(int id) =>
        _productos.FirstOrDefault(p => p.Id == id);

    public Producto? PorSlug(string slug) =>
        _productos.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<Producto> Relacionados(Producto producto, int cantidad = 3) =>
        _productos
            .Where(p => p.Id != producto.Id && p.Tipo == producto.Tipo)
            .Take(cantidad)
            .ToList();

    public IReadOnlyList<Producto> Filtrar(string? tipo, string? busqueda, string? orden)
    {
        IEnumerable<Producto> query = _productos;

        if (!string.IsNullOrWhiteSpace(tipo) && Enum.TryParse<TipoCalenton>(tipo, true, out var tipoEnum))
        {
            query = query.Where(p => p.Tipo == tipoEnum);
        }

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var termino = busqueda.Trim();
            query = query.Where(p =>
                p.Nombre.Contains(termino, StringComparison.OrdinalIgnoreCase) ||
                p.Resumen.Contains(termino, StringComparison.OrdinalIgnoreCase) ||
                p.TipoEtiqueta.Contains(termino, StringComparison.OrdinalIgnoreCase) ||
                p.Combustible.Contains(termino, StringComparison.OrdinalIgnoreCase));
        }

        query = orden switch
        {
            "precio-asc" => query.OrderBy(p => p.Precio),
            "precio-desc" => query.OrderByDescending(p => p.Precio),
            "capacidad" => query.OrderByDescending(p => p.CapacidadLitros),
            _ => query.OrderByDescending(p => p.Destacado).ThenBy(p => p.Nombre)
        };

        return query.ToList();
    }
}
