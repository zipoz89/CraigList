using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CraigList.Data;
using CraigList.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CraigList.Controllers
{
    public class AuctionModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IWebHostEnvironment WebHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;


        public AuctionModelsController(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Return view of all your auctions
        /// </summary>
        /// <returns></returns>
        // GET: AuctionModels
        public async Task<IActionResult> Index()
        {
            var all = await _context.AuctionModel.ToListAsync();

            List<AuctionModel> thisUserAuctions = new List<AuctionModel>();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            foreach (var item in all)
            {
                if (item.UserId == user.Id)
                {
                    thisUserAuctions.Add(item);
                }
            }

            return View(thisUserAuctions);
        }

        /// <summary>
        /// open view with details of specific auction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: AuctionModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auctionModel = await _context.AuctionModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auctionModel == null)
            {
                return NotFound();
            }

            return View(auctionModel);
        }

        /// <summary>
        /// get of create method
        /// </summary>
        /// <returns></returns>
        // GET: AuctionModels/Create
        public IActionResult Create()
        {
            return View(new CreateAuctionModel());
        }

        /// <summary>
        /// creates new entry with auction model 
        /// </summary>
        /// <param name="createAuctionModel"></param>
        /// <returns></returns>
        // POST: AuctionModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,Picture")] CreateAuctionModel createAuctionModel)
        {
            string fileName = UploadFile(createAuctionModel);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var auctionModel = new AuctionModel
            {
                UserId = user.Id,
                Price = createAuctionModel.Price,
                Title = createAuctionModel.Title,
                Description = createAuctionModel.Description,
                Picture = fileName,
                EndDate = DateTime.Now.AddDays(14)
            };

            if (ModelState.IsValid)
            {
                _context.Add(auctionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createAuctionModel);
        }

        /// <summary>
        /// worst method on earth that uploads image to Img folder in wwwroot xD
        /// </summary>
        /// <param name="createAuctionModel"></param>
        /// <returns></returns>
        private string UploadFile(CreateAuctionModel createAuctionModel)
        {
            string fileName = null;
            if (createAuctionModel != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Img");
                fileName = Guid.NewGuid().ToString() + "-" + createAuctionModel.Picture.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath,FileMode.Create)) 
                {
                    createAuctionModel.Picture.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        /// <summary>
        /// returns list of cards with all active auctions
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Auctions() 
        {
            var auctions = await _context.AuctionModel.ToListAsync();

            List<AuctionModel> active = new List<AuctionModel>();
            foreach (var item in auctions)
            {
                if (item.EndDate < DateTime.Now)
                {
                    active.Add(item);
                }
            }
            return View(auctions);
        }

        /// <summary>
        /// display more info of specific auction
        /// </summary>
        /// <param name="auctionId"></param>
        /// <returns></returns>
        public async Task<IActionResult> DisplayAuction(string auctionId)
        {
            

            if (auctionId == null)
            {
                return NotFound();
            }

            var auctionModel = await _context.AuctionModel.FindAsync(auctionId);
            if (auctionModel == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(auctionModel.UserId);
            Tuple<AuctionModel, ApplicationUser> auctionWithOwner = new Tuple<AuctionModel, ApplicationUser>(auctionModel, user);

            return View(auctionWithOwner);
        }

        /// <summary>
        /// opens view of delete function
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: AuctionModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auctionModel = await _context.AuctionModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auctionModel == null)
            {
                return NotFound();
            }

            return View(auctionModel);
        }

        /// <summary>
        /// deletes auction entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: AuctionModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var auctionModel = await _context.AuctionModel.FindAsync(id);
            _context.AuctionModel.Remove(auctionModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuctionModelExists(string id)
        {
            return _context.AuctionModel.Any(e => e.Id == id);
        }
    }
}
