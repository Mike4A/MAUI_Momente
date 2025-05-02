using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente.Services
{
    static class DebugService
    {
        public static async Task WriteMomentEntries()
        {
            List<Moment> moments = await DatabaseService.Instance.GetMomentsReversedAsync();
            foreach (Moment moment in moments)
            {
                Debug.WriteLine($"{moment.CreatedAt} | {moment.Id} | {moment.Icon} | {moment.Headline} | {moment.Description}");
            }
        }
    }
}
