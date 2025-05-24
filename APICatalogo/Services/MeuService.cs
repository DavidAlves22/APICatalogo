namespace APICatalogo.Services
{
    public class MeuService : IMeuService
    {
        public string GetMensagem(string nome)
        {
            return $"Bem vindo, {nome}";
        }
    }
}