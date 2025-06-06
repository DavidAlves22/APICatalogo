﻿using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private IProdutoRepository _produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        //IActionResult - Retorna o resultado da ação, independente do tipo de retorno (Precisa retornar uma ActionResult sempre)
        //ActionResult - Retorna o resultado da ação, mas não importa o tipo de retorno (pode ser um objeto ou uma string)
        //ActionResult<T> - Retorna o resultado da ação, mas importa o tipo de retorno (pode ser um objeto ou uma string)
        //T - Tipo de retorno da ação (pode ser um objeto ou uma string)

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _produtoRepository.GetProdutos().ToList(); // Chama o repositório para obter os produtos

            if (produtos is null)
                return NotFound("Produtos não encontrados");

            return Ok(produtos);
        }

        //{id:int:length(2)} - O id deve ser um inteiro e ter 2 dígitos (Existem outras formas de restringir o id, como por exemplo: {id:int:min(1)} - O id deve ser um inteiro e maior que 1)
        [HttpGet("{id:int:length(2)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id) //[BindRequired] exige que o atributo nome seja obrigatório
        {
            var produto = await _produtoRepository.GetProduto(id); // Chama o repositório para obter o produto

            if (produto is null)
                return NotFound("Produto não encontrado");

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Produto inválido");

            var produtoCriado = await _produtoRepository.Incluir(produto);

            if(produtoCriado is null)
                return BadRequest("Produto inválido");

            return Ok(produtoCriado);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> Put(Produto produto)
        {
            if (produto is null)
                return BadRequest("Produto inválido");
            if (produto.Id == 0)
                return BadRequest("Produto inválido");

            var alterou = await _produtoRepository.Alterar(produto);

            if (!alterou)
                return StatusCode(500, "Produto não encontrado para alteração");

            return Ok("Produto alterado");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            if (id == 0)
                return BadRequest("Produto inválido");

            var deletado = await _produtoRepository.Excluir(id);

            if(!deletado)
                return StatusCode(500, "Produto não encontrado para exclusão");

            return Ok("Produto deletado");
        }
    }
}
