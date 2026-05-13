using Domain.Entities;

namespace Application;

public class OwerApp : IOwerApp
{
    private readonly IOwerRepo _owerRepo;
    private readonly IUserRepo _userRepo;
    public OwerApp(IOwerRepo owerRepo, IUserRepo userRepo)
    {
        _userRepo = userRepo;
        _owerRepo = owerRepo;
    }
    public async Task<int> AddAsync(Ower ower)
    {
        await ValidateOwerInformation(ower);

        return await _owerRepo.AddAsync(ower);
    }
    public async Task<Ower> GetByIdAsync(int idOwer)
    {
        return await ValidateOwerExistsByIdAsync(idOwer);
    }
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
    public async Task<IEnumerable<Ower>> GetAllAsync()
    {
        return await _owerRepo.GetAllAsync();
    }
    public async Task<IEnumerable<Ower>> GetAllByStatusAsync(bool statusOwer)
    {
        return await _owerRepo.GetAllByStatusAsync(statusOwer);
    }
    public async Task UpdateAsync(Ower ower)
    {
        var owerEntity = await ValidateOwerExistsByIdAsync(ower.ID);

        await ValidateOwerInformation(ower);

        owerEntity.Name = ower.Name;
        owerEntity.UserID = ower.UserID;
        owerEntity.IsActive = ower.IsActive;

        await _owerRepo.UpdateAsync(owerEntity);
    }
    public async Task DeleteAsync(int idOwer)
    {
        var owerEntity = await ValidateOwerExistsByIdAsync(idOwer);

        await _owerRepo.DeleteAsync(owerEntity);
    }
    public async Task DeactivateAsync(int idOwer)
    {
        var owerEntity = await ValidateOwerExistsByIdAsync(idOwer);

        owerEntity.Deactivate();

        await _owerRepo.UpdateAsync(owerEntity);
    }
    public async Task ActivateAsync(int idOwer)
    {
        var owerEntity = await ValidateOwerExistsByIdAsync(idOwer);

        owerEntity.Activate();

        await _owerRepo.UpdateAsync(owerEntity);
    }

    #region Métodos auxiliares
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
    private async Task<Ower> ValidateOwerExistsByIdAsync(int idOwer)
    {
        var owerEntity = await _owerRepo.GetByIdAsync(idOwer);

        if (owerEntity == null)
            throw new KeyNotFoundException("Ower não localizado.");

        return owerEntity;
    }
    private async Task<User> ValidateUserExistsByIdAsync(int idUser)
    {
        var userEntity = await _userRepo.GetByIdAsync(idUser);
        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");

        return userEntity;
    }

    #endregion
}