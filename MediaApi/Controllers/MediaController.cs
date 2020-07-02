using MediaApi.Domain;
using MediaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.Controllers
{
    public class MediaController : ControllerBase // use ControllerBase for APIs
    {
        MediaDataContext Context;

        public MediaController(MediaDataContext context)
        {
            Context = context;
        }


        // GET /media
        [HttpGet("media")]
        public async Task<IActionResult> GetAllMedia([FromQuery] string kind = "All")
        {
            var query = Context.MediaItems
                .Where(m => m.Removed == false)
                .Select(m => new MediaResponseItem
                {
                    Id = m.Id,
                    Title = m.Title,
                    Consumed = m.Consumed,
                    DateConsumed = m.DateConsumed,
                    Kind = m.Kind,
                    RecommendedBy = m.RecommendedBy
                });
            if (kind != "All")
            {
                query = query.Where(q => q.Kind == kind);
            }
            var response = new GetMediaResponse
            {
                Data = await query.ToListAsync(), // can only use "await" keyword if the method's async
                FilteredBy = kind
            };
            return Ok(response);
        }
    }
}

