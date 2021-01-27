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

namespace League.Pages.Teams
{
    public class IndexModel : PageModel
    {

        private readonly LeagueContext _context;

        public IndexModel(LeagueContext context)
        {
            _context = context;
        }


        public List<Team> Teams { get; set; }
        public List<Conference> Conferences { get; set; }
        public List<Division> Divisions { get; set; }



        [BindProperty(SupportsGet = true)]
        public string FavoriteTeam { get; set; }

        public SelectList AllTeams { get; set; }



        public List<Division> MatchingDivision(string id)
        {
            return Divisions.Where(d => d.ConferenceId == id).ToList();
        }

        public List<Team> MatchingTeams(string id)
        {
            return Teams.Where(t => t.DivisionId == id).ToList();
        }



        public async Task OnGetAsync()
        {
            Teams = await _context.Teams.ToListAsync();
            Conferences = await _context.Conferences.ToListAsync();
            Divisions = await _context.Divisions.ToListAsync();

            // make a list of teams for the favorite select dropdown

            IQueryable<string> teamQuery = from t in _context.Teams
                                           orderby t.TeamId
                                           select t.TeamId;

            if (FavoriteTeam != null)
            {
                HttpContext.Session.SetString("_Favorite", FavoriteTeam);
            }
            else
            {
                FavoriteTeam = HttpContext.Session.GetString("_Favorite");
            }

            AllTeams = new SelectList(await teamQuery.ToListAsync());
        }
    }
}
