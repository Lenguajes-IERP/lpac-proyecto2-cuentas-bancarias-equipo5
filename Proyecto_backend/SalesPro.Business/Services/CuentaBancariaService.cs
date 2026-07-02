using SalesPro.Contracts.CuentasBancarias;
using SalesPro.Data.Repositories;
using SalesPro.Domain.Exceptions;

namespace SalesPro.Business.Services;

public sealed class CuentaBancariaService : ICuentaBancariaService
{
    // Catálogos pequeños permitidos por el enunciado/proyecto.
    // Se validan aquí para que la API no guarde valores inventados.
    private static readonly HashSet<string> TiposCuentaPermitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "Corriente",
        "Ahorro",
        "Planilla"
    };

    private static readonly HashSet<string> DivisasPermitidas = new(StringComparer.OrdinalIgnoreCase)
    {
        "CRC",
        "USD",
        "EUR"
    };

    private readonly ICuentaBancariaRepository _cuentaRepository;

    public CuentaBancariaService(ICuentaBancariaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public Task<IReadOnlyCollection<CuentaBancariaDto>> ListarAsync(string? buscar, CancellationToken cancellationToken)
    {
        return _cuentaRepository.ListarAsync(buscar, cancellationToken);
    }

    public async Task<CuentaBancariaDto> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _cuentaRepository.ObtenerPorIdAsync(id, cancellationToken)
               ?? throw new NotFoundException($"No existe una cuenta bancaria con id {id}.");
    }

    public async Task<CuentaBancariaDto> CrearAsync(CrearCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        // Primero se valida negocio; después se inserta.
        // Si la validación falla, no se toca la base.
        await ValidarAsync(request, null, cancellationToken);

        var id = await _cuentaRepository.CrearAsync(request, cancellationToken);
        return await ObtenerPorIdAsync(id, cancellationToken);
    }

    public async Task<CuentaBancariaDto> ActualizarAsync(int id, ActualizarCuentaBancariaRequest request, CancellationToken cancellationToken)
    {
        // Se verifica que exista antes de validar duplicados.
        // El idExcluir permite editar la misma cuenta sin chocar contra su propio número.
        _ = await ObtenerPorIdAsync(id, cancellationToken);
        await ValidarAsync(request, id, cancellationToken);

        var updated = await _cuentaRepository.ActualizarAsync(id, request, cancellationToken);
        if (!updated)
        {
            throw new NotFoundException($"No existe una cuenta bancaria con id {id}.");
        }

        return await ObtenerPorIdAsync(id, cancellationToken);
    }

    public async Task EliminarAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await _cuentaRepository.EliminarAsync(id, cancellationToken);
        if (!deleted)
        {
            throw new NotFoundException($"No existe una cuenta bancaria con id {id}.");
        }
    }

    private async Task ValidarAsync(CrearCuentaBancariaRequest request, int? idExcluir, CancellationToken cancellationToken)
    {
        // Validaciones de campos requeridos. Se dejan en negocio para que a WPF le llegue un mensaje entendible.
        ValidarTexto(request.NumeroCuenta, "El número de cuenta es obligatorio.");
        ValidarTexto(request.TipoCuenta, "El tipo de cuenta es obligatorio.");
        ValidarTexto(request.TipoDivisa, "La moneda/divisa es obligatoria.");
        ValidarTexto(request.Pais, "El país es obligatorio.");
        ValidarTexto(request.Provincia, "La provincia es obligatoria.");
        ValidarTexto(request.NombreDueno, "El nombre del dueño es obligatorio.");
        ValidarTexto(request.ApellidosDueno, "Los apellidos del dueño son obligatorios.");

        if (!TiposCuentaPermitidos.Contains(request.TipoCuenta.Trim()))
        {
            throw new ValidationFailureException("Tipo de cuenta inválido. Use Corriente, Ahorro o Planilla.");
        }

        if (!DivisasPermitidas.Contains(request.TipoDivisa.Trim()))
        {
            throw new ValidationFailureException("Tipo de divisa inválido. Use CRC, USD o EUR.");
        }

        if (!await _cuentaRepository.ExisteBancoAsync(request.BancoId, cancellationToken))
        {
            // Banco/compañía se validan contra base porque son datos existentes, no texto libre.
            throw new ValidationFailureException($"El banco {request.BancoId} no existe o no está activo.");
        }

        if (!await _cuentaRepository.ExisteCompaniaAsync(request.CompaniaId, cancellationToken))
        {
            throw new ValidationFailureException($"La compañía {request.CompaniaId} no existe.");
        }

        if (await _cuentaRepository.ExisteNumeroCuentaAsync(request.NumeroCuenta, request.BancoId, idExcluir, cancellationToken))
        {
            // Regla de unicidad funcional: un mismo banco no debe tener dos veces el mismo número.
            throw new ConflictException("Ya existe una cuenta con ese número para el banco seleccionado.");
        }
    }

    private async Task ValidarAsync(ActualizarCuentaBancariaRequest request, int idExcluir, CancellationToken cancellationToken)
    {
        await ValidarAsync(
            new CrearCuentaBancariaRequest(
                request.NumeroCuenta,
                request.TipoCuenta,
                request.TipoDivisa,
                request.Estado,
                request.Pais,
                request.Provincia,
                request.BancoId,
                request.CompaniaId,
                request.NombreDueno,
                request.ApellidosDueno),
            idExcluir,
            cancellationToken);
    }

    private static void ValidarTexto(string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationFailureException(message);
        }
    }
}
