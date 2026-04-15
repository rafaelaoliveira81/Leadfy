using Domain.Entities;

namespace Application;

/// <summary>
/// Serviço de aplicação responsável por orquestrar os casos de uso relacionados a owers.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Application.
/// Sua responsabilidade é validar entradas, aplicar regras de fluxo,
/// coordenar chamadas ao domínio e persistir alterações por meio do repositório.
/// </remarks>
public class OwerApp : IOwerApp
{
    /// <summary>
    /// Repositório responsável pelo acesso e persistência dos owers.
    /// </summary>
    private readonly IOwerRepo _owerRepo;
    private readonly IUserRepo _userRepo;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="OwerApp"/>.
    /// </summary>
    /// <param name="owerRepo">Repositório de owers.</param>
    /// <param name="userRepo">Repositório de users.</param>
    public OwerApp(IOwerRepo owerRepo, IUserRepo userRepo)
    {
        _userRepo = userRepo;
        _owerRepo = owerRepo;
    }

    /// <summary>
    /// Adiciona um novo ower ao sistema.
    /// </summary>
    /// <param name="ower">Entidade de ower a ser cadastrada.</param>
    /// <returns>Retorna o identificador do ower criado.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do ower são inválidos.
    /// </exception>
    /// /// <exception cref="KeyNotFoundException">
    /// Lançada quando o user vinculado ao ower não é localizado.
    /// </exception>
    public async Task<int> AddAsync(Ower ower)
    {
        await ValidateOwerInformation(ower);

        return await _owerRepo.AddAsync(ower);
    }

    /// <summary>
    /// Obtém um ower pelo seu identificador.
    /// </summary>
    /// <param name="idOwer">ID do ower.</param>
    /// <returns>Ower encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o ower não é localizado.
    /// </exception>
    public async Task<Ower> GetByIdAsync(int idOwer)
    {
        return await ValidateOwerExistsByIdAsync(idOwer);
    }

    /// <summary>
    /// Busca owers cujo nome contenha o valor informado.
    /// </summary>
    /// <param name="nameOwer">
    /// Texto utilizado para filtrar os owers pelo nome.
    /// Não pode ser nulo, vazio ou composto apenas por espaços.
    /// </param>
    /// <returns>
    /// Uma coleção de owers que possuem o nome contendo o valor informado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o parâmetro <paramref name="nameOwer"/> é nulo ou inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando nenhum ower é encontrado para o critério informado.
    /// </exception>
    public async Task<IEnumerable<Ower>> GetByNameContainingAsync(string nameOwer)
    {
        if (string.IsNullOrWhiteSpace(nameOwer))
            throw new ArgumentException("Nome do ower não pode ser vazio.");

        nameOwer = nameOwer.Trim();

        var owerEntity = await _owerRepo.GetByNameContainingAsync(nameOwer);

        if (owerEntity == null || !owerEntity.Any())
            throw new KeyNotFoundException("Ower não localizado.");

        return owerEntity;
    }

    /// <summary>
    /// Obtém todos os owers cadastrados.
    /// </summary>
    /// <returns>Coleção com todos os owers.</returns>
    public async Task<IEnumerable<Ower>> GetAllAsync()
    {
        return await _owerRepo.GetAllAsync();
    }

    /// <summary>
    /// Obtém todos os owers filtrando pelo status.
    /// </summary>
    /// <param name="statusOwer">
    /// Status desejado para o filtro (true = ativo, false = inativo).
    /// </param>
    /// <returns>Coleção de owers com o status informado.</returns>
    public async Task<IEnumerable<Ower>> GetAllByStatusAsync(bool statusOwer)
    {
        return await _owerRepo.GetAllByStatusAsync(statusOwer);
    }

    /// <summary>
    /// Atualiza os dados de um ower existente.
    /// </summary>
    /// <param name="ower">Ower com os dados atualizados.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do ower são inválidos.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o ower a ser atualizado não é localizado.
    /// </exception>
    public async Task UpdateAsync(Ower ower)
    {
        var owerEntity = await ValidateOwerExistsByIdAsync(ower.ID);

        await ValidateOwerInformation(ower);

        owerEntity.Name = ower.Name;
        owerEntity.UserID = ower.UserID;
        owerEntity.IsActive = ower.IsActive;

        await _owerRepo.UpdateAsync(owerEntity);
    }

    /// <summary>
    /// Remove um ower do sistema.
    /// </summary>
    /// <param name="idOwer">ID do ower a ser removido.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o ower não é localizado.
    /// </exception>
    /// <remarks>
    /// Este método realiza remoção física do registro.
    /// </remarks>
    public async Task DeleteAsync(int idOwer)
    {
        var owerEntity = await ValidateOwerExistsByIdAsync(idOwer);

        await _owerRepo.DeleteAsync(owerEntity);
    }

    /// <summary>
    /// Desativa um ower no sistema.
    /// </summary>
    /// <param name="idOwer">ID do ower a ser desativado.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o ower não é localizado.
    /// </exception>
    public async Task DeactivateAsync(int idOwer)
    {
        var owerEntity = await ValidateOwerExistsByIdAsync(idOwer);

        owerEntity.Deactivate();

        await _owerRepo.UpdateAsync(owerEntity);
    }

    /// <summary>
    /// Ativa um ower no sistema.
    /// </summary>
    /// <param name="idOwer">ID do ower a ser ativado.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o ower não é localizado.
    /// </exception>
    public async Task ActivateAsync(int idOwer)
    {
        var owerEntity = await ValidateOwerExistsByIdAsync(idOwer);

        owerEntity.Activate();

        await _owerRepo.UpdateAsync(owerEntity);
    }

    #region Métodos auxiliares

    /// <summary>
    /// Valida as regras básicas e de negócio do ower.
    /// </summary>
    /// <param name="ower">Ower a ser validado.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando dados obrigatórios são inválidos,
    /// o usuário está inativo ou já possui outro ower ativo.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário vinculado não é encontrado.
    /// </exception>
    private async Task ValidateOwerInformation(Ower ower)
    {
        if (ower == null)
            throw new ArgumentException("Ower não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(ower.Name))
            throw new ArgumentException("O nome do ower deve ser informado.");

        if (ower.UserID <= 0)
            throw new ArgumentException("O usuário vinculado ao ower deve ser informado.");
        
        var userEntity = await ValidateUserExistsByIdAsync(ower.UserID);

        if (!userEntity.IsActive)
            throw new ArgumentException("O usuário vinculado ao ower deve estar ativo.");        
        
        if (userEntity.Owers != null && 
            userEntity.Owers.Any(o => o.ID != ower.ID && o.IsActive))
            throw new ArgumentException("O usuário já possui um ower ativo cadastrado.");
    }

    /// <summary>
    /// Valida se existe um ower com o ID informado.
    /// </summary>
    /// <param name="idOwer">ID do ower.</param>
    /// <returns>Ower encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o ower não é localizado.
    /// </exception>
    private async Task<Ower> ValidateOwerExistsByIdAsync(int idOwer)
    {
        var owerEntity = await _owerRepo.GetByIdAsync(idOwer);

        if (owerEntity == null)
            throw new KeyNotFoundException("Ower não localizado.");

        return owerEntity;
    }

    /// <summary>
    /// Valida se existe um usuário com o ID informado.
    /// </summary>
    /// <param name="idUser">ID do usuário.</param>
    /// <returns>Usuário encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário não é localizado.
    /// </exception>
    private async Task<User> ValidateUserExistsByIdAsync(int idUser)
    {
        var userEntity = await _userRepo.GetByIdAsync(idUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }
    
    #endregion
}