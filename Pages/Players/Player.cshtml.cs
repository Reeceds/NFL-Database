using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using League.Data;
using League.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace League.Pages.Players
{
    public class PlayerModel : PageModel
    {
        private readonly LeagueContext _context;

        public PlayerModel(LeagueContext context)
        {
            _context = context;
        }

        public Player Player { get; set; }



        public async Task OnGetAsync(string id)
        {
            Player = await _context.Players.FindAsync(id);
        }
    }
}
