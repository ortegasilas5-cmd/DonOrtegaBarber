
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaoBeleza.Data;
using SalaoBeleza.Models;

namespace SalaoBeleza.Controllers
{
    public class ProfissionalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfissionalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar profissionais
        public async Task<IActionResult> Index()
        {
            var profissionais = await _context.Profissionais.ToListAsync();

            return View(profissionais);
        }

        // Abrir formulário de cadastro
        public IActionResult Create()
        {
            return View();
        }

        // Salvar novo profissional
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profissional profissional)
        {
            if (ModelState.IsValid)
            {
                _context.Profissionais.Add(profissional);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profissional);
        }

        // Abrir formulário de edição
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profissional = await _context.Profissionais.FindAsync(id);

            if (profissional == null)
            {
                return NotFound();
            }

            return View(profissional);
        }

        // Salvar alterações
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profissional profissional)
        {
            if (id != profissional.id_profissional)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var profissionalBanco = await _context.Profissionais.FindAsync(id);

                if (profissionalBanco == null)
                {
                    return NotFound();
                }

                profissionalBanco.nome = profissional.nome;
                profissionalBanco.especialidade = profissional.especialidade;
                profissionalBanco.cpf = profissional.cpf;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profissional);
        }



        // Visualizar detalhes do profissional
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profissional = await _context.Profissionais
                .FirstOrDefaultAsync(p => p.id_profissional == id);

            if (profissional == null)
            {
                return NotFound();
            }

            return View(profissional);
        }


        // Confirmar exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profissional = await _context.Profissionais
                .FirstOrDefaultAsync(p => p.id_profissional == id);

            if (profissional == null)
            {
                return NotFound();
            }

            return View(profissional);
        }

        // Excluir profissional
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profissional = await _context.Profissionais.FindAsync(id);

            if (profissional != null)
            {
                _context.Profissionais.Remove(profissional);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
