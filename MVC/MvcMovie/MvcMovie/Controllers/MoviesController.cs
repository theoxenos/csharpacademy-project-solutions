using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers;

public class MoviesController(MvcMovieContext context) : Controller
{
    // GET: Movies
    public async Task<IActionResult> Index(string movieGenre, string searchString, string sortOrder)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (context.Movie == null)
        {
            return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
        }

        ViewData["CurrentSort"] = sortOrder;
        ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
        ViewData["GenreSortParm"] = sortOrder == "Genre" ? "genre_desc" : "Genre";
        ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
        ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";

        IQueryable<Movie> movies = context.Movie.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            movies = movies.Where(s => s.Title!.Contains(searchString));
        }

        if (!string.IsNullOrEmpty(movieGenre))
        {
            movies = movies.Where(x => x.Genre == movieGenre);
        }

        movies = sortOrder switch
        {
            "title_desc" => movies.OrderByDescending(s => s.Title),
            "Date" => movies.OrderBy(s => s.ReleaseDate),
            "date_desc" => movies.OrderByDescending(s => s.ReleaseDate),
            "Genre" => movies.OrderBy(s => s.Genre),
            "genre_desc" => movies.OrderByDescending(s => s.Genre),
            "Price" => movies.OrderBy(s => s.Price),
            "price_desc" => movies.OrderByDescending(s => s.Price),
            "Rating" => movies.OrderBy(s => s.Rating),
            "rating_desc" => movies.OrderByDescending(s => s.Rating),
            _ => movies.OrderBy(s => s.Title)
        };

        MovieGenreViewModel genreViewModel = new()
        {
            Genres = new SelectList(await context.Movie.Select(m => m.Genre).Distinct().ToListAsync()),
            Movies = await movies.ToListAsync(),
            SearchString = searchString,
            MovieGenre = movieGenre,
            SortOrder = sortOrder
        };

        return View(genreViewModel);
    }

    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Movie? movie = await context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create() => View();

    // POST: Movies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        context.Add(movie);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Movie? movie = await context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Movies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        try
        {
            context.Update(movie);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(movie.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Movie? movie = await context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Movie? movie = await context.Movie.FindAsync(id);
        if (movie != null)
        {
            context.Movie.Remove(movie);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    private bool MovieExists(int id) => context.Movie.Any(e => e.Id == id);
}
