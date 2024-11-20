using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Data;
using PrimeiraApi.DTOs;
using PrimeiraApi.Models;

namespace PrimeiraApi.Rotas;

public static class PessoaRotas
{
    public static void MapPessoaRotas(this WebApplication app)
    {
        var rotasPessoas = app.MapGroup("pessoas");
        
        rotasPessoas.MapGet("", async (AppDbContext context, CancellationToken ct) =>
        {
            var pessoas = await context
                .Pessoas
                .Select(pessoa => new PessoaDto(pessoa.Id, pessoa.Nome))
                .ToListAsync(ct);
            return pessoas;
        });

        rotasPessoas.MapGet("{id:guid}", async (Guid id, AppDbContext context, CancellationToken ct) =>
        {
            var pessoa = await context.Pessoas
                .SingleOrDefaultAsync(pessoa => pessoa.Id == id, ct);
            return pessoa;
        });

        rotasPessoas.MapPost("", async (AddPessoaRequest request, AppDbContext context, CancellationToken ct) =>
        {
            var jaExiste = await context.Pessoas
                .AnyAsync(pessoa => pessoa.Nome.StartsWith(request.Nome), ct);

            if (jaExiste) return Results.Conflict("Pessoa já existe!");
            
            var novaPessoa = new Pessoa(request.Nome);
            await context.Pessoas.AddAsync(novaPessoa, ct);
            await context.SaveChangesAsync(ct);

            var pessoaRetorno = new PessoaDto(novaPessoa.Id, novaPessoa.Nome);
            
            return Results.Ok(pessoaRetorno);
        });
        
        rotasPessoas.MapPut("{id:guid}", 
            async (Guid id, UpdatePessoaRequest request, AppDbContext context, CancellationToken ct) =>
            {
                var pessoa = await context.Pessoas
                    .SingleOrDefaultAsync(pessoa => pessoa.Id == id, ct);
                
                if (pessoa == null) 
                    return Results.NotFound();
                
                pessoa.AtualizarNome(request.Nome);
                
                await context.SaveChangesAsync(ct);
                return Results.Ok(new PessoaDto(pessoa.Id, pessoa.Nome));
        });

        rotasPessoas.MapDelete("{id:guid}",
            async (Guid id, AppDbContext context, CancellationToken ct) =>
            {
                var pessoa = await context.Pessoas
                    .SingleOrDefaultAsync(pessoa => pessoa.Id == id, ct);
                
                if (pessoa == null)
                    return Results.NotFound();
                
                context.Pessoas.Remove(pessoa);
                await context.SaveChangesAsync(ct);
                
                return Results.NoContent();
            });
    }
}