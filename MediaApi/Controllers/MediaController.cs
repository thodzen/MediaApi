using MediaApi.Domain;
using MediaApi.Helpers;
using MediaApi.Models;
using Microsoft.AspNetCore.Builder;
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
        ISystemTime Current;

        public MediaController(MediaDataContext context, ISystemTime current)
        {
            Context = context;
            Current = current;
        }

        [HttpPost("media/consumed")]
        public async Task<IActionResult> ConsumedMedia([FromBody] PostMediaConsumedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            var media = await Context.MediaItems
                .Where(m => m.Removed == false && m.Id == request.Id)
                .SingleOrDefaultAsync();
            if(media == null)
            {
                return BadRequest("Bad Media"); // Conflict
            }
            else
            {
                media.Consumed = true;
                media.DateConsumed = Current.GetCurrent();
                await Context.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpDelete("media/{id:int}")]
        public async Task<IActionResult> RemoveMediaitem(int id)
        {
            var item = await Context.MediaItems
                .Where(m => m.Removed == false && m.Id == id)
                .SingleOrDefaultAsync();
            if(item != null)
            {
                item.Removed = true;
                await Context.SaveChangesAsync();
            }
            return NoContent(); // this just means a sort of passive-aggressive "fine"
        }

        [HttpPost("media")]
        public async Task<IActionResult> AddMedia([FromBody] PostMediaRequest mediaToAdd)
        {

            // Add a FAKE delay to simulate more "real world"
            await Task.Delay(3000);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var media = new MediaItem
                {
                    Title = mediaToAdd.Title,
                    Kind = mediaToAdd.Kind,
                    Consumed = false,
                    DateConsumed = null,
                    RecommendedBy = mediaToAdd.RecommendedBy,
                    Removed = false
                };
                Context.MediaItems.Add(media);
                await Context.SaveChangesAsync();
                var response = new MediaResponseItem
                {
                    Id = media.Id,
                    Title = media.Title,
                    Consumed = media.Consumed,
                    DateConsumed = media.DateConsumed,
                    Kind = media.Kind,
                    RecommendedBy = media.RecommendedBy
                };
                //TEMP
                return CreatedAtRoute("media#getbyid", new { id = response.Id }, response);
            }
            
        }

        [HttpGet("media/{id:int}", Name = "media#getbyid")]
        public async Task<IActionResult> GetAMediaItem(int id)
        {
            var item = await Context.MediaItems
                .Where(m => m.Removed == false && m.Id == id)
                .Select(m => new MediaResponseItem
                {
                    Id = m.Id,
                    Title = m.Title,
                    RecommendedBy = m.RecommendedBy,
                    Consumed = m.Consumed,
                    DateConsumed = m.DateConsumed,
                    Kind = m.Kind
                }).SingleOrDefaultAsync();
            if(item == null)
            {
                return NotFound("No item with that id");
            }
            else
            {
                return Ok(item);
            }
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

