using PrimeiraApi.Models;

namespace PrimeiraApi.Rotas;

public static class PessoaRotas
{
    public static List<Pessoa> Pessoas = new List<Pessoa>()
    {
        new Pessoa(Guid.NewGuid(), "Neymar"),
        new Pessoa(Guid.NewGuid(), "Cristiano"),
        new Pessoa(Guid.NewGuid(), "Messi"),
    };
    public static void MapPessoaRotas(this WebApplication app)
    {
        app.MapGet("pessoas", () => Pessoas);
        
        app.MapGet("pessoas/{nome}", 
            (string nome) => Pessoas.Find(x => x.Nome == nome));

        app.MapPost("pessoas", (Pessoa pessoa) =>
        {
            Pessoas.Add(pessoa);
            return pessoa;
        });
        
        app.MapPut("pessoas/{id:guid}", (Guid id, Pessoa pessoa) =>
        {
            var encontrado = Pessoas.Find(x => x.Id == id);
            
            if (encontrado == null) return Results.NotFound();
            
            encontrado.Nome = pessoa.Nome;
            
            return Results.Ok(encontrado);
        });
    }
}