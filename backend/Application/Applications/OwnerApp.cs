using Domain.Entities;

namespace Application;

public class OwnerApp : IOwnerApp
{
    private readonly IOwnerRepo _ownerRepo;
    private readonly IUserRepo _userRepo;
    public OwnerApp(IOwnerRepo ownerRepo, IUserRepo userRepo)
    {
        _userRepo = userRepo;
        _ownerRepo = ownerRepo;
    }
    public async Task<int> AddAsync(Owner owner)
    {
        await ValidateOwnerInformation(owner);

        return await _ownerRepo.AddAsync(owner);
    }
    public async Task<Owner> GetByIdAsync(int idOwner)
    {
        return await ValidateOwnerExistsByIdAsync(idOwner);
    }

    public async Task<IEnumerable<Owner>> GetAllAsync(bool? statusOwner)
    {
        return await _ownerRepo.GetAllAsync(statusOwner);
    }
    public async Task UpdateAsync(Owner owner)
    {
        var ownerEntity = await ValidateOwnerExistsByIdAsync(owner.ID);

        await ValidateOwnerInformation(owner);

        ownerEntity.Name = owner.Name;
        ownerEntity.UserID = owner.UserID;
        ownerEntity.IsActive = owner.IsActive;

        await _ownerRepo.UpdateAsync(ownerEntity);
    }
    public async Task DeleteAsync(int idOwner)
    {
        var ownerEntity = await ValidateOwnerExistsByIdAsync(idOwner);

        await _ownerRepo.DeleteAsync(ownerEntity);
    }
    public async Task DeactivateAsync(int idOwner)
    {
        var ownerEntity = await ValidateOwnerExistsByIdAsync(idOwner);

        ownerEntity.Deactivate();

        await _ownerRepo.UpdateAsync(ownerEntity);
    }
    public async Task ActivateAsync(int idOwner)
    {
        var ownerEntity = await ValidateOwnerExistsByIdAsync(idOwner);

        ownerEntity.Activate();

        await _ownerRepo.UpdateAsync(ownerEntity);
    }

    #region Métodos auxiliares
    private async Task ValidateOwnerInformation(Owner owner)
    {
        if (owner == null)
            throw new ArgumentException("Responsável não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(owner.Name))
            throw new ArgumentException("O nome do responsável deve ser informado.");

        if (owner.UserID <= 0)
            throw new ArgumentException("O usuário vinculado ao responsável deve ser informado.");

        var userEntity = await ValidateUserExistsByIdAsync(owner.UserID);

        if (!userEntity.IsActive)
            throw new ArgumentException("O usuário vinculado ao responsável deve estar ativo.");

        if (userEntity.Owners != null &&
            userEntity.Owners.Any(o => o.ID != owner.ID && o.IsActive))
            throw new ArgumentException("O usuário já possui um responsável ativo cadastrado.");
    }
    private async Task<Owner> ValidateOwnerExistsByIdAsync(int idOwner)
    {
        var ownerEntity = await _ownerRepo.GetByIdAsync(idOwner);

        if (ownerEntity == null)
            throw new KeyNotFoundException("Responsável não localizado.");

        return ownerEntity;
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