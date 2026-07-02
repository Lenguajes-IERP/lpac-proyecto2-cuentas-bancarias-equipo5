using SalesPro.Business.Services;
using SalesPro.Contracts.Ordenes;
using SalesPro.Data.Repositories;
using SalesPro.Domain.Exceptions;

namespace SalesPro.Business.Tests;

public sealed class OrdenServiceTests
{
    [Fact]
    public async Task CrearOrdenAsync_CuandoSolicitudEsValida_CreaOrdenConIvaConfigurado()
    {
        var ordenRepository = new FakeOrdenRepository();
        var parametroRepository = new FakeParametroSistemaRepository { Iva = 13m };
        var service = new OrdenService(ordenRepository, parametroRepository);
        var request = CrearRequestValido();

        var result = await service.CrearOrdenAsync(request, CancellationToken.None);

        Assert.Equal(1, result.NumeroOrden);
        Assert.Equal(13m, ordenRepository.PorcentajeIvaRecibido);
        Assert.Same(request, ordenRepository.RequestRecibido);
    }

    [Fact]
    public async Task CrearOrdenAsync_CuandoClienteEsInvalido_LanzaValidationFailure()
    {
        var service = CrearService();
        var request = CrearRequestValido() with { ClienteId = 0 };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearOrdenAsync(request, CancellationToken.None));

        Assert.Contains("cliente", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearOrdenAsync_CuandoEmpleadoEsInvalido_LanzaValidationFailure()
    {
        var service = CrearService();
        var request = CrearRequestValido() with { EmpleadoId = 0 };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearOrdenAsync(request, CancellationToken.None));

        Assert.Contains("empleado", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearOrdenAsync_CuandoNoTieneDetalles_LanzaValidationFailure()
    {
        var service = CrearService();
        var request = CrearRequestValido() with { Detalles = [] };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearOrdenAsync(request, CancellationToken.None));

        Assert.Contains("al menos un producto", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearOrdenAsync_CuandoProductoEsInvalido_LanzaValidationFailure()
    {
        var service = CrearService();
        var request = CrearRequestValido() with
        {
            Detalles = [new CrearOrdenDetalleRequest(0, 1)]
        };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearOrdenAsync(request, CancellationToken.None));

        Assert.Contains("producto válido", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearOrdenAsync_CuandoCantidadEsInvalida_LanzaValidationFailure()
    {
        var service = CrearService();
        var request = CrearRequestValido() with
        {
            Detalles = [new CrearOrdenDetalleRequest(1, 0)]
        };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearOrdenAsync(request, CancellationToken.None));

        Assert.Contains("cantidad", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearOrdenAsync_CuandoTraeProductosRepetidos_LanzaValidationFailure()
    {
        var service = CrearService();
        var request = CrearRequestValido() with
        {
            Detalles =
            [
                new CrearOrdenDetalleRequest(1, 1),
                new CrearOrdenDetalleRequest(1, 2)
            ]
        };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearOrdenAsync(request, CancellationToken.None));

        Assert.Contains("productos repetidos", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearOrdenAsync_CuandoNoExisteParametroIva_LanzaNotFound()
    {
        var service = new OrdenService(new FakeOrdenRepository(), new FakeParametroSistemaRepository { Iva = null });

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => service.CrearOrdenAsync(CrearRequestValido(), CancellationToken.None));

        Assert.Contains("IVA", exception.Message);
    }

    [Fact]
    public async Task ObtenerPorNumeroAsync_CuandoNoExiste_LanzaNotFound()
    {
        var service = CrearService();

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => service.ObtenerPorNumeroAsync(999, CancellationToken.None));

        Assert.Contains("No existe", exception.Message);
    }

    private static OrdenService CrearService()
    {
        return new OrdenService(new FakeOrdenRepository(), new FakeParametroSistemaRepository { Iva = 13m });
    }

    private static CrearOrdenRequest CrearRequestValido()
    {
        return new CrearOrdenRequest(
            ClienteId: 1,
            EmpleadoId: 1,
            Detalles: [new CrearOrdenDetalleRequest(ProductoId: 1, Cantidad: 2)]);
    }

    private sealed class FakeOrdenRepository : IOrdenRepository
    {
        public CrearOrdenRequest? RequestRecibido { get; private set; }
        public decimal? PorcentajeIvaRecibido { get; private set; }

        public Task<OrdenDto> CrearOrdenAsync(CrearOrdenRequest request, decimal porcentajeImpuestoVenta, CancellationToken cancellationToken)
        {
            RequestRecibido = request;
            PorcentajeIvaRecibido = porcentajeImpuestoVenta;
            var subtotal = request.Detalles.Sum(d => d.Cantidad * 1000m);
            var impuesto = subtotal * porcentajeImpuestoVenta / 100m;
            var orden = new OrdenDto(
                NumeroOrden: 1,
                ClienteId: request.ClienteId,
                ClienteNombre: "Cliente de prueba",
                FechaOrden: new DateTime(2026, 7, 1),
                EmpleadoId: request.EmpleadoId,
                Subtotal: subtotal,
                Impuesto: impuesto,
                Total: subtotal + impuesto,
                Detalles:
                [
                    new OrdenDetalleDto(
                        ProductoId: 1,
                        NombreProducto: "Producto de prueba",
                        PrecioUnitario: 1000m,
                        Cantidad: 2,
                        Subtotal: 2000m,
                        Impuesto: 260m)
                ]);
            return Task.FromResult(orden);
        }

        public Task<OrdenDto?> ObtenerPorNumeroAsync(int numeroOrden, CancellationToken cancellationToken)
        {
            return Task.FromResult<OrdenDto?>(null);
        }
    }

    private sealed class FakeParametroSistemaRepository : IParametroSistemaRepository
    {
        public decimal? Iva { get; init; }

        public Task<decimal?> ObtenerValorDecimalAsync(string nombre, CancellationToken cancellationToken)
        {
            return Task.FromResult(nombre == "IVA" ? Iva : null);
        }
    }
}
