using Application.DTO;
using Domain.Entities;
using Domain.Enuns;

namespace Application;

public class PromptApp : IPromptApp
{
    private readonly IPromptRepo _promptRepo;
    public PromptApp(IPromptRepo promptRepo)
    {
        _promptRepo = promptRepo;
    }
    public async Task<int> AddAsync(PromptRequest request)
    {
        await ValidatePromptInformation(request);

        var prompt = MapToPromptRequest(request);

        return await _promptRepo.AddAsync(prompt);
    }

    public async Task<PromptResponse> GetByIdAsync(int idPrompt)
    {
        var prompt = await ValidatePromptExistsByIdAsync(idPrompt);

        return MapToPromptResponse(prompt);
    }

    public async Task<PromptPagedResponse> GetAllAsync(bool? status, int pagina, int quantidadePorPagina)
    {
        var prompt = await _promptRepo.GetPagedAsync(status, pagina, quantidadePorPagina);

        var response = prompt.Dados.Select(MapToPromptResponse).ToList();
        return new PromptPagedResponse
        {
            TotalRegistros = prompt.TotalRegistros,
            Dados = response
        };
    }

    public async Task UpdateAsync(PromptRequest request)
    {
        var prompt = await ValidatePromptExistsByIdAsync(request.Id);

        await ValidatePromptInformation(request);

        prompt.Title = request.Title;
        prompt.Content = request.Content;

        await _promptRepo.UpdateAsync(prompt);
    }

    public async Task DeleteAsync(int idPrompt)
    {
        var promptEntity = await ValidatePromptExistsByIdAsync(idPrompt);

        await _promptRepo.DeleteAsync(promptEntity);
    }

    public async Task DeactivateAsync(int idPrompt)
    {
        var promptEntity = await ValidatePromptExistsByIdAsync(idPrompt);

        promptEntity.Deactivate();

        await _promptRepo.UpdateAsync(promptEntity);
    }

    public async Task ActivateAsync(int idPrompt)
    {
        var promptEntity = await ValidatePromptExistsByIdAsync(idPrompt);

        promptEntity.Activate();

        await _promptRepo.UpdateAsync(promptEntity);
    }

    #region Métodos auxiliares
    private async Task ValidatePromptInformation(PromptRequest prompt)
    {
        if (prompt == null)
            throw new ArgumentException("Prompt não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(prompt.Title))
            throw new ArgumentException("O titulo do prompt deve ser informado.");

        if (prompt.Title.Length > 150)
            throw new ArgumentException("O titulo do prompt não pode exceder 150 caracteres.");

        if (string.IsNullOrWhiteSpace(prompt.Content))
            throw new ArgumentException("O prompt deve ser informado.");

        if (prompt.Title.Length > 1000)
            throw new ArgumentException("O prompt não pode exceder 1000 caracteres.");
    }

    private async Task<Prompt> ValidatePromptExistsByIdAsync(int idPrompt)
    {
        var prompt = await _promptRepo.GetByIdAsync(idPrompt);

        if (prompt == null)
            throw new KeyNotFoundException("Prompt não localizado.");

        return prompt;
    }

    private static Prompt MapToPromptRequest(PromptRequest request)
    {
        return new Prompt
        {
            Title = request.Title,
            Content = request.Content
        };
    }

    private static PromptResponse MapToPromptResponse(Prompt prompt)
    {
        return new PromptResponse
        {
            Id = prompt.Id,
            Title = prompt.Title,
            Content = prompt.Content,
            IsActive = prompt.IsActive
        };
    }

    #endregion
}
