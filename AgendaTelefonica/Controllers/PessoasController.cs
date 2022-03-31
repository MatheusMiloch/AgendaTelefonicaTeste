using AgendaTelefonica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaTelefonica.Controllers
{
    public class PessoasController : Controller
    {
        private readonly Contexto? _contexto;
        private readonly IWebHostEnvironment? _webHostEnvironment;

        public PessoasController(Contexto? contexto, IWebHostEnvironment? webHostEnvironment)
        {
            _contexto = contexto;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Pessoas.ToListAsync());
        }

        [HttpGet]
        public IActionResult CriarPessoa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarPessoa(Pessoa pessoa, IFormFile foto)
        {
            if(pessoa != null)
            {
                if (foto != null)
                {
                    string diretorio = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    string nomeFoto = Guid.NewGuid().ToString() + foto.FileName;

                    using (FileStream fileStream = new FileStream(Path.Combine(diretorio, nomeFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                        pessoa.Foto = "~/img/" + nomeFoto;
                    }
                }
                else
                {
                    pessoa.Foto = "~/img/usuario.png";
                }

                _contexto.Add(pessoa);
                await _contexto.SaveChangesAsync();
                TempData["ContatoNovo"] = $"Contato com nome {pessoa.Name} {pessoa.Sobrenome} incluído com sucesso";

                return RedirectToAction(nameof(Index));
            }

            return View(pessoa);
        }

        [HttpGet]
        public async Task<IActionResult> AtualizarPessoa(int pessoaiId)
        {
            Pessoa pessoa = await _contexto.Pessoas.FindAsync(pessoaiId);

            if (pessoa == null)
            {
                return NotFound();
            }

            TempData["Foto"] = pessoa.Foto;
            return View(pessoa);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarPessoa(Pessoa pessoa, IFormFile foto)
        {
            if (pessoa != null)
            {
                if (foto != null)
                {
                    string diretorio = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    string nomeFoto = Guid.NewGuid().ToString() + foto.FileName;

                    using(FileStream fileStream = new FileStream(Path.Combine(diretorio, nomeFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                        pessoa.Foto = "~/img/" + nomeFoto;
                        TempData["Foto"] = null;
                    }
                }
                else
                {
                    pessoa.Foto = TempData["Foto"].ToString();
                }

                _contexto.Update(pessoa);
                await _contexto.SaveChangesAsync();
                TempData["ContatoAtualizado"] = $"Contato com nome {pessoa.Name} {pessoa.Sobrenome} atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }

            return View(pessoa);
        }

        [HttpPost]
        public async Task<JsonResult> ExcluirPessoa(int pessoaId)
        {
            Pessoa pessoa = await _contexto.Pessoas.FindAsync(pessoaId);
            _contexto.Pessoas.Remove(pessoa);
            await _contexto.SaveChangesAsync();

            TempData["ContatoExcluido"] = $"Contato com nome {pessoa.Name} {pessoa.Sobrenome} excluído com sucesso!";

            return Json(true);
        }
    }
}
