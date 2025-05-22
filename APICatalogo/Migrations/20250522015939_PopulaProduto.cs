using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" INSERT INTO PRODUTOS (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) 
                      VALUES('Coca cola','Bebida gasosa',8,'refrigerante.jpg',20,NOW(),1);");

            migrationBuilder.Sql(@" INSERT INTO PRODUTOS (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) 
                      VALUES('Pepsi','Bebida gasosa',8,'refrigerante.jpg',20,NOW(),1);");

            migrationBuilder.Sql(@" INSERT INTO PRODUTOS (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) 
                      VALUES('Polenta Frita','Fritura de fubá',18.9,'polentafrita.jpg',10,NOW(),2);");

            migrationBuilder.Sql(@" INSERT INTO PRODUTOS (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) 
                      VALUES('Brownie','Bolo de chocolate',9.90,'polentafrita.jpg',15,NOW(),3);");

            migrationBuilder.Sql(@" INSERT INTO PRODUTOS (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) 
                      VALUES('Palha Italiana','Chocolate com bolacha',9.90,'polentafrita.jpg',15,NOW(),3);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" Delete from PRODUTOS");
        }
    }
}
