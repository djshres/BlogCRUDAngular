﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Data;
using BlogAPI.Models;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly BlogPostsContext _context;
        private readonly IDataRepository<BlogPost> _repo;

        public BlogPostsController(BlogPostsContext context, IDataRepository<BlogPost> repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/BlogPosts
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPost()
        //{
        //    return await _context.BlogPost.ToListAsync();
        //}

        [HttpGet]
        public IEnumerable<BlogPost> GetBlogPosts()
        {
            return _context.BlogPost.OrderByDescending(p => p.PostId);
        }

        // GET: api/BlogPosts/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
        //{
        //    var blogPost = await _context.BlogPost.FindAsync(id);

        //    if (blogPost == null)
        //    {
        //        return NotFound();
        //    }

        //    return blogPost;
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogPost([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogPost = await _context.BlogPost.FindAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);
        }

        // PUT: api/BlogPosts/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBlogPost(int id, BlogPost blogPost)
        //{
        //    if (id != blogPost.PostId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(blogPost).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BlogPostExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogPost([FromRoute] int id, [FromBody] BlogPost blogPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blogPost.PostId)
            {
                return BadRequest();
            }

            _context.Entry(blogPost).State = EntityState.Modified;

            try
            {
                _repo.Update(blogPost);
                var save = await _repo.SaveAsync(blogPost);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }


        // POST: api/BlogPosts
        //[HttpPost]
        //public async Task<ActionResult<BlogPost>> PostBlogPost(BlogPost blogPost)
        //{
        //    _context.BlogPost.Add(blogPost);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBlogPost", new { id = blogPost.PostId }, blogPost);
        //}

        [HttpPost]
        public async Task<IActionResult> PostBlogPost([FromBody] BlogPost blogPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.Add(blogPost);
            var save = await _repo.SaveAsync(blogPost);

            return CreatedAtAction("GetBlogPost", new { id = blogPost.PostId }, blogPost);
        }

        // DELETE: api/BlogPosts/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<BlogPost>> DeleteBlogPost(int id)
        //{
        //    var blogPost = await _context.BlogPost.FindAsync(id);
        //    if (blogPost == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.BlogPost.Remove(blogPost);
        //    await _context.SaveChangesAsync();

        //    return blogPost;
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            _repo.Delete(blogPost);
            var save = await _repo.SaveAsync(blogPost);

            return Ok(blogPost);
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPost.Any(e => e.PostId == id);
        }
    }
}
