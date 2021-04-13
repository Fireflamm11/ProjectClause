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
    public class BattlesModel : PageModel
    {
        public List<BattleStatus> RoundStati = new();

        public BattlesModel()
        {
            RoundStati = RandomBattleGenerator.GetCurrentStatus();
        }

        public void OnPostNextRound()
        {
            RoundStati = RandomBattleGenerator.NextRound();
        }

        private void BattleOver()
        {
            
        }

        public void OnPostAddBattle()
        {
            RandomBattleGenerator.AddNewBattle();
            RoundStati = RandomBattleGenerator.GetCurrentStatus();
        }
    }
}
