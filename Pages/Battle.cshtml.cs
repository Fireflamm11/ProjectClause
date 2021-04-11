using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectClause.Model;
using ProjectClause.Model.Battles;

namespace ProjectClause.Pages
{
    public class BattleModel : PageModel
    {
        public BattleStatus RoundStatus;

        public BattleModel()
        {
            RoundStatus = RandomBattleGenerator.GetCurrentStatus();
        }

        public void OnPostNextRound()
        {
            RoundStatus = RandomBattleGenerator.NextRound();
        }
        public IActionResult OnPostEndBattle()
        {
            return RedirectToPage("./Index");
        }
    }
}
