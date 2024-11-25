using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.General;

public class RollTheDicePageModel
    : MawFormPageModel<RollTheDiceForm>
{
    public List<int>? ThrowCounts { get; private set; }
    public IEnumerable<int>? WinnerList
    {
        get
        {
            if (ThrowCounts == null)
            {
                return null;
            }

            // so far have not gotten the linq to work...
            //return ThrowCounts.Select((idx, num) => idx + 1)
            //                  .Where((num, idx) => num == ThrowCounts.Max());

            int max = 0;
            var winnerList = new List<int>();

            for (int i = 0; i < Form.Sides; i++)
            {
                if (ThrowCounts[i] > max)
                {
                    winnerList.Clear();
                    winnerList.Add(i + 1);
                    max = ThrowCounts[i];
                }
                else if (ThrowCounts[i] == max)
                {
                    winnerList.Add(i + 1);
                }
            }

            return winnerList;
        }
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            ThrowDice();
            SubmitSuccess = true;
        }

        return Page();
    }

    public void ThrowDice()
    {
        ThrowCounts = [.. new int[Form.Sides]];

        for (int i = 0; i < Form.Throws; i++)
        {
            ThrowCounts[RandomNumberGenerator.GetInt32(Form.Sides)]++;
        }
    }
}

public class RollTheDiceForm
{
    [Required(ErrorMessage = "Please enter the number of sides")]
    [Range(1, 20)]
    public int Sides { get; set; } = 5;

    [Required(ErrorMessage = "Please enter the number of throws")]
    [Range(1, 1000000)]
    public int Throws { get; set; } = 10_000;
}
