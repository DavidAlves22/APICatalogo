using APICatalogo.Domain;
using APICatalogo.Domain.DTOs.Produtos;
using APICatalogo.Repositories.UnitOfWork;
using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapsterMapper.IMapper _mapsterMapper;

        public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper, MapsterMapper.IMapper mapsterMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mapsterMapper = mapsterMapper;
        }

        //IActionResult - Retorna o resultado da ação, independente do tipo de retorno (Precisa retornar uma ActionResult sempre)
        //ActionResult - Retorna o resultado da ação, mas não importa o tipo de retorno (pode ser um objeto ou uma string)
        //ActionResult<T> - Retorna o resultado da ação, mas importa o tipo de retorno (pode ser um objeto ou uma string)
        //T - Tipo de retorno da ação (pode ser um objeto ou uma string)

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAsync()
        {
            try
            {
                var produtos = await _unitOfWork.ProdutoRepository.GetAsync(); // Chama o repositório para obter os produtos

                if (produtos is null)
                    return NotFound("Produtos não encontrados");

                var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos); // Mapeia os produtos para o DTO usando AutoMapper
                                                                                  // OU
                var produtosDTO2 = produtos.Adapt<IEnumerable<ProdutoDTO>>(); // Mapeia os produtos para o DTO usando Mapster
                var produtosDTO3 = _mapsterMapper.Map<IEnumerable<ProdutoDTO>>(produtos); // Mapeia os produtos para o DTO usando Mapster com MapsterMapper

                return Ok(produtosDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //{id:int:length(2)} - O id deve ser um inteiro e ter 2 dígitos (Existem outras formas de restringir o id, como por exemplo: {id:int:min(1)} - O id deve ser um inteiro e maior que 1)
        [HttpGet("{id:int:length(2)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> GetAsync(int id)
        {
            if (id == 0)
                return BadRequest("Id inválido");

            var produto = await _unitOfWork.ProdutoRepository.GetPorIdAsync(id); // Chama o repositório para obter o produto

            if (produto is null)
                return NotFound("Produto não encontrado");

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostAsync(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest("Produto inválido");

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoCriado = _unitOfWork.ProdutoRepository.Incluir(produto);

            if (produtoCriado is null)
                return BadRequest("Produto inválido");

            await _unitOfWork.CommitAsync();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(produtoCriado);

            return new CreatedAtRouteResult("Post", novoProdutoDTO);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> PutAsync(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest("Produto inválido");
            if (produtoDTO.Id == 0)
                return BadRequest("Produto inválido");

            var produto = _mapper.Map<Produto>(produtoDTO);
            var alterou = _unitOfWork.ProdutoRepository.Alterar(produto);

            if (!alterou)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Produto não encontrado para alteração");

            await _unitOfWork.CommitAsync();

            return Ok("Produto alterado");
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            if (id == 0)
                return BadRequest("Produto inválido");

            var deletado = _unitOfWork.ProdutoRepository.Excluir(id);

            if (!deletado)
                return NotFound("Produto não encontrado para exclusão");

            await _unitOfWork.CommitAsync();

            return Ok("Produto deletado");
        }

        //Paginação do retorno dos registro para evitar que a API retorne muitos registros de uma vez só
        [HttpGet("paginado")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetComPaginacao([FromQuery] ParametrosProduto parametros)
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetComPaginacaoAsync(parametros); // Chama o repositório para obter os produtos com paginação

            var produtosDTO = produtos.Adapt<IEnumerable<ProdutoDTO>>(); // Mapeia os produtos para o DTO usando Mapster

            return Ok(produtos);
        }
    }
}
