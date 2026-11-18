
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalaoBeleza.Data;
using SalaoBeleza.Models;

namespace SalaoBeleza.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgendamentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar agendamentos
        public async Task<IActionResult> Index()
        {
            var agendamentos = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .OrderBy(a => a.data_hora)
                .ToListAsync();

            return View(agendamentos);
        }

        // Abrir formulário de cadastro
        public async Task<IActionResult> Create()
        {
            await CarregarListas();
            return View();
        }

        // Salvar novo agendamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                var conflito = await VerificarConflito(agendamento);

                if (conflito)
                {
                    ModelState.AddModelError(
                        "data_hora",
                        "Este profissional já possui um agendamento nesse horário."
                    );
                }
                else
                {
                    _context.Agendamentos.Add(agendamento);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            await CarregarListas(agendamento);
            return View(agendamento);
        }

        // Abrir formulário de edição
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var agendamento = await _context.Agendamentos.FindAsync(id);

            if (agendamento == null)
                return NotFound();

            await CarregarListas(agendamento);
            return View(agendamento);
        }

        // Salvar alterações
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Agendamento agendamento)
        {
            if (id != agendamento.id_agendamento)
                return NotFound();

            if (ModelState.IsValid)
            {
                var conflito = await VerificarConflito(
                    agendamento,
                    id
                );

                if (conflito)
                {
                    ModelState.AddModelError(
                        "data_hora",
                        "Este profissional já possui um agendamento nesse horário."
                    );
                }
                else
                {
                    _context.Entry(agendamento).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            await CarregarListas(agendamento);
            return View(agendamento);
        }

        // Visualizar detalhes
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var agendamento = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(a => a.id_agendamento == id);

            if (agendamento == null)
                return NotFound();

            return View(agendamento);
        }

        // Abrir confirmação de exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var agendamento = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(a => a.id_agendamento == id);

            if (agendamento == null)
                return NotFound();

            return View(agendamento);
        }

        // Confirmar exclusão
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);

            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Carregar clientes, profissionais e serviços
        private async Task CarregarListas(Agendamento? agendamento = null)
        {
            ViewBag.Clientes = new SelectList(
                await _context.Clientes.ToListAsync(),
                "id_cliente",
                "nome",
                agendamento?.id_cliente
            );

            ViewBag.Profissionais = new SelectList(
                await _context.Profissionais.ToListAsync(),
                "id_profissional",
                "nome",
                agendamento?.id_profissional
            );

            ViewBag.Servicos = new SelectList(
                await _context.Servicos.ToListAsync(),
                "id_servico",
                "nome",
                agendamento?.id_servico
            );
        }

        // Verificar conflito de horários
        private async Task<bool> VerificarConflito(
            Agendamento agendamento,
            int? idIgnorado = null)
        {
            var servico = await _context.Servicos
                .FindAsync(agendamento.id_servico);

            if (servico == null)
                return false;

            var inicio = agendamento.data_hora;
            var fim = inicio.AddMinutes(servico.duracao_min);

            var agendamentos = await _context.Agendamentos
                .Include(a => a.Servico)
                .Where(a =>
                    a.id_profissional == agendamento.id_profissional &&
                    a.status != "cancelado" &&
                    (!idIgnorado.HasValue ||
                     a.id_agendamento != idIgnorado.Value))
                .ToListAsync();

            return agendamentos.Any(a =>
                inicio < a.data_hora.AddMinutes(a.Servico!.duracao_min) &&
                fim > a.data_hora
            );
        }
    }
}
