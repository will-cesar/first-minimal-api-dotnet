namespace PrimeiraApi.Models;

public class Pessoa
{
    public Guid Id { get; init; }
    public string Nome { get; private set; }

    public Pessoa(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }

    public void AtualizarNome(string nome)
    {
        Nome = nome;
    }
}