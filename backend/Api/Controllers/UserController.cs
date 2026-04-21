using Microsoft.AspNetCore.Mvc;
using Models.Response;
using Models.Request;
using Domain.Entities;
using Domain.Enuns;

/// <summary>
/// Controller responsável pelos endpoints de gerenciamento de usuários.
/// </summary>
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserApp _userApp;

    /// <summary>
    /// Inicializa uma nova instância do controller de usuários.
    /// </summary>
    /// <param name="userApp">Serviço de aplicação responsável pelas operações de usuário.</param>
    public UserController(IUserApp userApp)
    {
        _userApp = userApp;
    }

    /// <summary>
    /// Adiciona um novo usuário ao sistema.
    /// </summary>
    /// <param name="user">Dados necessários para criação do usuário.</param>
    /// <returns>
    /// Retorna status 201 com o identificador do usuário criado.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    /// <remarks>
    /// Este endpoint recebe os dados de entrada, monta a entidade de domínio
    /// e delega o processo de cadastro para a camada de aplicação.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Add([FromBody] UserAdd user)
    {
        try
        {
            var userRepository = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Role = (UserRole)user.IdRole
            };

            var idUser = await _userApp.AddAsync(userRepository, user.Password);

            return CreatedAtAction(nameof(GetById), new { id = idUser }, new { id = idUser });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém um usuário pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do usuário.</param>
    /// <returns>
    /// Retorna status 200 com os dados do usuário encontrado.
    /// Retorna status 404 quando o usuário não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var userRepository = await _userApp.GetByIdAsync(id);

            var userResponse = new UserResponse()
            {
                ID = userRepository.ID,
                Name = userRepository.Name,
                Email = userRepository.Email,
                Role = new UserRoleResponse()
                {
                    ID = (int)userRepository.Role,
                    Name = userRepository.Role.ToString()
                },
                IsActive = userRepository.IsActive,
                CreatedAt = userRepository.CreatedAt
            };

            return Ok(userResponse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém um usuário pelo e-mail.
    /// </summary>
    /// <param name="email">E-mail do usuário.</param>
    /// <returns>
    /// Retorna status 200 com os dados do usuário encontrado.
    /// Retorna status 400 quando o e-mail informado é inválido.
    /// Retorna status 404 quando nenhum usuário é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet("email/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetByEmail([FromRoute] string email)
    {
        try
        {
            var userRepository = await _userApp.GetByEmailAsync(email);

            var userResponse = new UserResponse()
            {
                ID = userRepository.ID,
                Name = userRepository.Name,
                Email = userRepository.Email,
                Role = new UserRoleResponse()
                {
                    ID = (int)userRepository.Role,
                    Name = userRepository.Role.ToString()
                },
                IsActive = userRepository.IsActive,
                CreatedAt = userRepository.CreatedAt
            };

            return Ok(userResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém usuários cadastrados. Permite filtrar por status de ativação ou nome.
    /// </summary>
    /// <param name="isActive">Filtra por status de ativação (opcional).</param>
    /// <param name="name">Filtra por nome contendo o valor (opcional).</param>
    /// <returns>
    /// Retorna status 200 com a coleção de usuários.
    /// Retorna status 400 quando os parâmetros informados são inválidos.
    /// Retorna status 404 quando nenhum usuário é localizado na busca por nome.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get([FromQuery] bool? isActive, [FromQuery] string name)
    {
        try
        {
            IEnumerable<User> users;

            if (isActive.HasValue)
            {
                users = await _userApp.GetAllByStatusAsync(isActive.Value);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                users = await _userApp.GetByNameContainingAsync(name);
            }
            else
            {
                users = await _userApp.GetAllAsync();
            }

            var usersResponse = users.Select(u => new UserResponse()
            {
                ID = u.ID,
                Name = u.Name,
                Email = u.Email,
                Role = new UserRoleResponse()
                {
                    ID = (int)u.Role,
                    Name = u.Role.ToString()
                },
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });

            return Ok(usersResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza os dados cadastrais de um usuário.
    /// </summary>
    /// <param name="id">Identificador do usuário a ser atualizado.</param>
    /// <param name="userRequest">Dados atualizados do usuário.</param>
    /// <returns>
    /// Retorna status 204 quando a atualização é realizada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 404 quando o usuário não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UserUpdate userRequest)
    {
        try
        {
            var user = new User()
            {
                ID = id,
                Name = userRequest.Name,
                Email = userRequest.Email,
                Role = (UserRole)userRequest.IdRole
            };

            await _userApp.UpdateAsync(user);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza a senha de um usuário.
    /// </summary>
    /// <param name="id">Identificador do usuário.</param>
    /// <param name="request">Dados necessários para alteração de senha.</param>
    /// <returns>
    /// Retorna status 204 quando a senha é alterada com sucesso.
    /// Retorna status 400 quando os dados informados são inválidos.
    /// Retorna status 401 quando a senha atual informada é inválida.
    /// Retorna status 404 quando o usuário não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPatch("{id:int}/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdatePassword([FromRoute] int id, [FromBody] UserUpdatePassword request)
    {
        try
        {
            await _userApp.UpdatePasswordAsync(id, request.CurrentPassword, request.NewPassword);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove um usuário do sistema.
    /// </summary>
    /// <param name="id">Identificador do usuário a ser removido.</param>
    /// <returns>
    /// Retorna status 204 quando a exclusão é realizada com sucesso.
    /// Retorna status 404 quando o usuário não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await _userApp.DeleteAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Desativa um usuário.
    /// </summary>
    /// <param name="id">Identificador do usuário a ser desativado.</param>
    /// <returns>
    /// Retorna status 204 quando a desativação é realizada com sucesso.
    /// Retorna status 404 quando o usuário não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPatch("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Deactivate([FromRoute] int id)
    {
        try
        {
            await _userApp.DeactivateAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Ativa um usuário.
    /// </summary>
    /// <param name="id">Identificador do usuário a ser ativado.</param>
    /// <returns>
    /// Retorna status 204 quando a ativação é realizada com sucesso.
    /// Retorna status 404 quando o usuário não é localizado.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpPatch("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Activate([FromRoute] int id)
    {
        try
        {
            await _userApp.ActivateAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Lista todos os perfis de usuário disponíveis.
    /// </summary>
    /// <remarks>
    /// Endpoint utilizado para obter os tipos de papéis (roles) que podem ser atribuídos a um usuário.
    /// </remarks>
    /// // <returns>
    /// Retorna status 200 com a lista de roles disponíveis.
    /// Retorna status 500 em caso de erro interno.
    /// </returns>
    [HttpGet("roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetAvailableRoles()
    {
        try
        {
            var roles = Enum.GetValues<UserRole>()
                .Select(role => new UserRoleResponse
                {
                    ID = (int)role,
                    Name = role.ToString(),
                    DisplayName = role switch
                    {
                        UserRole.Admin => "Administrador",
                        UserRole.Manager => "Gerente",
                        UserRole.SalesRepresentative => "Representante Comercial",
                        UserRole.CustomerSupport => "Atendimento ao Cliente",
                        UserRole.RegularUser => "Usuário Comum",
                        _ => role.ToString()
                    }
                })
                .ToList();

            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
