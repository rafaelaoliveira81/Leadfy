using Application.DTO;
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
    public async Task<int> AddAsync(OwnerRequest request)
    {
        await ValidateOwnerRequest(request);

        var owner = MapToOwnerRequest(request);

        return await _ownerRepo.AddAsync(owner);
    }

    public async Task<OwnerResponse> GetByIdAsync(int idOwner)
    {
        var owner = await ValidateOwnerExistsByIdAsync(idOwner);

        return MapToOwnerResponse(owner);
    }

    public async Task<IEnumerable<OwnerResponse>> GetAllAsync(bool? statusOwner)
    {
        var owners = await _ownerRepo.GetAllAsync(statusOwner);

        var response = owners.Select(o => MapToOwnerResponse(o)).ToList();

        return response;
    }

    public async Task UpdateAsync(OwnerRequest request)
    {
        var ownerEntity = await ValidateOwnerExistsByIdAsync(request.Id);

        await ValidateOwnerRequest(request);

        ownerEntity.Name = request.Name;
        ownerEntity.UserID = request.UserID;

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
    private async Task ValidateOwnerRequest(OwnerRequest owner)
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

        if (userEntity.Owners.Any(o => o.ID != owner.Id && o.IsActive))
            throw new ArgumentException("O usuário vinculado ao responsável já possui um responsável ativo.");
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

    private static Owner MapToOwnerRequest(OwnerRequest request)
    {
        return new Owner
        {
            Name = request.Name,
            UserID = request.UserID,
            IsActive = true
        };
    }

    private static OwnerResponse MapToOwnerResponse(Owner owner)
    {
        return new OwnerResponse
        {
            ID = owner.ID,
            Name = owner.Name,
            UserID = owner.UserID,
            IsActive = owner.IsActive
        };
    }

    #endregion
}