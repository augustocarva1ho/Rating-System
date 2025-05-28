using Microsoft.AspNetCore.Mvc;
using ApiAvaliacao.Data;
using ApiAvaliacao.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAvaliacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AvaliacaoContext _context;

        public AvaliacoesController(AvaliacaoContext context)
        {
            _context = context;
        }

        [HttpPost("avaliacao")]
        public async Task<IActionResult> PostAvaliacao([FromBody] Avaliacao avaliacao)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Dados inválidos"); // Log
                return BadRequest("Dados inválidos");
            }

            bool existeRegistro = await _context.Avaliacao
                .AnyAsync(a => a.SenhaAval == avaliacao.SenhaAval && a.DataAval == avaliacao.DataAval && a.TipoAval == avaliacao.TipoAval);

            if (existeRegistro)
            {
                // Retorna um código de status HTTP 409 Conflict
                return Conflict("essa senha já foi usada para avaliar hoje.");
            }

            // Buscar o valor mais recente de idAtendimento no banco de dados
            var ultimoIdAtendimento = await _context.Avaliacao
                .OrderByDescending(r => r.idAtendimento)
                .Select(r => r.idAtendimento)
                .FirstOrDefaultAsync();

            // Incrementar o valor de idAtendimento para a próxima avaliação
            var novoIdAtendimento = ultimoIdAtendimento + 1;
            avaliacao.idAtendimento = novoIdAtendimento;

            // bool senhaInexistente = !await _context.Senhas
            //  .AnyAsync(s => s.Senha == avaliacao.SenhaAval);

            // if(senhaInexistente)
            // {
            //     return Conflict("essa senha não existe.");
            // }
            
            Console.WriteLine("Recebendo dados..."); // Log
            _context.Avaliacao.Add(avaliacao);
            await _context.SaveChangesAsync();

            Console.WriteLine("Dados armazenados com sucesso"); // Log
            return Ok(new { success = true, message = "Dados recebidos e armazenados com sucesso" });
        }

        [HttpPost("resposta")]
        public async Task<IActionResult> PostRespostas([FromBody] IEnumerable<RespostaDTO> respostasDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obtém a senha e a data da avaliação para verificação
            string senha = respostasDTO.FirstOrDefault()?.SenhaAval;
            string dataAval = DateTime.Now.ToString("yyyy-MM-dd");
            int? tipoAval = respostasDTO.FirstOrDefault()?.TipoAval;

            // Verifica se já existe um registro com a senha e data fornecida
            bool existeRegistro = await _context.Avaliacao
                .AnyAsync(u => u.SenhaAval == senha && u.DataAval == dataAval && u.TipoAval == tipoAval);

            if (existeRegistro)
            {
                // Retorna um código de status HTTP 409 Conflict com uma mensagem explicativa
                return Conflict("Essa senha já foi usada para avaliar hoje.");
            }

            // Buscar o valor mais recente de idAtendimento no banco de dados
            var ultimoIdAtendimento = await _context.Avaliacao
                .OrderByDescending(r => r.idAtendimento)
                .Select(r => r.idAtendimento)
                .FirstOrDefaultAsync();

            // Incrementar o valor de idAtendimento para a próxima avaliação
            var novoIdAtendimento = ultimoIdAtendimento + 1;
            
            var idsPerguntas = respostasDTO.Select(r => r.IdPergunta).Distinct().ToList();
            
            // Verifica se todas as perguntas existem
            var perguntasExistentes = await _context.Perguntas
                .Where(p => idsPerguntas.Contains(p.IdPergunta))
                .Select(p => p.IdPergunta)
                .ToListAsync();

            if (idsPerguntas.Count != perguntasExistentes.Count)
            {
                return BadRequest(new { success = false, message = "Uma ou mais perguntas não existem." });
            }

            foreach (var respostaDTO in respostasDTO)
            {
                var resposta = new Avaliacao
                {
                    IdPergunta = respostaDTO.IdPergunta,
                    NotaAval = respostaDTO.NotaAval,
                    DataAval = DateTime.Now.ToString("yyyy-MM-dd"),
                    SenhaAval = respostaDTO.SenhaAval,
                    UnidadeAval = respostaDTO.UnidadeAval,
                    idAtendimento = novoIdAtendimento,
                    TipoAval = 2
                };

                _context.Avaliacao.Add(resposta);
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Respostas salvas com sucesso!" });
        }


        [HttpPost("respostas-teste")]
        public async Task<IActionResult> InserirRespostasTeste()
        {
            try
            {
                // Cria três respostas com notas de 1 a 4 para as perguntas existentes
                var respostas = new List<Respostas>
                {
                    new Respostas { IdPergunta = 1, NotaResp = 4, SenhaResp = "AA0001", UnidadeResp = "MB" },
                    new Respostas { IdPergunta = 2, NotaResp = 3, SenhaResp = "AA0001", UnidadeResp = "MB" },
                    new Respostas { IdPergunta = 3, NotaResp = 2, SenhaResp = "AA0001", UnidadeResp = "MB" }
                };

                // Adiciona as respostas ao contexto
                await _context.Respostas.AddRangeAsync(respostas);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Respostas inseridas com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao inserir respostas.", error = ex.Message });
            }

        }
        [HttpGet("pergunta/{id}")]
        public async Task<ActionResult<Perguntas>> GetPergunta(int id)
        {
            var pergunta = await _context.Perguntas
                .Where(p => p.IdPergunta == id && p.StatusPergunta == true)
                .FirstOrDefaultAsync();

            if (pergunta == null)
            {
                return NotFound(new { message = "Pergunta não encontrada" });
            }

            return Ok(pergunta);
        }
        [HttpGet("perguntas-ativas")]
        public async Task<ActionResult<IEnumerable<Perguntas>>> GetPerguntasAtivas()
        {
            var perguntas = await _context.Perguntas
                .Where(p => p.StatusPergunta == true)
                .ToListAsync();

            if (perguntas == null || !perguntas.Any())
            {
                return NotFound(new { message = "Nenhuma pergunta ativa encontrada" });
            }

            return Ok(perguntas);
        }
        [HttpGet("total-perguntas-count")]
        public async Task<ActionResult<int>> GetTotalPerguntasCount()
        {
            int count = await _context.Perguntas
                .CountAsync(p => p.IdPergunta > 0);

            return Ok(count);
        }
        [HttpGet("resposta/{id}")]
        public async Task<IActionResult> GetResposta(int id)
        {
            var resposta = await _context.Respostas.FindAsync(id);

            if (resposta == null)
            {
                return NotFound();
            }

            return Ok(resposta);
        }

        [HttpGet("respostas")]
        public async Task<IActionResult> GetRespostas()
        {
            var respostas = await _context.Respostas
                .Include(r => r.Perguntas) // Inclui os dados da pergunta associada
                .Select(r => new
                {
                    r.IdResp,
                    r.IdPergunta,
                    r.Perguntas.TextoPergunta, // Inclui o texto da pergunta
                    r.NotaResp,
                    r.DataResp,
                    r.SenhaResp,
                    r.UnidadeResp,
                    NomeUnidade = r.Unidades.NomeUni
                })
                .ToListAsync();

            return Ok(respostas);
        }
        [HttpGet]
        public async Task<IActionResult> GetAvaliacoes()
        {
            var avaliacoes = await _context.Avaliacao
            .Include(a => a.Unidades) // Faz a junção com a tabela Unidades
            .Select(a => new
            {
                a.IdAval,
                a.SenhaAval,
                a.NotaAval,
                a.UnidadeAval,
                NomeUnidade = a.Unidades.NomeUni // Seleciona o nome da unidade
            })
            .ToListAsync();
            return Ok(avaliacoes);
        }
        [HttpGet("media")]
        public async Task<IActionResult> GetMedia()
        {
            try
            {
                // Calcula a média dos valores da coluna numérica NotaAval
               if (await _context.Avaliacao.AnyAsync())
                {
                    // Calcula a média dos valores da coluna numérica NotaAval
                    var media = await _context.Avaliacao
                        .AverageAsync(a => a.NotaAval);

                    return Ok(new { success = true, media });
                }
                else
                {
                    return Ok(new { success = true, media = 0 }); // Retorna média 0 se não houver registros
                }
            }
            catch (Exception ex)
            {
                // Retorna uma resposta com erro caso ocorra uma exceção
                return StatusCode(500, new { success = false, message = "Erro ao calcular a média", error = ex.Message });
            }
        }
        [HttpGet("senha-existe")]
        public async Task<IActionResult> VerificarSenhas([FromQuery] string senha, [FromQuery] string dataAval)
        {
            // Converte a string dataAval para DateTime

            Console.WriteLine($"Verificando senha: {senha}, Data: {dataAval}");

            // Verifica se já existe um registro com a mesma senha e data
            var avaliacao = await _context.Senhas
                .Where(s => s.Senha == senha && s.DataSenha == dataAval)
                .FirstOrDefaultAsync();

            if (avaliacao == null)
            {                
                // Retorna um código de status HTTP 404 (Conflict) com uma mensagem explicativa
                return NotFound("Essa ainda não foi chamada.");
            } 
            if (avaliacao.StatusSenha != 3) //status 3 = senha finalizada
            {
                Console.WriteLine($"Este atendimento ainda não foi finalizado");
                // Retorna um código de status HTTP 403 (Forbidden) se a senha não for válida
                return StatusCode(403,"Senha não finalizada.");                
            }
            
            // Caso não exista o registro, retorna um código HTTP 200 (OK)
            return Ok("Essa senha ainda não foi usada para avaliar hoje.");
        }
    }
}
