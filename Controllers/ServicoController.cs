
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaoBeleza.Data;
using SalaoBeleza.Models;

namespace SalaoBeleza.Controllers
{
    public class ServicoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar serviços
        public async Task<IActionResult> Index()
        {
            var servicos = await _context.Servicos.ToListAsync();

            return View(servicos);
        }

        // Abrir formulário de cadastro
        public IActionResult Create()
        {
            return View();
        }

        // Salvar novo serviço
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Servico servico)
        {
            if (ModelState.IsValid)
            {
                _context.Servicos.Add(servico);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(servico);
        }

        // Abrir formulário de edição
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos.FindAsync(id);

            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        // Salvar alterações

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Servico servico)
        {
            if (id != servico.id_servico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var servicoBanco = await _context.Servicos.FindAsync(id);

                if (servicoBanco == null)
                {
                    return NotFound();
                }

                servicoBanco.nome = servico.nome;
                servicoBanco.preco = servico.preco;
                servicoBanco.duracao_min = servico.duracao_min;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(servico);
        }


        // Confirmar exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos
                .FirstOrDefaultAsync(s => s.id_servico == id);

            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        // Excluir serviço
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);

            if (servico != null)
            {
                _context.Servicos.Remove(servico);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
