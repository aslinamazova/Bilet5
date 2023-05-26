using Anyar.Areas.Admin.ViewModels;
using Anyar.DAL;
using Anyar.Models;
using Bizland.Utilities.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anyar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.OrderByDescending(t => t.Id).ToListAsync();
            return View(teams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamVM teamVM)
        {
            if (!ModelState.IsValid) return View(teamVM);
            if (!teamVM.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileMustBeImageType);
                return View(teamVM);
            }
            if (!teamVM.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileSizeMustLessThan200KB);
                return View(teamVM);
            }

            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
            string fileName = await teamVM.Photo.SaveAsync(rootPath);

            Team team = new Team
            {
                Name = teamVM.Name,
                Position = teamVM.Position,

                ImagePath = fileName
            };
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Team team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", team.ImagePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Team team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();
            UpdateTeamVM updateTeamVM = new UpdateTeamVM()
            {
                Id = id,
                Name = team.Name,
                Profession = team.Profession
            };
            return View(updateTeamVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTeamVM teamVM)
        {
            if (!ModelState.IsValid) return View(teamVM);
            if (!teamVM.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileMustBeImageType);
                return View(teamVM);
            }
            if (!teamVM.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileSizeMustLessThan200KB);
                return View(teamVM);
            }
            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
            Team team = await _context.Teams.FindAsync(teamVM.Id);
            string filePath = Path.Combine(rootPath, team.ImagePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            string newFileName = await teamVM.Photo.SaveAsync(rootPath);
            team.ImagePath = newFileName;
            team.Name = teamVM.Name;
            team.Profession = teamVM.Profession;
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }


}
