using SalesPro.Business.Services;
using SalesPro.Contracts.CuentasBancarias;
using SalesPro.Data.Repositories;
using SalesPro.Domain.Exceptions;

namespace SalesPro.Business.Tests;

public sealed class CuentaBancariaServiceTests
{
    [Fact]
    public async Task CrearAsync_CuandoSolicitudEsValida_CreaYDevuelveCuenta()
    {
        var repository = new FakeCuentaBancariaRepository();
        var service = new CuentaBancariaService(repository);
        var request = CrearRequestValido();

        var result = await service.CrearAsync(request, CancellationToken.None);

        Assert.Equal(1, result.Id);
        Assert.Equal("CR000000000001", result.NumeroCuenta);
        Assert.True(repository.CrearFueInvocado);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CrearAsync_CuandoNumeroCuentaEstaVacio_LanzaValidationFailure(string numeroCuenta)
    {
        var service = new CuentaBancariaService(new FakeCuentaBancariaRepository());
        var request = CrearRequestValido() with { NumeroCuenta = numeroCuenta };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearAsync(request, CancellationToken.None));

        Assert.Contains("número de cuenta", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearAsync_CuandoTipoCuentaEsInvalido_LanzaValidationFailure()
    {
        var service = new CuentaBancariaService(new FakeCuentaBancariaRepository());
        var request = CrearRequestValido() with { TipoCuenta = "Inversion" };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearAsync(request, CancellationToken.None));

        Assert.Contains("Tipo de cuenta inválido", exception.Message);
    }

    [Fact]
    public async Task CrearAsync_CuandoDivisaEsInvalida_LanzaValidationFailure()
    {
        var service = new CuentaBancariaService(new FakeCuentaBancariaRepository());
        var request = CrearRequestValido() with { TipoDivisa = "COL" };

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearAsync(request, CancellationToken.None));

        Assert.Contains("Tipo de divisa inválido", exception.Message);
    }

    [Fact]
    public async Task CrearAsync_CuandoBancoNoExiste_LanzaValidationFailure()
    {
        var repository = new FakeCuentaBancariaRepository { BancoExiste = false };
        var service = new CuentaBancariaService(repository);

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearAsync(CrearRequestValido(), CancellationToken.None));

        Assert.Contains("banco", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearAsync_CuandoCompaniaNoExiste_LanzaValidationFailure()
    {
        var repository = new FakeCuentaBancariaRepository { CompaniaExiste = false };
        var service = new CuentaBancariaService(repository);

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(
            () => service.CrearAsync(CrearRequestValido(), CancellationToken.None));

        Assert.Contains("compañía", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearAsync_CuandoNumeroCuentaYaExiste_LanzaConflict()
    {
        var repository = new FakeCuentaBancariaRepository { NumeroCuentaExiste = true };
        var service = new CuentaBancariaService(repository);

        var exception = await Assert.ThrowsAsync<ConflictException>(
            () => service.CrearAsync(CrearRequestValido(), CancellationToken.None));

        Assert.Contains("Ya existe", exception.Message);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_LanzaNotFound()
    {
        var service = new CuentaBancariaService(new FakeCuentaBancariaRepository());

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => service.ObtenerPorIdAsync(999, CancellationToken.None));

        Assert.Contains("No existe", exception.Message);
    }

    [Fact]
    public async Task EliminarAsync_CuandoNoExiste_LanzaNotFound()
    {
        var service = new CuentaBancariaService(new FakeCuentaBancariaRepository());

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => service.EliminarAsync(999, CancellationToken.None));

        Assert.Contains("No existe", exception.Message);
    }

    private static CrearCuentaBancariaRequest CrearRequestValido()
    {
        return new CrearCuentaBancariaRequest(
            NumeroCuenta: "CR000000000001",
            TipoCuenta: "Corriente",
            TipoDivisa: "CRC",
            Estado: true,
            Pais: "Costa Rica",
            Provincia: "San José",
            BancoId: 1,
            CompaniaId: 1,
            NombreDueno: "Sebastián",
            ApellidosDueno: "Cordero");
    }

    private sealed class FakeCuentaBancariaRepository : ICuentaBancariaRepository
    {
        private readonly Dictionary<int, CuentaBancariaDto> _cuentas = [];
        private int _nextId = 1;

        public bool BancoExiste { get; init; } = true;
        public bool CompaniaExiste { get; init; } = true;
        public bool NumeroCuentaExiste { get; init; }
        public bool CrearFueInvocado { get; private set; }

        public Task<IReadOnlyCollection<CuentaBancariaDto>> ListarAsync(string? buscar, CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyCollection<CuentaBancariaDto>>(_cuentas.Values.ToArray());
        }

        public Task<CuentaBancariaDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
        {
            _cuentas.TryGetValue(id, out var cuenta);
            return Task.FromResult(cuenta);
        }

        public Task<int> CrearAsync(CrearCuentaBancariaRequest request, CancellationToken cancellationToken)
        {
            CrearFueInvocado = true;
            var id = _nextId++;
            _cuentas[id] = new CuentaBancariaDto(
                id,
                request.NumeroCuenta,
                request.TipoCuenta,
                request.TipoDivisa,
                request.Estado,
                request.Pais,
                request.Provincia,
                request.BancoId,
                "Banco Nacional",
                request.CompaniaId,
                "Compañía Principal",
                request.NombreDueno,
                request.ApellidosDueno);
            return Task.FromResult(id);
        }

        public Task<bool> ActualizarAsync(int id, ActualizarCuentaBancariaRequest request, CancellationToken cancellationToken)
        {
            if (!_cuentas.ContainsKey(id))
            {
                return Task.FromResult(false);
            }

            _cuentas[id] = new CuentaBancariaDto(
                id,
                request.NumeroCuenta,
                request.TipoCuenta,
                request.TipoDivisa,
                request.Estado,
                request.Pais,
                request.Provincia,
                request.BancoId,
                "Banco Nacional",
                request.CompaniaId,
                "Compañía Principal",
                request.NombreDueno,
                request.ApellidosDueno);
            return Task.FromResult(true);
        }

        public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_cuentas.Remove(id));
        }

        public Task<bool> ExisteBancoAsync(int bancoId, CancellationToken cancellationToken)
        {
            return Task.FromResult(BancoExiste);
        }

        public Task<bool> ExisteCompaniaAsync(int companiaId, CancellationToken cancellationToken)
        {
            return Task.FromResult(CompaniaExiste);
        }

        public Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta, int bancoId, int? idExcluir, CancellationToken cancellationToken)
        {
            return Task.FromResult(NumeroCuentaExiste);
        }
    }
}
