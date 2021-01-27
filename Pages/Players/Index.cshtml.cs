using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using League.Data;
using League.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace League.Pages.Players
{
    public class IndexModel : PageModel
    {

        private readonly LeagueContext _context;

        public IndexModel(LeagueContext context)
        {
            _context = context;
        }




        public List<Player> Players { get; set; }
        public SelectList Teams { get; set; }
        public SelectList Positions { get; set; }
        public SelectList Colleges { get; set; }
        public string FavoriteTeam { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedTeam { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedPosition { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedCollege { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortField { get; set; } = "Name";




        public string AddPlayerClass(Player player)
        {
            string Class = "d-flex";
            if (player.Depth == 1)
            {
                Class += " starter";
            }
            if (player.TeamId == FavoriteTeam)
            {
                Class += " favorite";
            }
            return Class;
        }



        public async Task OnGetAsync()
        {
            var players = from p in _context.Players
                          select p;

            if (!string.IsNullOrEmpty(SearchString))
            {
                players = players.Where(p => p.Name.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SelectedTeam))
            {
                players = players.Where(p => p.TeamId == SelectedTeam);
            }

            if (!string.IsNullOrEmpty(SelectedPosition))
            {
                players = players.Where(p => p.Position == SelectedPosition);
            }

            if (!string.IsNullOrEmpty(SelectedCollege))
            {
                players = players.Where(p => p.College == SelectedCollege);
            }




            IQueryable<string> teamQuery = from t in _context.Teams
                                           orderby t.TeamId
                                           select t.TeamId;

            Teams = new SelectList(await teamQuery.ToListAsync());

            IQueryable<string> positionQuery = from p in _context.Players
                                               select p.Position;

            Positions = new SelectList(await positionQuery.Distinct().OrderBy(p => p).ToListAsync());

            IQueryable<string> collegeQuery = from c in _context.Players
                                              select c.College;

            Colleges = new SelectList(await collegeQuery.Distinct().OrderBy(c => c).ToListAsync());




            switch (SortField)
            {
                case "Number": players = players.OrderBy(p => p.Number).ThenBy(p => p.TeamId); break;
                case "Name": players = players.OrderBy(p => p.Name).ThenBy(p => p.TeamId); break;
                case "Position": players = players.OrderBy(p => p.Position).ThenBy(p => p.TeamId); break;
            }




            FavoriteTeam = HttpContext.Session.GetString("_Favorite");


            Players = await players.ToListAsync();
        }



        
    }
}
