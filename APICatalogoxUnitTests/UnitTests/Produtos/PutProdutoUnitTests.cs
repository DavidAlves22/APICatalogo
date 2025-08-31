using APICatalogo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogoxUnitTests.UnitTests.Produtos
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.unitOfWork, controller.mapper, controller.mapsterMapper);
        }
    }
}
